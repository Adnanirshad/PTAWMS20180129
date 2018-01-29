using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using System.Data.Entity.Validation;
using System.Text;
using PTA.Authentication;
using HRM_IKAN.Authentication;

namespace PTAWMS.Controllers
{
    [CustomControllerAttributes]
    public class UserController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /User/
        public ActionResult Index()
        {
            return View(db.ViewUserEmps.Where(aa=>aa.UserID>0).OrderBy(aa=>aa.EmpStatus).ToList());
        }
        [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.UserType = new SelectList(db.UserTypes.ToList(), "PUTID", "UTypeName", "N");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,UserType,Status,CanEdit,CanDelete,CanView,CanAdd,MAttProcess,MOption,MAttDevice,MAttDeviceUtility,MAttEditAttendance,MAttJobCard,MAttShift,MAttPolicy,MAttDownloadTime,MAttHoliday,MAttOTPolicy,MAttOTCreate,MAttOTEdit,MAttOTRequest,MAttLeaves,MHRCompHierarchy,MUser,MGrade,MHREmployee,MHREmpPersonal,MHREmpJob,MHREmpAtt,HRModule,HREmpType,HRLocation,HRDeptartment,HRDesignation,HRSection,AttendanceModule,OUserID,VisitorApplication,VisitorEntry,VisitorSupervisor,OTBudget,OTBudgetCreditDebit")] User user)
        {
            if (String.IsNullOrWhiteSpace(user.UserName))
                ModelState.AddModelError("UserName", "User name is mandatory");
            if (db.Users.Where(aa => aa.UserName == user.UserName).Count() > 0)
                ModelState.AddModelError("UserName", "User name must be unique");
            if (String.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", "Password is mandatory");
            string empNo = Request.Form["EmpNo"].ToString();
            if (db.HR_Employee.Where(aa => aa.EmpNo == empNo).Count() == 0)
                ModelState.AddModelError("Password", "Employee number is not found");
            else
            {
                user.UserID = db.HR_Employee.Where(aa => aa.EmpNo == empNo).First().EmployeeID;
                if (db.Users.Where(aa => aa.UserID == user.UserID).Count() > 0)
                    ModelState.AddModelError("Password", "User of this employee already exist");
            }
            user.DateCreated = DateTime.Today;
            // Get users Permissions on forms
            user.UserType = Request.Form["UserType"];
            if (ModelState.IsValid)
            {
                GetValuesFromCheckboxes(user);
                db.Users.Add(user);
                SaveChanges(db);
                SetUserAccessLevelData(user);
                return RedirectToAction("Index");
            }
            ViewBag.UserType = new SelectList(db.UserTypes.ToList(), "PUTID", "UTypeName", user.UserType);
            return View(user);
        }
        private void GetValuesFromCheckboxes(User user)
        {
            #region-- Checkboxes --
            user.Status = (bool)ValueProvider.GetValue("Status").ConvertTo(typeof(bool));
            user.HRReport = (bool)ValueProvider.GetValue("HRReport").ConvertTo(typeof(bool));
            user.HRRequest = (bool)ValueProvider.GetValue("HRRequest").ConvertTo(typeof(bool));


            #endregion
        }

        private void RemoveUserRoleDatas(Models.User user)
        {
            List<UserRoleData> UserRoleDatas = db.UserRoleDatas.Where(aa => aa.RoleUserID == user.UserID).ToList();
            foreach (var roleData in UserRoleDatas)
            {
                db.UserRoleDatas.Remove(roleData);
                db.SaveChanges();
            }
        }
        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }

        private void SetUserAccessLevelData(User user)
        {
            //Save Location Admin
            if (Request.Form["UserRoleL"] == "A")
            {
                UserRoleData urd = new UserRoleData();
                urd.RoleDataLegend = "A";
                urd.UserRoleLegend = "L";
                urd.RoleDataValue = 0;
                urd.RoleUserID = user.UserID;
                db.UserRoleDatas.Add(urd);
                db.SaveChanges();
            }
            //Save Admin of Divs, Depts and Secs
            if (Request.Form["UserRoleD"] == "G")
            {
                UserRoleData urd = new UserRoleData();
                urd.RoleDataLegend = "G";
                urd.UserRoleLegend = "D";
                urd.RoleDataValue = 0;
                urd.RoleUserID = user.UserID;
                db.UserRoleDatas.Add(urd);
                db.SaveChanges();
            }
            if (Request.Form["UserRoleT"] == "Y")
            {
                UserRoleData urd = new UserRoleData();
                urd.RoleDataLegend = "Y";
                urd.UserRoleLegend = "T";
                urd.RoleDataValue = 0;
                urd.RoleUserID = user.UserID;
                db.UserRoleDatas.Add(urd);
                db.SaveChanges();
            }
            //Save UserLoc
            if (Request.Form["uLocationCount"] != "" && Request.Form["UserRoleL"] == "L")
            {
                int locationCount = Convert.ToInt32(Request.Form["uLocationCount"]);
                List<HR_Location> locs = new List<HR_Location>();
                locs = db.HR_Location.ToList();
                for (int i = 1; i <= locationCount; i++)
                {
                    string uLocID = "uLocation" + i;
                    string LocName = Request.Form[uLocID].ToString();
                    int locID = locs.Where(aa => aa.LocationName == LocName).FirstOrDefault().LocID;
                    UserRoleData urd = new UserRoleData();
                    urd.RoleDataLegend = "L";
                    urd.UserRoleLegend = "L";
                    urd.RoleDataValue = (short)locID;
                    urd.RoleUserID = user.UserID;
                    db.UserRoleDatas.Add(urd);
                    db.SaveChanges();
                }
            }
            //Save User City
            if (Request.Form["uCityCount"] != "" && Request.Form["UserRoleL"] == "C")
            {
                int cityCount = Convert.ToInt32(Request.Form["uCityCount"]);
                List<HR_City> cities = new List<HR_City>();
                cities = db.HR_City.ToList();
                for (int i = 1; i <= cityCount; i++)
                {
                    string uCityID = "uCity" + i;
                    string CityName = Request.Form[uCityID].ToString();
                    int cityID = cities.Where(aa => aa.CityName == CityName).FirstOrDefault().CityID;
                    UserRoleData urd = new UserRoleData();
                    urd.RoleDataLegend = "C";
                    urd.UserRoleLegend = "L";
                    urd.RoleDataValue = (short)cityID;
                    urd.RoleUserID = user.UserID;
                    db.UserRoleDatas.Add(urd);
                    db.SaveChanges();
                }
            }
            //Save User Region
            if (Request.Form["uRegionCount"] != "" && Request.Form["UserRoleL"] == "R")
            {
                int regionCount = Convert.ToInt32(Request.Form["uRegionCount"]);
                List<HR_City> regions = new List<HR_City>();
                regions = db.HR_City.ToList();
                for (int i = 1; i <= regionCount; i++)
                {
                    string uRegionID = "uRegion" + i;
                    string RegionName = Request.Form[uRegionID].ToString();
                    //  int regionID = regions.Where(aa => aa.CityName == RegionName).FirstOrDefault().RegID;
                    UserRoleData urd = new UserRoleData();
                    urd.RoleDataLegend = "R";
                    urd.UserRoleLegend = "L";
                    // urd.RoleDataValue = (short)regionID;
                    urd.RoleUserID = user.UserID;
                    db.UserRoleDatas.Add(urd);
                    db.SaveChanges();
                }
            }
            //Save User Section
            if (Request.Form["uSectionCount"] != "" && Request.Form["UserRoleD"] == "S")
            {
                int sectionCount = Convert.ToInt32(Request.Form["uSectionCount"]);
                List<HR_Section> sections = new List<HR_Section>();
                sections = db.HR_Section.ToList();
                for (int i = 1; i <= sectionCount; i++)
                {
                    string uSectionID = "uSection" + i;
                    string SectionName = Request.Form[uSectionID].ToString();
                    int sectionID = sections.Where(aa => aa.SectionName == SectionName).FirstOrDefault().SecID;
                    UserRoleData urd = new UserRoleData();
                    urd.RoleDataLegend = "S";
                    urd.UserRoleLegend = "D";
                    urd.RoleDataValue = (short)sectionID;
                    urd.RoleUserID = user.UserID;
                    db.UserRoleDatas.Add(urd);
                    db.SaveChanges();
                }
            }
            //Save User Department
            if (Request.Form["uDepartmentCount"] != "" && Request.Form["UserRoleD"] == "D")
            {
                int departmentCount = Convert.ToInt32(Request.Form["uDepartmentCount"]);
                List<HR_Department> departments = new List<HR_Department>();
                departments = db.HR_Department.ToList();
                for (int i = 1; i <= departmentCount; i++)
                {
                    string uDepartmentID = "uDepartment" + i;
                    string DepartmentName = Request.Form[uDepartmentID].ToString();
                    int DepartmentID = departments.Where(aa => aa.DepartmentName == DepartmentName).FirstOrDefault().DeptID;
                    UserRoleData urd = new UserRoleData();
                    urd.RoleDataLegend = "D";
                    urd.UserRoleLegend = "D";
                    urd.RoleDataValue = (short)DepartmentID;
                    urd.RoleUserID = user.UserID;
                    db.UserRoleDatas.Add(urd);
                    db.SaveChanges();
                }
            }
            //Save User Division
            //if (Request.Form["uDivisionCount"] != "" && Request.Form["UserRoleD"] == "V")
            //{
            //    int divisionCount = Convert.ToInt32(Request.Form["uDivisionCount"]);
            //    List<HR_Division> divisions = new List<HR_Division>();
            //    divisions = db.HR_Division.ToList();
            //    for (int i = 1; i <= divisionCount; i++)
            //    {
            //       // string uDivisionID = "uDivision" + i;
            //        //string DivisionName = Request.Form[uDivisionID].ToString();
            //        //int divisionID = divisions.Where(aa => aa.DivisionName == DivisionName).FirstOrDefault().DivID;
            //        UserRoleData urd = new UserRoleData();
            //        urd.RoleDataLegend = "V";
            //        urd.UserRoleLegend = "D";
            //        urd.RoleDataValue = (short)divisionID;
            //        urd.RoleUserID = user.UserID;
            //        db.UserRoleDatas.Add(urd);
            //        db.SaveChanges();
            //    }
            //}
            //Save User Types
            if (Request.Form["uETypeCount"] != "" && Request.Form["UserRoleT"] == "T")
            {
                int eTypeCount = Convert.ToInt32(Request.Form["uETypeCount"]);
                List<HR_EmpType> etypes = new List<HR_EmpType>();
                etypes = db.HR_EmpType.ToList();
                for (int i = 1; i <= eTypeCount; i++)
                {
                    string uEtypeID = "uEType" + i;
                    string TypeName = Request.Form[uEtypeID].ToString();
                    int typeID = etypes.Where(aa => aa.TypeName == TypeName).FirstOrDefault().TypID;
                    UserRoleData urd = new UserRoleData();
                    urd.RoleDataLegend = "T";
                    urd.UserRoleLegend = "T";
                    urd.RoleDataValue = (short)typeID;
                    urd.RoleUserID = user.UserID;
                    db.UserRoleDatas.Add(urd);
                    db.SaveChanges();
                }
            }
        }
        // GET: /User/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            //ViewBag.UserRoleL = new SelectList(db.UserRoles.Where(aa => aa.RoleType == "L"), "RoleID", "RoleName", user.UserRoleL);
            //ViewBag.UserRoleD = new SelectList(db.UserRoles.Where(aa => aa.RoleType == "D"), "RoleID", "RoleName", user.UserRoleD);
            //ViewBag.UserRoleT = new SelectList(db.UserRoles.Where(aa => aa.RoleType == "T"), "RoleID", "RoleName", user.UserRoleT);
            ViewBag.UserType = new SelectList(db.UserTypes.ToList(), "PUTID", "UTypeName", user.UserType);
            ViewBag.UserList = new SelectList(db.ViewUserEmps.Where(aa => aa.Status == true).ToList(), "UserID", "UserName");
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        //public ActionResult Edit([Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,UserType,Status,CanEdit,CanDelete,CanView,CanAdd,MAttProcess,MOption,MAttDevice,MAttDeviceUtility,MAttEditAttendance,MAttJobCard,MAttShift,MAttPolicy,MAttDownloadTime,MAttHoliday,MAttOTPolicy,MAttOTCreate,MAttOTEdit,MAttOTRequest,MAttLeaves,MHRCompHierarchy,MUser,MGrade,MHREmployee,MHREmpPersonal,MHREmpJob,MHREmpAtt,HRModule,HREmpType,HRLocation,HRDeptartment,HRDesignation,HRSection,AttendanceModule,OUserID,VisitorModule,VisitorApplication,VisitorEntry,VisitorSupervisor,OTBudget,OTBudgetCreditDebit,HRRequest,HRReport")] User user)
        public ActionResult Edit(User user)
        {
            if (String.IsNullOrWhiteSpace(user.UserName))
                ModelState.AddModelError("UserName", "User name is mandatory");
            if (db.Users.Where(aa => aa.UserName == user.UserName && aa.EmpID != user.EmpID).Count() > 1)
                ModelState.AddModelError("UserName", "User name must be unique");
            if (String.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", "Password is mandatory");
            user.DateCreated = DateTime.Today;
            user.UserType = Request.Form["UserType"];

            if (ModelState.IsValid)
            {
                GetValuesFromCheckboxes(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                RemoveUserRoleDatas(user);
                SetUserAccessLevelData(user);
                return RedirectToAction("Index");
            }
            ViewBag.UserType = new SelectList(db.UserTypes.ToList(), "PUTID", "UTypeName", user.UserType);
            ViewBag.UserList = new SelectList(db.ViewUserEmps.Where(aa=>aa.Status == true).ToList(), "UserID", "UserName");
            return View(user);
        }
        // GET: /User/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult FocalUsers(int? id)
        {
            ViewBag.FocalUID = new SelectList(db.ViewUserEmps.Where(aa => aa.Status == true).ToList(), "UserID", "FullName");
            return View();
        }
         [HttpPost]
        public ActionResult FocalUsers([Bind(Include = "PID,FocalUID,UserID,StartDate,EndDate")]int? id, FocalUser focaluser)
        {           
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;          
            if (focaluser.StartDate == null)
            {
                ViewBag.error = "required field must have a value";
            }
            else
            {
                if (db.FocalUsers.Where(aa => aa.FocalUID == focaluser.FocalUID && aa.EndDate == null).Count() > 0)
                {
                    FocalUser OldEntry = db.FocalUsers.Where(aa => aa.FocalUID == focaluser.FocalUID && aa.EndDate == null).OrderByDescending(aa => aa.PID).First();
                    OldEntry.EndDate = DateTime.Now.AddDays(-1);

                }
                focaluser.UserID = id;              
                focaluser.FocalUName = db.ViewUserEmps.First(aa => aa.UserID == focaluser.FocalUID).FullName;
                ViewBag.FocalUID = new SelectList(db.ViewUserEmps.Where(aa => aa.Status == true).ToList(), "UserID", "FullName");
                db.FocalUsers.Add(focaluser);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            ViewBag.FocalUID = new SelectList(db.ViewUserEmps.Where(aa => aa.Status == true).ToList(), "UserID", "FullName");              
            return View();
        }
        [HttpGet]
        public ActionResult GetUserType(string types)
        {
            var utypes = db.UserTypes.Where(aa => aa.PUTID == types).ToList();
            return new JsonResult() { Data = utypes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Template(string id)
        {
            switch (id.ToLower())
            {
                case "locationaccess":
                    return PartialView("~/Views/User/Partials/LocationAccess.cshtml");
                case "cityaccess":
                    return PartialView("~/Views/User/Partials/CityAccess.cshtml");
                case "regionaccess":
                    return PartialView("~/Views/User/Partials/RegionAccess.cshtml");
                case "sectionaccess":
                    return PartialView("~/Views/User/Partials/SectionAccess.cshtml");
                case "departmentaccess":
                    return PartialView("~/Views/User/Partials/DepartmentAccess.cshtml");
                case "divisionaccess":
                    return PartialView("~/Views/User/Partials/DivisionAccess.cshtml");
                case "employeetypeaccess":
                    return PartialView("~/Views/User/Partials/EmployeeTypeAccess.cshtml");

                default:
                    throw new Exception("template not known");
            }
        }
        public ActionResult ListofFocalUsers()
        {
            var list = db.FocalUsers.ToList();
            return View(list);
        }

        public List<UserTypeModel> GetUserTypes()
        {
            List<UserTypeModel> utl = new List<UserTypeModel>();
            UserTypeModel utma = new UserTypeModel();
            utma.UTID = "Admin";
            utma.UTName = "Admin";
            UserTypeModel utmh = new UserTypeModel();
            utmh.UTID = "Employee";
            utmh.UTName = "Employee";
            UserTypeModel utmd = new UserTypeModel();
            utmd.UTID = "Director";
            utmd.UTName = "Director";
            UserTypeModel utmM = new UserTypeModel();
            utmM.UTID = "Member";
            utmM.UTName = "Member";
            UserTypeModel utmG = new UserTypeModel();
            utmG.UTID = "DG";
            utmG.UTName = "DG";
            utl.Add(utma);
            utl.Add(utmh);
            utl.Add(utmd);
            utl.Add(utmM);
            utl.Add(utmG);
            return utl;
        }
    }
    public class UserTypeModel
    {
        public string UTID { get; set; }
        public string UTName { get; set; }
    }
}
