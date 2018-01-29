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
    public class EmpTypeController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/EmpType/
        public ActionResult Index()
        {
            try
            {
                List<HR_EmpType> _View = new List<HR_EmpType>();
                List<HR_EmpType> _TempView = new List<HR_EmpType>();
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                QueryBuilder qb = new QueryBuilder();
                string query = qb.QueryForReportsDepartment(LoggedInUser);
                DataTable dt = new DataTable();
                if (query != "where")
                    dt = qb.GetValuesfromDB("select * from HR_EmpType" + query + " TypeName");
                _View = dt.ToList<HR_EmpType>();
                List<HR_Employee> emps = db.HR_Employee.Where(aa => aa.Status == "Active").ToList();
                List<EmpTypeList> typelist = new List<EmpTypeList>();
                foreach (var item in _View.ToList())
                {
                    EmpTypeList sl = new EmpTypeList();
                    sl.TypeID = item.TypID;
                    sl.TypeName = item.TypeName;                   
                    sl.NoOfEmps = emps.Where(aa => aa.EmpTypeID == item.TypID).Count();
                    typelist.Add(sl);
                }
                return View(typelist.ToList());


            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/EmpType/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_EmpType hr_emptype = db.HR_EmpType.Find(id);
                if (hr_emptype == null)
                {
                    return HttpNotFound();
                }
                return View(hr_emptype);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/EmpType/Create
        public ActionResult Create()
        {
            //ViewBag.CategoryID = new SelectList(db.HR_Category, "CatID", "CategoryName");
            return View();
        }

        // POST: /HumanResource/EmpType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypID,TypeName,CategoryID")] HR_EmpType hr_emptype)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.HR_EmpType.Add(hr_emptype);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

            //ViewBag.CategoryID = new SelectList(db.HR_Category, "CatID", "CategoryName", hr_emptype.CategoryID);
            return View(hr_emptype);
        }

        // GET: /HumanResource/EmpType/Edit/5
        public ActionResult Edit(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_EmpType hr_emptype = db.HR_EmpType.Find(id);
                if (hr_emptype == null)
                {
                    return HttpNotFound();
                }
                // ViewBag.CategoryID = new SelectList(db.HR_Category, "CatID", "CategoryName", hr_emptype.CategoryID);
                return View(hr_emptype);
            }
            catch (Exception)
            {

                return View();
            }

        }

        // POST: /HumanResource/EmpType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypID,TypeName,CategoryID")] HR_EmpType hr_emptype)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(hr_emptype).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Type), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)hr_emptype.TypID);             
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }
            // ViewBag.CategoryID = new SelectList(db.HR_Category, "CatID", "CategoryName", hr_emptype.CategoryID);
            return View(hr_emptype);
        }

        // GET: /HumanResource/EmpType/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_EmpType hr_emptype = db.HR_EmpType.Find(id);
                if (hr_emptype == null)
                {
                    return HttpNotFound();
                }
                return View(hr_emptype);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/EmpType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                HR_EmpType hr_emptype = db.HR_EmpType.Find(id);
                db.HR_EmpType.Remove(hr_emptype);
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
    public class EmpTypeList
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int NoOfEmps { get; set; }
    }
}
