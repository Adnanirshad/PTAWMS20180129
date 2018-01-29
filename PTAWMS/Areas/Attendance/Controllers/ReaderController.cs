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

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class ReaderController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/Reader/
        public ActionResult Index()
        {
            return View(db.Att_Reader.ToList());
        }

        // GET: /Attendance/Reader/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_Reader att_reader = db.Att_Reader.Find(id);
                if (att_reader == null)
                {
                    return HttpNotFound();
                }
                return View(att_reader);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/Reader/Create
        public ActionResult Create()
        {
            ViewBag.DutyCode = new SelectList(db.Att_ReaderDutyCode, "RdrDuty", "DutyName");
            ViewBag.ReaderType = new SelectList(db.Att_ReaderType, "RdrTypeID", "RdrTypeName");
            ViewBag.Location = new SelectList(db.HR_Location, "LocID", "LocationName");
            return View();
        }

        // POST: /Attendance/Reader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RdrID,RdrName,ReaderDutyCode,IpAdd,IpPort,RdrTypeID,Status,LocID")] Att_Reader att_reader)
        {
            //att_shift.GZDays = (bool)ValueProvider.GetValue("GZDays").ConvertTo(typeof(bool)); 
            ViewBag.Location = new SelectList(db.HR_Location, "LocID", "LocationName");
            ViewBag.DutyCode = new SelectList(db.Att_ReaderDutyCode, "RdrDuty", "DutyName");
            ViewBag.ReaderType = new SelectList(db.Att_ReaderType, "RdrTypeID", "RdrTypeName");
            try
            {
                if (string.IsNullOrEmpty(att_reader.IpAdd))
                {
                    ModelState.AddModelError("IpAdd", "Ip Address is required!");
                }
                else
                {
                    att_reader.ClearRecords = true;
                    if (ModelState.IsValid)
                    {

                        db.Att_Reader.Add(att_reader);
                        db.SaveChanges();
                        ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                        AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Device), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)att_reader.RdrID);                   
                        return RedirectToAction("Index");
                    }

                    return View(att_reader);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
    

        // GET: /Attendance/Reader/Edit/5
        public ActionResult Edit(short? id)
        {
            ViewBag.ReaderType = new SelectList(db.Att_ReaderType, "RdrTypeID", "RdrTypeName");
            ViewBag.Location = new SelectList(db.HR_Location, "LocID", "LocationName");
            ViewBag.DutyCode = new SelectList(db.Att_ReaderDutyCode, "RdrDuty", "DutyName");
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_Reader att_reader = db.Att_Reader.Find(id);
                if (att_reader == null)
                {
                    return HttpNotFound();
                }
                return View(att_reader);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/Reader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RdrID,RdrName,ReaderDutyCode,IpAdd,IpPort,RdrTypeID,Status,LocID")] Att_Reader att_reader)
        {
            ViewBag.ReaderType = new SelectList(db.Att_ReaderType, "RdrTypeID", "RdrTypeName");
            ViewBag.Location = new SelectList(db.HR_Location, "LocID", "LocationName");
            ViewBag.DutyCode = new SelectList(db.Att_ReaderDutyCode, "RdrDuty", "DutyName");
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(att_reader).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Device), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_reader.RdrID);                   
                        
                    return RedirectToAction("Index");
                }
                return View(att_reader);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/Reader/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_Reader att_reader = db.Att_Reader.Find(id);
                if (att_reader == null)
                {
                    return HttpNotFound();
                }
                return View(att_reader);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/Reader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                Att_Reader att_reader = db.Att_Reader.Find(id);
                db.Att_Reader.Remove(att_reader);
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Device), Convert.ToInt16(AuditManager.AuditOperation.Delete), DateTime.Now, (int)att_reader.RdrID);                   
                   
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
    }
}
