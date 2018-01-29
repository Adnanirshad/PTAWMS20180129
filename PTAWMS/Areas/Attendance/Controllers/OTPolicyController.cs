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

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class OTPolicyController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/OTPolicy/
        public ActionResult Index()
        {
            return View(db.Att_OTPolicy.ToList());
        }

        // GET: /Attendance/OTPolicy/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_OTPolicy att_otpolicy = db.Att_OTPolicy.Find(id);
            if (att_otpolicy == null)
            {
                return HttpNotFound();
            }
            return View(att_otpolicy);
        }

        // GET: /Attendance/OTPolicy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Attendance/OTPolicy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OTPolicyID,OTPolicyName,Enable,CalculateNOT,CalculateGZOT,CalculateRestOT,PerDayOTStartLimitHour,PerDayOTEndLimitHour,PerDayROTStartLimitHour,PerDayROTEndLimitHour,PerDayGOTStartLimitHour,PerDayGOTEndLimitHour,MinMinutesForOneHour,DaysInMonth,DaysInWeek")] Att_OTPolicy att_otpolicy)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(att_otpolicy.OTPolicyName))
                {
                    ModelState.AddModelError("OTPolicyName", "OT Policy Name is required!");                   
                }
                else
                {
                    if(att_otpolicy.MinMinutesForOneHour ==null)
                        att_otpolicy.MinMinutesForOneHour = 0;
                    if (att_otpolicy.DaysInMonth == null)
                        att_otpolicy.DaysInMonth = 0;

                    if (att_otpolicy.DaysInWeek == null)
                        att_otpolicy.DaysInWeek = 0;

                    if (att_otpolicy.PerDayOTEndLimitHour == null)
                        att_otpolicy.PerDayOTEndLimitHour = 0;

                    if (att_otpolicy.PerDayOTStartLimitHour == null)
                        att_otpolicy.PerDayOTStartLimitHour = 0;


                    if (att_otpolicy.PerDayROTEndLimitHour == null)
                        att_otpolicy.PerDayROTEndLimitHour = 0;

                    if (att_otpolicy.PerDayROTStartLimitHour == null)
                        att_otpolicy.PerDayROTStartLimitHour = 0;

                    if (att_otpolicy.PerDayGOTStartLimitHour == null)
                        att_otpolicy.PerDayGOTStartLimitHour = 0;

                    if (att_otpolicy.PerDayGOTEndLimitHour == null)
                        att_otpolicy.PerDayGOTEndLimitHour = 0;
     
                    db.Att_OTPolicy.Add(att_otpolicy);
                    db.SaveChanges();
                    db.Att_OTPolicy.Add(att_otpolicy);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            return View(att_otpolicy);
        }

        // GET: /Attendance/OTPolicy/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_OTPolicy att_otpolicy = db.Att_OTPolicy.Find(id);
            if (att_otpolicy == null)
            {
                return HttpNotFound();
            }
            return View(att_otpolicy);
        }

        // POST: /Attendance/OTPolicy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OTPolicyID,OTPolicyName,Enable,CalculateNOT,CalculateGZOT,CalculateRestOT,PerDayOTStartLimitHour,PerDayOTEndLimitHour,PerDayROTStartLimitHour,PerDayROTEndLimitHour,PerDayGOTStartLimitHour,PerDayGOTEndLimitHour,MinMinutesForOneHour,DaysInMonth,DaysInWeek")] Att_OTPolicy att_otpolicy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(att_otpolicy).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(att_otpolicy);
        }

        // GET: /Attendance/OTPolicy/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_OTPolicy att_otpolicy = db.Att_OTPolicy.Find(id);
            if (att_otpolicy == null)
            {
                return HttpNotFound();
            }
            return View(att_otpolicy);
        }

        // POST: /Attendance/OTPolicy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Att_OTPolicy att_otpolicy = db.Att_OTPolicy.Find(id);
            db.Att_OTPolicy.Remove(att_otpolicy);
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
