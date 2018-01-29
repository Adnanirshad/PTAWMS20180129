using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using PagedList;
using PTAWMS.App_Start;

namespace PTAWMS.Areas.Attendance.Controllers
{
    public class FinancialYearController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/FinancialYear/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(db.PR_FinYear.OrderByDescending(aa => aa.EndDate).ToPagedList(pageNumber, pageSize));
        }

        // GET: /Attendance/FinancialYear/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_FinYear pr_finyear = db.PR_FinYear.Find(id);
            if (pr_finyear == null)
            {
                return HttpNotFound();
            }
            return View(pr_finyear);
        }

        // GET: /Attendance/FinancialYear/Create
        public ActionResult Create()
        {
            PR_FinYear pr_finyear = new PR_FinYear();
            pr_finyear.StartDate = new DateTime(DateTime.Today.Year,07,1);
            pr_finyear.EndDate = new DateTime(DateTime.Today.Year+1, 06, 30);
            return View();
        }

        // POST: /Attendance/FinancialYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FinYearName,StartDate,EndDate,Status")] PR_FinYear pr_finyear)
        {
            if (ModelState.IsValid)
            {
                db.PR_FinYear.Add(pr_finyear);
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.FinYear), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)pr_finyear.PFinYearID);
                return RedirectToAction("Index");
            }

            return View(pr_finyear);
        }
        private void CreateNewPayrollPeriod(PR_FinYear pr_financialyear)
        {
            //DateTime cdt = pr_financialyear.StartDate.Value;
            //int cMonth = 1;
            //while (cdt <= pr_financialyear.EndDate)
            //{
            //    if(cdt.Month)
            //    cdt = cdt.AddDays(1);
            //}
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.StartDate.Value.Year, 7, 1);
                prp.PEndDate = new DateTime(pr_financialyear.StartDate.Value.Year, 7, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.StartDate.Value.Year, 8, 1);
                prp.PEndDate = new DateTime(pr_financialyear.StartDate.Value.Year, 8, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.StartDate.Value.Year, 9, 1);
                prp.PEndDate = new DateTime(pr_financialyear.StartDate.Value.Year, 9, 30);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.StartDate.Value.Year, 10, 1);
                prp.PEndDate = new DateTime(pr_financialyear.StartDate.Value.Year, 10, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.StartDate.Value.Year, 11, 1);
                prp.PEndDate = new DateTime(pr_financialyear.StartDate.Value.Year, 11, 30);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.StartDate.Value.Year, 12, 1);
                prp.PEndDate = new DateTime(pr_financialyear.StartDate.Value.Year, 12, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.EndDate.Value.Year, 1, 1);
                prp.PEndDate = new DateTime(pr_financialyear.EndDate.Value.Year, 1, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.EndDate.Value.Year, 2, 1);
                int daysInMonths = DateTime.DaysInMonth(prp.PStartDate.Value.Year, 2);
                prp.PEndDate = new DateTime(pr_financialyear.EndDate.Value.Year, 2, daysInMonths);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.EndDate.Value.Year, 3, 1);
                prp.PEndDate = new DateTime(pr_financialyear.EndDate.Value.Year, 3, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.EndDate.Value.Year, 4, 1);
                prp.PEndDate = new DateTime(pr_financialyear.EndDate.Value.Year, 4, 30);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.EndDate.Value.Year, 5, 1);
                prp.PEndDate = new DateTime(pr_financialyear.EndDate.Value.Year, 5, 31);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
            {
                PR_PayrollPeriod prp = new PR_PayrollPeriod();
                prp.PStartDate = new DateTime(pr_financialyear.EndDate.Value.Year, 6, 1);
                prp.PEndDate = new DateTime(pr_financialyear.EndDate.Value.Year, 6, 30);
                prp.FinYearID = pr_financialyear.PFinYearID;
                prp.PStageID = "H";
                db.PR_PayrollPeriod.Add(prp);
                db.SaveChanges();
            }
        }
        // GET: /Attendance/FinancialYear/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_FinYear pr_finyear = db.PR_FinYear.Find(id);
            if (pr_finyear == null)
            {
                return HttpNotFound();
            }
            return View(pr_finyear);
        }

        // POST: /Attendance/FinancialYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PFinYearID,FinYearName,StartDate,EndDate,Status")] PR_FinYear pr_finyear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pr_finyear).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.FinYear), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)pr_finyear.PFinYearID);
                return RedirectToAction("Index");
            }
            return View(pr_finyear);
        }

        // GET: /Attendance/FinancialYear/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_FinYear pr_finyear = db.PR_FinYear.Find(id);
            if (pr_finyear == null)
            {
                return HttpNotFound();
            }
            return View(pr_finyear);
        }

        // POST: /Attendance/FinancialYear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PR_FinYear pr_finyear = db.PR_FinYear.Find(id);
            db.PR_FinYear.Remove(pr_finyear);
            db.SaveChanges();
            ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
            AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.FinYear), Convert.ToInt16(AuditManager.AuditOperation.Delete), DateTime.Now, (int)pr_finyear.PFinYearID);
                
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
