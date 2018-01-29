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
    public class OTDivController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/OTDiv/
        public ActionResult Index()
        {
            var bg_otdivision = db.ViewBGOTDivisions;
            return View(bg_otdivision.ToList());
        }

        // GET: /Attendance/OTDiv/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTDivision bg_otdivision = db.BG_OTDivision.Find(id);
            if (bg_otdivision == null)
            {
                return HttpNotFound();
            }
            return View(bg_otdivision);
        }

        // GET: /Attendance/OTDiv/Create
        public ActionResult Create()
        {
            ViewBag.DivID = new SelectList(db.HR_Department, "DeptID", "DepartmentName");
            ViewBag.FinYear = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName");
            return View();
        }

        // POST: /Attendance/OTDiv/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PBDivID,DivID,FinYear,TotalBudget,PaidBudget,RemainingBudget")] BG_OTDivision bg_otdivision)
        {          
            if (ModelState.IsValid)
            {            
                    db.BG_OTDivision.Add(bg_otdivision);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }

            ViewBag.DivID = new SelectList(db.HR_Department, "DeptID", "DepartmentName", bg_otdivision.DivID);
            ViewBag.FinYear = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otdivision.FinYear);
            return View(bg_otdivision);
        }

        // GET: /Attendance/OTDiv/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTDivision bg_otdivision = db.BG_OTDivision.Find(id);
            if (bg_otdivision == null)
            {
                return HttpNotFound();
            }
            ViewBag.DivID = new SelectList(db.HR_Department, "DeptID", "DepartmentName", bg_otdivision.DivID);
            ViewBag.FinYear = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otdivision.FinYear);
            return View(bg_otdivision);
        }

        // POST: /Attendance/OTDiv/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PBDivID,DivID,FinYear,TotalBudget,PaidBudget,RemainingBudget")] BG_OTDivision bg_otdivision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bg_otdivision).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DivID = new SelectList(db.HR_Department, "DeptID", "DepartmentName", bg_otdivision.DivID);
            ViewBag.FinYear = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otdivision.FinYear);
            return View(bg_otdivision);
        }

        // GET: /Attendance/OTDiv/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTDivision bg_otdivision = db.BG_OTDivision.Find(id);
            if (bg_otdivision == null)
            {
                return HttpNotFound();
            }
            return View(bg_otdivision);
        }

        // POST: /Attendance/OTDiv/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BG_OTDivision bg_otdivision = db.BG_OTDivision.Find(id);
            db.BG_OTDivision.Remove(bg_otdivision);
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
