using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PTAWMS.Models;
using HRM_IKAN.Authentication;
using PTAWMS.App_Start;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class ShiftController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Shift/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.TypeSortParm = sortOrder == "LvType" ? "LvType_desc" : "LvType";
                ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
                ViewBag.LeaveSortParm = sortOrder == "Leave" ? "Leave_desc" : "Leave";
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                User LoggedInUser = Session["LoggedUser"] as User;
                List<Att_Shift> shifts = db.Att_Shift.OrderBy(aa => aa.ShiftName).ToList();
                ViewBag.CurrentFilter = searchString;
                //var lvapplications = db.LvApplications.Where(aa=>aa.ToDate>=dt2).Include(l => l.Emp).Include(l => l.LvType1);
                if (!String.IsNullOrEmpty(searchString))
                {
                    shifts = shifts.Where(s => s.ShiftName.ToUpper().Contains(searchString.ToUpper())).ToList();
                }
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View(shifts.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/Shift/Details/5
        public ActionResult Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_Shift att_shift = db.Att_Shift.Find(id);
                if (att_shift == null)
                {
                    return HttpNotFound();
                }
                return View(att_shift);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/Shift/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.DayOff1 = new SelectList(db.Att_DaysName, "ID", "Name");
                ViewBag.DayOff2 = new SelectList(db.Att_DaysName, "ID", "Name");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/Shift/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShftID,ShiftName,StartTime,DayOff1,DayOff2,MonMin,TueMin,WedMin,ThuMin,FriMin,SatMin,SunMin,LateIn,EarlyIn,EarlyOut,LateOut,OverTimeMin,MinHrs,HasBreak,BreakMin,HalfDayBreakMin,GZDays,OpenShift,RoundOffWorkMin,SubtractLOFromWork,SubtractEIFromWork,PresentAtIN,CalDiffOnly")] Att_Shift att_shift)
        {
            try
            {
                att_shift.GZDays = (bool)ValueProvider.GetValue("GZDays").ConvertTo(typeof(bool));
                att_shift.OpenShift = (bool)ValueProvider.GetValue("OpenShift").ConvertTo(typeof(bool));
                att_shift.HasBreak = (bool)ValueProvider.GetValue("HasBreak").ConvertTo(typeof(bool));
                att_shift.RoundOffWorkMin = (bool)ValueProvider.GetValue("RoundOffWorkMin").ConvertTo(typeof(bool));
                att_shift.SubtractLOFromWork = (bool)ValueProvider.GetValue("SubtractLOFromWork").ConvertTo(typeof(bool));
                att_shift.SubtractEIFromWork = (bool)ValueProvider.GetValue("SubtractEIFromWork").ConvertTo(typeof(bool));
                att_shift.PresentAtIN = (bool)ValueProvider.GetValue("PresentAtIN").ConvertTo(typeof(bool));
                #region -- Validation--
                if (att_shift.ShiftName == null || att_shift.ShiftName == "")
                    ModelState.AddModelError("ShiftName", "Please add Name");
                if (att_shift.StartTime == null)
                    ModelState.AddModelError("StartTime", "Please add Start Time");
                if (att_shift.MonMin == null)
                    ModelState.AddModelError("MonMin", "Please add Minutes");
                if (att_shift.MonMin == null)
                    ModelState.AddModelError("MonMin", "Please add Minutes");
                if (att_shift.TueMin == null)
                    ModelState.AddModelError("TueMin", "Please add Minutes");
                if (att_shift.WedMin == null)
                    ModelState.AddModelError("WedMin", "Please add Minutes");
                if (att_shift.ThuMin == null)
                    ModelState.AddModelError("ThuMin", "Please add Minutes");
                if (att_shift.FriMin == null)
                    ModelState.AddModelError("FriMin", "Please add Minutes");
                if (att_shift.SatMin == null)
                    ModelState.AddModelError("SatMin", "Please add Minutes");
                if (att_shift.SunMin == null)
                    ModelState.AddModelError("SunMin", "Please add Minutes");
                if (att_shift.LateIn == null)
                    ModelState.AddModelError("LateIn", "Please add Minutes");
                if (att_shift.LateOut == null)
                    ModelState.AddModelError("LateOut", "Please add Minutes");
                if (att_shift.EarlyIn == null)
                    ModelState.AddModelError("EarlyIn", "Please add Minutes");
                if (att_shift.EarlyOut == null)
                    ModelState.AddModelError("EarlyOut", "Please add Minutes");
                if (att_shift.OverTimeMin == null)
                    ModelState.AddModelError("OverTimeMin", "Please add Minutes");
                if (att_shift.MinHrs == null)
                    ModelState.AddModelError("MinHrs", "Please add Minutes");
                if (att_shift.HasBreak == true)
                {
                    if (att_shift.BreakMin == null)
                        ModelState.AddModelError("BreakMin", "Please add Break Minutes");
                    if (att_shift.HalfDayBreakMin == null)
                        ModelState.AddModelError("HalfDayBreakMin", "Please add Half Day Break Minutes");
                }
                if (db.Att_Shift.Where(aa => aa.ShiftName == att_shift.ShiftName).Count() > 0)
                    ModelState.AddModelError("ShiftName", "Shift name must be unique");
                #endregion
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (att_shift.HasBreak == false)
                        {
                            att_shift.BreakMin = 0;
                            att_shift.HalfDayBreakMin = 0;
                        }
                        db.Att_Shift.Add(att_shift);
                        db.SaveChanges();
                        ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                        AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Shift), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)att_shift.ShftID);               
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    return View(ex);
                }
                ViewBag.DayOff1 = new SelectList(db.Att_DaysName, "ID", "Name");
                ViewBag.DayOff2 = new SelectList(db.Att_DaysName, "ID", "Name");
                return View(att_shift);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/Shift/Edit/5
        public ActionResult Edit(short? id)
        {

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_Shift att_shift = db.Att_Shift.Find(id);

                if (att_shift == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DayOff1 = new SelectList(db.Att_DaysName, "ID", "Name", att_shift.DayOff1);
                ViewBag.DayOff2 = new SelectList(db.Att_DaysName, "ID", "Name", att_shift.DayOff2);
                return View(att_shift);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/Shift/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShftID,ShiftName,StartTime,DayOff1,DayOff2,MonMin,TueMin,WedMin,ThuMin,FriMin,SatMin,SunMin,LateIn,EarlyIn,EarlyOut,LateOut,OverTimeMin,MinHrs,HasBreak,BreakMin,HalfDayBreakMin,GZDays,OpenShift,RoundOffWorkMin,SubtractLOFromWork,SubtractEIFromWork,PresentAtIN,CalDiffOnly")] Att_Shift att_shift)
        {
            try
            {
                att_shift.GZDays = (bool)ValueProvider.GetValue("GZDays").ConvertTo(typeof(bool));
                att_shift.OpenShift = (bool)ValueProvider.GetValue("OpenShift").ConvertTo(typeof(bool));
                att_shift.HasBreak = (bool)ValueProvider.GetValue("HasBreak").ConvertTo(typeof(bool));
                att_shift.RoundOffWorkMin = (bool)ValueProvider.GetValue("RoundOffWorkMin").ConvertTo(typeof(bool));
                att_shift.SubtractLOFromWork = (bool)ValueProvider.GetValue("SubtractLOFromWork").ConvertTo(typeof(bool));
                att_shift.SubtractEIFromWork = (bool)ValueProvider.GetValue("SubtractEIFromWork").ConvertTo(typeof(bool));
                att_shift.PresentAtIN = (bool)ValueProvider.GetValue("PresentAtIN").ConvertTo(typeof(bool));
                #region -- Validation--
                if (att_shift.ShiftName == null || att_shift.ShiftName == "")
                    ModelState.AddModelError("ShiftName", "Please add Name");
                if (att_shift.StartTime == null)
                    ModelState.AddModelError("StartTime", "Please add Start Time");
                if (att_shift.MonMin == null)
                    ModelState.AddModelError("MonMin", "Please add Minutes");
                if (att_shift.MonMin == null)
                    ModelState.AddModelError("MonMin", "Please add Minutes");
                if (att_shift.TueMin == null)
                    ModelState.AddModelError("TueMin", "Please add Minutes");
                if (att_shift.WedMin == null)
                    ModelState.AddModelError("WedMin", "Please add Minutes");
                if (att_shift.ThuMin == null)
                    ModelState.AddModelError("ThuMin", "Please add Minutes");
                if (att_shift.FriMin == null)
                    ModelState.AddModelError("FriMin", "Please add Minutes");
                if (att_shift.SatMin == null)
                    ModelState.AddModelError("SatMin", "Please add Minutes");
                if (att_shift.SunMin == null)
                    ModelState.AddModelError("SunMin", "Please add Minutes");
                if (att_shift.LateIn == null)
                    ModelState.AddModelError("LateIn", "Please add Minutes");
                if (att_shift.LateOut == null)
                    ModelState.AddModelError("LateOut", "Please add Minutes");
                if (att_shift.EarlyIn == null)
                    ModelState.AddModelError("EarlyIn", "Please add Minutes");
                if (att_shift.EarlyOut == null)
                    ModelState.AddModelError("EarlyOut", "Please add Minutes");
                if (att_shift.OverTimeMin == null)
                    ModelState.AddModelError("OverTimeMin", "Please add Minutes");
                if (att_shift.MinHrs == null)
                    ModelState.AddModelError("MinHrs", "Please add Minutes");
                if (att_shift.HasBreak == true)
                {
                    if (att_shift.BreakMin == null)
                        ModelState.AddModelError("BreakMin", "Please add Break Minutes");
                    if (att_shift.HalfDayBreakMin == null)
                        ModelState.AddModelError("HalfDayBreakMin", "Please add Half Day Break Minutes");
                }
                if (db.Att_Shift.Where(aa => aa.ShiftName == att_shift.ShiftName).Count() > 1)
                    ModelState.AddModelError("ShiftName", "Shift name must be unique");
                #endregion
                if (ModelState.IsValid)
                {
                    if (att_shift.HasBreak == false)
                    {
                        att_shift.BreakMin = 0;
                        att_shift.HalfDayBreakMin = 0;
                    }
                    db.Entry(att_shift).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Shift), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_shift.ShftID); 
                    return RedirectToAction("Index");
                }
                ViewBag.DayOff1 = new SelectList(db.Att_DaysName, "ID", "Name", att_shift.DayOff1);
                ViewBag.DayOff2 = new SelectList(db.Att_DaysName, "ID", "Name", att_shift.DayOff2);
                return View(att_shift);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/Shift/Delete/5
        public ActionResult Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_Shift att_shift = db.Att_Shift.Find(id);
                if (att_shift == null)
                {
                    return HttpNotFound();
                }
                return View(att_shift);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/Shift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            try
            {
                Att_Shift att_shift = db.Att_Shift.Find(id);
                db.Att_Shift.Remove(att_shift);
                db.SaveChanges();
                ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Shift), Convert.ToInt16(AuditManager.AuditOperation.Delete), DateTime.Now, (int)att_shift.ShftID); 
                   
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
}
