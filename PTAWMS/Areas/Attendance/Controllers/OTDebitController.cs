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
    public class OTDebitController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/OTDebit/
        public ActionResult Index()
        {
            var bg_otdebit = db.ViewBGOTDebits;
            return View(bg_otdebit.ToList());
        }

        // GET: /Attendance/OTDebit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTDebit bg_otdebit = db.BG_OTDebit.Find(id);
            if (bg_otdebit == null)
            {
                return HttpNotFound();
            }
            return View(bg_otdebit);
        }

        // GET: /Attendance/OTDebit/Create
        public ActionResult Create()
        {
            ViewBag.BOTDivID = new SelectList(db.HR_Department.OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear.OrderBy(aa => aa.FinYearName).ToList(), "PFinYearID", "FinYearName");
            return View();
        }

        // POST: /Attendance/OTDebit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PDID,BOTDivID,FinYearID,Amount,Remarks")] BG_OTDebit bg_otdebit)
        {
            if (ModelState.IsValid)
            {
                if (db.BG_OTDivision.Where(aa => aa.DivID == bg_otdebit.BOTDivID && aa.FinYear == bg_otdebit.FinYearID).Count() > 0)
                {
                    BG_OTDivision bg = new BG_OTDivision();
                    bg = db.BG_OTDivision.First(aa => aa.DivID == bg_otdebit.BOTDivID && aa.FinYear == bg_otdebit.FinYearID);
                    bg.RemainingBudget = bg.RemainingBudget - bg_otdebit.Amount;
                    db.SaveChanges();
                }
                db.BG_OTDebit.Add(bg_otdebit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BOTDivID = new SelectList(db.HR_Department.OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear.OrderBy(aa => aa.FinYearName).ToList(), "PFinYearID", "FinYearName");
            return View(bg_otdebit);
        }

        // GET: /Attendance/OTDebit/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.BOTDivID = new SelectList(db.HR_Department.OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear.OrderBy(aa => aa.FinYearName).ToList(), "PFinYearID", "FinYearName");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTDebit bg_otdebit = db.BG_OTDebit.Find(id);           
            if (bg_otdebit == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BOTDivID = new SelectList(db.BG_OTDivision, "PBDivID", "PBDivID", bg_otdebit.BOTDivID);
            //ViewBag.FinYearID = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otdebit.FinYearID);
            return View(bg_otdebit);
        }

        // POST: /Attendance/OTDebit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PDID,BOTDivID,FinYearID,Amount,Remarks")] BG_OTDebit bg_otdebit)
        {
            ViewBag.BOTDivID = new SelectList(db.HR_Department.OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear.OrderBy(aa => aa.FinYearName).ToList(), "PFinYearID", "FinYearName");
            if (ModelState.IsValid)
            {
                if (db.BG_OTDivision.Where(aa => aa.DivID == bg_otdebit.BOTDivID && aa.FinYear == bg_otdebit.FinYearID).Count() > 0)
                {
                    BG_OTDivision bg = new BG_OTDivision();
                    bg = db.BG_OTDivision.First(aa => aa.DivID == bg_otdebit.BOTDivID && aa.FinYear == bg_otdebit.FinYearID);
                    bg.RemainingBudget = bg.RemainingBudget - bg_otdebit.Amount;
                    db.SaveChanges();
                }
                db.Entry(bg_otdebit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.BOTDivID = new SelectList(db.BG_OTDivision, "PBDivID", "PBDivID", bg_otdebit.BOTDivID);
           // ViewBag.FinYearID = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otdebit.FinYearID);
            return View(bg_otdebit);
        }

        // GET: /Attendance/OTDebit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTDebit bg_otdebit = db.BG_OTDebit.Find(id);
            if (bg_otdebit == null)
            {
                return HttpNotFound();
            }
            return View(bg_otdebit);
        }

        // POST: /Attendance/OTDebit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BG_OTDebit bg_otdebit = db.BG_OTDebit.Find(id);
            db.BG_OTDebit.Remove(bg_otdebit);
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
