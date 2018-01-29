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
    public class OTCreditController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/OTCredit/
        public ActionResult Index()
        {
            var bg_otcredit = db.ViewBGCredits;
            return View(bg_otcredit.ToList());
        }

        // GET: /Attendance/OTCredit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTCredit bg_otcredit = db.BG_OTCredit.Find(id);
            if (bg_otcredit == null)
            {
                return HttpNotFound();
            }
            return View(bg_otcredit);
        }

        // GET: /Attendance/OTCredit/Create
        public ActionResult Create()
        {
            ViewBag.BOTDivID = new SelectList(db.HR_Department, "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName");
            return View();
        }

        // POST: /Attendance/OTCredit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PCID,BOTDivID,FinYearID,Amount,Remarks")] BG_OTCredit bg_otcredit)
        {
            if (ModelState.IsValid)
            {
                if (db.BG_OTDivision.Where(aa => aa.DivID == bg_otcredit.BOTDivID && aa.FinYear == bg_otcredit.FinYearID).Count() > 0)
                {
                    BG_OTDivision bg = new BG_OTDivision();
                    bg = db.BG_OTDivision.First(aa => aa.DivID == bg_otcredit.BOTDivID && aa.FinYear == bg_otcredit.FinYearID);
                    bg.RemainingBudget = bg.RemainingBudget + bg_otcredit.Amount;
                    db.SaveChanges();
                }
                db.BG_OTCredit.Add(bg_otcredit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BOTDivID = new SelectList(db.BG_OTDivision, "PBDivID", "PBDivID", bg_otcredit.BOTDivID);
            ViewBag.FinYearID = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otcredit.FinYearID);
            return View(bg_otcredit);
        }

        // GET: /Attendance/OTCredit/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.BOTDivID = new SelectList(db.HR_Department.OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear.OrderBy(aa => aa.FinYearName).ToList(), "PFinYearID", "FinYearName");
            //ViewBag.BOTDivID = new SelectList(db.BG_OTDivision, "PBDivID", "PBDivID", bg_otcredit.BOTDivID);
            //ViewBag.FinYearID = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otcredit.FinYearID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTCredit bg_otcredit = db.BG_OTCredit.Find(id);          
            db.BG_OTCredit.Add(bg_otcredit);
            db.SaveChanges();
            if (bg_otcredit == null)
            {
                return HttpNotFound();
            }           
            return View(bg_otcredit);
        }

        // POST: /Attendance/OTCredit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PCID,BOTDivID,FinYearID,Amount,Remarks")] BG_OTCredit bg_otcredit)
        {
            ViewBag.BOTDivID = new SelectList(db.HR_Department.OrderBy(aa => aa.DepartmentName).ToList(), "DeptID", "DepartmentName");
            ViewBag.FinYearID = new SelectList(db.PR_FinYear.OrderBy(aa => aa.FinYearName).ToList(), "PFinYearID", "FinYearName");
            //ViewBag.BOTDivID = new SelectList(db.BG_OTDivision, "PBDivID", "PBDivID", bg_otcredit.BOTDivID);
            //ViewBag.FinYearID = new SelectList(db.PR_FinYear, "PFinYearID", "FinYearName", bg_otcredit.FinYearID);
            if (ModelState.IsValid)
            {
                if (db.BG_OTDivision.Where(aa => aa.DivID == bg_otcredit.BOTDivID && aa.FinYear == bg_otcredit.FinYearID).Count() > 0)
                {
                    BG_OTDivision bg = new BG_OTDivision();
                    bg = db.BG_OTDivision.First(aa => aa.DivID == bg_otcredit.BOTDivID && aa.FinYear == bg_otcredit.FinYearID);
                    bg.RemainingBudget = bg.RemainingBudget + bg_otcredit.Amount;
                    db.SaveChanges();
                }
                db.Entry(bg_otcredit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }           
            return View(bg_otcredit);
        }

        // GET: /Attendance/OTCredit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BG_OTCredit bg_otcredit = db.BG_OTCredit.Find(id);
            if (bg_otcredit == null)
            {
                return HttpNotFound();
            }
            return View(bg_otcredit);
        }

        // POST: /Attendance/OTCredit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BG_OTCredit bg_otcredit = db.BG_OTCredit.Find(id);
            db.BG_OTCredit.Remove(bg_otcredit);
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
