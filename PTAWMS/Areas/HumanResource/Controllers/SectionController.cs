using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using PTAWMS.Helper;
using PagedList;
using HRM_IKAN.Authentication;
using PTA.Authentication;
using PTAWMS.App_Start;
using System.Data.Entity.Validation;
using System.Text;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    [CustomControllerAttributes]
    public class SectionController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Section/
        public ActionResult Index()
        {
           // FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewHRSection> _View = new List<ViewHRSection>();
            List<ViewHRSection> _TempView = new List<ViewHRSection>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForSectionReport(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select * from ViewHRSection order by Status desc, SectionName asc");
            _View = dt.ToList<ViewHRSection>();
            List<HR_Employee> emps = db.HR_Employee.Where(aa => aa.Status == "Active").ToList();
            List<SectionList> sectionList = new List<SectionList>();
            foreach (var item in _View)
            {
                SectionList sl = new SectionList();
                sl.SecID = item.SecID;
                sl.SectionName = item.SectionName;
                sl.DepartmentName = item.DepartmentName;
                if (item.Status == true)
                    sl.Status = "Active";
                else
                    sl.Status = "Inactive";
                sl.NoOfEmps = emps.Where(aa => aa.SectionID == item.SecID).Count();
                sectionList.Add(sl);
            }
            return View(sectionList.ToList());
        }

        // GET: /HumanResource/Section/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Section hr_section = db.HR_Section.Find(id);
                if (hr_section == null)
                {
                    return HttpNotFound();
                }
                return View(hr_section);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Section/Create
        public ActionResult Create()
        {
            //ViewBag.DivisionID = new SelectList(db.HR_Division, "DivID", "DivisionName");
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DeptID", "DepartmentName");
            return View();
        }

        // POST: /HumanResource/Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecID,SectionName,DepartmentID")] HR_Section hr_section)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    hr_section.Status = true;
                    db.HR_Section.Add(hr_section);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

            // ViewBag.DivisionID = new SelectList(db.HR_Division, "DivID", "DivisionName", hr_section.HR_Department.DivsionID);
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DeptID", "DepartmentName", hr_section.DepartmentID);
            return View(hr_section);
        }

        // GET: /HumanResource/Section/Edit/5
        public ActionResult Edit(short? id)
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DeptID", "DepartmentName");
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.Department = db.HR_Section.First(aa => aa.SecID == id).SectionName;
                HR_Section hr_section = db.HR_Section.Find(id);
                if (hr_section == null)
                {
                    return HttpNotFound();
                }
                // ViewBag.DivisionID = new SelectList(db.HR_Division, "DivID", "DivisionName", hr_section.HR_Department.DivsionID);
                ViewBag.DepartmentID = new SelectList(db.HR_Department, "DeptID", "DepartmentName", hr_section.DepartmentID);
                return View(hr_section);
        }

        // POST: /HumanResource/Section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "SecID,SectionName,DepartmentID,Status")] HR_Section hr_section)
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DeptID", "DepartmentName");
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(hr_section).State = System.Data.Entity.EntityState.Modified;
                    SaveChanges(db);
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID,Convert.ToInt16(AuditManager.AuditForm.Section), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now,(int)hr_section.SecID);
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentID = new SelectList(db.HR_Department, "DeptID", "DepartmentName", hr_section.DepartmentID);
                // ViewBag.DivisionID = new SelectList(db.HR_Division, "DivID", "DivisionName", hr_section.HR_Department.DivsionID);
                return View(hr_section);
            }
            catch (Exception ex)
            {
                return View(hr_section);
            }
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
        // GET: /HumanResource/Section/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Section hr_section = db.HR_Section.Find(id);
                if (hr_section == null)
                {
                    return HttpNotFound();
                }
                return View(hr_section);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                HR_Section hr_section = db.HR_Section.Find(id);
                db.HR_Section.Remove(hr_section);
                db.SaveChanges();
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
        public ActionResult DeptList(string ID)
        {
            try
            {
                var depts = db.HR_Department.OrderBy(s => s.DepartmentName);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    depts.ToArray(),
                                    "DeptID",
                                    "DepartmentName")
                               , JsonRequestBehavior.AllowGet);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public class SectionList
    {
        public int SecID { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string Status { get; set; }
        public int NoOfEmps { get; set; }
    }
}
