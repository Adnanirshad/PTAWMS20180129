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
using HRM_IKAN.Authentication;
using PTAWMS.App_Start;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    [CustomControllerAttributes]
    public class DepartmentController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Department/
        public ActionResult Index()
        {
            try
            {
                List<HR_Department> _View = new List<HR_Department>();
                List<HR_Department> _TempView = new List<HR_Department>();
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                QueryBuilder qb = new QueryBuilder();
                string query = qb.QueryForReportsDepartment(LoggedInUser);
                DataTable dt = new DataTable();
                if (query != "where")
                    dt = qb.GetValuesfromDB("select * from HR_Department" + query + " DepartmentName");
                _View = dt.ToList<HR_Department>();
                List<HR_Employee> emps = db.HR_Employee.Where(aa => aa.Status == "Active").ToList();
                List<DeptList> deptList = new List<DeptList>();
                foreach (var item in _View.ToList())
                {
                    DeptList sl = new DeptList();
                    sl.DeptID = item.DeptID;
                    sl.DepartmentName = item.DepartmentName;
                    if (item.Status == true)
                        sl.Status = "Active";
                    else
                        sl.Status = "Inactive";
                    sl.NoOfEmps = emps.Where(aa => aa.HR_Section.DepartmentID == item.DeptID).Count();
                    deptList.Add(sl);
                }
                return View(deptList.ToList());


            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Department/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Department hr_department = db.HR_Department.Find(id);
                if (hr_department == null)
                {
                    return HttpNotFound();
                }
                return View(hr_department);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Department/Create
        public ActionResult Create()
        {
            try
            {
                // ViewBag.DivsionID = new SelectList(db.HR_Division, "DivID", "DivisionName");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeptID,DepartmentName,DivsionID")] HR_Department hr_department)
        {
            try
            {
                if (ModelState.IsValid)
                {                 
                    hr_department.Status = true;
                    db.HR_Department.Add(hr_department);
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

            //  ViewBag.DivsionID = new SelectList(db.HR_Division, "DivID", "DivisionName", hr_department.DivsionID);
            return View(hr_department);
        }

        // GET: /HumanResource/Department/Edit/5
        public ActionResult Edit(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Department hr_department = db.HR_Department.Find(id);
                if (hr_department == null)
                {
                    return HttpNotFound();
                }
                // ViewBag.DivsionID = new SelectList(db.HR_Division, "DivID", "DivisionName", hr_department.DivsionID);
                return View(hr_department);
            }
            catch (Exception)
            {

                return View();
            }

        }

        // POST: /HumanResource/Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeptID,DepartmentName,DivsionID,Status")] HR_Department hr_department)
        {
            if (hr_department.DepartmentName == null || hr_department.DepartmentName == "")
            {
                ViewBag.error = "Empty Feild";
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(hr_department).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                        AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Department), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)hr_department.DeptID);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            //ViewBag.DivsionID = new SelectList(db.HR_Division, "DivID", "DivisionName", hr_department.DivsionID);
            return View(hr_department);
        }

        // GET: /HumanResource/Department/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Department hr_department = db.HR_Department.Find(id);
                if (hr_department == null)
                {
                    return HttpNotFound();
                }
                return View(hr_department);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                HR_Department hr_department = db.HR_Department.Find(id);
                db.HR_Department.Remove(hr_department);
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
    }
    public class DeptList
    {
        public int DeptID { get; set; }
        public string DepartmentName { get; set; }
        public string Status { get; set; }
        public int NoOfEmps { get; set; }
    }
}
