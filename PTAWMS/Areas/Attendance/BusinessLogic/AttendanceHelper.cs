using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic.AttendaceHelper
{
    public static class ProcessSupportFunc
    {
        #region -- Helper Function--

        public static string ReturnDayOfWeek(DayOfWeek dayOfWeek)
        {
            string _DayName = "";
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    _DayName = "Monday";
                    break;
                case DayOfWeek.Tuesday:
                    _DayName = "Tuesday";
                    break;
                case DayOfWeek.Wednesday:
                    _DayName = "Wednesday";
                    break;
                case DayOfWeek.Thursday:
                    _DayName = "Thursday";
                    break;
                case DayOfWeek.Friday:
                    _DayName = "Friday";
                    break;
                case DayOfWeek.Saturday:
                    _DayName = "Saturday";
                    break;
                case DayOfWeek.Sunday:
                    _DayName = "Sunday";
                    break;
            }
            return _DayName;
        }

        public static TimeSpan CalculateShiftEndTime(Att_Shift shift, DayOfWeek dayOfWeek)
        {
            Int16 workMins = 0;
            try
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        workMins = shift.MonMin;
                        break;
                    case DayOfWeek.Tuesday:
                        workMins = shift.TueMin;
                        break;
                    case DayOfWeek.Wednesday:
                        workMins = shift.WedMin;
                        break;
                    case DayOfWeek.Thursday:
                        workMins = shift.ThuMin;
                        break;
                    case DayOfWeek.Friday:
                        workMins = shift.FriMin;
                        break;
                    case DayOfWeek.Saturday:
                        workMins = shift.SatMin;
                        break;
                    case DayOfWeek.Sunday:
                        workMins = shift.SunMin;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return shift.StartTime + (new TimeSpan(0, workMins, 0));
        }

        public static Int16 CalculateShiftMinutes(MyShift shift, DayOfWeek dayOfWeek)
        {
            Int16 workMins = 0;
            try
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        workMins = shift.MonMin;
                        break;
                    case DayOfWeek.Tuesday:
                        workMins = shift.TueMin;
                        break;
                    case DayOfWeek.Wednesday:
                        workMins = shift.WedMin;
                        break;
                    case DayOfWeek.Thursday:
                        workMins = shift.ThuMin;
                        break;
                    case DayOfWeek.Friday:
                        workMins = shift.FriMin;
                        break;
                    case DayOfWeek.Saturday:
                        workMins = shift.SatMin;
                        break;
                    case DayOfWeek.Sunday:
                        workMins = shift.SunMin;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return workMins;
        }

        public static DateTime CalculateShiftEndTime(Att_Shift shift, DateTime _AttDate, TimeSpan _DutyTime)
        {
            Int16 workMins = 0;
            try
            {
                switch (_AttDate.Date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        workMins = shift.MonMin;
                        break;
                    case DayOfWeek.Tuesday:
                        workMins = shift.TueMin;
                        break;
                    case DayOfWeek.Wednesday:
                        workMins = shift.WedMin;
                        break;
                    case DayOfWeek.Thursday:
                        workMins = shift.ThuMin;
                        break;
                    case DayOfWeek.Friday:
                        workMins = shift.FriMin;
                        break;
                    case DayOfWeek.Saturday:
                        workMins = shift.SatMin;
                        break;
                    case DayOfWeek.Sunday:
                        workMins = shift.SunMin;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            DateTime _datetime = new DateTime();
            TimeSpan _Time = new TimeSpan(0, workMins, 0);
            _datetime = _AttDate.Date.Add(_DutyTime);
            _datetime = _datetime.Add(_Time);
            return _datetime;
        }

        #endregion
        public static int ReturnDayNoOfWeek(DayOfWeek dayOfWeek)
        {
            int _DayName = 0;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    _DayName = 2;
                    break;
                case DayOfWeek.Tuesday:
                    _DayName = 3;
                    break;
                case DayOfWeek.Wednesday:
                    _DayName = 4;
                    break;
                case DayOfWeek.Thursday:
                    _DayName = 5;
                    break;
                case DayOfWeek.Friday:
                    _DayName = 6;
                    break;
                case DayOfWeek.Saturday:
                    _DayName = 7;
                    break;
                case DayOfWeek.Sunday:
                    _DayName = 1;
                    break;
            }
            return _DayName;
        }
        internal static DateTime CalculateShiftEndTimeWithAttData(DateTime dateTime, TimeSpan timeSpan, short? shiftMins)
        {
            DateTime _datetime = new DateTime();
            int shiftMin = (int)shiftMins;
            TimeSpan _Time = new TimeSpan(0, shiftMin, 0);
            _datetime = dateTime.Date.Add(timeSpan);
            _datetime = _datetime.Add(_Time);
            return _datetime;
        }
        internal static Att_Shift GetEmployeeChangedShift(HR_Employee emp, List<Att_ShiftChngedEmp> empshiftCh, DateTime currentDate, List<Att_Shift> shifts)
        {
            Att_Shift shift = emp.Att_Shift;
            foreach (var item in empshiftCh)
            {
                if (item.EndDate == null)
                {
                    if (currentDate >= item.StartDate)
                    {
                        shift = shifts.First(aa => aa.ShftID == item.ShiftID);
                    }
                }
                else
                {
                    if (currentDate >= item.StartDate && currentDate <= item.EndDate)
                    {
                        shift = shifts.First(aa => aa.ShftID == item.ShiftID);
                    }

                }
            }
            return shift;
        }
        public static MyShift GetEmployeeShift(Att_Shift att_Shift)
        {
            MyShift shift = new MyShift();
            shift.ShftID = att_Shift.ShftID;
            shift.StartTime = att_Shift.StartTime;
            shift.DayOff1 = att_Shift.DayOff1;
            shift.DayOff2 = att_Shift.DayOff2;
            shift.MonMin = att_Shift.MonMin;
            shift.TueMin = att_Shift.TueMin;
            shift.WedMin = att_Shift.WedMin;
            shift.ThuMin = att_Shift.ThuMin;
            shift.FriMin = att_Shift.FriMin;
            shift.SatMin = att_Shift.SatMin;
            shift.SunMin = att_Shift.SunMin;
            shift.LateIn = (short)att_Shift.LateIn;
            shift.EarlyIn = (short)att_Shift.EarlyIn;
            shift.EarlyOut = (short)att_Shift.EarlyOut;
            shift.LateOut = (short)att_Shift.LateOut;
            shift.OverTimeMin = (short)att_Shift.OverTimeMin;
            shift.MinHrs = (short)att_Shift.MinHrs;
            shift.HasBreak = (bool)att_Shift.HasBreak;
            shift.BreakMin = (short)att_Shift.BreakMin;
            shift.HalfDayBreakMin = (short)att_Shift.HalfDayBreakMin;
            shift.GZDays = (bool)att_Shift.GZDays;
            shift.OpenShift = (bool)att_Shift.OpenShift;
            shift.RoundOffWorkMin = (bool)att_Shift.RoundOffWorkMin;
           // shift.SubtractOTFromWork = (bool)att_Shift.SubtractOTFromWork;
            shift.SubtractEIFromWork = (bool)att_Shift.SubtractEIFromWork;
           // shift.AddEIInOT = (bool)att_Shift.AddEIInOT;
            shift.PresentAtIN = (bool)att_Shift.PresentAtIN;
            shift.CalDiffOnly = (bool)att_Shift.CalDiffOnly;

            return shift;
        }

        public static void ProcessAttendanceRequest(DateTime Dts, DateTime Dte, int EmpID, string EmpNo)
        {
            try
            {
                int days = (Dte - Dts).Days;
                if (days < 40)
                {
                    if (Dte <= DateTime.Today)
                    {
                        using (var db = new HRMEntities())
                        {
                            Att_ProcessRequest ap = new Att_ProcessRequest();
                            ap.PeriodTag = "D";
                            ap.DateFrom = Dts;
                            ap.DateTo = Dte;
                            ap.LocationID = db.HR_Location.FirstOrDefault().LocID;
                           // ap.CatID = db.HR_Category.FirstOrDefault().CatID;
                            ap.ProcessingDone = false;
                            ap.Criteria = "E";
                            ap.ProcessCat = true;
                            ap.UserID = 5;
                            ap.CreatedDate = DateTime.Now;
                            ap.EmpID = EmpID;
                            ap.EmpNo = EmpNo;
                            ap.SystemGenerated = true;
                           // ap.ManualProcessed = true;
                            db.Att_ProcessRequest.Add(ap);
                            db.SaveChanges();
                            db.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        internal static void ProcessAttendanceRequestMonthly(DateTime Dts, DateTime Dte, string empID)
        {
            try
            {
                int days = (Dte - Dts).Days;
                if (days < 40)
                {
                    int DaysInPreviousMonth = System.DateTime.DaysInMonth(Dts.Year, Dts.Month);
                    if (Dts.Month != Dte.Month)
                        Dte = new DateTime(Dts.Year, Dts.Month, DaysInPreviousMonth);
                    using (var db = new HRMEntities())
                    {
                        Att_ProcessRequest ap = new Att_ProcessRequest();
                        ap.PeriodTag = "M";
                        ap.DateFrom = Dts;
                        ap.DateTo = Dte;
                        ap.LocationID = db.HR_Location.FirstOrDefault().LocID;
                        //ap.CatID = db.HR_Category.FirstOrDefault().CatID;
                        ap.ProcessingDone = false;
                        ap.Criteria = "E";
                        ap.ProcessCat = true;
                        ap.UserID = 5;
                        ap.CreatedDate = DateTime.Now.AddSeconds(2);
                        ap.EmpID = Convert.ToInt32(empID);
                        ap.EmpNo = empID;
                        ap.SystemGenerated = true;
                       // ap.ManualProcessed = true;
                        db.Att_ProcessRequest.Add(ap);
                        db.SaveChanges();
                        db.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}