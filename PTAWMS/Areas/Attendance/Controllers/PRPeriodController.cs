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
    public class PRPeriodController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/PRPeriod/
        public ActionResult Index()
        {
            return View(db.PR_PayrollPeriod.OrderByDescending(aa=>aa.PID).ToList());
        }

        // GET: /Attendance/PRPeriod/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_PayrollPeriod pr_payrollperiod = db.PR_PayrollPeriod.Find(id);
            if (pr_payrollperiod == null)
            {
                return HttpNotFound();
            }
            return View(pr_payrollperiod);
        }

        // GET: /Attendance/PRPeriod/Create
        public ActionResult Create()
        {
            ViewBag.PStageID = new SelectList(GetPeriodStage(), "ID", "Name");
            return View();
        }
        public List<ModelPeriod> GetPeriodStage()
        {
            List<ModelPeriod> list = new List<ModelPeriod>();
            {
                ModelPeriod mp = new ModelPeriod();
                mp.ID = "O";
                mp.Name = "Open";
                list.Add(mp);
            }
            {
                ModelPeriod mp = new ModelPeriod();
                mp.ID = "C";
                mp.Name = "Close";
                list.Add(mp);
            }
            return list;
        }
        // POST: /Attendance/PRPeriod/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PID,PName,PStartDate,PEndDate,PStageID")] PR_PayrollPeriod pr_payrollperiod)
        {
            if (ModelState.IsValid)
            {            
                db.PR_PayrollPeriod.Add(pr_payrollperiod);
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.OT_Period), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)pr_payrollperiod.PID);                   
                return RedirectToAction("Index");
            }

            ViewBag.PStageID = new SelectList(GetPeriodStage(), "ID", "Name");
            return View(pr_payrollperiod);
        }

        // GET: /Attendance/PRPeriod/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_PayrollPeriod pr_payrollperiod = db.PR_PayrollPeriod.Find(id);
            if (pr_payrollperiod == null)
            {
                return HttpNotFound();
            }
            ViewBag.PStageID = new SelectList(GetPeriodStage(), "ID", "Name",pr_payrollperiod.PStageID);
            return View(pr_payrollperiod);
        }

        // POST: /Attendance/PRPeriod/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PID,PName,PStartDate,PEndDate,PStageID,SupervisorCutOffDate,RecommendCutOffDate,ApprovedCutOffDate")] PR_PayrollPeriod pr_payrollperiod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pr_payrollperiod).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.OT_Period), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)pr_payrollperiod.PID);                   
                
                return RedirectToAction("Index");
            }
            ViewBag.PStageID = new SelectList(GetPeriodStage(), "ID", "Name", pr_payrollperiod.PStageID);
            return View(pr_payrollperiod);
        }

        // GET: /Attendance/PRPeriod/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_PayrollPeriod pr_payrollperiod = db.PR_PayrollPeriod.Find(id);
            if (pr_payrollperiod == null)
            {
                return HttpNotFound();
            }
            return View(pr_payrollperiod);
        }

        // POST: /Attendance/PRPeriod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PR_PayrollPeriod pr_payrollperiod = db.PR_PayrollPeriod.Find(id);
            db.PR_PayrollPeriod.Remove(pr_payrollperiod);
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
    public class ModelPeriod
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
