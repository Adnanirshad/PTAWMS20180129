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
using System.Configuration;
using PTAWMS.Areas.HumanResource.Models;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class JobCardController : Controller
    {
        private HRMEntities db = new HRMEntities();
        private static string BaseURL
        {
            get { return ConfigurationManager.AppSettings["BaseURL"].ToString(); }
        }

        // GET: /Attendance/JobCard/
        public ActionResult Index()
        {
            Session["SelectedMenu"] = "JobCardHistory";
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var att_jobcardapp = db.ViewJobCards.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).OrderByDescending(aa => aa.DateCreated);
            Utilities.DeleteNotificationByType(LoggedInUser.EmpID ?? 0, Convert.ToInt16(Utilities.NotificationType.Job_Card));
            return View(att_jobcardapp.ToList());
        }
        public ActionResult EmployeeJCList()
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var att_jobcardapp = db.ViewJobCards.Where(aa => aa.StatusID == "Approved");
            return View(att_jobcardapp.ToList());
        }
        // GET: /Attendance/JobCard/Details/5
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

        // GET: /Attendance/JobCard/Create
        public ActionResult Create()
        {

            Att_JobCardApp jb = new Att_JobCardApp();
            jb.DateStarted = DateTime.Today;
            jb.StartTime = DateTime.Now;
            jb.TimeBased = true;
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderBy(aa => aa.FullName).ToList(), "UserID", "FullName");
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName");
            return View(jb);
        }

        // POST: /Attendance/JobCard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobCardAppID,DateCreated,DateStarted,DateEnded,JCTypeID,UserID,EmpID,Status,TimeBased,StartTime,EndTime,TotalMins,Remarks,SupervisorID,StatusID,ApprovedDate")] Att_JobCardApp att_jobcardapp)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (att_jobcardapp.TimeBased == false)
            {
                if (att_jobcardapp.DateStarted >= DateTime.Today && att_jobcardapp.DateEnded >= att_jobcardapp.DateStarted)
                {
                    if (att_jobcardapp.DateEnded == null || att_jobcardapp.DateEnded == new DateTime(1, 1, 1))
                        ModelState.AddModelError("DateEnded", "End Date is required");
                    else
                    {
                        if (att_jobcardapp.DateStarted.Value > att_jobcardapp.DateEnded.Value)
                            ModelState.AddModelError("DateEnded", "End Date must be greater than Start Date");
                        // Check for Duplication
                        if (CheckForValidation(db.Att_JobCardApp.Where(aa => aa.EmpID == LoggedInUser.EmployeeID || aa.StatusID == "Approved" || aa.StatusID == "Pending").ToList(), att_jobcardapp))
                            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
                        ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderByDescending(aa => aa.FullName).ToList(), "UserID", "FullName");
                        ModelState.AddModelError("DateStarted", "You already have applied on this date");
                        return View(att_jobcardapp);
                    }
                }
                else
                {
                    ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
                    ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderByDescending(aa => aa.FullName).ToList(), "UserID", "FullName");
                    ModelState.AddModelError("DateStarted", "Date Must be Grater then today");
                    return View(att_jobcardapp);
                }
            }
            else
            {
                var AM = DateTime.Now.ToString("08:30:00");
                var PM = DateTime.Now.ToString("16:30:00");
                if (att_jobcardapp.StartTime >= Convert.ToDateTime(AM) && att_jobcardapp.EndTime <= Convert.ToDateTime(PM))
                {
                    if (att_jobcardapp.StartTime == null)
                        ModelState.AddModelError("StartTime", "Start Time is required");
                    if (att_jobcardapp.EndTime == null)
                        ModelState.AddModelError("StartTime", "Start Time is required");
                    if (att_jobcardapp.StartTime != null && att_jobcardapp.EndTime != null)
                    {
                        if (att_jobcardapp.StartTime.Value > att_jobcardapp.EndTime.Value)
                            ModelState.AddModelError("StartTime", "End Time must be greater than Start Time");
                        // Check for Duplication
                        if (db.Att_JobCardApp.Where(aa => aa.DateStarted == att_jobcardapp.DateStarted && aa.EmpID == LoggedInUser.EmployeeID && aa.StatusID != "Rejected").Count() > 0)
                            ModelState.AddModelError("DateStarted", "You already have applied on this date");
                    }
                }
                else
                {
                    ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
                    ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderByDescending(aa => aa.FullName).ToList(), "UserID", "FullName");
                    ViewBag.TimeError = "Incorrenct Time";
                    return View(att_jobcardapp);
                }
            }
            int RecordID = 0;
            if (ModelState.IsValid)
            {
                if (att_jobcardapp.DateEnded != null)
                {
                    att_jobcardapp.DateCreated = DateTime.Now;
                    att_jobcardapp.EmpID = LoggedInUser.EmpID;
                    att_jobcardapp.StartTime = null;
                    att_jobcardapp.EndTime = null;
                    att_jobcardapp.UserID = LoggedInUser.UserID;
                    att_jobcardapp.StatusID = "Pending";
                    db.Att_JobCardApp.Add(att_jobcardapp);
                    db.SaveChanges();
                    if (att_jobcardapp.StatusID == "Pending")
                    {
                        var EName = db.EmpViews.First(aa => aa.EmployeeID == att_jobcardapp.EmpID).FullName;
                        string Toadd = "awais.cns@gmail.com";
                        string subject = "Job Card Request" + EName;
                        string body = "Dear Sir/Madam, <br/> job card request was intiated of < br /> Date: " + att_jobcardapp.DateCreated + "+ < br />  , kindly review it. <br/> Employee Name: " + EName + "<br/> Employee No: " + att_jobcardapp.EmpID + "<br/>  Date Time: " + att_jobcardapp.DateCreated.Value.ToShortDateString() + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                        EmailManager.SendEmail(Toadd, subject, body);
                    }
                    RecordID = att_jobcardapp.JobCardAppID;
                    //return RedirectToAction("Index");
                }
                else
                {
                    att_jobcardapp.DateCreated = DateTime.Now;
                    att_jobcardapp.EmpID = LoggedInUser.EmpID;
                    att_jobcardapp.StatusID = "Pending";
                    att_jobcardapp.UserID = LoggedInUser.UserID;
                    db.Att_JobCardApp.Add(att_jobcardapp);
                    db.SaveChanges();
                    if (att_jobcardapp.StatusID == "Pending")
                    {
                        var EName = db.EmpViews.First(aa => aa.EmployeeID == att_jobcardapp.EmpID).FullName;
                        string Toadd = "awais.cns@gmail.com";
                        string subject = "Job Card Request" + EName;
                        string body = "Dear Sir/Madam, <br/> job card request was intiated of < br /> Time : " + att_jobcardapp.DateCreated + "+ < br /> , kindly review it. <br/> Employee Name: " + EName + "<br/> Employee No: " + att_jobcardapp.EmpID + "<br/>  Date Time: " + att_jobcardapp.DateCreated.Value.ToShortDateString() + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                        EmailManager.SendEmail(Toadd,subject, body);
                    }
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)att_jobcardapp.JobCardAppID);
                    RecordID = att_jobcardapp.JobCardAppID;
                    // Below Code is for Notification
                    int TypeID = Convert.ToInt16(Utilities.NotificationType.Pending_Job_Card);
                    string TypeName = Utilities.NotificationType.Pending_Job_Card.ToString().Replace("_", " ");
                    int EmployeeID = LoggedInUser.EmpID ?? 0;
                    Utilities.InsertEMPNotification(TypeID, TypeName, EmployeeID, RecordID, att_jobcardapp.SupervisorID ?? 0, BaseURL + "Attendance/PendingJobCards/Index");

                    return RedirectToAction("Index");
                }
            }

            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
            ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderByDescending(aa => aa.FullName).ToList(), "UserID", "FullName");
            //return View(att_jobcardapp);
            return RedirectToAction("Index");
        }

        private bool CheckForValidation(List<Att_JobCardApp> queryable, Att_JobCardApp att_jobcardapp)
        {
            bool check = false;
            foreach (var item in queryable)
            {
                DateTime dts = new DateTime();
                DateTime dte = new DateTime();
                if (item.TimeBased == true)
                {
                    dts = item.DateStarted.Value;
                    dte = item.DateStarted.Value;
                }
                else
                {
                    dts = item.DateStarted.Value;
                    dte = item.DateEnded.Value;
                }
                while (dts <= dte)
                {
                    if (dts == att_jobcardapp.DateStarted.Value || dts == att_jobcardapp.DateEnded.Value)
                    {
                        check = true;
                    }
                    dts = dts.AddDays(1);
                }
            }
            return check;
        }

        // GET: /Attendance/JobCard/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_JobCardApp att_jobcardapp = db.Att_JobCardApp.Find(id);
            if (att_jobcardapp == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderBy(aa => aa.FullName).ToList(), "UserID", "FullName");
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
            return View(att_jobcardapp);
        }

        // POST: /Attendance/JobCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobCardAppID,DateCreated,DateStarted,DateEnded,JCTypeID,UserID,EmpID,Status,TimeBased,StartTime,EndTime,TotalMins,Remarks,SupervisorID,StatusID,ApprovedDate")] Att_JobCardApp att_jobcardapp)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (ModelState.IsValid)
            {
                db.Entry(att_jobcardapp).State = System.Data.Entity.EntityState.Modified;
                att_jobcardapp.DateCreated = DateTime.Now;
                att_jobcardapp.EmpID = LoggedInUser.EmpID;
                att_jobcardapp.StatusID = "Pending";
                att_jobcardapp.SupervisorID = LoggedInUser.ReportingToID;
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_jobcardapp.JobCardAppID);

                return RedirectToAction("Index");
            }
            ViewBag.SupervisorID = new SelectList(db.ViewUserEmps.Where(aa => aa.DepartmentID == LoggedInUser.DepartmentID).OrderBy(aa => aa.FullName).ToList(), "UserID", "FullName");
            ViewBag.JCTypeID = new SelectList(db.Att_JobCard, "JCID", "JCName", att_jobcardapp.JCTypeID);
            return View(att_jobcardapp);
        }

        // GET: /Attendance/JobCard/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: /Attendance/JobCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Att_JobCardApp att_jobcardapp = db.Att_JobCardApp.Find(id);
            db.Att_JobCardApp.Remove(att_jobcardapp);
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
    }
}
