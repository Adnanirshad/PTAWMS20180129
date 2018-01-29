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
    public class SettingsController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/Settings/
        public ActionResult Index()
        {
            return RedirectToAction("Edit", new { id=1});
        }

        // GET: /Attendance/Settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_Setting att_setting = db.Att_Setting.Find(id);
            if (att_setting == null)
            {
                return HttpNotFound();
            }
            return View(att_setting);
        }

        // GET: /Attendance/Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Attendance/Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SupervisorCutOffDate,RecommendCutOffDate,ApprovedCutOffDate")] Att_Setting att_setting)
        {
            if (ModelState.IsValid)
            {
                db.Att_Setting.Add(att_setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(att_setting);
        }

        // GET: /Attendance/Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_Setting att_setting = db.Att_Setting.Find(id);
            if (att_setting == null)
            {
                return HttpNotFound();
            }
            return View(att_setting);
        }

        // POST: /Attendance/Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SupervisorCutOffDate,RecommendCutOffDate,ApprovedCutOffDate")] Att_Setting att_setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(att_setting).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Att_Setting), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_setting.ID); 
                return RedirectToAction("Index");
            }
            return View(att_setting);
        }

        // GET: /Attendance/Settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Att_Setting att_setting = db.Att_Setting.Find(id);
            if (att_setting == null)
            {
                return HttpNotFound();
            }
            return View(att_setting);
        }

        // POST: /Attendance/Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Att_Setting att_setting = db.Att_Setting.Find(id);
            db.Att_Setting.Remove(att_setting);
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
