using HRM_IKAN.Authentication;
using PTAWMS.App_Start;
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
    public class OTSupervisorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        HRMEntities db = new HRMEntities();
        [HttpGet]
        public ActionResult SearchEmps(string sortOrder, string searchString, string currentFilter, int? page)
        {
            List<ViewUserEmp> emps = new List<ViewUserEmp>();
            emps = db.ViewUserEmps.Where(aa => aa.Status == true).ToList();
            return View(emps);
        }
        #region -- Department Pending OT --
        [HttpGet]
        public ActionResult SOTDeptList(int? PayrollPeriodID)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<short?> deptIds = new List<short?>();
            List<short?> divIds = new List<short?>();
            List<int?> empIds = new List<int?>();
            List<EmpView> totalEmps = new List<EmpView>();
            List<EmpView> emps = new List<EmpView>();
            List<PR_PayrollPeriod> activePeriods = new List<PR_PayrollPeriod>();
            List<int?> activePeriodIds = new List<int?>();
            int SelectedPeriodID = 0;
            List<ViewDailyOTEntry> otList = new List<ViewDailyOTEntry>();
            PR_PayrollPeriod prp = new PR_PayrollPeriod();
            ModelRDeptPending vm = new ModelRDeptPending();
            totalEmps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            SelectedPeriodID = Convert.ToInt32(Session["PRID"].ToString());
            if (PayrollPeriodID == null) { PayrollPeriodID = SelectedPeriodID; } else { Session["PRID"] = PayrollPeriodID.ToString(); }
            activePeriodIds = db.ViewDailyOTEntries.Where(aa =>(aa.SupervisorID == LoggedInUser.EmployeeID || aa.ForwardToID == LoggedInUser.EmployeeID) && (aa.StatusID == "P" || aa.StatusID == "F")).Select(aa => aa.PeriodID).Distinct().ToList();
            activePeriods = OTHelperManager.GetActivePeriods(db.PR_PayrollPeriod.ToList(),activePeriodIds);
            prp = OTHelperManager.GetPayrollPeriod(activePeriods, (int)PayrollPeriodID);
            empIds = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && (aa.SupervisorID == LoggedInUser.EmployeeID || aa.ForwardToID == LoggedInUser.EmployeeID) && (aa.StatusID == "P" || aa.StatusID == "F")).Select(aa => aa.EmployeeID).Distinct().ToList();
            emps = OTHelperManager.GetEmployeeConvertedFromIds(empIds, totalEmps);
            deptIds = OTHelperManager.GetDeptIDs(emps);
            divIds = OTHelperManager.GetDivisionIDs(deptIds, db.HR_Section.ToList());
            if(deptIds.Count ==1)
                return RedirectToAction("SOTEmpList",new { DeptID = deptIds.First(), PayrollPeriodID = prp.PID});
            else
            {
                otList = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && (aa.SupervisorID == LoggedInUser.EmployeeID || aa.ForwardToID == LoggedInUser.EmployeeID) && (aa.StatusID == "P" || aa.StatusID == "F")).ToList();
                vm.List = OTHelperRecommended.GetConvertedOTListDepts(otList, emps, deptIds);
                vm.Count = vm.List.Count;
                vm.PayrollPeriodID = (int)PayrollPeriodID;
                vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(divIds, db.BG_OTDivision.ToList(), (int)prp.FinYearID);
                vm.TotalOTAmount = OTHelperManager.GetTotalOTAmount(vm.List);
                ViewBag.PayrollPeriodID = new SelectList(activePeriods, "PID", "PName", PayrollPeriodID); 
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                vm.Message = new List<string>();
                return View(vm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SOTDeptList()
        {

            return View();
        }
        #endregion
        #region -- Employee Pending OT--
        public ModelSOTEmpList GetEmpPending(ViewUserEmp LoggedInUser, int? PayrollPeriodID, int DeptID)
        {
            // Varibale Initilization
            ModelSOTEmpList vm = new ModelSOTEmpList();
            List<short?> deptIds = new List<short?>();
            List<short?> divIds = new List<short?>();
            List<int?> empIds = new List<int?>();
            List<EmpView> totalEmps = new List<EmpView>();
            List<EmpView> emps = new List<EmpView>();
            List<PR_PayrollPeriod> activePeriods = new List<PR_PayrollPeriod>();
            List<int?> activePeriodIds = new List<int?>();
            PR_PayrollPeriod prp = new PR_PayrollPeriod();
            int SelectedPeriodID = 0;
            List<ViewDailyOTEntry> otList = new List<ViewDailyOTEntry>();
            deptIds.Add((short)DeptID);
            divIds = OTHelperManager.GetDivisionIDs(deptIds, db.HR_Section.ToList());
            totalEmps = db.EmpViews.Where(aa => aa.Status == "Active").ToList(); 

            // Get Payroll Period
            SelectedPeriodID = Convert.ToInt32(Session["PRID"].ToString());
            if (PayrollPeriodID == null) { PayrollPeriodID = SelectedPeriodID; } else { Session["PRID"] = PayrollPeriodID.ToString(); }
            activePeriodIds = db.ViewDailyOTEntries.Where(aa => (aa.SupervisorID == LoggedInUser.EmployeeID && aa.StatusID == "P") || ( aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.EmployeeID)).Select(aa => aa.PeriodID).Distinct().ToList();
            activePeriods = OTHelperManager.GetActivePeriods(db.PR_PayrollPeriod.ToList(), activePeriodIds);
            prp = OTHelperManager.GetPayrollPeriod(activePeriods, (int)PayrollPeriodID);

            empIds = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && ((aa.SupervisorID == LoggedInUser.EmployeeID && aa.StatusID == "P") || (aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.EmployeeID))).Select(aa => aa.EmployeeID).Distinct().ToList();
            emps = OTHelperManager.GetEmployeeConvertedFromIds(empIds, totalEmps);

            otList = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && ((aa.SupervisorID == LoggedInUser.EmployeeID && aa.StatusID == "P") || (aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.EmployeeID))).ToList();
            vm.DeptID = (int)DeptID;
            vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(divIds, db.BG_OTDivision.ToList(), (int)prp.FinYearID);
            vm.List = OTHelperRecommended.GetConvertedOTListEmp(otList, emps);
            if (DateTime.Today > prp.SupervisorCutOffDate)
                vm.IsLate = true;
            else
                vm.IsLate = false;
            if (vm.List.Count() > 1)
            {
                vm.TotalOTAmount = vm.List.Sum(aa=>aa.OTAmount);
                vm.SystemOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.SystemOTMins));
                vm.ClaimedOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.ClaimedOTMins));
                vm.TotalEmps = vm.List.Count().ToString();              
            }
            if (vm.List.Count() > 0)
            {
                vm.RecommendID = (int)LoggedInUser.ReportingToID;
                vm.RecommenderName = db.EmpViews.First(aa => aa.EmployeeID == LoggedInUser.ReportingToID).FullName;
            }
            vm.Count = vm.List.Count;
            ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
            ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            ViewBag.PayrollPeriodID = new SelectList(activePeriods, "PID", "PName", PayrollPeriodID);
            return vm;
        }
        [HttpGet]
        public ActionResult SOTEmpList(int? DeptID, int? PayrollPeriodID)
        {
            ModelSOTEmpList vm = new ModelSOTEmpList();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            vm = GetEmpPending(LoggedInUser, (int)PayrollPeriodID, (int)DeptID);
            vm.Message = new List<string>();
            return View(vm);
        }
        [HttpPost]
        public ActionResult SOTEmpList(ModelSOTEmpList model)
        {
            model.RecommendID = Convert.ToInt32(Request.Form["RecommendID"]);
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var empsChecked = Request.Form.GetValues("cbEmployee");
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            List<EmpView> tempEmps = new List<EmpView>();
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)model.PayrollPeriodID);
            List<ViewDailyOTEntry> otList = new List<ViewDailyOTEntry>();
            List<string> messages = new List<string>();
            messages = IsValidateEmpList(model, empsChecked);
            if (messages.Count == 0)
            {
                foreach (var id in empsChecked)
                {
                    int chNo = Convert.ToInt32(id);
                    tempEmps.Add(emps.First(aa => aa.EmployeeID == chNo));
                }
                otList = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate).ToList();
                List<ViewDailyOTEntry> vdat = OTHelperManager.GetOTListEmp(otList, tempEmps);
                foreach (var emp in tempEmps)
                {
                    //DeptID = (int)emp.SecID;
                    if (emp.DaysInMonth > 0)
                    {
                        // check for Monthly Limit
                        if (vdat.Where(aa => aa.EmpID == emp.EmployeeID).Count() > emp.DaysInMonth)
                            messages.Add("Your Monthly overtime claim limit exceeds for:" + emp.FullName);
                        else
                        {
                            // check for weekly limit
                            if (emp.DaysInWeek > 0)
                            {
                                if (OTHelperManager.IsValidWeekPolicy(vdat.Where(aa => aa.EmpID == emp.EmployeeID).OrderByDescending(aa => aa.OTDate).ToList(), prp, emp.DaysInWeek))
                                    messages.Add("Your weekly overtime claim limit exceeds for:" + emp.FullName);
                            }
                        }
                    }
                    // check for daily ot limit
                    {
                        if (OTHelperManager.IsValidDailyOT(vdat.Where(aa => aa.EmpID == emp.EmployeeID), emp))
                            messages.Add("Your daily overtime claim limit exceeds for:"+ emp.FullName);
                    }
                }
            }
            if (messages.Count == 0)
            {
                int CurrentPeriodID = db.PR_PayrollPeriod.OrderByDescending(aa => aa.PID).First().PID;
                List<Att_OTDailyEntry> attOts = db.Att_OTDailyEntry.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate).ToList();
                foreach (var emp in tempEmps)
                {
                    foreach (var atot in attOts.Where(aa => aa.EmpID == emp.EmployeeID).ToList())
                    {
                        // Save Log
                        db.Att_OTDForward.Add(OTHelperManager.CreateOTLog(atot, LoggedInUser, model.RecommendID,model.Justification,model.DecisionID, "Forward", CurrentPeriodID));
                        atot.StatusID = model.DecisionID;
                        if (atot.StatusID == "C")
                        {
                            atot.PtoFCDateTime = DateTime.Now;
                            atot.CancelByID = LoggedInUser.UserID;
                            atot.PtoFCUserID = LoggedInUser.UserID;
                            atot.OTProcessingPeriodID = CurrentPeriodID;
                        }
                        else
                        {
                            if (model.IsLate == true)
                                atot.Remarks = model.Justification;
                            atot.ForwardToID = model.RecommendID;
                            atot.PtoFCDateTime = DateTime.Now;
                            atot.PtoFCUserID = LoggedInUser.UserID;
                            atot.OTProcessingPeriodID = CurrentPeriodID;
                        }
                    }
                    db.SaveChanges();
                    // Cancel All others
                    foreach (var item in attOts.Where(aa => aa.EmpID == emp.EmployeeID && aa.StatusID == "P").ToList())
                    {
                        item.StatusID = "C";
                        item.PtoFCDateTime = DateTime.Now;
                        item.PtoFCUserID = LoggedInUser.UserID;
                        item.CancelByID = LoggedInUser.UserID;
                        item.OTProcessingPeriodID = CurrentPeriodID;
                    }
                    db.SaveChanges();
                }
                return Json("OK");
            }
            else
            {

            }
            return Json(messages);
        }
        private List<string> IsValidateEmpList(ModelSOTEmpList model, string[] checkedEmpDates)
        {
            List<string> messages = new List<string>();
            if (model.IsLate == true)
            {
                if (model.Justification == null || model.Justification == "")
                    messages.Add("Please provide justification for late subission of overtime");
            }
            if (model.Certified == false)
                messages.Add("Please verify this employee does not claim daily overtime allowance on these dates");
            if (checkedEmpDates != null)
            {
            }
            else
                messages.Add("Select Atleast One Entry");
            return messages;
        }
        #endregion
        #region -- Employee Detail Pending OT --
        [HttpGet]
        public ActionResult SOTDetailList(int? EmpID, int? PayrollPeriodID)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            ModelSOTPEmpDetail vm = new ModelSOTPEmpDetail();
            int prid = Convert.ToInt32(Session["PRID"].ToString());
            vm = GetDetailPending(prid, (int)EmpID, LoggedInUser);
            vm.Message = new List<string>();
            return View(vm);
        }
        [HttpPost]
        public ActionResult SOTDetailList(ModelSOTPEmpDetail model)
        {
            List<string> messages = new List<string>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            var checkedEmpDates = Request.Form.GetValues("cbEmployee");
            HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == model.EmpID);
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), model.PeriodID);
            messages = IsValidate(model,checkedEmpDates);
            if (messages.Count == 0)
            {
                List<Att_OTDailyEntry> attOts = db.Att_OTDailyEntry.Where(aa => aa.EmpID == model.EmpID && aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate).ToList();
                List<Att_OTDailyEntry> TempattOts = new List<Att_OTDailyEntry>();
                foreach (var id in checkedEmpDates)
                {
                    int chNo = Convert.ToInt32(id);
                    string EmpDate = Request.Form["ED" + (chNo - 1).ToString()].ToString();
                    string COTHour = Request.Form["ClaimOT" + (chNo - 1).ToString()].ToString();
                    int COTMins = OTHelperManager.ConvertStringTimeToMin(COTHour);
                    Att_OTDailyEntry obj = attOts.First(aa => aa.EmpDate == EmpDate);
                    obj.ApprovedOTMin = COTMins;
                    obj.OTAmount = OTHelperManager.GetOTAmount(emp.HR_Grade, obj.ApprovedOTMin,obj.DutyCode);
                    TempattOts.Add(obj);
                }
                if (emp.Att_OTPolicy.DaysInMonth > 0)
                {
                    List<Att_OTDailyEntry> atoTemp = new List<Att_OTDailyEntry>();
                    atoTemp.AddRange(TempattOts);
                    atoTemp.AddRange(attOts.Where(aa => aa.StatusID == "F" || aa.StatusID == "A").ToList());
                    // check for Monthly Limit
                    if (atoTemp.Count > emp.Att_OTPolicy.DaysInMonth)
                        messages.Add("Your Monthly overtime claim limit exceeds.");
                    else
                    {
                        // check for weekly limit
                        if (emp.Att_OTPolicy.DaysInWeek > 0)
                        {
                            if (OTHelperManager.IsValidWeekPolicy(atoTemp.OrderByDescending(aa => aa.OTDate).ToList(), prp, emp.Att_OTPolicy.DaysInWeek))
                                messages.Add("Your weekly overtime claim limit exceeds.");
                        }
                    }
                }
                // check for daily ot limit
                {
                    List<string> msgs = OTHelperManager.IsValidDailyOT(TempattOts, emp.Att_OTPolicy);
                    if (msgs.Count>0)
                        messages.AddRange(msgs);
                }
                if (checkedEmpDates != null)
                {
                    if (messages.Count == 0)
                    {
                        int CurrentPeriodID = db.PR_PayrollPeriod.OrderByDescending(aa=>aa.PID).First().PID;
                        foreach (var id in checkedEmpDates)
                        {
                            int chNo = Convert.ToInt32(id);
                            string EmpDate = Request.Form["ED" + (chNo - 1).ToString()].ToString();
                            string COTHour = Request.Form["ClaimOT" + (chNo - 1).ToString()].ToString();
                            Att_OTDailyEntry atot = attOts.First(aa => aa.EmpDate == EmpDate);
                            // Save Log
                            db.Att_OTDForward.Add(OTHelperManager.CreateOTLog(atot, LoggedInUser, model.RecommendID, model.Justification, model.DecisionID, "Forward", CurrentPeriodID));
                            atot.StatusID = model.DecisionID;
                            if (LoggedInUser.UserType == "N")
                            {
                                if (atot.StatusID == "C")
                                {
                                    atot.PtoFCDateTime = DateTime.Now;
                                    atot.PtoFCUserID = LoggedInUser.UserID;
                                    atot.CancelByID = LoggedInUser.UserID;
                                    atot.OTProcessingPeriodID = CurrentPeriodID;
                                }
                                else
                                {
                                    if(model.IsLate==true)
                                        atot.Remarks = model.Justification;
                                    atot.ForwardToID = model.RecommendID;
                                    atot.PtoFCDateTime = DateTime.Now;
                                    atot.PtoFCUserID = LoggedInUser.UserID;
                                    atot.OTProcessingPeriodID = CurrentPeriodID;
                                }
                            }
                        }
                        db.SaveChanges();
                        // Cancel All others
                        foreach (var item in attOts.Where(aa => aa.EmpID == model.EmpID && aa.StatusID == "P").ToList())
                        {
                            item.StatusID = "C";
                            item.PtoFCDateTime = DateTime.Now;
                            item.PtoFCUserID = LoggedInUser.UserID;
                            item.CancelByID = LoggedInUser.UserID;
                            item.OTProcessingPeriodID = CurrentPeriodID;
                        }
                        db.SaveChanges();
                        return Json("OK");
                    }
                }
                else
                    messages.Add("No Entry Selected");
            }
            return Json(messages);
        }
        private List<string> IsValidate(ModelSOTPEmpDetail model, string[] checkedEmpDates)
        {
            List<string> messages = new List<string>();
            if (model.IsLate == true)
            {
                if (model.Justification == null || model.Justification == "")
                    messages.Add("Please provide justification for late subission of overtime");
            }
            if (model.Certified == false)
                messages.Add("Please verify this employee does not claim daily overtime allowance on these dates");
            if (checkedEmpDates!=null)
            {
                foreach (var id in checkedEmpDates)
                {
                    int chNo = Convert.ToInt32(id);
                    string COTHour = Request.Form["ClaimOT" + (chNo - 1).ToString()].ToString();
                    if (COTHour.Count() == 4)
                    {
                        try
                        {
                            int COTMins = OTHelperManager.ConvertStringTimeToMin(COTHour);
                        }
                        catch (Exception ex)
                        {
                            messages.Add("Claimed Overtime is not in correct format");
                        }
                    }
                    else
                        messages.Add("Claimed Overtime is not in correct format");
                } 
            }
            else
                messages.Add("Select Atleast One Entry");
            return messages;
        }
        public ModelSOTPEmpDetail GetDetailPending(int prid, int EmpID, ViewUserEmp LoggedInUser)
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
            vm.DeptID = (int)emp.SectionID;
            vm.List = OTHelperRecommended.GetConvertedDailyOTList(db.ViewDailyOTEntries.Where(aa => aa.EmployeeID == vm.EmpID && aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && ((aa.SupervisorID == LoggedInUser.EmployeeID && aa.StatusID == "P") || (aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.EmployeeID))).ToList());
            if (vm.List.Count > 0)
            {
                vm.SystemOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.SystemOTMins));
                vm.ClaimedOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.ClaimedOTMins));
                vm.TotalDays = vm.List.Count();
                vm.TotalAmount = vm.List.Sum(aa => aa.OTAmount);
            }
            if (vm.List.Count() > 0)
            {
                vm.RecommendID = (int)LoggedInUser.ReportingToID;
                vm.RecommendName = db.EmpViews.First(aa => aa.EmployeeID == LoggedInUser.ReportingToID).FullName;
            }
            vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(OTHelperManager.GetDivisionIDs(OTHelperManager.ConvertDeptIDList(emp.SectionID),db.HR_Section.ToList()), db.BG_OTDivision.ToList(), (int)prp.FinYearID);
            vm.PeriodName = prp.PName;
            vm.PeriodID = prp.PID;
            vm.OTPolicy = "OT Days Forward Policy: Maximum Days in Week = " + emp.Att_OTPolicy.DaysInWeek.ToString() + " , Maximum Days in Month = " + emp.Att_OTPolicy.DaysInMonth.ToString();
            vm.OTPolicyDays = "OT Claimed Hours Policy: Normal Day= " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayOTStartLimitHour) + " to " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayOTEndLimitHour) + ", Rest & GZ Day= " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayGOTStartLimitHour) + " to " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayGOTEndLimitHour);
            vm.Count = vm.List.Count;
            ViewBag.DecisionID = new SelectList(db.Att_OTStatus.Where(aa => aa.PSID == "F" || aa.PSID == "C").ToList(), "PSID", "StatusName", "F");
            ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active" && aa.UserType == "R").ToList()), "UserID", "FullName");
            return vm;
        }
        #endregion
	}
}