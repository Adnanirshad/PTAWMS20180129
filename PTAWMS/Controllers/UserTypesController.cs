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

namespace PTAWMS.Controllers
{
    [CustomControllerAttributes]
    public class UserTypesController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /UserTypes/
        public ActionResult Index()
        {
            return View(db.UserTypes.ToList());
        }

        // GET: /UserTypes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType usertype = db.UserTypes.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);
        }

        // GET: /UserTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /UserTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PUTID,UTypeName,CanEdit,CanDelete,CanView,CanAdd,MAttProcess,MOption,MAttDevice,MAttDeviceUtility,MAttEditAttendance,MAttJobCard,MAttShift,MAttPolicy,MAttDownloadTime,MAttHoliday,MAttOTPolicy,MAttOTCreate,MAttOTEdit,MAttOTRequest,MAttLeaves,MHRCompHierarchy,MUser,MGrade,MHREmployee,MHREmpPersonal,MHREmpJob,MHREmpAtt,HRModule,HREmpType,HRLocation,HRDeptartment,HRDesignation,HRSection,AttendanceModule,OUserID,VisitorApplication,VisitorEntry,VisitorSupervisor,OTBudget,OTBudgetCreditDebit")] UserType usertype)
        {
            if (ModelState.IsValid)
            {
                db.UserTypes.Add(usertype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usertype);
        }
        private void GetValuesFromCheckboxes(UserType user)
        {
            #region-- Checkboxes --
           // user.Status = (bool)ValueProvider.GetValue("Status").ConvertTo(typeof(bool));
            user.CanEdit = (bool)ValueProvider.GetValue("CanEdit").ConvertTo(typeof(bool));
            user.CanDelete = (bool)ValueProvider.GetValue("CanDelete").ConvertTo(typeof(bool));
            user.CanView = (bool)ValueProvider.GetValue("CanView").ConvertTo(typeof(bool));
            user.CanAdd = (bool)ValueProvider.GetValue("CanAdd").ConvertTo(typeof(bool));
            user.MAttProcess = (bool)ValueProvider.GetValue("MAttProcess").ConvertTo(typeof(bool));
            user.MOption = (bool)ValueProvider.GetValue("MOption").ConvertTo(typeof(bool));
            user.MAttDevice = (bool)ValueProvider.GetValue("MAttDevice").ConvertTo(typeof(bool));
            user.MAttEditAttendance = (bool)ValueProvider.GetValue("MAttEditAttendance").ConvertTo(typeof(bool));
            user.MAttJobCard = (bool)ValueProvider.GetValue("MAttJobCard").ConvertTo(typeof(bool));
            user.MAttShift = (bool)ValueProvider.GetValue("MAttShift").ConvertTo(typeof(bool));
            user.MAttPolicy = (bool)ValueProvider.GetValue("MAttPolicy").ConvertTo(typeof(bool));

            user.MAttOTCreate = (bool)ValueProvider.GetValue("MAttOTCreate").ConvertTo(typeof(bool));
            user.MAttOTEdit = (bool)ValueProvider.GetValue("MAttOTEdit").ConvertTo(typeof(bool));

            user.MAttDownloadTime = (bool)ValueProvider.GetValue("MAttDownloadTime").ConvertTo(typeof(bool));
            user.MAttHoliday = (bool)ValueProvider.GetValue("MAttHoliday").ConvertTo(typeof(bool));
            // user.MAttCPL = (bool)ValueProvider.GetValue("MAttCPL").ConvertTo(typeof(bool));
            //user.MAttOTPolicy = (bool)ValueProvider.GetValue("MOTPolicy").ConvertTo(typeof(bool));
            user.MAttLeaves = (bool)ValueProvider.GetValue("MAttLeaves").ConvertTo(typeof(bool));
            user.MUser = (bool)ValueProvider.GetValue("MUser").ConvertTo(typeof(bool));
            user.MGrade = (bool)ValueProvider.GetValue("MGrade").ConvertTo(typeof(bool));
            user.MHREmployee = (bool)ValueProvider.GetValue("MHREmployee").ConvertTo(typeof(bool));
            user.MHREmpPersonal = (bool)ValueProvider.GetValue("MHREmpPersonal").ConvertTo(typeof(bool));
            user.MHREmpJob = (bool)ValueProvider.GetValue("MHREmpJob").ConvertTo(typeof(bool));
            user.MHREmpAtt = (bool)ValueProvider.GetValue("MHREmpAtt").ConvertTo(typeof(bool));
            user.HRModule = (bool)ValueProvider.GetValue("HRModule").ConvertTo(typeof(bool));
            user.HREmpType = (bool)ValueProvider.GetValue("HREmpType").ConvertTo(typeof(bool));
            user.HRLocation = (bool)ValueProvider.GetValue("HRLocation").ConvertTo(typeof(bool));
            user.HRDeptartment = (bool)ValueProvider.GetValue("HRDeptartment").ConvertTo(typeof(bool));
            user.HRDesignation = (bool)ValueProvider.GetValue("HRDesignation").ConvertTo(typeof(bool));
            //user.HRSection = (bool)ValueProvider.GetValue("HRSection").ConvertTo(typeof(bool));
            user.AttendanceModule = (bool)ValueProvider.GetValue("AttendanceModule").ConvertTo(typeof(bool));


            #endregion
        }
        // GET: /UserTypes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType usertype = db.UserTypes.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);
        }

        // POST: /UserTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PUTID,UTypeName,CanEdit,CanDelete,CanView,CanAdd,MAttProcess,MOption,MAttDevice,MAttDeviceUtility,MAttEditAttendance,MAttJobCard,MAttShift,MAttPolicy,MAttDownloadTime,MAttHoliday,MAttOTPolicy,MAttOTCreate,MAttOTEdit,MAttOTRequest,MAttLeaves,MHRCompHierarchy,MUser,MGrade,MHREmployee,MHREmpPersonal,MHREmpJob,MHREmpAtt,HRModule,HREmpType,HRLocation,HRDeptartment,HRDesignation,HRSection,AttendanceModule,OUserID,VisitorApplication,VisitorEntry,VisitorSupervisor,OTBudget,OTBudgetCreditDebit")] UserType usertype)
        {
            if (ModelState.IsValid)
            {
                //var user = db.Users.Where(aa => aa.UserType == usertype.PUTID).ToList();
               // GetValuesFromCheckboxes(usertype);
                db.Entry(usertype).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usertype);
        }

        // GET: /UserTypes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType usertype = db.UserTypes.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);
        }

        // POST: /UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserType usertype = db.UserTypes.Find(id);
            db.UserTypes.Remove(usertype);
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
