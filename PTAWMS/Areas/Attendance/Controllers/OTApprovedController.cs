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
    public class OTApprovedController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        HRMEntities db = new HRMEntities();

        #region -- Department Pending OT --
        [HttpGet]
        public ActionResult HAOTDeptList(int? PayrollPeriodID)
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
            if (LoggedInUser.UserType != "H" && LoggedInUser.UserType != "A")
                totalEmps = OTHelperManager.GetEmployees(totalEmps, LoggedInUser);
            SelectedPeriodID = Convert.ToInt32(Session["PRID"].ToString());
            if (PayrollPeriodID == null) { PayrollPeriodID = SelectedPeriodID; } else { Session["PRID"] = PayrollPeriodID.ToString(); }
            //activePeriodIds = db.ViewDailyOTEntries.Where(aa => ((aa.SupervisorID == LoggedInUser.EmployeeID && aa.StatusID == "P") || (aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.EmployeeID))).Select(aa => aa.PeriodID).Distinct().ToList();
            //activePeriods = OTHelperManager.GetActivePeriods(db.PR_PayrollPeriod.ToList(), activePeriodIds);
            prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)PayrollPeriodID);
            empIds = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID==prp.PID && aa.StatusID=="A").Select(aa => aa.EmployeeID).Distinct().ToList();
            emps = OTHelperManager.GetEmployeeConvertedFromIds(empIds, totalEmps);
            deptIds = OTHelperManager.GetDeptIDs(emps);
            divIds = OTHelperManager.GetDivisionIDs(deptIds, db.HR_Section.ToList());
            if (deptIds.Count == 1)
                return RedirectToAction("HAOTEmpList", new { DeptID = deptIds.First(), PayrollPeriodID = prp.PID });
            else
            {
                otList = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID==prp.PID && aa.StatusID=="A").ToList();

                vm.List = OTHelperRecommended.GetConvertedOTListDepts(otList, emps, deptIds);
                vm.Count = vm.List.Count;
                vm.PayrollPeriodID = (int)PayrollPeriodID;
                if (prp.FinYearID != null) { vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(divIds, db.BG_OTDivision.ToList(), (int)prp.FinYearID); }
                vm.TotalOTAmount = OTHelperManager.GetTotalOTAmount(vm.List);

                ViewBag.PayrollPeriodID = new SelectList(activePeriods, "PID", "PName", PayrollPeriodID);
                ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForSupervisor(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
                //ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForSupervisor(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active" && aa.UserType == "R" && aa.sec).ToList()), "UserID", "FullName");
                vm.Message = new List<string>();
                return View(vm);
            }
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
            if(LoggedInUser.UserType!="H" && LoggedInUser.UserType!="A")
                totalEmps = OTHelperManager.GetEmployees(totalEmps, LoggedInUser);
            // Get Payroll Period
            SelectedPeriodID = Convert.ToInt32(Session["PRID"].ToString());
            if (PayrollPeriodID == null) { PayrollPeriodID = SelectedPeriodID; } else { Session["PRID"] = PayrollPeriodID.ToString(); }
            prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)PayrollPeriodID);

            empIds = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID==prp.PID && aa.SecID == DeptID && aa.StatusID=="A").Select(aa => aa.EmployeeID).Distinct().ToList();
            emps = OTHelperManager.GetEmployeeConvertedFromIds(empIds, totalEmps);

            otList = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID==prp.PID && aa.SecID == DeptID && aa.StatusID=="A").ToList();
            vm.DeptID = (int)DeptID;
            if (prp.FinYearID != null) { vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(divIds, db.BG_OTDivision.ToList(), (int)prp.FinYearID); }
            vm.List = OTHelperRecommended.GetConvertedOTListEmp(otList, emps);
            //if (DateTime.Today > prp.SupervisorCutOffDate)
            //    vm.IsLate = true;
            //else
            vm.IsLate = false;
            if (vm.List.Count() > 1)
            {
                vm.TotalOTAmount = vm.List.Sum(aa => aa.OTAmount);
                vm.SystemOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.SystemOTMins));
                vm.ClaimedOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.ClaimedOTMins));
                vm.TotalEmps = vm.List.Count().ToString();
            }
            vm.Count = vm.List.Count;
            ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForApprover(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
            ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForRecommender(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            ViewBag.PayrollPeriodID = new SelectList(db.PR_PayrollPeriod.OrderByDescending(aa=>aa.PID).ToList(), "PID", "PName", PayrollPeriodID);
            return vm;
        }
        [HttpGet]
        public ActionResult HAOTEmpList(int? DeptID, int? PayrollPeriodID)
        {
            ModelSOTEmpList vm = new ModelSOTEmpList();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            vm = GetEmpPending(LoggedInUser, (int)PayrollPeriodID, (int)DeptID);
            vm.Message = new List<string>();
            return View(vm);
        }
        #endregion

        #region -- Employee Detail Pending OT --
        [HttpGet]
        public ActionResult HAOTDetailList(int? EmpID, int? PayrollPeriodID)
        {
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            ModelSOTPEmpDetail vm = new ModelSOTPEmpDetail();
            int prid = Convert.ToInt32(Session["PRID"].ToString());
            vm = GetDetailPending(prid, (int)EmpID, LoggedInUser);

            vm.Message = new List<string>();
            return View(vm);
        }

        public ModelSOTPEmpDetail GetDetailPending(int prid, int EmpID, ViewUserEmp LoggedInUser)
        {

            ModelSOTPEmpDetail vm = new ModelSOTPEmpDetail();
            PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), prid);
            HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == EmpID);
            //if (DateTime.Today > prp.SupervisorCutOffDate)
            //    vm.IsLate = true;
            //else
            vm.IsLate = false;
            vm.EmpID = (int)EmpID;
            vm.EmpName = emp.FullName;
            vm.DeptID = (int)emp.SectionID;
            vm.List = OTHelperRecommended.GetConvertedDailyOTList(db.ViewDailyOTEntries.Where(aa => aa.EmployeeID == vm.EmpID && aa.StatusID=="A" && aa.OTProcessingPeriodID==prp.PID).ToList());
            if (vm.List.Count > 0)
            {
                vm.SystemOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.SystemOTMins));
                vm.ClaimedOT = OTHelperManager.ConverMinIntoHours(vm.List.Sum(aa => aa.ClaimedOTMins));
                vm.TotalDays = vm.List.Count();
                vm.TotalAmount = vm.List.Sum(aa => aa.OTAmount);
            }
            vm.DivRemainingBudget = OTHelperManager.GetDivRemainingBudget(OTHelperManager.GetDivisionIDs(OTHelperManager.ConvertDeptIDList(emp.SectionID), db.HR_Section.ToList()), db.BG_OTDivision.ToList(), (int)prp.FinYearID);
            vm.PeriodName = prp.PName;
            vm.PeriodID = prp.PID;
            vm.OTPolicy = "OT Days Forward Policy: Maximum Days in Week = " + emp.Att_OTPolicy.DaysInWeek.ToString() + " , Maximum Days in Month = " + emp.Att_OTPolicy.DaysInMonth.ToString();
            vm.OTPolicyDays = "OT Claimed Hours Policy: Normal Day= " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayOTStartLimitHour) + " to " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayOTEndLimitHour) + ", Rest & GZ Day= " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayGOTStartLimitHour) + " to " + OTHelperManager.ConverMinIntoHours((int)emp.Att_OTPolicy.PerDayGOTEndLimitHour);
            vm.Count = vm.List.Count;

            ViewBag.DecisionID = new SelectList(OTHelperManager.GetOTStatusForApprover(db.Att_OTStatus.ToList()), "PSID", "StatusName", "F");
            ViewBag.RecommendID = new SelectList(OTHelperManager.GetUsersForRecommender(db.ViewUserEmps.Where(aa => aa.EmpStatus == "Active").ToList()), "UserID", "FullName");
            return vm;
        }

        #endregion
	}
}