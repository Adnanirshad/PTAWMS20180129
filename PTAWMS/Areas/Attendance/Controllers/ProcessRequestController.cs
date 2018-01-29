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
    public class ProcessRequestController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: /Attendance/ProcessRequest/
        public ActionResult Index()
        {
            try
            {
                DateTime dt = DateTime.Now;
                dt = dt - new TimeSpan(04, 0, 0);
                List<Att_ProcessRequest> attp = new List<Att_ProcessRequest>();
                attp = db.Att_ProcessRequest.Where(aa => aa.SystemGenerated == false && aa.ProcessingDone == false).ToList();
                attp.AddRange(db.Att_ProcessRequest.Where(aa => aa.SystemGenerated == false && aa.ProcessingDone == true && aa.CreatedDate >= dt).ToList());
                return View(attp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/ProcessRequest/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_ProcessRequest att_processrequest = db.Att_ProcessRequest.Find(id);
                if (att_processrequest == null)
                {
                    return HttpNotFound();
                }
                return View(att_processrequest);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/ProcessRequest/Create
        public ActionResult Create(int? ID)
        {
            try
            {
                if (ID != null)
                {
                    ViewBag.EmployeeName = db.EmpViews.First(aa => aa.EmployeeID == ID).FullName;
                    ViewBag.Designation = db.EmpViews.First(aa => aa.EmployeeID == ID).DesignationName;
                    ViewBag.Department = db.EmpViews.First(aa => aa.EmployeeID == ID).SectionName;
                }
                //QueryBuilder qb = new QueryBuilder();
                //String query = qb.QueryForCompanyViewLinq(LoggedInUser);
                ViewBag.PeriodTag = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Daily", Value = "D"},
                new SelectListItem { Selected = false, Text = "Monthly", Value = "M"},
                new SelectListItem { Selected = false, Text = "OT Processing", Value = "O"},

            }, "Value", "Text", 1);
                ViewBag.CriteriaID = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Company", Value = "C"},
                new SelectListItem { Selected = false, Text = "Location", Value = "L"},
                new SelectListItem { Selected = false, Text = "Employee", Value = "E"},

            }, "Value", "Text", 1);
                ViewBag.ProcessCats = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Yes", Value = "1"},
                new SelectListItem { Selected = false, Text = "No", Value = "0"},

            }, "Value", "Text", 1);
                // query = qb.QueryForLocationTableSegerationForLinq(LoggedInUser);
                ViewBag.LocationID = new SelectList(db.HR_Location.OrderBy(s => s.LocationName), "LocID", "LocationName");            
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/ProcessRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttProcesserSchedulerID,WhenToProcess,PeriodTag,DateFrom,DateTo,CompanyID,LocationID,CatID,ProcessingDone,Criteria,ProcessCat,UserID,CreatedDate,EmpID,EmpNo")] Att_ProcessRequest att_processrequest)
        {            
                try
                {
                    string d = Request.Form["CriteriaID"].ToString();
                    ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                    switch (d)
                    {
                        case "C":
                            att_processrequest.Criteria = "C";
                            break;
                        case "L": att_processrequest.Criteria = "L"; break;
                        case "A": att_processrequest.Criteria = "A"; break;
                        case "E":
                            {
                                att_processrequest.Criteria = "E";
                                att_processrequest.ProcessCat = false;
                                string ee = Request.Form["EmpNo"].ToString();
                                //int cc = Convert.ToInt16(Request.Form["CompanyIDForEmp"].ToString());
                                List<HR_Employee> empss = new List<HR_Employee>();
                                empss = db.HR_Employee.Where(aa => aa.EmpNo == ee).ToList();
                                if (empss.Count() > 0)
                                {
                                    att_processrequest.EmpID = empss.First().EmployeeID;
                                    att_processrequest.EmpNo = empss.First().EmpNo;
                                }
                            }
                            break;
                    }
                    att_processrequest.ProcessingDone = false;
                    //att_processrequest.WhenToProcess = DateTime.Today;
                    att_processrequest.CreatedDate = DateTime.Now;
                    att_processrequest.UserID = LoggedInUser.UserID;
                    //if (att_processrequest.DateFrom > att_processrequest.DateTo)
                   // {
                     //   ViewBag.error = "From Date must be less then To Date!";
                   // }
                   //else
                   // {
                        if (ModelState.IsValid)
                        {
                            att_processrequest.SystemGenerated = false;
                            db.Att_ProcessRequest.Add(att_processrequest);
                            db.SaveChanges();
                            ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                            AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Att_Process), Convert.ToInt16(AuditManager.AuditOperation.Add), DateTime.Now, (int)att_processrequest.AttProcesserSchedulerID);                   
                
                            return RedirectToAction("Index");
                        }
                   // }

                    return View(att_processrequest);

                }
                catch (Exception)
                {

                    throw;
                }
            }

        // GET: /Attendance/ProcessRequest/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_ProcessRequest att_processrequest = db.Att_ProcessRequest.Find(id);
                if (att_processrequest == null)
                {
                    return HttpNotFound();
                }
                return View(att_processrequest);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/ProcessRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttProcesserSchedulerID,WhenToProcess,PeriodTag,DateFrom,DateTo,CompanyID,LocationID,CatID,ProcessingDone,Criteria,ProcessCat,UserID,CreatedDate,EmpID,EmpNo")] Att_ProcessRequest att_processrequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(att_processrequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewUserEmp loggedUser = Session["LoggedUser"] as ViewUserEmp;
                    AuditManager.SaveAuditLog(loggedUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Att_Process), Convert.ToInt16(AuditManager.AuditOperation.Edit), DateTime.Now, (int)att_processrequest.AttProcesserSchedulerID);                   
                    return RedirectToAction("Index");
                }
                return View(att_processrequest);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: /Attendance/ProcessRequest/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_ProcessRequest att_processrequest = db.Att_ProcessRequest.Find(id);
                if (att_processrequest == null)
                {
                    return HttpNotFound();
                }
                return View(att_processrequest);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: /Attendance/ProcessRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Att_ProcessRequest att_processrequest = db.Att_ProcessRequest.Find(id);
                db.Att_ProcessRequest.Remove(att_processrequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetEmpInfo(string empNo)
        {
            try
            {
                List<EmpView> emp = db.EmpViews.Where(aa => aa.EmpNo == empNo).ToList();
                string Name = "";
                string Designation = "";
                string Section = "";
                string Type = "";
                string DOJ = "";
                if (emp.Count > 0)
                {
                    Name = "Name: " + emp.FirstOrDefault().FullName;
                    Designation = "Designation: " + emp.FirstOrDefault().DesignationName;
                    Section = "Section: " + emp.FirstOrDefault().SectionName;
                    Type = "Type: " + emp.FirstOrDefault().TypeName;
                    if (emp.FirstOrDefault().DOJ != null)
                        DOJ = "Join Date: " + emp.FirstOrDefault().DOJ.Value.ToString("dd-MMM-yyyy");
                    else
                        DOJ = "Join Date: Not Added";
                    if (HttpContext.Request.IsAjaxRequest())
                        return Json(Name + "@" + Designation + "@" + Section + "@" + Type + "@" + DOJ, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Name = "Name: Not found";
                    Designation = "Designation: Not found";
                    Section = "Section: Not found";
                    Type = "Type: Not found";
                    DOJ = "Join Date: Not found";
                    if (HttpContext.Request.IsAjaxRequest())
                        return Json(Name + "@" + Designation + "@" + Section + "@" + Type + "@" + DOJ
                           , JsonRequestBehavior.AllowGet);
                }

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
