using HRM_IKAN.Authentication;
using PTAWMS.Areas.Attendance.BusinessLogic;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class OTHRController : Controller
    {

        #region HR TotalRecord View
        public ActionResult SearchEmp(string sortOrder, string searchString, string currentFilter, int? page)
        {
            List<ViewUserEmp> emps = new List<ViewUserEmp>();
            emps = db.ViewUserEmps.Where(aa => aa.Status == true).ToList();
            return View(emps);
        }
        #region -- Department Pending OT --
        HRMEntities db = new HRMEntities();

        [HttpGet]
        public ActionResult HROTDeptList(int? PayrollPeriodID)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<short?> deptIds = new List<short?>();
            List<short?> divIds = new List<short?>();
            ModelRDeptPending vm = new ModelRDeptPending();
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            int CurrentPID = Convert.ToInt32(Session["PRID"].ToString());
            if (PayrollPeriodID == null)
                PayrollPeriodID = CurrentPID;
            else
                Session["PRID"] = PayrollPeriodID.ToString();
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)PayrollPeriodID);
            List<ViewDailyOTEntry> otList = new List<ViewDailyOTEntry>();
            otList = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID == PayrollPeriodID).ToList();
            if (LoggedInUser.UserType == "H" || LoggedInUser.UserType == "A")
            {
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            }
            else
            {
                List<int?> empIds = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID == PayrollPeriodID).Select(aa => aa.EmployeeID).Distinct().ToList();
                emps = OTHelperManager.GetEmployees(emps, LoggedInUser);
                emps = OTHelperManager.GetEmployeeConvertedFromIds(empIds, emps);
            }
            deptIds = OTHelperManager.GetDeptIDs(emps);
            divIds = OTHelperManager.GetDivisionIDs(deptIds, db.HR_Section.ToList());
            vm.List = OTHelperRecommended.GetConvertedOTListDeptsHR(otList, emps, deptIds);
            vm.Count = vm.List.Count;
            vm.PayrollPeriodID = (int)PayrollPeriodID;
            vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(deptIds, db.BG_OTDivision.ToList(), (int)prp.FinYearID);
            vm.TotalOTAmount = OTHelperManager.GetTotalOTAmount(vm.List);

            ViewBag.PayrollPeriodID = new SelectList(db.PR_PayrollPeriod.ToList(), "PID", "PName", PayrollPeriodID);
            vm.Message = new List<string>();
            return View(vm);
        }
        #endregion

        #region -- Employee Pending OT--
        public ModelSOTEmpList GetEmpPendingHR(ViewUserEmp LoggedInUser, int PayrollPeriodID, int DeptID)
        {
            ModelSOTEmpList vm = new ModelSOTEmpList();
            int CurrentPID = Convert.ToInt32(Session["PRID"].ToString());
            if (PayrollPeriodID == null)
                PayrollPeriodID = CurrentPID;
            else
                Session["PRID"] = PayrollPeriodID.ToString();
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active" && aa.SecID == DeptID).ToList();
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)PayrollPeriodID);
            vm.DeptID = (int)DeptID;
            emps = emps.Where(aa => aa.SecID == DeptID).ToList();
            List<ViewDailyOTEntry> otList = new List<ViewDailyOTEntry>();
            vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(OTHelperManager.ConvertDeptIDList(DeptID), db.BG_OTDivision.ToList(), (int)prp.FinYearID);
            otList = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID == PayrollPeriodID).ToList();
            if (LoggedInUser.UserType == "H" || LoggedInUser.UserType == "A")
            {
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            }
            else
            {
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
                List<int?> empIds = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID == PayrollPeriodID).Select(aa => aa.EmployeeID).Distinct().ToList();
                emps = OTHelperManager.GetEmployees(emps, LoggedInUser);
                emps = OTHelperManager.GetEmployeeConvertedFromIds(empIds, emps);
            }
            vm.List = OTHelperRecommended.GetConvertedOTListEmpHR(otList, emps);
            if (vm.List.Count() > 0)
                vm.TotalOTAmount = OTHelperManager.GetTotalOTAmount(vm.List);
            vm.Count = vm.List.Count;
            vm.PayrollPeriodID = (int)PayrollPeriodID;
            if (vm.List.Count() > 0)
            {
                vm.RecommendID = (int)LoggedInUser.ReportingToID;
                vm.RecommenderName = db.EmpViews.First(aa => aa.EmployeeID == LoggedInUser.ReportingToID).FullName;
            }
            ViewBag.PayrollPeriodID = new SelectList(db.PR_PayrollPeriod.ToList(), "PID", "PName", PayrollPeriodID);
            return vm;
        }
        [HttpGet]
        public ActionResult HROTEmpList(int? DeptID, int? PayrollPeriodID)
        {
            ModelSOTEmpList vm = new ModelSOTEmpList();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            vm = GetEmpPendingHR(LoggedInUser, (int)PayrollPeriodID, (int)DeptID);
            vm.Message = new List<string>();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HROTEmpList(int? PayrollPeriodID)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var empsChecked = Request.Form.GetValues("cbEmployee");
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            List<EmpView> tempEmps = new List<EmpView>();
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)PayrollPeriodID);
            List<ViewDailyOTEntry> otList = new List<ViewDailyOTEntry>();
            List<string> messages = new List<string>();
            string val = Request.Form["Certified"].ToString();
            int DeptID = Convert.ToInt32(Request.Form["DeptID"].ToString());
            if (val == "true,false")
            {
                foreach (var id in empsChecked)
                {
                    int chNo = Convert.ToInt32(id);
                    tempEmps.Add(emps.First(aa => aa.EmployeeID == chNo));
                }
                otList = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && (aa.StatusID == "P")).ToList();
                if (LoggedInUser.UserType == "N" || LoggedInUser.UserType == "R")
                {
                    List<ViewDailyOTEntry> vdat = OTHelperManager.GetOTListEmp(otList, tempEmps);
                    foreach (var emp in tempEmps)
                    {
                        //DeptID = (int)emp.SecID;
                        if (emp.DaysInMonth > 0)
                        {
                            // check for Monthly Limit
                            if (vdat.Where(aa => aa.EmpID == emp.EmployeeID).Count() > emp.DaysInMonth)
                                messages.Add("Your Monthly overtime claim limit exceeds.");
                            else
                            {
                                // check for weekly limit
                                if (emp.DaysInWeek > 0)
                                {
                                    if (OTHelperManager.IsValidWeekPolicy(vdat.Where(aa => aa.EmpID == emp.EmployeeID).OrderByDescending(aa => aa.OTDate).ToList(), prp, emp.DaysInWeek))
                                        messages.Add("Your weekly overtime claim limit exceeds.");
                                }
                            }
                        }
                        // check for daily ot limit
                        {
                            if (OTHelperManager.IsValidDailyOT(vdat.Where(aa => aa.EmpID == emp.EmployeeID), emp))
                                messages.Add("Your daily overtime claim limit exceeds.");

                        }
                    }
                    if (messages.Count == 0)
                    {
                        List<Att_OTDailyEntry> attOts = db.Att_OTDailyEntry.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate).ToList();

                        foreach (var emp in tempEmps)
                        {
                            foreach (var atot in attOts.Where(aa => aa.EmpID == emp.EmployeeID).ToList())
                            {
                                atot.StatusID = Request.Form["DecisionID"].ToString();
                                if (atot.StatusID == "C")
                                {
                                    //atot.NtoPDateTime = DateTime.Now;
                                    atot.CancelByID = LoggedInUser.UserID;
                                }
                                else
                                {
                                    atot.ForwardToID = Convert.ToInt32(Request.Form["RecommendID"]);
                                    //atot.NtoPDateTime = DateTime.Now;
                                    //atot.NtoPUserID = LoggedInUser.UserID;
                                    //atot.OTProcessingPeriod = PayrollPeriodID;
                                }
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                    }
                }
            }
            else
                messages.Add("Please verify, this employee does not claim daily allowance on these dates");

            //
            ModelSOTEmpList vm = new ModelSOTEmpList();
            //vm = GetEmpPending(LoggedInUser, (int)PayrollPeriodID, (int)DeptID);
            vm.Message = messages;
            return View(vm);
        }

        #endregion

        #region -- Employee Detail Pending OT --
        [HttpGet]
        public ActionResult HROTDetailList(int? EmpID, int? PayrollPeriodID)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            ModelSOTPEmpDetail vm = new ModelSOTPEmpDetail();
            int prid = Convert.ToInt32(Session["PRID"].ToString());
            vm = GetDetailPendingHR(prid, (int)EmpID, LoggedInUser);

            vm.Message = new List<string>();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HROTDetailList(ModelSOTPEmpDetail model)
        {
            List<string> messages = new List<string>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var checkedEmpDates = Request.Form.GetValues("cbEmployee");
            HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == model.EmpID);
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), model.PeriodID);
            //messages = IsValidate(model);
            if (messages.Count == 0)
            {
                List<Att_OTDailyEntry> attOts = db.Att_OTDailyEntry.Where(aa => aa.EmpID == model.EmpID && aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate).ToList();
                List<Att_OTDailyEntry> TempattOts = new List<Att_OTDailyEntry>();
                foreach (var id in checkedEmpDates)
                {
                    int chNo = Convert.ToInt32(id);
                    string EmpDate = Request.Form["ED" + (chNo - 1).ToString()].ToString();
                    string COTHour = Request.Form["ClaimOT" + (chNo - 1).ToString()].ToString();
                    TempattOts.Add(attOts.First(aa => aa.EmpDate == EmpDate));
                }
                if (emp.Att_OTPolicy.DaysInMonth > 0)
                {
                    // check for Monthly Limit
                    if (TempattOts.Count > emp.Att_OTPolicy.DaysInMonth)
                        messages.Add("Your Monthly overtime claim limit exceeds.");
                    else
                    {
                        // check for weekly limit
                        if (emp.Att_OTPolicy.DaysInWeek > 0)
                        {
                            if (OTHelperManager.IsValidWeekPolicy(TempattOts.OrderByDescending(aa => aa.OTDate).ToList(), prp, emp.Att_OTPolicy.DaysInWeek))
                                messages.Add("Your weekly overtime claim limit exceeds.");
                        }
                    }
                }
                // check for daily ot limit
                {
                    //if (OTHelperManager.IsValidDailyOT(TempattOts, emp.Att_OTPolicy))
                    //    messages.Add("Your daily overtime claim limit exceeds.");

                }
                if (checkedEmpDates != null)
                {
                    if (messages.Count == 0)
                    {
                        foreach (var id in checkedEmpDates)
                        {
                            int chNo = Convert.ToInt32(id);
                            string EmpDate = Request.Form["ED" + (chNo - 1).ToString()].ToString();
                            string COTHour = Request.Form["ClaimOT" + (chNo - 1).ToString()].ToString();
                            Att_OTDailyEntry atot = attOts.First(aa => aa.EmpDate == EmpDate);
                            atot.StatusID = Request.Form["DecisionID"].ToString();
                            if (LoggedInUser.UserType == "N")
                            {
                                if (atot.StatusID == "C")
                                {
                                    //atot.NtoPDateTime = DateTime.Now;
                                    atot.CancelByID = LoggedInUser.UserID;
                                }
                                else
                                {
                                    atot.ForwardToID = Convert.ToInt32(Request.Form["RecommendID"]);
                                    //atot.NtoPDateTime = DateTime.Now;
                                    //atot.NtoPUserID = LoggedInUser.UserID;
                                    //atot.OTProcessingPeriod = model.PeriodID;
                                }
                            }
                            else if (LoggedInUser.UserType == "R")
                            {
                                if (atot.StatusID == "R")
                                {
                                    atot.PtoFCDateTime = DateTime.Now;
                                    atot.RejectByID = LoggedInUser.UserID;
                                }
                                else
                                {
                                    atot.ForwardToID = Convert.ToInt32(Request.Form["RecommendID"]);
                                    atot.PtoFCDateTime = DateTime.Now;
                                    atot.PtoFCUserID = LoggedInUser.UserID;
                                    atot.OTProcessingPeriodID = model.PeriodID;
                                }
                            }
                            else if (LoggedInUser.UserType == "P")
                            {
                                if (atot.StatusID == "R")
                                {
                                    atot.FtoARDateTime = DateTime.Now;
                                    atot.RejectByID = LoggedInUser.UserID;
                                }
                                else
                                {
                                    atot.ApprovedByID = LoggedInUser.UserID;
                                    atot.FtoARDateTime = DateTime.Now;
                                    atot.FtoARUserID = LoggedInUser.UserID;
                                    atot.OTProcessingPeriodID = model.PeriodID;
                                }
                            }
                            db.SaveChanges();
                        }
                        return RedirectToAction("REmpPending", new { DeptID = emp.SectionID, PayrollPeriodID = prp.PID });
                    }
                }
            }
            ModelSOTPEmpDetail vm = new ModelSOTPEmpDetail();
            //vm = GetDetailPending(model.PeriodID, (int)emp.EmployeeID, LoggedInUser);
            vm.Message = messages;
            return View(vm);
        }

        public ModelSOTPEmpDetail GetDetailPendingHR(int prid, int EmpID, ViewUserEmp LoggedInUser)
        {

            ModelSOTPEmpDetail vm = new ModelSOTPEmpDetail();
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), prid);
            HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == EmpID);
            if (DateTime.Today > prp.SupervisorCutOffDate)
                vm.IsLate = true;
            else
                vm.IsLate = false;
            vm.EmpID = (int)EmpID;
            vm.EmpName = emp.FullName;
            int amount = 0;
            if (LoggedInUser.UserType == "H" || LoggedInUser.UserType == "A")
            {
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            }
            else
            {
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            }
            vm.List = OTHelperRecommended.GetConvertedDailyOTListSimple(db.ViewDailyOTEntries.Where(aa => aa.EmpID == EmpID && aa.OTProcessingPeriodID == prp.PID).ToList());
            vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(OTHelperManager.ConvertDeptIDList(emp.SectionID), db.BG_OTDivision.ToList(), (int)prp.FinYearID);
            vm.PeriodName = prp.PName;
            vm.PeriodID = prp.PID;
            vm.OTPolicy = "Policy Details: Maximum Days in Week = " + emp.Att_OTPolicy.DaysInWeek.ToString() + " , Maximum Days in Month = " + emp.Att_OTPolicy.DaysInMonth.ToString();
            vm.TotalAmount = amount;
            vm.Count = vm.List.Count;
            return vm;
        }
        #endregion
        #endregion
	}
}