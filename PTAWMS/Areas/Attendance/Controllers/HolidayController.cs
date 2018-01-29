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
    public class HolidayController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/Holiday/
        public ActionResult Index()
        {
            return View(db.Att_Holiday.ToList());
        }

        // GET: /Attendance/Holiday/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_Holiday att_holiday = db.Att_Holiday.Find(id);
            if (att_holiday == null)
            {
                return HttpNotFound();
            }
            return View(att_holiday);
        }

        // GET: /Attendance/Holiday/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Attendance/Holiday/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="HolidayID,HolidayDate,HolidayName")] Att_Holiday att_holiday)
        {
            if (att_holiday.HolidayDate == null)
            {
                ViewBag.date = "required field!";
            }
                if(att_holiday.HolidayName==null || att_holiday.HolidayName == "")
                    ViewBag.name = "required field!";
            else
            {
                if (ModelState.IsValid)
                {
                    db.Att_Holiday.Add(att_holiday);
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Holiday), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)att_holiday.HolidayID);
                    return RedirectToAction("Index");
                }
            }
                return View(att_holiday);
            
        }

        // GET: /Attendance/Holiday/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_Holiday att_holiday = db.Att_Holiday.Find(id);
            if (att_holiday == null)
            {
                return HttpNotFound();
            }
            return View(att_holiday);
        }

        // POST: /Attendance/Holiday/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="HolidayID,HolidayDate,HolidayName")] Att_Holiday att_holiday)
        {
            if (att_holiday.HolidayDate == null)
            {
                ViewBag.date = "required field!";
            }             
            
            else
            {
                if (att_holiday.HolidayName == null || att_holiday.HolidayName == "")
                    ViewBag.name = "required field!"; 
                else{
                    if (ModelState.IsValid)
                    {
                        db.Entry(att_holiday).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                        AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Holiday), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_holiday.HolidayID);
                    
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(att_holiday);
        }

        // GET: /Attendance/Holiday/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_Holiday att_holiday = db.Att_Holiday.Find(id);
            if (att_holiday == null)
            {
                return HttpNotFound();
            }
            return View(att_holiday);
        }

        // POST: /Attendance/Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Att_Holiday att_holiday = db.Att_Holiday.Find(id);
            db.Att_Holiday.Remove(att_holiday);
            db.SaveChanges();
            ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
            AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Holiday), Convert.ToInt16(AuditManager.AuditOperation.Delete), DateTime.Now, (int)att_holiday.HolidayID);
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
