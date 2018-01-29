using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using System.Web.Mvc;
using PTAWMS.Models;
using WMSLibrary;
using PTAWMS.Helper;
using HRM_IKAN.Authentication;
using PTAWMS.App_Start;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class EmployeeJobCardController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/EmployeeJobCard/
        public ActionResult Index()
        {
            var att_jobcardapp = db.ViewJobCards.Where(aa => aa.StatusID == "Approved").OrderByDescending(aa=>aa.DateCreated).ToList();
            return View(att_jobcardapp);
        }

        // GET: /Attendance/EmployeeJobCard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_JobCardApp att_jobcardapp = db.Att_JobCardApp.Find(id);
            if (att_jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(att_jobcardapp);
        }

        // GET: /Attendance/EmployeeJobCard/Create
        public ActionResult Create(int? ID)
        {
            Att_JobCardApp jb = new Att_JobCardApp();
            jb.DateStarted = DateTime.Today;
            jb.EmpID = ID;
            jb.StartTime = DateTime.Now;

            if (ID != null)
            {
                ViewBag.EmployeeName = db.EmpViews.First(aa => aa.EmployeeID == ID).FullName;
                ViewBag.Designation = db.EmpViews.First(aa => aa.EmployeeID == ID).DesignationName;
                ViewBag.Department = db.EmpViews.First(aa => aa.EmployeeID == ID).SectionName;
            }
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName");
            return View(jb);
        }

        // POST: /Attendance/EmployeeJobCard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobCardAppID,DateCreated,DateStarted,DateEnded,JCTypeID,UserID,JobCardCriteria,CriteriaData,Status,TimeIn,TimeOut,WorkMin,Remarks,OtherValue,SupervisorID,StartTime,EndTime,EmpID,Justification")] Att_JobCardApp att_jobcardapp)
        {
            if (att_jobcardapp.EmpID == null)
            {
                ViewBag.error = "required field!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (att_jobcardapp.DateEnded != null)
                    {
                        att_jobcardapp.DateCreated = DateTime.Now;
                        att_jobcardapp.StatusID = "Approved";
                        att_jobcardapp.StartTime = null;
                        att_jobcardapp.EndTime = null;
                        db.Att_JobCardApp.Add(att_jobcardapp);
                        db.SaveChanges();
                    }
                    else
                    {
                        att_jobcardapp.DateCreated = DateTime.Now;
                        att_jobcardapp.StatusID = "Approved";
                        db.Att_JobCardApp.Add(att_jobcardapp);
                        db.SaveChanges();
                        ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                        AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)att_jobcardapp.JobCardAppID);
                    }
                }
                ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
                return RedirectToAction("Index");
            }

            return View(att_jobcardapp);
        }

        // GET: /Attendance/EmployeeJobCard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_JobCardApp att_jobcardapp = db.Att_JobCardApp.Find(id);
            if (att_jobcardapp == null)
            {
                return HttpNotFound();
            }
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
            return View(att_jobcardapp);
        }

        // POST: /Attendance/EmployeeJobCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="JobCardAppID,DateCreated,DateStarted,DateEnded,JCTypeID,UserID,EmpID,Status,TimeBased,StartTime,EndTime,TotalMins,Remarks,SupervisorID,StatusID,ApprovedDate")] Att_JobCardApp att_jobcardapp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(att_jobcardapp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_jobcardapp.JobCardAppID);
                return RedirectToAction("Index");
            }
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
            return View(att_jobcardapp);
        }

        // GET: /Attendance/EmployeeJobCard/Delete/5
        public ActionResult Delete(int? id)
        {
            Att_JobCardApp att_jobcardapp = db.Att_JobCardApp.Find(id);
            db.Att_JobCardApp.Remove(att_jobcardapp);
            db.SaveChanges();
            ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
            AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Delete), DateTime.Now, (int)att_jobcardapp.JobCardAppID);
                
            return RedirectToAction("Index");
        }
     [HttpGet]
        public ActionResult SEmps(string sortOrder, string searchString, string currentFilter, int? page)
        {
            List<EmpView> emps = new List<EmpView>();
            emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();       
            return View(emps);
        }
        [HttpGet]
        public ActionResult GetID(int? ID)
        {
            Att_JobCardApp jb = new Att_JobCardApp();
            jb.DateStarted = DateTime.Today;
            jb.EmpID = ID;
            jb.StartTime = DateTime.Now;
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderBy(aa => aa.FullName).ToList(), "UserID", "FullName");
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName");
            return View("Create",jb);
        }        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateLoc()
        {
            VMJCApplication vmDemo = new VMJCApplication();
            vmDemo.LocEmployees = db.HR_Location.ToList();
            vmDemo.TypeEmployees = db.HR_EmpType.ToList();
            vmDemo.DeptEmployees = db.HR_Department.ToList();
            vmDemo.SecEmployees = db.HR_Section.ToList();
            vmDemo.ShiftEmployees = db.Att_Shift.ToList();
            ViewBag.JobCardType = new SelectList(db.Att_JobCard.OrderBy(aa => aa.JCName).ToList(), "JCID", "JCName");
            return View(vmDemo);
        }

        [HttpPost]
        public ActionResult SelectEmployee(FormCollection form)
        {
            try
            {
                string ErrorMessage = "";
                List<EmpView> emps = new List<EmpView>();
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
                List<EmpView> ViewEmps = new List<EmpView>();
                List<EmpView> tempEmps = new List<EmpView>();
                VMJCApplication vm = new VMJCApplication();
                ViewEmps = emps.ToList();
                vm.CardType = Request.Form["JobCardType"].ToString();
                vm.DateEnded = Convert.ToDateTime(Request.Form["JobDateTo"]);
                vm.JCValue = Convert.ToInt32(Request.Form["JCValue"]);
                vm.DateStarted = Convert.ToDateTime(Request.Form["JobDateFrom"]);
                if (Request.Form["SelectionRB"].ToString() == "rbAll")
                {
                    ViewEmps = emps.ToList();
                    vm.JCCriteria = "A";
                }
                else
                {
                    var checkedLoc = form.GetValues("cbLocation");
                    List<HR_Location> locs = new List<HR_Location>();
                    locs = db.HR_Location.ToList();
                    if (checkedLoc != null)
                    {
                        List<HR_Location> tempLocs = new List<HR_Location>();
                        foreach (var item in checkedLoc)
                        {
                            short id = Convert.ToInt16(item);
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.LocID == id).ToList());
                            tempLocs.Add(locs.First(aa => aa.LocID == id));
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.LocEmployees = tempLocs;
                        vm.JCCriteria = "L";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    //
                    var checkedType = form.GetValues("cbType");
                    List<HR_EmpType> types = new List<HR_EmpType>();
                    types = db.HR_EmpType.ToList();
                    if (checkedType != null)
                    {
                        List<HR_EmpType> tempTypes = new List<HR_EmpType>();
                        foreach (var item in checkedType)
                        {
                            short id = Convert.ToInt16(item);
                            string name = types.First(aa => aa.TypID == id).TypeName;
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.TypeName == name).ToList());
                            tempTypes.Add(types.First(aa => aa.TypID == id));
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.TypeEmployees = tempTypes;
                        vm.JCCriteria = "T";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    //
                    var checkedShift = form.GetValues("cbShift");
                    List<Att_Shift> shifts = new List<Att_Shift>();
                    shifts = db.Att_Shift.ToList();
                    if (checkedShift != null)
                    {
                        List<Att_Shift> tempShifts = new List<Att_Shift>();
                        tempEmps.Clear();
                        foreach (var item in checkedShift)
                        {
                            short id = Convert.ToInt16(item);
                            //string name = depts.First(aa => aa.DeptID == id).DeptID;
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.ShftID == id).ToList());
                            tempShifts.Add(shifts.First(aa => aa.ShftID == id));
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.ShiftEmployees = tempShifts;
                        vm.JCCriteria = "H";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                    }
                }
                foreach (var emp in tempEmps.ToList())
                {
                    Att_JobCardApp jobCardApp = new Att_JobCardApp();
                    jobCardApp.JCTypeID = Convert.ToInt16(vm.CardType);
                    jobCardApp.DateCreated = DateTime.Now;
                    jobCardApp.UserID = LoggedInUser.UserID;
                    jobCardApp.DateStarted = vm.DateStarted;
                    jobCardApp.DateEnded = vm.DateEnded;
                    jobCardApp.Status = false;
                    jobCardApp.EmpID = emp.EmployeeID;
                    jobCardApp.TimeBased = false;
                    if(vm.JCValue!=null)
                        jobCardApp.TotalMins = (short)vm.JCValue;
                    jobCardApp.Remarks = "ByApp";
                    jobCardApp.StatusID = "Approved";
                    jobCardApp.ApprovedDate = DateTime.Now;
                    db.Att_JobCardApp.Add(jobCardApp);

                }
                db.SaveChanges();
                if (ErrorMessage == "")
                    return View(vm);
                else
                {
                    VMJCApplication vmDemo = new VMJCApplication();
                    vmDemo.LocEmployees = db.HR_Location.ToList();
                    vmDemo.TypeEmployees = db.HR_EmpType.ToList();
                    vmDemo.DeptEmployees = db.HR_Department.ToList();
                    vmDemo.SecEmployees = db.HR_Section.ToList();
                    vmDemo.ShiftEmployees = db.Att_Shift.ToList();
                    ViewBag.JobCardType = new SelectList(db.Att_JobCard.OrderBy(aa => aa.JCName).ToList(), "JCID", "JCName");
                    return View("Index", vmDemo);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    //public class VMJCApplication
    //{
    //    public int JCAppID { get; set; }
    //    public Nullable<System.DateTime> DateStarted { get; set; }
    //    public Nullable<System.DateTime> DateEnded { get; set; }
    //    public string JCCriteria { get; set; }
    //    public string CardType { get; set; }
    //    public string CardName { get; set; }
    //    public int JCValue { get; set; }
    //    public List<HR_Location> LocEmployees { get; set; }
    //    public List<HR_EmpType> TypeEmployees { get; set; }
    //    public List<Att_Shift> ShiftEmployees { get; set; }
    //    public List<HR_Department> DeptEmployees { get; set; }
    //    public List<HR_Section> SecEmployees { get; set; }
    //}
}
