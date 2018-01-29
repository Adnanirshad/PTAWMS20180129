using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using PTAWMS.App_Start;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    public class GradeController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Grade/
        public ActionResult Index()
        {
            return View(db.HR_Grade.ToList());
        }

        // GET: /HumanResource/Grade/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Grade hr_grade = db.HR_Grade.Find(id);
            if (hr_grade == null)
            {
                return HttpNotFound();
            }
            return View(hr_grade);
        }

        // GET: /HumanResource/Grade/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HumanResource/Grade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="GrdID,GradeName,OGradeID,NormalOTAmount,RestOTAmount,GZOTAmount")] HR_Grade hr_grade)
        {
            if (ModelState.IsValid)
            {
                db.HR_Grade.Add(hr_grade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hr_grade);
        }

        // GET: /HumanResource/Grade/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Grade hr_grade = db.HR_Grade.Find(id);
            if (hr_grade == null)
            {
                return HttpNotFound();
            }
            return View(hr_grade);
        }

        // POST: /HumanResource/Grade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="GrdID,GradeName,OGradeID,NormalOTAmount,RestOTAmount,GZOTAmount")] HR_Grade hr_grade)
        {
            if (ModelState.IsValid)
            {
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                using (var ctx = new HRMEntities())
                {
                    HR_Grade OldGrade = ctx.HR_Grade.First(aa => aa.GrdID == hr_grade.GrdID);
                    HR_GradeHistory GradeHistory = new HR_GradeHistory();
                    GradeHistory.CDateTime = DateTime.Now;
                    GradeHistory.GradeName = OldGrade.GradeName;
                    GradeHistory.GrdID = OldGrade.GrdID;
                    GradeHistory.GZOTAmount = OldGrade.GZOTAmount;
                    GradeHistory.IP = AuditManager.GetIPAddress();
                    GradeHistory.NormalOTAmount = OldGrade.NormalOTAmount;
                    GradeHistory.OGradeID =OldGrade.OGradeID;
                    GradeHistory.OGradeID =OldGrade.OGradeID;
                    GradeHistory.RestOTAmount = OldGrade.RestOTAmount;
                    GradeHistory.UserID = loggedUser.UserID;
                    ctx.HR_GradeHistory.Add(GradeHistory);
                    ctx.SaveChanges();
                    hr_grade.OGradeID = OldGrade.OGradeID;
                }
                db.Entry(hr_grade).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Grade), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)hr_grade.GrdID);
                return RedirectToAction("Index");
            }
            return View(hr_grade);
        }

        // GET: /HumanResource/Grade/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Grade hr_grade = db.HR_Grade.Find(id);
            if (hr_grade == null)
            {
                return HttpNotFound();
            }
            return View(hr_grade);
        }

        // POST: /HumanResource/Grade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            HR_Grade hr_grade = db.HR_Grade.Find(id);
            db.HR_Grade.Remove(hr_grade);
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
