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
    public class LocationController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Location/
        public ActionResult Index()
        {
            try
            {
               // FiltersModel fm = Session["FiltersModel"] as FiltersModel;
                List<HR_Location> _View = new List<HR_Location>();
                List<HR_Location> _TempView = new List<HR_Location>();
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                QueryBuilder qb = new QueryBuilder();
                string query = qb.QueryForLocReport(LoggedInUser);
                DataTable dt = new DataTable();
                if (query != "where")
                    dt = qb.GetValuesfromDB("select * from HR_Location " + query + " LocationName");
                _View = dt.ToList<HR_Location>();
                List<HR_Employee> emps = db.HR_Employee.Where(aa => aa.Status == "Active").ToList();
                List<LocationList> locList = new List<LocationList>();
                foreach (var item in _View)
                {
                    LocationList sl = new LocationList();
                    sl.LocID = item.LocID;
                    sl.LocName = item.LocationName;
                    if (item.Status == true)
                        sl.Status = "Active";
                    else
                        sl.Status = "Inactive";
                    sl.NoOfEmps = emps.Where(aa => aa.LocationID == item.LocID).Count();
                    locList.Add(sl);
                }
                return View(locList.Where(aa=>aa.Status == "Active").ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Location/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Location hr_location = db.HR_Location.Find(id);
                if (hr_location == null)
                {
                    return HttpNotFound();
                }
                return View(hr_location);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HumanResource/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocID,LocationName")] HR_Location hr_location)
        {
            try
            {
                // hr_location.Status = (bool)ValueProvider.GetValue("Status").ConvertTo(typeof(bool));
                if (ModelState.IsValid)
                {
                    db.HR_Location.Add(hr_location);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                // ViewBag.CityID = new SelectList(db.HR_City, "CitID", "CityName", hr_location.CityID);
                return View(hr_location);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Location/Edit/5
        public ActionResult Edit(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.Station = db.HR_Location.First(aa => aa.LocID == id).LocationName;
                HR_Location hr_location = db.HR_Location.Find(id);
                if (hr_location == null)
                {
                    return HttpNotFound();
                }
                return View(hr_location);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocID,LocationName,OLocationID,Status")] HR_Location hr_location)
        {
            try
            {
                // hr_location.Status = (bool)ValueProvider.GetValue("Status").ConvertTo(typeof(bool));
                if (ModelState.IsValid)
                {
                    db.Entry(hr_location).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Location), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)hr_location.LocID);
                    return RedirectToAction("Index");
                }
                return View(hr_location);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /HumanResource/Location/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HR_Location hr_location = db.HR_Location.Find(id);
                if (hr_location == null)
                {
                    return HttpNotFound();
                }
                return View(hr_location);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /HumanResource/Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                HR_Location hr_location = db.HR_Location.Find(id);
                db.HR_Location.Remove(hr_location);
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
    public class LocationList
    {
        public int LocID { get; set; }
        public string LocName { get; set; }
        public string Status { get; set; }
        public int NoOfEmps { get; set; }
    }
}
