using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public static class EditAttManager
    {
        public static AttEditSingleEmployee GetAttendanceAttributes(List<Att_DailyAttendance> dailyAttendance, DateTime dtFrom, DateTime dtTo)
        {
            AttEditSingleEmployee entries = new AttEditSingleEmployee();

            entries.EmployeeID = (int)dailyAttendance.FirstOrDefault().EmpID;
            entries.EmpNo = dailyAttendance.FirstOrDefault().EmpNo;
            entries.EmpName = dailyAttendance.FirstOrDefault().HR_Employee.FullName;
            entries.DateFrom = dtFrom.ToString("dd-MMM-yyyy");
            entries.DateTo = dtTo.ToString("dd-MMM-yyyy");
            List<EditAttendanceListDateWise> list = new List<EditAttendanceListDateWise>();
            foreach (var item in dailyAttendance)
            {
                EditAttendanceListDateWise eal = new EditAttendanceListDateWise();
                eal.Date = item.AttDate.Value.ToString("dd-MMM-yyyy");
                eal.EmployeeID = (int)item.EmpID;
                eal.EmpNo = item.EmpNo;
                eal.Date = item.AttDate.Value.ToString("dd-MMM-yyyy");
                eal.DutyTime = item.DutyTime.Value.Hours.ToString("00") + item.DutyTime.Value.Minutes.ToString("00");
                eal.EmpDate = item.EmpDate;
                eal.DutyCode = item.DutyCode;
                if (item.Remarks == null)
                    eal.Remarks = "";
                else
                    eal.Remarks = item.Remarks;
                TimeSpan shiftTime = new TimeSpan(0, (int)item.ShifMin, 0);
                eal.ShiftTime = shiftTime.Hours.ToString("00") + shiftTime.Minutes.ToString("00");
                if (item.TimeIn != null)
                    eal.TimeIn = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                else
                    eal.TimeIn = "";
                if (item.TimeOut != null)
                    eal.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                else
                    eal.TimeOut = "";
                if (item.WorkMin > 0)
                {
                    TimeSpan WorkTime = new TimeSpan(0, (int)item.WorkMin, 0);
                    eal.WorkMinutes = WorkTime.Hours.ToString("00") + ":" + WorkTime.Minutes.ToString("00");
                }
                else
                    eal.WorkMinutes = "";
                if (item.DutyCode == "G")
                {
                    if (item.GZOTMin > 0)
                    {
                        TimeSpan GZTime = new TimeSpan(0, (int)item.GZOTMin, 0);
                        eal.OTMin = GZTime.Hours.ToString("00") + ":" + GZTime.Minutes.ToString("00");
                    }
                    else
                        eal.OTMin = "0000";
                }
                else
                {
                    if (item.NOTMin > 0)
                    {

                        TimeSpan OTTime = new TimeSpan(0, (int)item.NOTMin, 0);
                        eal.OTMin = OTTime.Hours.ToString("00") + ":" + OTTime.Minutes.ToString("00");
                    }
                    else
                        eal.OTMin = "00:00";
                }
                if (item.Tin0 != null)
                {
                    eal.TimeIn1 = item.Tin0.Value.TimeOfDay.Hours.ToString("00") + item.Tin0.Value.TimeOfDay.Minutes.ToString("00");
                }
                else
                    eal.TimeIn1 = "";
                if (item.Tin1 != null)
                {
                    eal.TimeIn2 = item.Tin1.Value.TimeOfDay.Hours.ToString("00") + item.Tin1.Value.TimeOfDay.Minutes.ToString("00");
                }
                else
                    eal.TimeIn2 = "";
                if (item.Tin2 != null)
                {
                    eal.TimeIn3 = item.Tin2.Value.TimeOfDay.Hours.ToString("00") + item.Tin2.Value.TimeOfDay.Minutes.ToString("00");
                }
                else
                    eal.TimeIn3 = "";
                if (item.Tout0 != null)
                {
                    eal.TimeOut1 = item.Tout0.Value.TimeOfDay.Hours.ToString("00") + item.Tout0.Value.TimeOfDay.Minutes.ToString("00");
                }
                else
                    eal.TimeOut1 = "";
                if (item.Tout1 != null)
                {
                    eal.TimeOut2 = item.Tout1.Value.TimeOfDay.Hours.ToString("00") + item.Tout1.Value.TimeOfDay.Minutes.ToString("00");
                }
                else
                    eal.TimeOut2 = "";
                if (item.Tout2 != null)
                {
                    eal.TimeOut3 = item.Tout2.Value.TimeOfDay.Hours.ToString("00") + item.Tout2.Value.TimeOfDay.Minutes.ToString("00");
                }
                else
                    eal.TimeOut3 = "";
                list.Add(eal);
            }
            entries.list = list;
            return entries;
        }
        public static EditAttendanceList GetEditAttendanceList(string empDate, string DutyCode, string DutyTime, string ShiftTime, string BreakMin, string TimeIn, string TimeOut)
        {
            EditAttendanceList el = new EditAttendanceList();
            el.EmpDate = empDate;
            el.DutyCode = DutyCode;
            el.DutyTime = Convert.ToDateTime(DutyTime).TimeOfDay;
            el.BreakMin = Convert.ToDateTime(BreakMin).TimeOfDay;
            el.ShiftTime = Convert.ToDateTime(ShiftTime).TimeOfDay;
            el.TimeIn = Convert.ToDateTime(TimeIn).TimeOfDay;
            el.TimeOut = Convert.ToDateTime(TimeOut).TimeOfDay;
            return el;
        }


        public static bool CheckRecordIsEdited(Models.Att_DailyAttendance att, EditAttendanceList editlist)
        {
            //check for attendance is edited
            bool edited = false;
            TimeSpan breakmin = new TimeSpan();
            if (att.BreakMin > 0)
                breakmin = new TimeSpan(0, (int)att.BreakMin, 0);
            else
                breakmin = new TimeSpan(0, 0, 0);
            if (att.DutyCode != editlist.DutyCode)
                edited = true;
            if (att.DutyTime != editlist.DutyTime)
                edited = true;
            if (att.ShifMin != editlist.ShiftTime.TotalMinutes)
                edited = true;
            if (att.TimeIn != null)
            {
                if (editlist.TimeIn != null)
                {
                    if (att.TimeIn.Value.TimeOfDay.Hours.ToString("00") + att.TimeIn.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeIn.Hours.ToString("00") + editlist.TimeIn.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeIn != null)
                    if (editlist.TimeIn.TotalMinutes > 0)
                        edited = true;
            }
            if (att.TimeOut != null)
            {
                if (editlist.TimeOut != null)
                {
                    if (att.TimeOut.Value.TimeOfDay.Hours.ToString("00") + att.TimeOut.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeOut.Hours.ToString("00") + editlist.TimeOut.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeOut != null)
                    if (editlist.TimeOut.TotalMinutes > 0)
                        edited = true;
            }//Tin0
            if (att.Tin0 != null)
            {
                if (editlist.TimeIn1 != null)
                {
                    if (att.Tin0.Value.TimeOfDay.Hours.ToString("00") + att.Tin0.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeIn1.Value.Hours.ToString("00") + editlist.TimeIn1.Value.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeIn1 != null)
                    if (editlist.TimeIn1.Value.TotalMinutes > 0)
                        edited = true;
            }//Tou0
            if (att.Tout0 != null)
            {
                if (editlist.TimeOut1 != null)
                {
                    if (att.Tout0.Value.TimeOfDay.Hours.ToString("00") + att.Tout0.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeOut1.Value.Hours.ToString("00") + editlist.TimeOut1.Value.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeOut1 != null)
                    if (editlist.TimeOut1.Value.TotalMinutes > 0)
                        edited = true;
            }//Tin1
            if (att.Tin1 != null)
            {
                if (editlist.TimeIn2 != null)
                {
                    if (att.Tin1.Value.TimeOfDay.Hours.ToString("00") + att.Tin1.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeIn2.Value.Hours.ToString("00") + editlist.TimeIn2.Value.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeIn2 != null)
                    if (editlist.TimeIn2.Value.TotalMinutes > 0)
                        edited = true;
            }//tout1
            if (att.Tout1 != null)
            {
                if (editlist.TimeOut2 != null)
                {
                    if (att.Tout1.Value.TimeOfDay.Hours.ToString("00") + att.Tout1.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeOut2.Value.Hours.ToString("00") + editlist.TimeOut2.Value.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeOut2 != null)
                    if (editlist.TimeOut2.Value.TotalMinutes > 0)
                        edited = true;
            }//Tin2
            if (att.Tin2 != null)
            {
                if (editlist.TimeIn3 != null)
                {
                    if (att.Tin2.Value.TimeOfDay.Hours.ToString("00") + att.Tin2.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeIn3.Value.Hours.ToString("00") + editlist.TimeIn3.Value.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeIn3 != null)
                    if (editlist.TimeIn3.Value.TotalMinutes > 0)
                        edited = true;
            }//tout2
            if (att.Tout2 != null)
            {
                if (editlist.TimeOut3 != null)
                {
                    if (att.Tout2.Value.TimeOfDay.Hours.ToString("00") + att.Tout2.Value.TimeOfDay.Minutes.ToString("00") != editlist.TimeOut3.Value.Hours.ToString("00") + editlist.TimeOut3.Value.Minutes.ToString("00"))
                        edited = true;
                }
                else
                    edited = true;
            }
            else
            {
                if (editlist.TimeOut3 != null)
                    if (editlist.TimeOut3.Value.TotalMinutes > 0)
                        edited = true;
            }
            if (att.DutyCode != editlist.DutyCode)
                edited = true;

            return edited;
        }
        public static VMEditAttendanceDateWise GetAttendanceAttributesDateWise(List<Att_DailyAttendance> dailyAttendance, DateTime dtTo, string Criteria, int CriteriaData)
        {
            VMEditAttendanceDateWise entries = new VMEditAttendanceDateWise();
            List<EditAttendanceListDateWise> list = new List<EditAttendanceListDateWise>();
            foreach (var item in dailyAttendance)
            {
                EditAttendanceListDateWise eal = new EditAttendanceListDateWise();
                eal.EmployeeID = (int)item.EmpID;
                eal.EmpNo = item.EmpNo;
                eal.EmpName = item.HR_Employee.FullName;
                eal.Date = item.AttDate.Value.ToString("dd-MMM-yyyy");
                eal.DutyTime = item.DutyTime.Value.Hours.ToString("00") + item.DutyTime.Value.Minutes.ToString("00");
                eal.EmpDate = item.EmpDate;
                eal.DutyCode = item.DutyCode;
                eal.Remarks = item.Remarks;
                TimeSpan shiftTime = new TimeSpan(0, (int)item.ShifMin, 0);
                eal.ShiftTime = shiftTime.Hours.ToString("00") + shiftTime.Minutes.ToString("00");
                if (item.TimeIn != null)
                    eal.TimeIn = item.TimeIn.Value.TimeOfDay.Hours.ToString("00") + item.TimeIn.Value.TimeOfDay.Minutes.ToString("00");
                if (item.TimeOut != null)
                    eal.TimeOut = item.TimeOut.Value.TimeOfDay.Hours.ToString("00") + item.TimeOut.Value.TimeOfDay.Minutes.ToString("00");
                if (item.WorkMin > 0)
                {
                    TimeSpan WorkTime = new TimeSpan(0, (int)item.WorkMin, 0);
                    eal.WorkMinutes = WorkTime.Hours.ToString("00") + ":" + WorkTime.Minutes.ToString("00");
                }
                else
                    eal.WorkMinutes = "0000";
                if (item.DutyCode == "G")
                {
                    if (item.GZOTMin > 0)
                    {
                        TimeSpan GZTime = new TimeSpan(0, (int)item.GZOTMin, 0);
                        eal.OTMin = GZTime.Hours.ToString("00") + ":" + GZTime.Minutes.ToString("00");
                    }
                    else
                        eal.OTMin = "0000";
                }
                else
                {
                    if (item.NOTMin > 0)
                    {

                        TimeSpan OTTime = new TimeSpan(0, (int)item.NOTMin, 0);
                        eal.OTMin = OTTime.Hours.ToString("00") + ":" + OTTime.Minutes.ToString("00");
                    }
                    else
                        eal.OTMin = "00:00";
                }
                if (item.Tin0 != null)
                {
                    eal.TimeIn1 = item.Tin0.Value.TimeOfDay.Hours.ToString("00") + item.Tin0.Value.TimeOfDay.Minutes.ToString("00");
                }
                if (item.Tin1 != null)
                {
                    eal.TimeIn2 = item.Tin1.Value.TimeOfDay.Hours.ToString("00") + item.Tin1.Value.TimeOfDay.Minutes.ToString("00");
                }
                if (item.Tin2 != null)
                {
                    eal.TimeIn3 = item.Tin2.Value.TimeOfDay.Hours.ToString("00") + item.Tin2.Value.TimeOfDay.Minutes.ToString("00");
                }
                if (item.Tout0 != null)
                {
                    eal.TimeOut1 = item.Tout0.Value.TimeOfDay.Hours.ToString("00") + item.Tout0.Value.TimeOfDay.Minutes.ToString("00");
                }
                if (item.Tout1 != null)
                {
                    eal.TimeOut2 = item.Tout1.Value.TimeOfDay.Hours.ToString("00") + item.Tout1.Value.TimeOfDay.Minutes.ToString("00");
                }
                if (item.Tout2 != null)
                {
                    eal.TimeOut3 = item.Tout2.Value.TimeOfDay.Hours.ToString("00") + item.Tout2.Value.TimeOfDay.Minutes.ToString("00");
                }
                list.Add(eal);
            }
            entries.list = list;
            entries.Count = list.Count;
            entries.Criteria = Criteria;
            entries.CriteriaData = CriteriaData;
            entries.Date = dtTo.ToString("dd-MMM-yyyy");
            return entries;
        }

        //
        public static EditAttendanceList GetEditAttendanceList(string empDate, string DutyCode, string DutyTime, string ShiftTime, string Tin1, string Tout1, string Tin2, string Tout2, string Tin3, string Tout3, string Remarks)
        {
            EditAttendanceList el = new EditAttendanceList();
            el.EmpDate = empDate;
            el.DutyCode = DutyCode;
            el.DutyTime = ConvertTime(DutyTime);
            el.ShiftTime = ConvertTime(ShiftTime);
            if (Tin1 != "")
                el.TimeIn = ConvertTime(Tin1);
            if (Tin1 != "")
                el.TimeIn1 = ConvertTime(Tin1);
            else
                el.TimeIn1 = null;
            if (Tout1 != "")
                el.TimeOut1 = ConvertTime(Tout1);
            else
                el.TimeOut1 = null;
            if (Tin2 != "")
                el.TimeIn2 = ConvertTime(Tin2);
            else
                el.TimeIn2 = null;
            if (Tout2 != "")
                el.TimeOut2 = ConvertTime(Tout2);
            else
                el.TimeOut2 = null;
            if (Tin3 != "")
                el.TimeIn3 = ConvertTime(Tin3);
            else
                el.TimeIn3 = null;
            if (Tout3 != "")
                el.TimeOut3 = ConvertTime(Tout3);
            else
                el.TimeOut3 = null;
            if (Tout1 != "")
                el.TimeOut = (TimeSpan)el.TimeOut1;
            if (Tout2 != "")
                el.TimeOut = (TimeSpan)el.TimeOut2;
            if (Tout3 != "")
                el.TimeOut = (TimeSpan)el.TimeOut3;
            el.Remarks = Remarks;
            return el;
        }

        public static TimeSpan ConvertTime(string p)
        {
            try
            {
                string hour = "";
                string min = "";
                int count = 0;
                int chunkSize = 2;
                int stringLength = 4;
                TimeSpan _currentTime = new TimeSpan();
                if (p != "")
                {
                    for (int i = 0; i < stringLength; i += chunkSize)
                    {
                        count++;
                        if (count == 1)
                        {
                            hour = p.Substring(i, chunkSize);
                        }
                        if (count == 2)
                        {
                            min = p.Substring(i, chunkSize);
                        }
                        if (i + chunkSize > stringLength)
                        {
                            chunkSize = stringLength - i;
                        }
                    }
                    _currentTime = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), 00);
                }
                return _currentTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay;
            }
        }

        //public static VMEditMonthlyAttendance GetMonthlyAttendanceAttributes(List<ViewAttMonthlySummary> MonthlyAttendance, int PayrollID, string PRName,List<HR_Location> locs)
        //{
        //    VMEditMonthlyAttendance entries = new VMEditMonthlyAttendance();
        //    List<EditMonthlyAttendanceList> list = new List<EditMonthlyAttendanceList>();
        //    foreach (var item in MonthlyAttendance)
        //    {
        //        EditMonthlyAttendanceList eal = new EditMonthlyAttendanceList();
        //        eal.EmployeeID = (int)item.EmployeeID;
        //        eal.LocID= (int)item.LocIDMonth;
        //        eal.LocName = item.LocNameMonth;
        //        eal.EmpNo = item.EmpNo;
        //        eal.EmpName = item.FullName;
        //        eal.TotalDays = 0;
        //        eal.PaidDays = 0;
        //        eal.AbsentDays = 0;
        //        eal.ActualOT=0;
        //        eal.RestDays = (float)item.RestDays;
        //        eal.GZDays = (float)item.GZDays;
        //        eal.LeaveDays = (float)item.LeaveDays;
        //        if (item.TotalShortMins > 0)
        //            eal.ShortHr = (int)(item.TotalShortMins / 60);
        //        else
        //            eal.ShortHr = 0;
        //        if (item.TotalOT > 0)
        //            eal.TotalOT = (int)(item.TotalOT / 60);
        //        else
        //            eal.TotalOT = 0;
        //        if (item.AbDays != null)
        //        {
        //            eal.AbsentDays = (float)item.AbDays;
        //        }
        //        if (item.WorkDays != null)
        //        {
        //            eal.PaidDays = (float)item.WorkDays;
        //        }
        //        if (item.TotalDays != null)
        //        {
        //            eal.TotalDays = (float)item.TotalDays;
        //        }
        //        if (item.ActualOT != null)
        //        {
        //            eal.ActualOT = (int)(item.ActualOT / 60);
        //        }
        //        if (item.Remarks == null)
        //            eal.Remarks = "";
        //        else
        //            eal.Remarks = item.Remarks;
        //        list.Add(eal);
        //    }
        //    list.Add(GetTotalCount(list));
        //    entries.Locations = locs;
        //    entries.list = list.OrderBy(aa=>aa.EmployeeID).ToList();
        //    entries.Count = list.Count;
        //    entries.PayrollID = PayrollID;
        //    entries.PayrollName = PRName;
        //    return entries;
        //}

        private static EditMonthlyAttendanceList GetTotalCount(List<EditMonthlyAttendanceList> MonthlyAttendance)
        {
            EditMonthlyAttendanceList eal = new EditMonthlyAttendanceList();
            eal.EmployeeID = 999999;
            eal.EmpNo = "";
            eal.EmpName = "Total: " + MonthlyAttendance.Count;
            eal.TotalDays = (float)MonthlyAttendance.ToList().Select(c => c.TotalDays).Sum();
            eal.PaidDays = (float)MonthlyAttendance.ToList().Select(c => c.PaidDays).Sum();
            eal.AbsentDays = (float)MonthlyAttendance.ToList().Select(c => c.AbsentDays).Sum();
            eal.TotalOT = (float)MonthlyAttendance.ToList().Select(c => c.TotalOT).Sum();
            eal.ShortHr = (float)MonthlyAttendance.ToList().Select(c => c.ShortHr).Sum();
            eal.ActualOT = (float)MonthlyAttendance.ToList().Select(c => c.ActualOT).Sum();
            eal.LeaveDays = (float)MonthlyAttendance.ToList().Select(c => c.LeaveDays).Sum();
            return eal;
        }

        internal static EditMonthlyAttendanceList GetEditMonthlyAttendanceList(int EmpID, int prid, string TotalDays, string PaidDays, string AbsentDays, string ActualOT, int LocID, string Remarks)
        {
            EditMonthlyAttendanceList eal = new EditMonthlyAttendanceList();
            if (TotalDays != null && TotalDays != "")
                eal.TotalDays = (float)Convert.ToDouble(TotalDays);
            else
                eal.TotalDays = 0;
            if (PaidDays != null && PaidDays != "")
                eal.PaidDays = (float)Convert.ToDouble(PaidDays);
            else
                eal.PaidDays = 0;

            if (AbsentDays != null && AbsentDays != "")
                eal.AbsentDays = (float)Convert.ToDouble(AbsentDays);
            else
                eal.AbsentDays = 0;

            if (ActualOT != null && ActualOT != "")
                eal.ActualOT = (float)Convert.ToDouble(ActualOT);
            else
                eal.ActualOT = 0;
            if (Remarks != null && Remarks != "")
            {
                eal.Remarks = Remarks;
            }
            else
                eal.Remarks = "";
            eal.LocID = LocID;
            return eal;
        }

        internal static string CheckMonthRecordIsEdited(Att_MonthData att, EditMonthlyAttendanceList editlist)
        {
            string edited = "No";
            if (editlist.PaidDays != att.WorkDays)
                edited = "Time";
            if (editlist.TotalDays != att.TotalDays)
                edited = "Time";
            if (editlist.AbsentDays != att.AbDays)
                edited = "Time";
            if ((editlist.ActualOT * 60) != att.TotalOT)
                edited = "Time";
            return edited;
        }
    }
    public class AttEditSingleEmployee
    {
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<EditAttendanceListDateWise> list { get; set; }
    }

    public class EditAttendanceList
    {
        public string EmpDate { get; set; }
        public string Date { get; set; }
        public string DutyCode { get; set; }
        public TimeSpan DutyTime { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public TimeSpan? TimeIn1 { get; set; }
        public TimeSpan? TimeOut1 { get; set; }
        public TimeSpan? TimeIn2 { get; set; }
        public TimeSpan? TimeOut2 { get; set; }
        public TimeSpan? TimeIn3 { get; set; }
        public TimeSpan? TimeOut3 { get; set; }
        public TimeSpan WorkMinutes { get; set; }
        public TimeSpan LateIn { get; set; }
        public TimeSpan LateOut { get; set; }
        public TimeSpan EarlyIn { get; set; }
        public TimeSpan EarlyOut { get; set; }
        public TimeSpan BreakMin { get; set; }
        public TimeSpan ShiftTime { get; set; }
        public TimeSpan OTMin { get; set; }
        public string Remarks { get; set; }
    }
    public class VMEditAttendanceDateWise
    {
        public int Count { get; set; }
        public string Date { get; set; }
        public string Criteria { get; set; }
        public int CriteriaData { get; set; }
        public string CriteriaDataName { get; set; }
        public List<EditAttendanceListDateWise> list { get; set; }
    }
    public class EditAttendanceListDateWise
    {
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string EmpDate { get; set; }
        public string Date { get; set; }
        public string DutyCode { get; set; }
        public string DutyTime { get; set; }
        public string ShiftTime { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string TimeIn1 { get; set; }
        public string TimeIn2 { get; set; }
        public string TimeOut1 { get; set; }
        public string TimeOut2 { get; set; }
        public string TimeIn3 { get; set; }
        public string TimeOut3 { get; set; }
        public string WorkMinutes { get; set; }
        public string OTMin { get; set; }
        public string Remarks { get; set; }
    }

    public class VMEditMonthlyAttendance
    {
        public int Count { get; set; }
        public string Date { get; set; }
        public int PayrollID { get; set; }
        public string PayrollName { get; set; }
        public List<EditMonthlyAttendanceList> list { get; set; }
        public List<HR_Location> Locations { get; set; }
    }
    public class EditMonthlyAttendanceList
    {
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public int LocID { get; set; }
        public string LocName { get; set; }
        public float TotalDays { get; set; }
        public float PaidDays { get; set; }
        public float AbsentDays { get; set; }
        public float RestDays { get; set; }
        public float LeaveDays { get; set; }
        public float GZDays { get; set; }
        public float TotalOT { get; set; }
        public float ShortHr { get; set; }
        public float ActualOT { get; set; }
        public string Remarks { get; set; }
    }
}