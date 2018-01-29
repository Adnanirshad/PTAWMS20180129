using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using HRM_IKAN.Authentication;
using PTAWMS.App_Start;
using System.Configuration;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class ScheduleVisitorController : Controller
    {
        private HRMEntities db = new HRMEntities();
        private static string BaseURL
        {
            get { return ConfigurationManager.AppSettings["BaseURL"].ToString(); }
        }
        // GET: /Attendance/ScheduleVisitor/
        public ActionResult Index()
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            Utilities.DeleteNotificationByType(LoggedInUser.EmpID ?? 0, Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry));
            return View(db.VMS_SVisitor.Where(aa => aa.EmpID == LoggedInUser.EmpID).OrderByDescending(aa => aa.ID).ToList());
        }
        public ActionResult ListOfPendingVisitor()
        {
            Session["SelectedMenu"] = "PendingVRequest";
            return View(db.ViewSVisitorEmps.ToList());
        }
        [HttpGet]
        public ActionResult ApprovedVisitor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMS_SVisitor vms_svisitor = db.VMS_SVisitor.Find(id);
            if (vms_svisitor != null)
            {
                vms_svisitor.Status = "Approved";
                vms_svisitor.ApprovedDate = DateTime.Today;
                db.SaveChanges();
                if (vms_svisitor.Status == "Approved")
                {
                    var EName = db.EmpViews.First(aa => aa.EmployeeID == vms_svisitor.EmpID).FullName;
                    string Toadd = "madnan.cns@gmail.com";
                    string subject = "Pending Visitor's Vehicle Access for Employee" + EName;
                    string body = "Dear Concerned, <br/> your request is approved <br/> Employee Name: " + EName + "<br/> Visitor Name: " + vms_svisitor.VisitorName + "<br/> Vehicle No: " + vms_svisitor.VehicleNo + "<br/> Date Time: " + vms_svisitor.Arrival_Date.Value.ToShortDateString() + vms_svisitor.ArrivalTime + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                    EmailManager.SendEmail(Toadd, subject, body);
                }
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int TypeID = Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry);
                string TypeName = Utilities.NotificationType.PendingVisitorEntry.ToString().Replace("_", " ");
                int EmployeeID = LoggedInUser.EmpID ?? 0;
                Utilities.DeleteNotification(vms_svisitor.ID, Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry));
                Utilities.InsertEMPNotification(TypeID, TypeName, EmployeeID, id ?? 0, vms_svisitor.EmpID ?? 0, BaseURL + "Attendance/ScheduleVisitor/Index");
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Visitor_Entry), Convert.ToInt16(AuditManager.AuditOperation.Approved), DateTime.Now, (int)id);
                return RedirectToAction("ListOfPendingVisitor");
            }
            return RedirectToAction("ListOfPendingVisitor");
        }
        public ActionResult Rejected(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMS_SVisitor vms_svisitor = db.VMS_SVisitor.Find(id);
            if (vms_svisitor != null)
            {
                vms_svisitor.Status = "Rejected";
                db.SaveChanges();
                if (vms_svisitor.Status == "Rejected")
                {
                    var EName = db.EmpViews.First(aa => aa.EmployeeID == vms_svisitor.EmpID).FullName;
                    string Toadd = "madnan.cns@gmail.com";
                    string subject = "Pending Visitor's Vehicle Access for Employee" + EName;
                    string body = "Dear Concerned, <br/> your request is Rejected. <br/> Employee Name: " + EName + "<br/> Visitor Name: " + vms_svisitor.VisitorName + "<br/> Vehicle No: " + vms_svisitor.VehicleNo + "<br/> Date Time: " + vms_svisitor.Arrival_Date.Value.ToShortDateString() + vms_svisitor.ArrivalTime + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                    EmailManager.SendEmail(Toadd, subject, body);
                }
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int TypeID = Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry);
                string TypeName = Utilities.NotificationType.PendingVisitorEntry.ToString().Replace("_", " ");
                int EmployeeID = LoggedInUser.EmpID ?? 0;
                Utilities.DeleteNotification(vms_svisitor.ID, Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry));
                Utilities.InsertEMPNotification(TypeID, TypeName, EmployeeID, id ?? 0, vms_svisitor.EmpID ?? 0, BaseURL + "Attendance/ScheduleVisitor/Index");

                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Visitor_Entry), Convert.ToInt16(AuditManager.AuditOperation.Reject), DateTime.Now, (int)id);
                return RedirectToAction("ListOfPendingVisitor");
            }
            return RedirectToAction("ListOfPendingVisitor");
        }
        public ActionResult CheckVisitorExpireEntries()
        {
            List<VMS_SVisitor> sv = new List<VMS_SVisitor>();
            if (db.VMS_SVisitor.Where(aa => aa.Status == "Pending").ToList().Count > 0)
            {
                sv = db.VMS_SVisitor.Where(aa => aa.Status == "Pending").ToList();
                foreach (var item in sv)
                {
                    DateTime CurrentTime = DateTime.Today;
                    DateTime Time = DateTime.Now;
                    if (item.Arrival_Date <= DateTime.Today)
                    {
                        if (item.ArrivalTime < Time)
                        {
                            item.Status = "Rejected";
                            db.SaveChanges();
                            //var EName = db.EmpViews.First(aa => aa.EmployeeID == item.EmpID).FullName;
                            var EName = item.EmpID;
                            string Toadd = "madnan.cns@gmail.com";
                            string subject = "Pending Visitor's Vehicle Access for Employee" + EName;
                            string body = "Dear Concerned, <br/> your pending visitor request has been rejected for time expire . <br/> Employee Name: " + EName + "<br/> Visitor Name: " + item.VisitorName + "<br/> Vehicle No: " + item.VehicleNo + "<br/> Date Time: " + item.Arrival_Date.Value.ToShortDateString() + item.ArrivalTime + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                            EmailManager.SendEmail(Toadd, subject, body);
                        }
                    }
                }
            }
            return View();
        }
        // GET: /Attendance/ScheduleVisitor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMS_SVisitor vms_svisitor = db.VMS_SVisitor.Find(id);
            if (vms_svisitor == null)
            {
                return HttpNotFound();
            }
            return View(vms_svisitor);
        }

        // GET: /Attendance/ScheduleVisitor/Create
        public ActionResult Create()
        {
            ViewBag.VisitTypeID = new SelectList(db.VMS_VisitType, "PVisitTypeID", "PVisitTypeName");
            ViewBag.ReasonID = new SelectList(db.VMS_Reason, "PReasonID", "ReasonName");
            VMS_SVisitor vs = new VMS_SVisitor();
            vs.Arrival_Date = DateTime.Today;
            return View(vs);
        }

        // POST: /Attendance/ScheduleVisitor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpID,VisitorName,Company,VehicleAccess,VehicleNo,CDateTime,ApprovedDate,Status,ArrivalTime,Arrival_Date,VisitTypeID,ReasonID,Remarks")] VMS_SVisitor vms_svisitor)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (CheckForValidations(db.VMS_SVisitor.Where(aa => aa.EmpID == LoggedInUser.EmployeeID || aa.Status == "Approved" || aa.Status == "Pending").ToList(), vms_svisitor))
            {
                ViewBag.VisitTypeID = new SelectList(db.VMS_VisitType, "PVisitTypeID", "PVisitTypeName");
                ViewBag.ReasonID = new SelectList(db.VMS_Reason, "PReasonID", "ReasonName");
                ModelState.AddModelError("DateStarted", "You already have applied on this date");
                return View(vms_svisitor);
            }
            else
            {
                var AM = DateTime.Now.ToString("08:30:00");
                var PM = DateTime.Now.ToString("16:30:00");
                if (vms_svisitor.ArrivalTime >= Convert.ToDateTime(AM) && vms_svisitor.ArrivalTime <= Convert.ToDateTime(PM))
                {
                    DateTime CurrentTime = DateTime.Today;
                    DateTime time = DateTime.Now;
                    if (vms_svisitor.ArrivalTime >= time)
                    {

                        VMS_SVisitor obj = new VMS_SVisitor();
                        List<VMS_SVisitor> vms = db.VMS_SVisitor.Where(aa => aa.VisitorName == obj.VisitorName && aa.Arrival_Date == obj.Arrival_Date && aa.Company == obj.Company).ToList();
                        if (vms.Count() > 0)
                            ModelState.AddModelError("VisitorName", "Visitor name must be unique");
                        int RecordID = 0;
                        //var time = DateTime.Now.TimeOfDay;
                        //vms_svisitor.ArrivalTime = time;
                        //if (ModelState.IsValid)
                        {

                            DateTime date = DateTime.Today;
                            if (vms_svisitor.Arrival_Date >= date)
                            {
                                // ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                                if (string.IsNullOrEmpty(vms_svisitor.VisitorName))
                                {
                                    ModelState.AddModelError("VisitorName", "Visitor Name is required!");
                                }
                                else
                                {
                                    vms_svisitor.EmpID = LoggedInUser.EmployeeID;
                                    if (vms_svisitor.VehicleAccess == "true")
                                    {
                                        vms_svisitor.Status = "Pending";
                                    }
                                    else
                                    {
                                        vms_svisitor.Status = "Approved";
                                    }
                                    if (LoggedInUser.UserType == "E")
                                    {
                                        vms_svisitor.Status = "Approved";
                                    }
                                    //ViewBag.VisitTypeID = new SelectList(db.VMS_VisitType, "PVisitTypeID", "PVisitTypeName", vms_svisitor.VisitTypeID);
                                    //ViewBag.ReasonID = new SelectList(db.VMS_Reason, "PReasonID", "ReasonName", vms_svisitor.ReasonID);
                                    if (ModelState.IsValid)
                                    {
                                        vms_svisitor.CDateTime = DateTime.Now;
                                        db.VMS_SVisitor.Add(vms_svisitor);
                                        db.SaveChanges();
                                        if (vms_svisitor.VehicleAccess == "true")
                                        {
                                            var EName = db.EmpViews.First(aa => aa.EmployeeID == vms_svisitor.EmpID).FullName;
                                            string Toadd = "madnan.cns@gmail.com";
                                            string subject = "Pending Visitor's Vehicle Access for Employee" + EName;
                                            string body = "Dear Sir/Madam, <br/> A new visitor entry is awaited for your approval. Details are Listed below <br/> Employee Name: " + EName + "<br/> Visitor Name: " + vms_svisitor.VisitorName + "<br/> Vehicle No: " + vms_svisitor.VehicleNo + "<br/> Date Time: " + vms_svisitor.Arrival_Date.Value.ToShortDateString() + vms_svisitor.ArrivalTime + "<br/> Please Click on below link to procced further <a href='http://192.168.0.21/wms'>http://192.168.0.21/ESSP</a>";
                                            EmailManager.SendEmail(Toadd, subject, body);
                                        }
                                        RecordID = vms_svisitor.ID;
                                        int TypeID = Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry);
                                        string TypeName = Utilities.NotificationType.PendingVisitorEntry.ToString().Replace("_", " ");
                                        int EmployeeID = LoggedInUser.EmpID ?? 0;
                                        int Sid = 13479;
                                        Utilities.InsertEMPNotification(TypeID, TypeName, EmployeeID, RecordID, Sid, BaseURL + "Attendance/ScheduleVisitor/ListOfPendingVisitor");
                                        ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                                        AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Visitor_Entry), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)vms_svisitor.ID);
                                        if (vms_svisitor.VehicleAccess == "true")
                                        {
                                            string Subject = "Pending Visitor's Vehicle Access";
                                            string Body = "";
                                        }
                                    }
                                    return RedirectToAction("Index");
                                }
                            }
                            else
                            {

                                ViewBag.Date = "can not insert previous date";
                            }

                        }

                        ViewBag.VisitTypeID = new SelectList(db.VMS_VisitType, "PVisitTypeID", "PVisitTypeName");
                        ViewBag.ReasonID = new SelectList(db.VMS_Reason, "PReasonID", "ReasonName");
                        return View(vms_svisitor);
                    }
                    else
                    {
                        ViewBag.VisitTypeID = new SelectList(db.VMS_VisitType, "PVisitTypeID", "PVisitTypeName");
                        ViewBag.ReasonID = new SelectList(db.VMS_Reason, "PReasonID", "ReasonName");
                        ViewBag.TimeError = "Time increment must be greater than current time";
                        return View(vms_svisitor);
                    }
                }
                else
                {
                    ViewBag.VisitTypeID = new SelectList(db.VMS_VisitType, "PVisitTypeID", "PVisitTypeName");
                    ViewBag.ReasonID = new SelectList(db.VMS_Reason, "PReasonID", "ReasonName");
                    ViewBag.TimeError = "Incorrenct Time";
                    return View(vms_svisitor);
                }
            }
        }
        private bool CheckForValidations(List<VMS_SVisitor> queryable, VMS_SVisitor vms_svisitor)
        {
            bool check = false;
            foreach (var item in queryable)
            {
                DateTime dts = new DateTime();
                DateTime dte = new DateTime();
                while (dts <= dte)
                {
                    if (dts == vms_svisitor.Arrival_Date.Value)
                    {
                        check = true;
                    }
                    dts = dts.AddDays(1);
                }
            }
            return check;
        }
        // GET: /Attendance/ScheduleVisitor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMS_SVisitor vms_svisitor = db.VMS_SVisitor.Find(id);
            if (vms_svisitor == null)
            {
                return HttpNotFound();
            }
            return View(vms_svisitor);
        }

        // POST: /Attendance/ScheduleVisitor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpID,VisitorName,Company,VehicleAccess,VehicleNo,CDateTime,ApprovedDate,Status,Arrival_Date")] VMS_SVisitor vms_svisitor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vms_svisitor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Visitor_Entry), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)vms_svisitor.ID);

                return RedirectToAction("Index");
            }
            return View(vms_svisitor);
        }

        // GET: /Attendance/ScheduleVisitor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMS_SVisitor vms_svisitor = db.VMS_SVisitor.Find(id);
            if (vms_svisitor == null)
            {
                return HttpNotFound();
            }
            return View(vms_svisitor);
        }

        // POST: /Attendance/ScheduleVisitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VMS_SVisitor vms_svisitor = db.VMS_SVisitor.Find(id);
            db.VMS_SVisitor.Remove(vms_svisitor);
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
