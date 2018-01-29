using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public static class OTHelper
    {
        
        #region --Employeee OT Function --
        public static List<ModelComboBox> GetListOfUsers(List<ViewUserEmp> list)
        {
            List<ModelComboBox> vm = new List<ModelComboBox>();
            foreach (var item in list)
            {
                if (item.EmpID != null)
                {
                    ModelComboBox obj = new ModelComboBox();
                    obj.ID = (int)item.EmpID;
                    obj.Name = item.FullName;
                    vm.Add(obj);
                }
            }
            return vm;
        }
        //public static List<ModelOTOtherEntries> GetConvertedDailyOTOtherList(List<ViewDailyOTEntry> list)
        //{
        //    List<ModelOTOtherEntries> vm = new List<ModelOTOtherEntries>();
        //    foreach (var item in list.OrderBy(aa => aa.OTDate).ToList())
        //    {
        //        ModelOTOtherEntries obj = new ModelOTOtherEntries();
        //        obj.EmpDate = item.EmpDate;
        //        obj.ClaimedOTHours = ConverMinIntoHour(item.ApprovedOTMin);
        //        obj.Date = ConverDateIntoDayString(item.OTDate);
        //        if (item.OTAmount != null)
        //            obj.OTAmount = item.OTAmount.ToString();
        //        else
        //            obj.OTAmount = "";
        //        obj.SystemOTHours = ConverMinIntoHours(item.ActualOTMin);
        //        if (item.TimeIn != null)
        //            obj.TimeIN = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
        //        else
        //            obj.TimeIN = "";
        //        if (item.TimeOut != null)
        //            obj.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
        //        else
        //            obj.TimeOut = "";
        //        obj.WorkHours = ConverMinIntoHours(item.WorkMin);
        //        obj.StatusID = item.StatusID;
        //        obj.StatusName = item.StatusName;
        //        if (obj.StatusID == "F")
        //            obj.StatusName = obj.StatusName + ": " + item.UserForName;
        //        if (obj.StatusID == "R")
        //            obj.StatusName = obj.StatusName + ": " + item.UserRejName;
        //        if (obj.StatusID == "C")
        //            obj.StatusName = obj.StatusName + ": " + item.UserCancelName;
        //        if (obj.StatusID == "P")
        //            obj.StatusName = obj.StatusName + ": " + item.SupervidorUserName;
        //        vm.Add(obj);
        //    }
        //    return vm;
        //}
        //public static List<ModelOTApprovedEntries> GetConvertedDailyOTAppList(List<ViewDailyOTEntry> list)
        //{
        //    List<ModelOTApprovedEntries> vm = new List<ModelOTApprovedEntries>();
        //    foreach (var item in list.OrderBy(aa => aa.OTDate).ToList())
        //    {
        //        ModelOTApprovedEntries obj = new ModelOTApprovedEntries();
        //        obj.EmpDate = item.EmpDate;
        //        obj.ClaimedOTHours = ConverMinIntoHour(item.ApprovedOTMin);
        //        obj.Date = ConverDateIntoDayString(item.OTDate);
        //        if (item.OTAmount != null)
        //            obj.OTAmount = item.OTAmount.ToString();
        //        else
        //            obj.OTAmount = "";
        //        obj.SystemOTHours = ConverMinIntoHours(item.ActualOTMin);
        //        if (item.TimeIn != null)
        //            obj.TimeIN = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
        //        else
        //            obj.TimeIN = "";
        //        if (item.TimeOut != null)
        //            obj.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
        //        else
        //            obj.TimeOut = "";
        //        obj.WorkHours = ConverMinIntoHours(item.WorkMin);
        //        obj.ApproveByName = item.UserAppName;
        //        vm.Add(obj);
        //    }
        //    return vm;
        //}
        public static List<Att_OTStatus> GetOTStatus(List<Att_OTStatus> list)
        {
            try
            {
                List<Att_OTStatus> temList = new List<Att_OTStatus>();

                Att_OTStatus es = new Att_OTStatus();
                es.PSID = "L";
                es.StatusName = "All";
                temList.Add(es);
                temList.AddRange(list);
                return temList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region --Supervisor OT Function --

        public static int GetOTAmount(List<ViewDailyOTEntry> list)
        {
            int otHours = 0;
            foreach (var item in list)
            {
                if (item.OTAmount > 0)
                    otHours = (int)(otHours + (item.OTAmount));
            }
            return otHours;
        }

        public static int GetEmployeeCOTHour(IEnumerable<ViewDailyOTEntry> list)
        {
            int otHours = 0;
            foreach (var item in list)
            {
                if (item.ApprovedOTMin > 0)
                    otHours = (int)(otHours + (item.ApprovedOTMin));
            }
            return otHours;
        }
        public static int GetEmployeeSOTHour(IEnumerable<ViewDailyOTEntry> list)
        {
            int otHours = 0;
            foreach (var item in list)
            {
                if (item.ActualOTMin > 0)
                    otHours = (int)(otHours + (item.ActualOTMin));
            }
            return otHours;
        }
        #endregion


    }

    public static class OTHelperManager
    {
        internal static PR_PayrollPeriod GetPayrollPeriod(List<PR_PayrollPeriod> list, int CurrentPID)
        {
            if (list.Where(aa => aa.PID == CurrentPID).Count() > 0)
                return list.First(aa => aa.PID == CurrentPID);
            else
                return new PR_PayrollPeriod();
        }
        internal static int GetPayrollPeriodID(List<PR_PayrollPeriod> list, DateTime dateTime)
        {
            DateTime dt = new DateTime(dateTime.Year, dateTime.Month, 1);
            int pid = 0;
            if (list.Where(aa => aa.PStartDate == dt).Count() > 0)
                pid = list.Where(aa => aa.PStartDate == dt).First().PID;
            return pid;
        }

        internal static bool IsValidWeekPolicy(List<Att_OTDailyEntry> attOts, PR_PayrollPeriod prp, int? days)
        {
            DateTime dts = prp.PStartDate.Value;
            bool FindMonday = false;
            bool checkForPreWeek = false;
            while (dts <= prp.PEndDate)
            {
                if (FindMonday == false)
                {
                    if (dts.Date.DayOfWeek == DayOfWeek.Monday)
                        FindMonday = true;
                }
                if (FindMonday == true)
                {
                    DateTime dtStart = dts;
                    if (attOts.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate < dtStart).Count() > days)
                    {
                        checkForPreWeek = true;
                    }
                    while (dtStart <= prp.PEndDate)
                    {
                        DateTime dtEndDate = dtStart.AddDays(7);
                        if (attOts.Where(aa => aa.OTDate >= dtStart && aa.OTDate < dtEndDate).Count() > days)
                        {
                            checkForPreWeek = true;
                        }
                        dtStart = dtStart.AddDays(7);
                    }
                    break;
                }
                dts = dts.AddDays(1);
            }
            return checkForPreWeek;
        }

        internal static List<string> IsValidDailyOT(List<Att_OTDailyEntry> TempattOts, Att_OTPolicy att_OTPolicy)
        {
            List<string> messages = new List<string>();
            foreach (var item in TempattOts)
            {
                if (item.ApprovedOTMin < item.ActualOTMin)
                    messages.Add("You can not deduct OT Minutes according to the policy");
                if (item.DutyCode == "D")
                {
                    if (att_OTPolicy.CalculateNOT == true)
                    {
                        //if (att_OTPolicy.PerDayOTStartLimitHour > 0)
                        //{
                            //int ClaimOT = (int)(item.ApprovedOTMin / 60);
                            int ClaimOT = (int)(item.ApprovedOTMin);
                            if (ClaimOT > att_OTPolicy.PerDayOTEndLimitHour)
                            {
                                messages.Add("Claimed OT Exceeds for Date: " + item.OTDate.Value.ToString("dd-MM-yyyy"));
                            }
                        //}
                    }
                }
                else if (item.DutyCode == "R")
                {
                    if (att_OTPolicy.CalculateRestOT == true)
                    {
                        //if (att_OTPolicy.PerDayROTStartLimitHour > 0)
                        //{
                            int ClaimOT = (int)(item.ApprovedOTMin / 60);
                            if ( ClaimOT > att_OTPolicy.PerDayROTEndLimitHour)
                            {
                                messages.Add("Claimed OT Exceeds for Date: " + item.OTDate.Value.ToString("dd-MM-yyyy"));
                            }
                        //}
                    }
                }
                else if (item.DutyCode == "G")
                {
                    if (att_OTPolicy.CalculateGZOT == true)
                    {
                        //if (att_OTPolicy.PerDayGOTStartLimitHour > 0)
                        //{
                            int ClaimOT = (int)(item.ApprovedOTMin / 60);
                            if (ClaimOT > att_OTPolicy.PerDayGOTEndLimitHour)
                            {
                                messages.Add("Claimed OT Exceeds for Date: " + item.OTDate.Value.ToString("dd-MM-yyyy"));
                            }
                        //}
                    }
                }
            }
            return messages;
        }

        internal static int GetAmount(List<ViewDailyOTEntry> list)
        {
            try
            {
                if (list.Count > 0)
                    return (int)list.Sum(aa => aa.OTAmount);
                else
                    return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static List<ViewDailyOTEntry> GetOTListEmp(List<ViewDailyOTEntry> list, List<EmpView> emps)
        {
            List<ViewDailyOTEntry> vm = new List<ViewDailyOTEntry>();
            foreach (var emp in emps)
            {
                if (list.Where(aa => aa.EmpID == emp.EmployeeID).Count() > 0)
                {
                    vm.AddRange(list.Where(aa => aa.EmpID == emp.EmployeeID));
                }
            }
            return vm.OrderBy(aa => aa.EmpID).ToList();
        }
        public static List<short?> GetDeptIDs(List<EmpView> emps)
        {
            return emps.Select(aa => aa.SecID).Distinct().ToList();
        }
        public static List<EmpView> GetEmployees(List<EmpView> emps, ViewUserEmp LoggedInUser)
        {
            List<EmpView> nEmps = emps.Where(aa => aa.ReportingToID == LoggedInUser.EmpID).ToList();
            // Add LoggedInUser Employee object in list
            if (nEmps.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).Count() == 0)
                nEmps.AddRange(emps.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).ToList());
            List<EmpView> rEmps = GetReportingToEmps(emps, nEmps);
            if (rEmps.Count > 0)
            {
                while (true)
                {
                    rEmps = GetReportingToEmps(emps, rEmps).ToList();
                    nEmps.AddRange(rEmps);
                    if (rEmps.Count == 0)
                        return nEmps;
                    if (nEmps.Count() >= emps.Count())
                    {
                        return nEmps;

                    }
                }
            }
            else
                return nEmps;
        }
        private static List<EmpView> GetReportingToEmps(List<EmpView> emps, List<EmpView> checkemps)
        {
            List<EmpView> rEmps = new List<EmpView>();
            foreach (var emp in checkemps)
            {
                rEmps.AddRange(emps.Where(aa => aa.ReportingToID == emp.EmployeeID).ToList());

            }
            return rEmps;

        }
        public static string ConverDateIntoDayString(DateTime? date)
        {
            return String.Format("{0:ddd, MMM d, yyyy}", date);
        }

        public static string ConverMinIntoHours(int? ApprovedOtMin)
        {
            if (ApprovedOtMin > 0)
            {
                int hours = 0;
                int mins = 0;
                hours = (int)(ApprovedOtMin / 60);
                mins = (int)((float)(ApprovedOtMin) - (hours * 60));
                if(hours<9)
                    return hours.ToString("00") + ":" + mins.ToString("00");
                else
                    return hours + ":" + mins.ToString("00");

            }
            else
                return "";
        }
        public static string ConverMinIntoHour(int? ApprovedOtMin)
        {
            if (ApprovedOtMin > 0)
            {
                TimeSpan ts = new TimeSpan(0, (int)ApprovedOtMin, 0);
                return ts.Hours.ToString("00") + ts.Minutes.ToString("00");
            }
            else
                return "";
        }


        internal static int GetDivRemainingBudget(List<short?> deptIds, List<BG_OTDivision> OTBudgetList, int FinYearID)
        {
            int DivRemainingBudget = 0;
            foreach (var item in deptIds)
            {
                if (OTBudgetList.Where(aa => aa.DivID == item && aa.FinYear == FinYearID).Count() > 0)
                    DivRemainingBudget = DivRemainingBudget + (int)OTBudgetList.First(aa => aa.DivID == item && aa.FinYear == FinYearID).RemainingBudget;
            }
            return DivRemainingBudget;
        }
        internal static List<short?> GetDivisionIDs(List<short?> secIds, List<HR_Section> secs)
        {
            List<HR_Section> TempSecs = new List<HR_Section>();
            foreach (var item in secIds)
            {
                TempSecs.AddRange(secs.Where(aa => aa.SecID == item));
            }
            return TempSecs.Select(aa => aa.DepartmentID).Distinct().ToList();
        }

        internal static int GetTotalOTAmount(List<ModelRDeptPendingList> list)
        {
            int TotalOTAmount = 0;
            foreach (var item in list)
            {
                TotalOTAmount = (int)(TotalOTAmount + item.OTAmount);
            }
            return TotalOTAmount;
        }

        internal static List<Att_OTStatus> GetOTStatusForSupervisor(List<Att_OTStatus> list)
        {
            return list.Where(aa => aa.PSID == "F" || aa.PSID == "C").ToList();
        }
        internal static List<Att_OTStatus> GetOTStatusForRecommender(List<Att_OTStatus> list)
        {
            return list.Where(aa => aa.PSID == "F" || aa.PSID == "R").ToList();
        }
        internal static List<Att_OTStatus> GetOTStatusForApprover(List<Att_OTStatus> list)
        {
            return list.Where(aa => aa.PSID == "A" || aa.PSID == "R").ToList();
        }
        internal static List<ViewUserEmp> GetUsersForSupervisor(List<ViewUserEmp> enumerable)
        {
            List<ViewUserEmp> uemps = enumerable.Where(aa => aa.UserType == "H" || aa.UserType == "R").ToList();
            return uemps;
        }
        internal static List<ViewUserEmp> GetUsersForRecommender(List<ViewUserEmp> enumerable)
        {
            List<ViewUserEmp> uemps = enumerable.Where(aa => aa.UserType == "H" || aa.UserType == "P").ToList();
            return uemps;
        }
        internal static List<ViewUserEmp> GetUsersForApproval(List<ViewUserEmp> enumerable)
        {
            List<ViewUserEmp> uemps = enumerable.Where(aa => aa.UserType == "H" || aa.UserType == "P").ToList();
            return uemps;
        }

        internal static int GetTotalOTAmount(List<ModelSOTPDList> list)
        {
            int Amount = 0;
            foreach (var li in list)
            {
                if (li.OTAmount >0)
                    Amount = Amount + Convert.ToInt32(li.OTAmount);
            }
            return Amount;
        }

        internal static List<short?> ConvertDeptIDList(int? DeptID)
        {
            List<short?> depts = new List<short?>();
            depts.Add((short)DeptID);
            return depts;
        }
        internal static bool IsValidWeekPolicy(List<ViewDailyOTEntry> attOts, PR_PayrollPeriod prp, byte? days)
        {
            DateTime dts = prp.PStartDate.Value;
            bool FindMonday = false;
            bool checkForPreWeek = false;
            while (dts <= prp.PEndDate)
            {
                if (FindMonday == false)
                {
                    if (dts.Date.DayOfWeek == DayOfWeek.Monday)
                        FindMonday = true;
                }
                if (FindMonday == true)
                {
                    DateTime dtStart = dts;
                    if (attOts.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate < dtStart).Count() > days)
                    {
                        checkForPreWeek = true;
                    }
                    while (dtStart <= prp.PEndDate)
                    {
                        DateTime dtEndDate = dtStart.AddDays(7);
                        if (attOts.Where(aa => aa.OTDate >= dtStart && aa.OTDate < dtEndDate).Count() > days)
                        {
                            checkForPreWeek = true;
                        }
                        dtStart = dtStart.AddDays(7);
                    }
                    break;
                }
                dts = dts.AddDays(1);
            }
            return checkForPreWeek;
        }

        internal static bool IsValidDailyOT(IEnumerable<ViewDailyOTEntry> TempattOts, EmpView emp)
        {
            bool check = false;
            foreach (var item in TempattOts)
            {
                if (item.DutyCode == "D")
                {
                    if (emp.CalculateNOT == true)
                    {
                        if (emp.PerDayOTStartLimitHour > 0)
                        {
                            //int ClaimOT = (int)(item.ApprovedOTMin / 60);
                            int ClaimOT = (int)(item.ApprovedOTMin);
                            if (ClaimOT >= emp.PerDayOTStartLimitHour && ClaimOT <= emp.PerDayOTEndLimitHour)
                            {

                            }
                            else
                                check = true;
                        }
                    }
                }
                else if (item.DutyCode == "R")
                {
                    if (emp.CalculateRestOT == true)
                    {
                        if (emp.PerDayROTStartLimitHour > 0)
                        {
                            int ClaimOT = (int)(item.ApprovedOTMin / 60);
                            if (ClaimOT >= emp.PerDayROTStartLimitHour && ClaimOT <= emp.PerDayROTEndLimitHour)
                            {

                            }
                            else
                                check = true;
                        }
                    }
                }
                else if (item.DutyCode == "G")
                {
                    if (emp.CalculateGZOT == true)
                    {
                        if (emp.PerDayGOTStartLimitHour > 0)
                        {
                            int ClaimOT = (int)(item.ApprovedOTMin / 60);
                            if (ClaimOT >= emp.PerDayGOTStartLimitHour && ClaimOT <= emp.PerDayGOTEndLimitHour)
                            {

                            }
                            else
                                check = true;
                        }
                    }
                }
            }
            return check;
        }

        internal static PR_PayrollPeriod GetPayrollPeriod(List<PR_PayrollPeriod> list, int p, List<int?> PendingPeriodIds)
        {
            throw new NotImplementedException();
        }

        internal static List<PR_PayrollPeriod> GetActivePeriods(List<PR_PayrollPeriod> list, List<int?> activePeriodIds)
        {
            List<PR_PayrollPeriod> temp = new List<PR_PayrollPeriod>();
            foreach (var item in activePeriodIds)
                temp.AddRange(list.Where(aa => aa.PID == item).ToList());
            return temp;
        }

        internal static List<EmpView> GetEmployeeConvertedFromIds(List<int?> empIds, List<EmpView> totalEmps)
        {
            List<EmpView> temp = new List<EmpView>();
            foreach (var empid in empIds)
            {
                temp.AddRange(totalEmps.Where(aa => aa.EmployeeID == empid).ToList());
            }
            return temp;
        }

        internal static int ConvertStringTimeToMin(string COTHour)
        {
            string STimeInH = COTHour.Substring(0, 2);
            string STimeInM = COTHour.Substring(2, 2);
            return Convert.ToInt32(STimeInH) * 60 + Convert.ToInt32(STimeInM);
        }

        internal static double? GetOTAmount(HR_Grade hR_Grade, int? mins,string DutyCode)
        {
            double OTAmount = 0;
            switch (DutyCode)
            {
                case "D":
                    OTAmount = GetAmount(hR_Grade.NormalOTAmount, mins);
                    break;
                case "G":
                    OTAmount = GetAmount(hR_Grade.GZOTAmount, mins);
                    break;
                case "R":
                    OTAmount = GetAmount(hR_Grade.RestOTAmount, mins);
                    break;
            }
            return OTAmount;
        }

        private static double GetAmount(double? amount, int? minutes)
        {
            if (amount == null)
                amount = 0;
            int amountForOneMin = (int)(amount / 60);
            return (double)(amountForOneMin * minutes);
        }


        internal static Att_OTDForward CreateOTLog(Att_OTDailyEntry atot, ViewUserEmp LoggedInUser, int RecommendToID,string Justification,string StatusID, string Operation, int CurrentPeriodID)
        {
            Att_OTDForward atf = new Att_OTDForward();
            atf.OTDailyEntryID = atot.POTAID;
            atf.ForwardFrom = LoggedInUser.UserID;
            atf.ForwardTo = RecommendToID;
            atf.UserID = LoggedInUser.UserID;
            atf.EmpID = atot.EmpID;
            atf.ProcessPeriodID = CurrentPeriodID;
            atf.Remarks = Justification;
            atf.CliamedOTMins = atot.ApprovedOTMin;
            atf.OldStatusID = atot.StatusID;
            atf.Operation = Operation;
            atf.NewStatusID = StatusID;
            atf.CDTime = DateTime.Now;
            atf.OTPeriodID = atot.PeriodID;
            return atf;
        }
    }
    public static class OTHelperRecommended
    {

        public static List<ModelDOTEntries> GetConvertedDailyOTList(List<ViewDailyOTEntry> list)
        {
            List<ModelDOTEntries> vm = new List<ModelDOTEntries>();
            foreach (var item in list.OrderBy(aa => aa.OTDate).ToList())
            {
                ModelDOTEntries obj = new ModelDOTEntries();
                obj.EmpDate = item.EmpDate;
                obj.ClaimedOTHours = OTHelperManager.ConverMinIntoHour(item.ApprovedOTMin);
                obj.SystemOTHours = OTHelperManager.ConverMinIntoHours(item.ActualOTMin);
                obj.ClaimedOTMins = (int)item.ApprovedOTMin;
                obj.SystemOTMins = (int)item.ActualOTMin;
                obj.Date = OTHelperManager.ConverDateIntoDayString(item.OTDate);
                if (item.OTAmount > 0)
                    obj.OTAmount = (int)item.OTAmount;
                if (item.TimeIn != null)
                    obj.TimeIN = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                else
                    obj.TimeIN = "";
                if (item.TimeOut != null)
                    obj.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                else
                    obj.TimeOut = "";
                obj.WorkHours = OTHelperManager.ConverMinIntoHours(item.WorkMin);
                obj.StatusID = item.StatusID;
                switch (item.StatusID)
                {
                    case "P":
                        obj.StatusRemarks = "Pending:"+item.USFullName;
                        break;
                    case "F":
                        obj.StatusRemarks = "Recommend By:" + item.UPFFullName;
                        if (list.First().UFFFullName!=null)
                            obj.StatusForward = "Recommend By:" + list.First().UFFFullName;
                        break;
                    case "C":
                        obj.StatusRemarks = "Cancel:" + item.UCFullName;
                        obj.StatusForward = " ";
                        break;
                    case "A":
                        obj.StatusRemarks = "Approved:" +item.UAFullName;
                        break;
                    case "R":
                        obj.StatusRemarks = "Reject:" + item.URFullName;
                        break;
                }

                vm.Add(obj);
            }
            return vm;
        }
        public static List<ModelDOTEntries> GetConvertedDailyOTListSimple(List<ViewDailyOTEntry> list)
        {
            List<ModelDOTEntries> vm = new List<ModelDOTEntries>();
            foreach (var item in list.OrderBy(aa => aa.OTDate).ToList())
            {
                ModelDOTEntries obj = new ModelDOTEntries();
                obj.EmpDate = item.EmpDate;
                obj.ClaimedOTHours = OTHelperManager.ConverMinIntoHour(item.ApprovedOTMin);
                obj.SystemOTHours = OTHelperManager.ConverMinIntoHours(item.ActualOTMin);
                obj.ClaimedOTMins = (int)item.ApprovedOTMin;
                obj.SystemOTMins = (int)item.ActualOTMin;
                obj.Date = OTHelperManager.ConverDateIntoDayString(item.OTDate);
                if (item.OTAmount > 0)
                    obj.OTAmount = (int)item.OTAmount;
                if (item.TimeIn != null)
                    obj.TimeIN = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                else
                    obj.TimeIN = "";
                if (item.TimeOut != null)
                    obj.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + ":" + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                else
                    obj.TimeOut = "";
                obj.WorkHours = OTHelperManager.ConverMinIntoHours(item.WorkMin);
                obj.StatusID = item.StatusID;
                switch (item.StatusID)
                {
                    case "P":
                        obj.StatusRemarks = "Pending";
                        obj.StatusForward = " " + item.USFullName;
                        break;
                    case "F":
                        obj.StatusRemarks = "Forward";
                        if (item.UFFFullName == null)
                            obj.StatusForward = item.UFFullName;
                        else
                            obj.StatusForward = " " + item.UFFFullName;
                        break;
                    case "C":
                        obj.StatusRemarks = "Cancel";
                        obj.StatusForward = " " + item.UCFullName;
                        break;
                    case "A":
                        obj.StatusRemarks = "Approved";
                        obj.StatusForward = " " + item.UAFullName;
                        break;
                    case "R":
                        obj.StatusRemarks = "Reject";
                        obj.StatusForward = " " + item.URFullName;
                        break;
                }

                vm.Add(obj);
            }
            return vm;
        }
        internal static List<ModelRDeptPendingList> GetConvertedOTListDepts(List<ViewDailyOTEntry> list, List<EmpView> emps, List<short?> secIds)
        {
            List<ModelRDeptPendingList> vm = new List<ModelRDeptPendingList>();
            foreach (var item in secIds)
            {

                List<EmpView> empsInDept = emps.Where(aa => aa.SecID == item).ToList();
                if(emps.Where(aa=>aa.SecID==item).Count()>0)
                {

                    List<ViewDailyOTEntry> empsDailyOTList = GetEmpsDailyOTList(list, empsInDept);
                    if (empsDailyOTList.Count > 0)
                    {
                        ModelRDeptPendingList obj = new ModelRDeptPendingList();
                        obj.DeptID = (short)empsInDept.First().SecID;
                        obj.DeptName = empsInDept.First().SectionName;
                        obj.NoOfEmps = empsDailyOTList.Select(aa => aa.EmpID).Distinct().ToList().Count.ToString();
                        obj.ClaimOvertime = (empsDailyOTList.Where(aa => aa.SecID == item).Sum(aa=>aa.ApprovedOTMin)/60).ToString();
                        obj.SystemOvertime = (empsDailyOTList.Where(aa => aa.SecID == item).Sum(aa => aa.ActualOTMin) / 60).ToString();
                        obj.OTAmount = OTHelper.GetOTAmount(empsDailyOTList.Where(aa => aa.SecID == item).ToList());
                        vm.Add(obj);
                    }
                }
            }
            return vm.OrderBy(aa => aa.DeptName).ToList();
        }

        private static List<ViewDailyOTEntry> GetEmpsDailyOTList(List<ViewDailyOTEntry> list, List<EmpView> empsInDept)
        {
           List<ViewDailyOTEntry> tempList = new List<ViewDailyOTEntry>();
           foreach (var item in empsInDept)
           {
               tempList.AddRange(list.Where(aa => aa.EmpID == item.EmployeeID));
           }
           return tempList;
        }

        internal static List<ModelRDeptPendingList> GetConvertedOTListDeptsHR(List<ViewDailyOTEntry> list, List<EmpView> emps, List<short?> secIds)
        {
            List<ModelRDeptPendingList> vm = new List<ModelRDeptPendingList>();
            foreach (var item in secIds)
            {

                List<EmpView> empsInDept = emps.Where(aa => aa.SecID == item).ToList();
                if (emps.Where(aa => aa.SecID == item).Count() > 0)
                {

                    List<ViewDailyOTEntry> empsDailyOTList = GetEmpsDailyOTList(list, empsInDept);
                    if (empsDailyOTList.Count > 0)
                    {
                        ModelRDeptPendingList obj = new ModelRDeptPendingList();
                        obj.DeptID = (short)empsInDept.First().SecID;
                        obj.DeptName = empsInDept.First().SectionName;
                        obj.NoOfEmps = empsDailyOTList.Select(aa => aa.EmpID).Distinct().ToList().Count.ToString();
                        obj.ClaimOvertime = (empsDailyOTList.Where(aa => aa.SecID == item && (aa.StatusID == "F" || aa.StatusID == "A" || aa.StatusID == "P")).Sum(aa => aa.ApprovedOTMin) / 60).ToString();
                        obj.SystemOvertime = (empsDailyOTList.Where(aa => aa.SecID == item && (aa.StatusID == "F" || aa.StatusID == "A" || aa.StatusID == "P")).ToList().Sum(aa => aa.ActualOTMin) / 60).ToString();
                        obj.OTAmount = OTHelper.GetOTAmount(empsDailyOTList.Where(aa => aa.SecID == item && (aa.StatusID == "F" || aa.StatusID == "A")).ToList());
                        obj.PendingAtSupervisor = empsDailyOTList.Where(aa => aa.SecID == item && aa.StatusID=="P").ToList().Count();
                        obj.PendingAtRecommender = empsDailyOTList.Where(aa => aa.SecID == item && aa.StatusID == "F" && aa.UFUserType == "R").ToList().Count();
                        obj.PendingAtApprover = empsDailyOTList.Where(aa => aa.SecID == item && aa.StatusID == "F" && aa.UFUserType =="P").ToList().Count();
                        obj.Approved = empsDailyOTList.Where(aa => aa.SecID == item && aa.StatusID == "A").ToList().Count(); 
                        vm.Add(obj);
                    }
                }
            }
            return vm.OrderBy(aa => aa.DeptName).ToList();
        }

        public static List<ModelSOTPDList> GetConvertedOTListEmp(List<ViewDailyOTEntry> list, List<EmpView> emps)
        {
            List<ModelSOTPDList> vm = new List<ModelSOTPDList>();
            foreach (var emp in emps)
            {
                List<ViewDailyOTEntry> TList = new List<ViewDailyOTEntry>();
                TList = list.Where(aa => aa.EmpID == emp.EmployeeID).ToList();
                if (TList.Count() > 0)
                {
                    ModelSOTPDList obj = new ModelSOTPDList();
                    obj.DailyOTRequestCount = TList.Count();
                    obj.Designation = emp.DesignationName;
                    obj.Section = emp.SectionName;
                    obj.EmpID = emp.EmployeeID;
                    obj.EmpName = emp.FullName;
                    obj.ClaimedOTMins = (int)TList.Sum(aa => aa.ApprovedOTMin);
                    obj.SystemOTMins = (int)TList.Sum(aa => aa.ActualOTMin);
                    obj.ClaimedOTHours = OTHelperManager.ConverMinIntoHours(obj.ClaimedOTMins);
                    obj.SystemOTHours = OTHelperManager.ConverMinIntoHours(obj.SystemOTMins);
                    obj.OTAmount = OTHelper.GetOTAmount(TList.ToList());
                    obj.Processed = 1;
                    if (TList.First().PeriodID != null)
                        obj.PeriodID = (int)TList.First().PeriodID;
                    else
                        obj.PeriodID = 0;
                    if (TList.First().OTProcessingPeriodID != null)
                        obj.OTProcessPeriodID = (int)TList.First().OTProcessingPeriodID;
                    else
                        obj.OTProcessPeriodID = 0;
                    switch (TList.First().StatusID)
                    {
                        case "P":
                            obj.StatusRemarks = "Pending:" + TList.First().USFullName;
                            break;
                        case "F":
                            obj.StatusRemarks = "Recommend By:" + TList.First().UPFFullName;
                            if (TList.First().UFFFullName != null)
                                obj.StatusForward = "Recommend By:" + TList.First().UFFFullName;
                            break;
                        case "C":
                            obj.StatusRemarks = "Cancel:" + TList.First().UCFullName;
                            obj.StatusForward = " ";
                            break;
                        case "A":
                            obj.StatusRemarks = "Approved:" + TList.First().UAFullName;
                            break;
                        case "R":
                            obj.StatusRemarks = "Reject:" + TList.First().URFullName;
                            break;
                    }
                    vm.Add(obj);
                }
            }
            vm = vm.OrderBy(aa => aa.EmpName).ToList();
            //if (vm.Count > 0)
            //{
            //    ModelSOTPDList obj = new ModelSOTPDList();
            //    obj.DailyOTRequestCount = list.Count();
            //    obj.Designation = "";
            //    obj.Section = "";
            //    obj.EmpID =0;
            //    obj.EmpName = "Total Employees: "+ vm.Count().ToString();
            //    obj.ClaimOvertime = vm.Sum(aa=>aa.ClaimOvertime);
            //    obj.SystemOvertime = vm.Sum(aa => aa.SystemOvertime);
            //    obj.OTAmount = vm.Sum(aa => aa.OTAmount);
            //    vm.Add(obj);
            //}
            return vm;
        }
        internal static List<ModelSOTPDList> GetConvertedOTListEmpHR(List<ViewDailyOTEntry> list, List<EmpView> emps)
        {
            List<ModelSOTPDList> vm = new List<ModelSOTPDList>();
            foreach (var emp in emps)
            {
                if (list.Where(aa => aa.EmpID == emp.EmployeeID).Count() > 0)
                {
                    ModelSOTPDList obj = new ModelSOTPDList();
                    obj.DailyOTRequestCount = list.Where(aa => aa.EmpID == emp.EmployeeID).Count();
                    obj.Designation = emp.DesignationName;
                    obj.Section = emp.SectionName;
                    obj.EmpID = emp.EmployeeID;
                    obj.EmpName = emp.FullName;
                    obj.ClaimedOTMins = OTHelper.GetEmployeeCOTHour(list.Where(aa => aa.EmpID == emp.EmployeeID).ToList());
                    obj.SystemOTMins = OTHelper.GetEmployeeSOTHour(list.Where(aa => aa.EmpID == emp.EmployeeID).ToList());
                    obj.ClaimedOTHours = OTHelperManager.ConverMinIntoHours(obj.ClaimedOTMins);
                    obj.SystemOTHours = OTHelperManager.ConverMinIntoHours(obj.SystemOTMins);
                    obj.OTAmount = OTHelper.GetOTAmount(list.Where(aa => aa.EmpID == emp.EmployeeID).ToList());
                    obj.PendingAtSupervisor = list.Where(aa => aa.EmpID == emp.EmployeeID && aa.StatusID == "P").ToList().Count();
                    obj.PendingAtRecommender = list.Where(aa => aa.EmpID == emp.EmployeeID && aa.StatusID == "F" && aa.UFUserType == "R").ToList().Count();
                    obj.PendingAtApprover = list.Where(aa => aa.EmpID == emp.EmployeeID && aa.StatusID == "F" && aa.UFUserType == "P").ToList().Count();
                    obj.Approved = list.Where(aa => aa.EmpID == emp.EmployeeID && aa.StatusID == "A").ToList().Count();
                    vm.Add(obj);
                }
            }
            return vm.OrderBy(aa => aa.EmpName).ToList();
        }
    }
}