using HRM_IKAN.Authentication;
using PTAWMS.App_Start;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class PendingJobCardsController : Controller
    {
        //
        // GET: /Attendance/PendingJobCards/
        HRMEntities db = new HRMEntities();
        private static string BaseURL
        {
            get { return ConfigurationManager.AppSettings["BaseURL"].ToString(); }
        }
        public ActionResult Index()
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var att_jobcardapp = db.ViewJobCards.Where(aa => aa.SupervisorID == LoggedInUser.EmpID && aa.StatusID == "Pending").OrderByDescending(aa => aa.DateCreated);
            return View(att_jobcardapp);
        }

        public ActionResult Approved(int? id)
        {
            Att_JobCardApp job = new Att_JobCardApp();
            job = db.Att_JobCardApp.First(aa => aa.JobCardAppID == id);
            if (job != null)
            {
                job.StatusID = "Approved";
                job.ApprovedDate = DateTime.Now;
                db.SaveChanges();
                if (job.StatusID == "Approved")
                {
                    var EName = db.EmpViews.First(aa => aa.EmployeeID == job.EmpID).FullName;
                    string Toadd = "awais.cns@gmail.com";
                    string subject = "Job Card Approval/Rejection" + EName;
                    string body = "Dear Sir/Madam, <br/> job card request was Approved of < br /> Date : " + job.DateCreated + "+ < br /> , kindly review it. <br/> Employee Name: " + EName + "<br/> Employee No: " + job.EmpID + "<br/>  Date Time: " + job.DateCreated.Value.ToShortDateString() + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                    EmailManager.SendEmail(Toadd, subject, body);
                }
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Approved), DateTime.Now, (int)id);
                // Below Code is for Notification
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Job_Card);
                string TypeName = Utilities.NotificationType.Job_Card.ToString().Replace("_", " ");
                int EmployeeID = LoggedInUser.EmpID ?? 0;
                Utilities.DeleteNotification(job.JobCardAppID, Convert.ToInt16(Utilities.NotificationType.Pending_Job_Card));
                Utilities.InsertEMPNotification(TypeID, TypeName, EmployeeID, id ?? 0, job.EmpID ?? 0, BaseURL + "Attendance/JobCard/Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Cancel(int? id)
        {
            Att_JobCardApp job = new Att_JobCardApp();
            job = db.Att_JobCardApp.First(aa => aa.JobCardAppID == id);
            job.StatusID = "Rejected";
            job.ApprovedDate = DateTime.Now;
            db.SaveChanges();
            if (job.StatusID == "Rejected")
            {
                var EName = db.EmpViews.First(aa => aa.EmployeeID == job.EmpID).FullName;
                string Toadd = "awais.cns@gmail.com";
                string subject = "Job Card Approval/Rejection" + EName;
                string body = "Dear Sir/Madam, <br/> job card request was Rejected < br /> Date : " + job.DateCreated + "+ < br /> , kindly review it. <br/> Employee Name: " + EName + "<br/> Employee No: " + job.EmpID + "<br/>  Date Time: " + job.DateCreated.Value.ToShortDateString() + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                EmailManager.SendEmail(Toadd,subject, body);
            }
            ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
            AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Job_Cards), Convert.ToInt16(AuditManager.AuditOperation.Reject), DateTime.Now, (int)id);

            int RecordID = job.JobCardAppID;
            // Below Code is for Notification
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            int TypeID = Convert.ToInt16(Utilities.NotificationType.Job_Card);
            string TypeName = Utilities.NotificationType.Job_Card.ToString().Replace("_", " ");
            int EmployeeID = LoggedInUser.EmpID ?? 0;
            Utilities.DeleteNotification(job.JobCardAppID, Convert.ToInt16(Utilities.NotificationType.Pending_Job_Card));
            Utilities.InsertEMPNotification(TypeID, TypeName, EmployeeID, id ?? 0, job.EmpID ?? 0, BaseURL + "Attendance/JobCard/Index");
            return RedirectToAction("Index");
        }
        public ActionResult CheckExpireEntries()
        {
            List<Att_JobCardApp> jb = new List<Att_JobCardApp>();
            jb = db.Att_JobCardApp.Where(aa => aa.StatusID == "Pending").ToList();
            foreach (var item in jb)
            {
                DateTime CurrentTime = DateTime.Today;
                if (item.DateStarted < DateTime.Today)
                {
                    item.StatusID = "Rejected";
                    db.SaveChanges();
                }
            }
            return View();
        }
    }
    public class ModelPendingJobCarads
    {
        public int PJobCardAppID { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string Criteria { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string JobCardName { get; set; }
        public string Status { get; set; }
        public string SupervisorName { get; set; }
    }
}