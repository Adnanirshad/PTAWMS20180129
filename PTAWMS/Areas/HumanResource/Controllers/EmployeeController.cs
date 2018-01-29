using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using System.IO;
using PagedList;
using System.Web.Helpers;
using PTAWMS.Areas.HumanResource.BusinessLogic;
using PTAWMS.Areas.Attendance.BusinessLogic;
using System.Data.Entity.Validation;
using System.Text;
using PTAWMS.Helper;
using WMSLibrary;
using HRM_IKAN.Authentication;
using PTA.Authentication;
using PTAWMS.Areas.Attendance.BusinessLogic.AttendaceHelper;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    [CustomControllerAttributes]
    public class EmployeeController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Employee/
        //[EncryptedActionParameter]
        public ActionResult Index(string Status, int? EmployeeID, int? id, int? deptid, int? divisionid, int? desigid, int? typeid, int? locid)
        {
            try
            {             
                 FiltersModel fm = Session["FiltersModel"] as FiltersModel;
                    List<EmpView> emps = new List<EmpView>();
                    ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                    if ((db.HR_Employee.Where(aa => aa.ReportingToID == LoggedInUser.EmpID).Count() == 0) && LoggedInUser.UserType != "A" && LoggedInUser.UserType != "H")
                    {
                        return RedirectToAction("EmpProfileIndex", new { id = LoggedInUser.EmpID });
                    }
                    if (Status == null || Status == "")
                        Status = "Active";
                    if (Status == "Active")
                    {
                        Status = "Active";
                    }
                    else if (Status == "Resigned")
                        Status = "Resigned";
                    else
                        Status = "";
                    ViewBag.Status = new SelectList(db.HR_EmpStatus.ToList(), "StatusName", "StatusName", Status);
                    QueryBuilder qb = new QueryBuilder();
                    string query = qb.QueryForEmployeeReports(LoggedInUser);
                    DataTable dt = qb.GetValuesfromDB("select * from EmpView where Status='" + Status + "'");
                    if (deptid != null)
                    {
                        emps = dt.ToList<EmpView>().Where(aa => aa.SecID == deptid).AsQueryable().OrderBy(aa => aa.EmployeeID).ToList();

                    }
                    else if (divisionid != null)
                    {
                        emps = dt.ToList<EmpView>().Where(aa => aa.DepartmentID == divisionid).AsQueryable().OrderBy(aa => aa.EmployeeID).ToList();                      
                    }
                    else if (desigid != null)
                    {
                        emps = dt.ToList<EmpView>().Where(aa => aa.DesgID == desigid).AsQueryable().OrderBy(aa => aa.EmployeeID).ToList();                        
                    }
                    else if(typeid !=null) 
                    {
                        emps = dt.ToList<EmpView>().Where(aa => aa.TypID == typeid).AsQueryable().OrderBy(aa => aa.EmployeeID).ToList();
                    }
                else if(locid!=null)
                    {
                        emps = dt.ToList<EmpView>().Where(aa => aa.LocID == locid).AsQueryable().OrderBy(aa => aa.EmployeeID).ToList();
                    }
                else 
                    {
                        emps = dt.ToList<EmpView>().AsQueryable().OrderBy(aa => aa.EmployeeID).ToList();
                    }
                    if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "E" && LoggedInUser.UserType != "H")
                        emps = GetEmployees(emps, LoggedInUser);

                    return View(emps.OrderBy(aa => aa.Status).ToList());
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<EmpView> GetEmployees(List<EmpView> emps, ViewUserEmp LoggedInUser)
        {
            List<EmpView> nEmps =  emps.Where(aa => aa.ReportingToID == LoggedInUser.EmpID).ToList();
            // Add LoggedInUser Employee object in list
            if (nEmps.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).Count() == 0)
                nEmps.AddRange(emps.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).ToList());
            List<EmpView> rEmps = GetReportingToEmps(emps, nEmps);
            if (rEmps.Count > 0)
            {
                while (true)
                {
                    rEmps = GetReportingToEmps(emps, rEmps).ToList();
                    nEmps.AddRange(rEmps);
                    if (rEmps.Count == 0)
                        return nEmps;
                }
            }
            else
                return nEmps;
        }
        private List<EmpView> GetReportingToEmps(List<EmpView> emps,List<EmpView> checkemps)
        {
            List<EmpView> rEmps = new List<EmpView>();
            foreach (var emp in checkemps)
            {
                rEmps.AddRange(emps.Where(aa => aa.ReportingToID == emp.EmployeeID).ToList());

            }
            return rEmps;

        }
        private List<HR_EmpStatus> GetStatus(List<HR_EmpStatus> list)
        {
            try
            {
                List<HR_EmpStatus> temList = new List<HR_EmpStatus>();

                HR_EmpStatus es = new HR_EmpStatus();
                es.StatusName = "All";
                temList.Add(es);
                temList.AddRange(list);
                return temList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<HR_Location> GetLocation(List<HR_Location> list)
        {
            try
            {
                HR_Location loc = new HR_Location();
                loc.LocID = 0;
                loc.LocationName = "All";
                list.Add(loc);
                return list.OrderBy(aa => aa.LocID).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region -- Employee New Forms --
        [CustomActionAttribute]
        public ActionResult EmpProfileIndex(int? id)
        {
            try
            {
                ModelEmpProfileIndex memp = new ModelEmpProfileIndex();
                HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == id);
                memp.EmpID = emp.EmployeeID;
                memp.FullName = emp.FullName;
                memp.EmpNo = emp.EmpNo;
                return View(memp);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult EPPersonal(int ID)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == ID);
                ViewBag.BloodGroup = new SelectList(db.HR_EmpBloodGroup.OrderBy(aa => aa.BloodGroupName).ToList(), "BloodGroupName", "BloodGroupName", hr_employee.BloodGroup);
                ViewBag.Gender = new SelectList(db.HR_EmpGender.OrderBy(aa => aa.GenderName).ToList(), "GenderName", "GenderName", hr_employee.Gender);
                //ViewBag.Nationality = new SelectList(db.HR_EmpNationality.OrderBy(aa => aa.NationalityName).ToList(), "NationalityName", "NationalityName", hr_employee.Nationality);
                //ViewBag.Religion = new SelectList(db.HR_EmpReligion.OrderBy(aa => aa.ReligionName).ToList(), "ReligionName", "ReligionName", hr_employee.Religion);
                ViewBag.MaritalStatus = new SelectList(db.HR_MartialStatus.OrderBy(aa => aa.MartialStatusName).ToList(), "MartialStatusName", "MartialStatusName", hr_employee.MaritalStatus);
                ModelEmpPersonal meJD = new ModelEmpPersonal();
                meJD.EmpID = hr_employee.EmployeeID;
                meJD.FirstName = hr_employee.FirstName;
                meJD.MiddleName = hr_employee.MiddleName;
                meJD.LastName = hr_employee.LastName;
                //meJD.OldIKNo = hr_employee.OldIKNo;
                meJD.FathersName = hr_employee.FathersName;
                // meJD.MothersName = hr_employee.MothersName;
                meJD.CNICNo = hr_employee.CNICNo;
                //meJD.PassportNo = hr_employee.PassportNo;
                if (hr_employee.DOB != null)
                    meJD.DOB = hr_employee.DOB.Value;
                //if (hr_employee.CNICExpireDate != null)
                //    meJD.CNICExpireDate = hr_employee.CNICExpireDate.Value;
                //meJD.PECNo = hr_employee.PECNo;
                return View(meJD);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPPersonal(ModelEmpPersonal data)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["ImageData"];
                if (file != null)
                {
                    string empid = Request.Form["EmpID"].ToString();
                    ImageConversion _Image = new ImageConversion();
                    int imageID = _Image.UploadImageInDataBase(file, Convert.ToInt32(empid));
                    if (imageID != 0)
                    {
                        using (var ctx = new HRMEntities())
                        {
                            int _empID = Convert.ToInt32(empid);
                            var _emp = ctx.HR_Employee.Where(aa => aa.EmployeeID == _empID).ToList();
                            if (_emp.Count > 0)
                            {
                                _emp.FirstOrDefault().EmpImage = imageID;
                                ctx.SaveChanges();
                                ctx.Dispose();
                            }
                        }
                    }
                    else
                    {

                    }
                }
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == data.EmpID);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                if (LoggedInUser.UserType !="A" && LoggedInUser.UserType !="H" )
                    SavePersonalEmpChange(data);
                else
                {
                    SavePersonalInfo(data, hr_employee);
                    db.SaveChanges();
                }
                ViewBag.BloodGroup = new SelectList(db.HR_EmpBloodGroup.OrderBy(aa => aa.BloodGroupName).ToList(), "BloodGroupName", "BloodGroupName", data.BloodGroup);
                ViewBag.Gender = new SelectList(db.HR_EmpGender.OrderBy(aa => aa.GenderName).ToList(), "GenderName", "GenderName", data.Gender);
                //ViewBag.Nationality = new SelectList(db.HR_EmpNationality.OrderBy(aa => aa.NationalityName).ToList(), "NationalityName", "NationalityName", data.Nationality);
                // ViewBag.Religion = new SelectList(db.HR_EmpReligion.OrderBy(aa => aa.ReligionName).ToList(), "ReligionName", "ReligionName", data.Religion);
                ViewBag.MaritalStatus = new SelectList(db.HR_MartialStatus.OrderBy(aa => aa.MartialStatusName).ToList(), "MartialStatusName", "MartialStatusName", data.MaritalStatus);

                return PartialView(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult EPImage()
        {
            try
            {

                HttpPostedFileBase file = Request.Files["ImageData"];
                if (file != null)
                {
                    string empid = Request.Form["EmpID"].ToString();
                    ImageConversion _Image = new ImageConversion();
                    int imageID = _Image.UploadImageInDataBase(file, Convert.ToInt32(empid));
                    if (imageID != 0)
                    {
                        using (var ctx = new HRMEntities())
                        {
                            int _empID = Convert.ToInt32(empid);
                            var _emp = ctx.HR_Employee.Where(aa => aa.EmployeeID == _empID).ToList();
                            if (_emp.Count > 0)
                            {
                                _emp.FirstOrDefault().EmpImage = imageID;
                                ctx.SaveChanges();
                                ctx.Dispose();
                            }
                        }
                    }
                    else
                    {

                    }
                }
                return Json(file.FileName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EPPersonal(ModelEmpPersonal data, HttpPostedFileBase file)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == data.EmpID);
                SavePersonalInfo(data, hr_employee);
                db.SaveChanges();
                return this.Json(string.Empty);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult EPJobDetail(int? ID)
        {

            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == ID);
                ViewBag.DesignationID = new SelectList(db.HR_Designation.OrderBy(aa => aa.DesignationName).ToList(), "DesgID", "DesignationName", hr_employee.DesignationID);
                //ViewBag.CMDesignationID = new SelectList(db.HR_CraftManDesig.OrderBy(aa => aa.CraftDesigName).ToList(), "PCraftDesigID", "CraftDesigName", hr_employee.CraftManDesignationID);
                ViewBag.Status = new SelectList(db.HR_EmpStatus.OrderBy(aa => aa.StatusName).ToList(), "StatusName", "StatusName", hr_employee.Status);
                ViewBag.EmpTypeID = new SelectList(db.HR_EmpType.OrderBy(aa => aa.TypeName).ToList(), "TypID", "TypeName", hr_employee.EmpTypeID);
                ViewBag.GradeID = new SelectList(db.HR_Grade.OrderBy(aa => aa.GradeName).ToList(), "GrdID", "GradeName", hr_employee.GradeID);
                //ViewBag.GroupID = new SelectList(db.HR_Group.OrderBy(aa => aa.GroupName).ToList(), "GrpID", "GroupName", hr_employee.GroupID);
                //ViewBag.JobTitleID = new SelectList(db.HR_JobTitle.OrderBy(aa => aa.JobTitleName).ToList(), "JobTitlID", "JobTitleName", hr_employee.JobTitleID);
                ViewBag.LocationID = new SelectList(db.HR_Location.OrderBy(aa => aa.LocationName).Where(aa => aa.Status == true).ToList(), "LocID", "LocationName", hr_employee.LocationID);
                //ViewBag.DivisionID = new SelectList(db.HR_Division.OrderBy(aa => aa.DivisionName).ToList(), "DivID", "DivisionName", hr_employee.HR_Section.HR_Department.DivsionID);
                ViewBag.SectionID = new SelectList(db.HR_Section.Where(aa=>aa.Status==true).OrderBy(s => s.SectionName).OrderBy(aa => aa.SectionName).ToList(), "SecID", "SectionName", hr_employee.SectionID);
                //ViewBag.BusinessAreaID = new SelectList(db.HR_BusniessArea.OrderBy(aa => aa.BusinessAreaName).ToList(), "BusAreaID", "BusinessAreaName", hr_employee.BusinessAreaID);
                ViewBag.DeptID = new SelectList(db.HR_Department.Where(aa => aa.Status == true).OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName", hr_employee.HR_Section.DepartmentID);
                //ViewBag.CatID = new SelectList(db.HR_Category.OrderBy(aa => aa.CategoryName).ToList(), "CatID", "CategoryName", hr_employee.HR_EmpType.CategoryID);
                ViewBag.UserID = new SelectList(db.Users.OrderBy(aa => aa.UserName).ToList(), "UserID", "UserName");
              
                ModelEmpJobDetail meJD = new ModelEmpJobDetail();
                meJD.EmpID = hr_employee.EmployeeID;
                //meJD.BusinessAreaID = (int)hr_employee.BusinessAreaID;
                // meJD.DivsionID = hr_employee.HR_Section.HR_Department.DivsionID;
                meJD.DepartmentID = hr_employee.HR_Section.DepartmentID;
                meJD.SectionID = (int)hr_employee.SectionID;
                //if (hr_employee.PermanentDate != null)
                //    meJD.PermanentDate = hr_employee.PermanentDate.Value;
                if (hr_employee.LeavingDate != null)
                    meJD.LeavingDate = hr_employee.LeavingDate.Value;
                //meJD.ReasonToLeave = hr_employee.ReasonToLeave;
                //if (hr_employee.Clereance != null)
                //    meJD.Clereance = (bool)hr_employee.Clereance;
                //if (hr_employee.ClearenceDate != null)
                //    meJD.ClearenceDate = hr_employee.ClearenceDate.Value;
                if (hr_employee.DOJ != null)
                    meJD.DOJ = hr_employee.DOJ.Value;
                //if (hr_employee.SAPIntegrated != null)
                //    meJD.SAPIntegrated = (bool)hr_employee.SAPIntegrated;
                //if (hr_employee.SAPID != null)
                //    meJD.SAPID = (int)hr_employee.SAPID;
                return View(meJD);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPJobDetail(ModelEmpJobDetail data)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == data.EmpID);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "H")
                    SaveJobDetailsChange(data);
                else
                {
                    SaveJobDetails(data, hr_employee);
                    db.SaveChanges();
                }
                return this.Json(string.Empty);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EPContact(int ID)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == ID);
                ModelEmpContact meJD = new ModelEmpContact();
                meJD.EmpID = hr_employee.EmployeeID;
                meJD.Address = hr_employee.Address;
                meJD.City = hr_employee.City;
                meJD.Country = hr_employee.Country;
                meJD.EmailID = hr_employee.EmailID;
                meJD.EmergencyContactNo = hr_employee.EmergencyContactNo;
                meJD.LandLine = hr_employee.LandLine;
                meJD.MobileNo = hr_employee.MobileNo;
                meJD.OfficialContactNo = hr_employee.OfficialContactNo;
                meJD.OfficialEmailID = hr_employee.OfficialEmailID;
                return View(meJD);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPContact(ModelEmpContact data)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == data.EmpID);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "H")
                    SaveContactInfoChange(data);
                else
                {
                    SaveContactInfo(data, hr_employee);
                    db.SaveChanges();
                }
                return this.Json(string.Empty);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public ActionResult EPAttendance(int ID)
        {
            try
            {
                ViewBag.EmpName = db.EmpViews.First(aa => aa.EmployeeID == ID).FullName;
                ViewBag.EmpID = ID;
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == ID);
                ModelEmpAttendance meJD = new ModelEmpAttendance();
                meJD.EmpID = hr_employee.EmployeeID;
                meJD.ShiftID = (short)hr_employee.ShiftID;
                meJD.OvertimePolicyID = (short)hr_employee.OTPolicyID;
                meJD.PinCode = hr_employee.PinCode;
                if (hr_employee.ValidDate != null)
                    meJD.ValidDate = hr_employee.ValidDate.Value;
                meJD.FPTemp = hr_employee.FPTemp ==1? true:false;
                meJD.FaceTemp = hr_employee.FaceTemp;
                meJD.ProcessAttendance = hr_employee.ProcessAttendance;
                meJD.CardNo = hr_employee.CardNo;
                meJD.PhoneNo = hr_employee.MobileNo;
                meJD.Email = hr_employee.EmailID;
                meJD.HomeAdd = hr_employee.Address;
                if (hr_employee.TaxRate == null)
                {
                    meJD.TaxRate = 0;
                }
                else
                {
                    meJD.TaxRate = (float)hr_employee.TaxRate;
                }
                meJD.BankAccount = hr_employee.BankAccount;
                ViewBag.ShiftID = new SelectList(db.Att_Shift.OrderBy(aa => aa.ShiftName).ToList(), "ShftID", "ShiftName", hr_employee.ShiftID);
                ViewBag.OvertimePolicyID = new SelectList(db.Att_OTPolicy.OrderBy(aa => aa.OTPolicyName).ToList(), "OTPolicyID", "OTPolicyName");
                ViewBag.OvertimePolicyID = new SelectList(db.Att_OTPolicy.OrderBy(aa => aa.OTPolicyName).ToList(), "OTPolicyID", "OTPolicyName", hr_employee.OTPolicyID);

                return View(meJD);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPAttendance(ModelEmpAttendance data)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == data.EmpID);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "H")
                {
                    SaveAttendanceChange(data);
                }
                else
                {
                    SaveAttendance(data, hr_employee);                  
                    db.SaveChanges();
                }            
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        private void SavePersonalEmpChange(ModelEmpPersonal data)
        {
            try
            {
                HR_EmpChange hr_employee = new HR_EmpChange();
                hr_employee.EmployeeID = data.EmpID;
                hr_employee.ChangeStageID = "Pending";
                hr_employee.DateTime = DateTime.Now;
                hr_employee.FirstName = data.FirstName;
                hr_employee.MiddleName = data.MiddleName;
                hr_employee.LastName = data.LastName;
                string FullName = "";
                if (data.MiddleName == " " && data.FirstName != " " && data.LastName != "")
                    FullName = data.FirstName + "" + data.LastName;
                else
                    FullName = data.FirstName + " " + data.MiddleName + " " + data.LastName;
                hr_employee.FullName = FullName;
                hr_employee.FathersName = data.FathersName;
                hr_employee.CNICNo = data.CNICNo;
                hr_employee.DOB = data.DOB;
                hr_employee.Gender = data.Gender;
                hr_employee.BloodGroup = data.BloodGroup;
                hr_employee.MaritalStatus = data.MaritalStatus;
                db.HR_EmpChange.Add(hr_employee);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveContactInfoChange(ModelEmpContact data)
        {
            try
            {
                HR_EmpChange hr_employee = new HR_EmpChange();
                hr_employee.EmployeeID = data.EmpID;
                hr_employee.ChangeStageID = "Pending";
                hr_employee.DateTime = DateTime.Now;
                hr_employee.Address = data.Address;
                hr_employee.City = data.City;
                hr_employee.Country = data.Country;
                hr_employee.EmailID = data.EmailID;
                hr_employee.EmergencyContactNo = data.EmergencyContactNo;
                hr_employee.LandLine = data.LandLine;
                hr_employee.MobileNo = data.MobileNo;
                hr_employee.OfficialContactNo = data.OfficialContactNo;
                hr_employee.OfficialEmailID = data.OfficialEmailID;
                db.HR_EmpChange.Add(hr_employee);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveJobDetailsChange(ModelEmpJobDetail data)
        {
            try
            {
                HR_EmpChange hr_employee = new HR_EmpChange();
                hr_employee.EmployeeID = data.EmpID;
                hr_employee.ChangeStageID = "Pending";
                hr_employee.DateTime = DateTime.Now;
                hr_employee.SectionID = (short)data.SectionID;
                hr_employee.EmpTypeID = (short)data.EmpTypeID;
                hr_employee.LocationID = (short)data.LocationID;
                hr_employee.GradeID = (short)data.GradeID;
                hr_employee.DesignationID = (short)data.DesignationID;
                // Check for Reprocess of Attendance in case of DOJ Change
                if (data.DOJ != null)
                {
                    if (hr_employee.DOJ != data.DOJ)
                    {
                        //Delete old Attendance
                        foreach (var item in db.Att_DailyAttendance.Where(aa => aa.EmpID == hr_employee.EmployeeID && aa.AttDate < data.DOJ).ToList())
                        {
                            db.Att_DailyAttendance.Remove(item);
                            db.SaveChanges();
                        }
                        if (data.DOJ.Value.Month == DateTime.Today.Month)
                        {
                            // Reprocess attendance
                            ProcessSupportFunc.ProcessAttendanceRequest((DateTime)data.DOJ, DateTime.Today, (int)data.EmpID, data.EmpID.ToString());
                        }
                        ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today, data.EmpID.ToString());
                    }
                }
                hr_employee.DOJ = data.DOJ;
                hr_employee.Status = data.Status;
                if (data.LeavingDate != null && data.Status != "Active")
                {
                    if (hr_employee.LeavingDate != data.LeavingDate)
                    {
                        //Delete other Attendance
                        foreach (var item in db.Att_DailyAttendance.Where(aa => aa.EmpID == hr_employee.EmployeeID && aa.AttDate > data.LeavingDate).ToList())
                        {
                            db.Att_DailyAttendance.Remove(item);
                            db.SaveChanges();
                        }
                        // Reprocess monthly attendance
                        ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(data.LeavingDate.Value.Year, data.LeavingDate.Value.Month, 1), (DateTime)data.LeavingDate, data.EmpID.ToString());
                    }
                }
                if (data.Status != "Active")
                {
                    hr_employee.LeavingDate = data.LeavingDate;
                }
                else
                {
                    hr_employee.LeavingDate = null;
                }

                db.HR_EmpChange.Add(hr_employee);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveAttendanceChange(ModelEmpAttendance data)
        {
            try
            {
                HR_EmpChange hr_employee = new HR_EmpChange();
                hr_employee.EmployeeID = data.EmpID;
                hr_employee.ChangeStageID = "Pending";
                hr_employee.DateTime = DateTime.Now;
                hr_employee.ShiftID = (short)data.ShiftID;
                hr_employee.PinCode = data.PinCode;
                hr_employee.ValidDate = data.ValidDate;
                hr_employee.ProcessAttendance = data.ProcessAttendance;
                hr_employee.FPTemp = Convert.ToInt32(data.FPTemp);
                hr_employee.FaceTemp = data.FaceTemp;
                hr_employee.CardNo = data.CardNo;
                hr_employee.MobileNo = data.PhoneNo;
                hr_employee.EmailID = data.Email;
                hr_employee.Address = data.HomeAdd;
                hr_employee.OTPolicyID = Convert.ToInt16(data.OvertimePolicyID);
                db.HR_EmpChange.Add(hr_employee);
                db.SaveChanges();              
            }
            catch (Exception)
            {

                throw;
            }

        }



        private void SavePersonalInfo(ModelEmpPersonal data, HR_Employee hr_employee)
        {
            try
            {
                hr_employee.EmployeeID = data.EmpID;
                hr_employee.FirstName = data.FirstName;
                hr_employee.MiddleName = data.MiddleName;
                hr_employee.LastName = data.LastName;
                string FullName = "";
                if (data.MiddleName == " " && data.FirstName != " " && data.LastName != "")
                    FullName = data.FirstName + "" + data.LastName;
                else
                    FullName = data.FirstName + " " + data.MiddleName + " " + data.LastName;
                hr_employee.FullName = FullName;
                hr_employee.FathersName = data.FathersName;
                hr_employee.CNICNo = data.CNICNo;
                hr_employee.DOB = data.DOB;
                hr_employee.Gender = data.Gender;
                hr_employee.BloodGroup = data.BloodGroup;
                hr_employee.MaritalStatus = data.MaritalStatus;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveContactInfo(ModelEmpContact data, HR_Employee hr_employee)
        {
            try
            {
                hr_employee.Address = data.Address;
                hr_employee.City = data.City;
                hr_employee.Country = data.Country;
                hr_employee.EmailID = data.EmailID;
                hr_employee.EmergencyContactNo = data.EmergencyContactNo;
                hr_employee.LandLine = data.LandLine;
                hr_employee.MobileNo = data.MobileNo;
                hr_employee.OfficialContactNo = data.OfficialContactNo;
                hr_employee.OfficialEmailID = data.OfficialEmailID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveJobDetails(ModelEmpJobDetail data, HR_Employee hr_employee)
        {
            try
            {
                hr_employee.SectionID = (short)data.SectionID;
                hr_employee.EmpTypeID = (short)data.EmpTypeID;
                hr_employee.LocationID = (short)data.LocationID;
                hr_employee.GradeID = (short)data.GradeID;
                hr_employee.DesignationID = (short)data.DesignationID;
                // Check for Reprocess of Attendance in case of DOJ Change
                if (data.DOJ != null)
                {
                    if (hr_employee.DOJ != data.DOJ)
                    {
                        //Delete old Attendance
                        foreach (var item in db.Att_DailyAttendance.Where(aa => aa.EmpID == hr_employee.EmployeeID && aa.AttDate < data.DOJ).ToList())
                        {
                            db.Att_DailyAttendance.Remove(item);
                            db.SaveChanges();
                        }
                        if (data.DOJ.Value.Month == DateTime.Today.Month)
                        {
                            // Reprocess attendance
                            ProcessSupportFunc.ProcessAttendanceRequest((DateTime)data.DOJ, DateTime.Today, (int)data.EmpID, data.EmpID.ToString());
                        }
                        ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today, data.EmpID.ToString());
                    }
                }
                hr_employee.DOJ = data.DOJ;
                hr_employee.Status = data.Status;
                if (data.LeavingDate != null && data.Status != "Active")
                {
                    if (hr_employee.LeavingDate != data.LeavingDate)
                    {
                        //Delete other Attendance
                        foreach (var item in db.Att_DailyAttendance.Where(aa => aa.EmpID == hr_employee.EmployeeID && aa.AttDate > data.LeavingDate).ToList())
                        {
                            db.Att_DailyAttendance.Remove(item);
                            db.SaveChanges();
                        }
                        // Reprocess monthly attendance
                        ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(data.LeavingDate.Value.Year, data.LeavingDate.Value.Month, 1), (DateTime)data.LeavingDate, data.EmpID.ToString());
                    }
                }
                if (data.Status != "Active")
                {
                    hr_employee.LeavingDate = data.LeavingDate;
                }
                else
                {
                    hr_employee.LeavingDate = null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveAttendance(ModelEmpAttendance data, HR_Employee hr_employee)
        {
            try
            {
                hr_employee.ShiftID = (short)data.ShiftID;
                hr_employee.PinCode = data.PinCode;
                hr_employee.ValidDate = data.ValidDate;
                hr_employee.ProcessAttendance = data.ProcessAttendance;
                hr_employee.FPTemp = Convert.ToInt32(data.FPTemp);
                hr_employee.FaceTemp = data.FaceTemp;
                hr_employee.CardNo = data.CardNo;
                hr_employee.MobileNo = data.PhoneNo;
                hr_employee.EmailID = data.Email;
                hr_employee.Address = data.HomeAdd;
                hr_employee.TaxRate = data.TaxRate;
                hr_employee.BankAccount = data.BankAccount;
                hr_employee.OTPolicyID = Convert.ToInt16(data.OvertimePolicyID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private double? GetSidatAmount(string sidatID)
        {
            try
            {
                double val = 0;
                if (sidatID != null)
                {
                    try
                    {
                        QueryBuilder qb = new QueryBuilder();
                        DataTable dt = qb.GetValuesfromDB("select PFBalance from Rpt_Vw_SiddatPF_Club where CA_ACC_CODE ='" + sidatID + "'");
                        if (dt.Rows.Count > 0)
                        {
                            string cc = dt.Rows[0].ItemArray[0].ToString();
                            val = Convert.ToDouble(cc);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return val;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Create()
        {
            try
            {
                //int MaxEmpID = db.HR_Employee.Max(p => p.EmployeeID);
                // int MaxSapID = (int)db.HR_Employee.Max(p => p.SAPID);

                ModelEmpCreate hremp = new ModelEmpCreate();
                //hremp.EmpID = MaxEmpID + 1;
                hremp.DOJ = DateTime.Today;
                //ViewBag.FirstPPeriodID = new SelectList(db.PR_PayRollPeriod.Where(aa => aa.PeriodStageID != "C").ToList().OrderByDescending(aa => aa.StartDate), "ID", "Name");
                ViewBag.BloodGroup = new SelectList(db.HR_EmpBloodGroup.OrderBy(aa => aa.BloodGroupName).ToList(), "BloodGroupName", "BloodGroupName");
                ViewBag.Gender = new SelectList(db.HR_EmpGender.OrderBy(aa => aa.GenderName).ToList(), "GenderName", "GenderName");
                //ViewBag.Nationality = new SelectList(db.HR_EmpNationality.OrderBy(aa => aa.NationalityName).ToList(), "NationalityName", "NationalityName");
                //ViewBag.Religion = new SelectList(db.HR_EmpReligion.OrderBy(aa => aa.ReligionName).ToList(), "ReligionName", "ReligionName");
                ViewBag.MaritalStatus = new SelectList(db.HR_MartialStatus.OrderBy(aa => aa.MartialStatusName).ToList(), "MartialStatusName", "MartialStatusName");
                ViewBag.LocationID = new SelectList(db.HR_Location.OrderBy(aa => aa.LocationName).Where(aa=>aa.Status==true).ToList(), "LocID", "LocationName");
                return View(hremp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpID,EmpNo,DOJ,FirstName,MiddleName,LastName,HasPayroll,FirstPPeriodID,Gender,BloodGroup,MaritalStatus,Religion,Nationality,LocationID")] ModelEmpCreate hr_employee)
        {
            try
            {
                int ss = 0;
                if (string.IsNullOrEmpty(hr_employee.EmpNo))
                    ModelState.AddModelError("EmpNo", "Emp No is required!");
                string id = hr_employee.EmpID.ToString("00000");
                if (string.IsNullOrEmpty(hr_employee.FirstName))
                    ModelState.AddModelError("FirstName", "Name is required!");

                if (string.IsNullOrEmpty(hr_employee.LastName))
                    ModelState.AddModelError("LastName", "Last Name is required!");
                if (hr_employee.DOJ == null)
                    ModelState.AddModelError("DOJ", "DOJ is required!");

                if (hr_employee.EmpNo != null)
                {
                    if (hr_employee.EmpNo.Length > 15)
                        ModelState.AddModelError("EmpNo", "String length exceeds!");
                    if (db.HR_Employee.Where(aa => aa.EmpNo.ToUpper() == hr_employee.EmpNo.ToUpper()).Count() > 0)
                        ModelState.AddModelError("EmpNo", "Emp No should be unique!");
                }
                if (ModelState.IsValid)
                {
                    string FullName = "";
                    if (hr_employee.MiddleName == " " && hr_employee.FirstName != " " && hr_employee.LastName != "")
                        FullName = hr_employee.FirstName + "" + hr_employee.LastName;
                    else
                        FullName = hr_employee.FirstName + " " + hr_employee.MiddleName + " " + hr_employee.LastName;

                    HR_Employee emp = new HR_Employee();
                    emp.EmployeeID = hr_employee.EmpID;
                    emp.EmpNo = hr_employee.EmpNo;
                    emp.LocationID = (Int16)hr_employee.LocationID;
                    emp.FirstName = hr_employee.FirstName;
                    emp.MiddleName = hr_employee.MiddleName;
                    emp.LastName = hr_employee.LastName;
                    if (emp.MiddleName != "" && emp.MiddleName != null)
                        emp.FullName = hr_employee.FirstName + " " + hr_employee.MiddleName + " " + hr_employee.LastName;
                    else
                        emp.FullName = hr_employee.FirstName + " " + hr_employee.LastName;
                    emp.DOJ = hr_employee.DOJ;
                    emp.Gender = hr_employee.Gender;
                    emp.BloodGroup = hr_employee.BloodGroup;
                    emp.MaritalStatus = hr_employee.MaritalStatus;
                    emp.Status = "Active";
                    emp.SectionID = db.HR_Section.FirstOrDefault().SecID;
                    emp.EmpTypeID = db.HR_EmpType.FirstOrDefault().TypID;
                    emp.GradeID = db.HR_Grade.FirstOrDefault().GrdID;
                    emp.DesignationID = db.HR_Designation.FirstOrDefault().DesgID;
                    emp.ShiftID = db.Att_Shift.FirstOrDefault().ShftID;
                    emp.LocationID = db.HR_Location.FirstOrDefault().LocID;
                    emp.ProcessAttendance = true;
                    emp.FaceTemp = false;
                    emp.FPTemp = 0;
                    db.HR_Employee.Add(emp);
                    db.SaveChanges();
                    if (SaveChanges(db))
                    {
                        CreateAttendanceProcessRequest(emp);
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.BloodGroup = new SelectList(db.HR_EmpBloodGroup.OrderBy(aa => aa.BloodGroupName).ToList(), "BloodGroupName", "BloodGroupName", hr_employee.BloodGroup);
                ViewBag.Gender = new SelectList(db.HR_EmpGender.OrderBy(aa => aa.GenderName).ToList(), "GenderName", "GenderName", hr_employee.Gender);
                ViewBag.MaritalStatus = new SelectList(db.HR_MartialStatus.OrderBy(aa => aa.MartialStatusName).ToList(), "MartialStatusName", "MartialStatusName", hr_employee.MaritalStatus);
                ViewBag.LocationID = new SelectList(db.HR_Location.OrderBy(aa => aa.LocationName).Where(aa => aa.Status == true).ToList(), "LocID", "LocationName");
                return View(hr_employee);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //HttpPostedFileBase file = Request.Files["ImageData"];
        //        if (file != null)
        //        {
        //            ImageConversion _Image = new ImageConversion();
        //            int imageID = _Image.UploadImageInDataBase(file, hr_employee.EmpNo);
        //            if (imageID != 0)
        //            {
        //                using (var ctx = new HRMEntities())
        //                {
        //                    var _emp = ctx.HR_Employee.Where(aa => aa.EmpNo == hr_employee.EmpNo).ToList();
        //                    if (_emp.Count > 0)
        //                    {
        //                        _emp.FirstOrDefault().EmpImage = imageID;
        //                        ctx.SaveChanges();
        //                        ctx.Dispose();
        //                    }
        //                }
        //            }
        //            else
        //            {

        //            }
        //        }
        //private int GetPayrollPeriodID(int EmpID, DateTime? DOJ)
        //{
        //    try
        //    {
        //        int periodID = 0;
        //        if (DOJ != null)
        //        {
        //            List<PR_PayRollPeriod> periods = db.PR_PayRollPeriod.Where(aa => aa.PeriodStageID != "C").ToList();
        //            foreach (var period in periods)
        //            {
        //                if (period.StartDate >= DOJ && period.EndDate <= DOJ)
        //                    periodID = period.ID;
        //            }
        //        }
        //        return periodID;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        private void CreateAttendanceProcessRequest(HR_Employee hr_employee)
        {
            try
            {
                if (hr_employee.DOJ != null)
                {
                    DateTime tempdoj = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    if (hr_employee.DOJ > tempdoj)
                        tempdoj = hr_employee.DOJ.Value;
                    ProcessSupportFunc.ProcessAttendanceRequest((DateTime)tempdoj, (DateTime)DateTime.Today, (int)hr_employee.EmployeeID, hr_employee.EmployeeID.ToString());
                    ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(tempdoj.Year, tempdoj.Month, 1), DateTime.Today, hr_employee.EmployeeID.ToString());
                }
                else
                {
                    DateTime tempdoj = DateTime.Today;
                    ProcessSupportFunc.ProcessAttendanceRequest((DateTime)tempdoj, (DateTime)DateTime.Today, (int)hr_employee.EmployeeID, hr_employee.EmployeeID.ToString());
                    ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(tempdoj.Year, tempdoj.Month, 1), DateTime.Today, hr_employee.EmployeeID.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private bool SaveChanges(DbContext context)
        {
            bool check = false;
            try
            {
                context.SaveChanges();
                check = true;
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
            return check;
        }

        private void DeleteAttendance(DateTime dt, int empID)
        {
            try
            {
                List<Att_DailyAttendance> att = new List<Att_DailyAttendance>();
                att = db.Att_DailyAttendance.Where(aa => aa.EmpID == empID && aa.AttDate >= dt).ToList();
                foreach (var item in att)
                {
                    db.Att_DailyAttendance.Remove(item);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(dt.Year, dt.Month, 1), DateTime.Today, empID.ToString());
        }

        // GET: /HumanResource/Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Employee hr_employee = db.HR_Employee.Find(id);
                if (hr_employee == null)
                {
                    return HttpNotFound();
                }
                return View(hr_employee);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.Find(id);
                db.HR_Employee.Remove(hr_employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult SectionList(string ID)
        {
            try
            {
                short deptID = Convert.ToInt16(ID);

                var secs = db.HR_Section.Where(aa => aa.Status == true).Where(aa => aa.DepartmentID == deptID).OrderBy(s => s.SectionName);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    secs.ToArray(),
                                    "SecID",
                                    "SectionName")
                               , JsonRequestBehavior.AllowGet);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DeptList(string ID)
        {
            try
            {
                short divID = Convert.ToInt16(ID);
                var depts = db.HR_Department.Where(aa => aa.Status == true).OrderBy(s => s.DepartmentName);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    depts.ToArray(),
                                    "DeptID",
                                    "DepartmentName")
                               , JsonRequestBehavior.AllowGet);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public ActionResult DivisionList(string ID)
        //{
        //    try
        //    {
        //        short divID = Convert.ToInt16(ID);
        //        var depts = db.HR_Division.Where(aa => aa.BussinessAreaID == divID).OrderBy(s => s.DivisionName);
        //        if (HttpContext.Request.IsAjaxRequest())
        //            return Json(new SelectList(
        //                            depts.ToArray(),
        //                            "DivID",
        //                            "DivisionName")
        //                       , JsonRequestBehavior.AllowGet);

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public ActionResult RetrieveImage(int id)
        {
            try
            {
                byte[] cover = GetImageFromDataBase(id);
                if (cover != null && cover.Count() > 0)
                {
                    return File(cover, "image/jpg");
                }
                else
                {
                    return File("~/images/defaultimage.png", "image/png");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        public byte[] GetImageFromDataBase(int Id)
        {
            try
            {
                try
                {
                    if (db.HR_EmpImage.Where(aa => aa.EmpID == Id).Count() > 0)
                    {
                        var q = db.HR_EmpImage.First(aa => aa.EmpID == Id);
                        byte[] cover = q.EmpPic;
                        return cover;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Avatar Controller
        private int _avatarWidth = 250; // ToDo - Change the size of the stored avatar image
        private int _avatarHeight = 250; // ToDo - Change the size of the stored avatar image

        #region --Upload Picture--
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult _Upload()
        {
            return PartialView();
        }

        [ValidateAntiForgeryToken]
        public ActionResult _Upload(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                string errorMessage = "";

                if (files != null && files.Count() > 0)
                {
                    // Get one only
                    var file = files.FirstOrDefault();
                    // Check if the file is an image
                    if (file != null && IsImage(file))
                    {
                        // Verify that the user selected a file
                        if (file != null && file.ContentLength > 0)
                        {
                            var webPath = SaveTemporaryFile(file);
                            return Json(new { success = true, fileName = webPath.Replace("/", "\\") }); // success
                        }
                        errorMessage = "File cannot be zero length."; //failure
                    }
                    errorMessage = "File is of wrong format."; //failure
                }
                errorMessage = "No file uploaded."; //failure

                return Json(new { success = false, errorMessage = errorMessage });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Save(string t, string l, string h, string w, string fileName)
        {
            try
            {
                // Get file from temporary folder
                var fn = Path.Combine(Server.MapPath("~/Temp"), Path.GetFileName(fileName));

                // Calculate dimesnions
                int top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
                int left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
                int height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
                int width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

                // Get image and resize it, ...
                var img = new WebImage(fn);
                img.Resize(width, height);
                // ... crop the part the user selected, ...
                img.Crop(top, left, img.Height - top - _avatarHeight, img.Width - left - _avatarWidth);
                // ... delete the temporary file,...
                System.IO.File.Delete(fn);
                // ... and save the new one.
                string newFileName = "/Avatars/" + Path.GetFileName(fn);
                Session["imagePath"] = newFileName;
                string newFileLocation = HttpContext.Server.MapPath(newFileName);
                if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));
                }

                img.Save(newFileLocation);
                Session["imageFullPath"] = newFileLocation;

                return Json(new { success = true, avatarFileLocation = newFileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Unable to upload file.\nERRORINFO: " + ex.Message });
            }
        }
        private bool IsImage(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentType.Contains("image"))
                {
                    return true;
                }

                var extensions = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if you like...

                // linq from Henrik Stenbæk
                return extensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {

                throw;
            }
        }
        private string SaveTemporaryFile(HttpPostedFileBase file)
        {
            // Define destination
            try
            {
                var folderName = "/Temp";
                var serverPath = HttpContext.Server.MapPath(folderName);
                if (Directory.Exists(serverPath) == false)
                {
                    Directory.CreateDirectory(serverPath);
                }

                // Generate unique file name
                var fileName = Path.GetFileName(file.FileName);
                fileName = SaveTemporaryAvatarFileImage(file, serverPath, fileName);

                // Clean up old files after every save
                CleanUpTempFolder(1);

                return Path.Combine(folderName, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string serverPath, string fileName)
        {
            try
            {
                var img = new WebImage(file.InputStream);
                double ratio = (double)img.Height / (double)img.Width;

                string fullFileName = Path.Combine(serverPath, fileName);

                img.Resize(400, (int)(400 * ratio)); // ToDo - Change the value of the width of the image oin the screen

                if (System.IO.File.Exists(fullFileName))
                    System.IO.File.Delete(fullFileName);

                img.Save(fullFileName);

                return Path.GetFileName(img.FileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CleanUpTempFolder(int hoursOld)
        {
            try
            {
                DateTime fileCreationTime;
                DateTime currentUtcNow = DateTime.UtcNow;

                var serverPath = HttpContext.Server.MapPath("/Temp");
                if (Directory.Exists(serverPath))
                {
                    string[] fileEntries = Directory.GetFiles(serverPath);
                    foreach (var fileEntry in fileEntries)
                    {
                        fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                        var res = currentUtcNow - fileCreationTime;
                        if (res.TotalHours > hoursOld)
                        {
                            System.IO.File.Delete(fileEntry);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}
