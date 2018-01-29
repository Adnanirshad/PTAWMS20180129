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
using PTAWMS.Helper;
using PTAWMS.App_Start;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    [CustomControllerAttributes]
    public class DesignationController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Designation/
        public ActionResult Index()
        {
            //return View(db.HR_Designation.OrderBy(aa => aa.DesignationName).ToList());
            List<HR_Designation> _View = new List<HR_Designation>();
            List<HR_Designation> _TempView = new List<HR_Designation>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForReportsDepartment(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select * from HR_Designation" + query + " DesignationName");
            _View = dt.ToList<HR_Designation>();
            List<HR_Employee> emps = db.HR_Employee.Where(aa => aa.Status == "Active").ToList();
            List<DesignationList> desigtList = new List<DesignationList>();
            foreach (var item in _View.ToList())
            {
                DesignationList sl = new DesignationList();
                sl.DesignationID = item.DesgID;
                sl.DesignationName = item.DesignationName;
                sl.CommonName = item.OCommonName;
                sl.NoOfEmps = emps.Where(aa => aa.HR_Designation.DesgID == item.DesgID).Count();
                desigtList.Add(sl);
            }
            return View(desigtList.OrderBy(aa => aa.DesignationName).ToList());
        }

        // GET: /HumanResource/Designation/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Designation hr_designation = db.HR_Designation.Find(id);
                if (hr_designation == null)
                {
                    return HttpNotFound();
                }
                return View(hr_designation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Designation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HumanResource/Designation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DesgID,DesignationName,OCommonName")] HR_Designation hr_designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.HR_Designation.Add(hr_designation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(hr_designation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Designation/Edit/5
        public ActionResult Edit(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Designation hr_designation = db.HR_Designation.Find(id);
                if (hr_designation == null)
                {
                    return HttpNotFound();
                }
                return View(hr_designation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Designation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DesgID,DesignationName,OCommonName")] HR_Designation hr_designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(hr_designation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Designation), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)hr_designation.DesgID);              
                    return RedirectToAction("Index");
                }
                return View(hr_designation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Designation/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Designation hr_designation = db.HR_Designation.Find(id);
                if (hr_designation == null)
                {
                    return HttpNotFound();
                }
                return View(hr_designation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Designation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                HR_Designation hr_designation = db.HR_Designation.Find(id);
                db.HR_Designation.Remove(hr_designation);
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
    public class DesignationList
    {
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string Status { get; set; }
        public string CommonName{ get; set; }
        public int NoOfEmps { get; set; }
    }
}
