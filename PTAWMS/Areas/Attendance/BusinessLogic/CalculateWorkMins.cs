using PTAWMS.Areas.Attendance.BusinessLogic.AttendaceHelper;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public static class CalculateWorkMins
    {
        #region -- Calculate Work Times --
        static DateTime Trim(this DateTime date, long roundTicks)
        {
            return new DateTime(date.Ticks - date.Ticks % roundTicks);
        }
        public static void CalculateShiftTimes(Att_DailyAttendance attendanceRecord, MyShift shift, Att_OTPolicy otPolicy, Att_OutPass op)
        {
            try
            {
                attendanceRecord.TimeIn = attendanceRecord.TimeIn.Value.Trim(TimeSpan.TicksPerMinute);
                attendanceRecord.TimeOut = attendanceRecord.TimeOut.Value.Trim(TimeSpan.TicksPerMinute);
                // Break start and End Times
                DateTime ts = attendanceRecord.TimeIn.Value.Date + new TimeSpan(13, 0, 0);
                DateTime te = attendanceRecord.TimeIn.Value.Date + new TimeSpan(14, 0, 0);
                //Work Mins
                TimeSpan mins = attendanceRecord.TimeOut.Value.TimeOfDay - attendanceRecord.TimeOut.Value.TimeOfDay;
                double _workHours = mins.TotalHours;
                attendanceRecord.WorkMin = (short)(mins.TotalMinutes);
                if (attendanceRecord.WorkMin > 0)
                {
                    if (attendanceRecord.Remarks != null)
                    {
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Manual]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[M]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[R-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[G-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[HA]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[DO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[GZ]", "");
                    }
                    else
                    {
                        attendanceRecord.Remarks = "";
                    }
                    if (attendanceRecord.StatusMN == true)
                        attendanceRecord.Remarks = "[M]" + attendanceRecord.Remarks;
                    //Check if GZ holiday then place all WorkMin in GZOTMin
                    if (attendanceRecord.StatusGZ == true && attendanceRecord.DutyCode == "G")
                    {
                        #region -- GZ Calculation--
                        if (otPolicy.CalculateGZOT == true)
                        {
                            if ((_workHours >= otPolicy.PerDayGOTStartLimitHour) && (_workHours <= otPolicy.PerDayGOTEndLimitHour))
                            {
                                int hour = (int)(mins.TotalMinutes / 60);
                                int min = hour * 60;
                                int remainingmin = (int)mins.TotalMinutes - min;
                                if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                {
                                    attendanceRecord.GZOTMin = (short)((hour + 1) * 60);
                                }
                                else
                                {
                                    attendanceRecord.GZOTMin = (short)((hour) * 60);
                                }
                            }
                            else
                            {
                                if (_workHours < otPolicy.PerDayGOTStartLimitHour)
                                {
                                    attendanceRecord.GZOTMin = 0;
                                }
                                else
                                {
                                    int policyOTLimitMin = (int)(otPolicy.PerDayGOTEndLimitHour * 60.0);
                                    attendanceRecord.GZOTMin = (short)policyOTLimitMin;
                                }
                            }
                        }
                        else
                        {
                            attendanceRecord.WorkMin = 0;
                            attendanceRecord.ExtraMin = (short)mins.TotalMinutes;
                        }
                        if (attendanceRecord.GZOTMin > 0)
                        {
                            attendanceRecord.StatusGZOT = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[G-OT]";
                        }
                        #endregion
                    }
                    //if Rest day then place all WorkMin in OTMin
                    else if (attendanceRecord.StatusDO == true && attendanceRecord.DutyCode == "R")
                    {
                        #region -- Rest Calculation --
                        if (otPolicy.CalculateRestOT == true)
                        {
                            if ((_workHours >= otPolicy.PerDayROTStartLimitHour) && (_workHours <= otPolicy.PerDayROTEndLimitHour))
                            {
                                if (mins.TotalMinutes < otPolicy.MinMinutesForOneHour)
                                    attendanceRecord.ROTMin = 0;
                                else if (attendanceRecord.ROTMin >= otPolicy.MinMinutesForOneHour && attendanceRecord.ROTMin <= 61)
                                {
                                    attendanceRecord.ROTMin = 60;
                                }
                                else
                                {
                                    int hour = (int)(mins.TotalMinutes / 60);
                                    int min = hour * 60;
                                    int remainingmin = (int)mins.TotalMinutes - min;
                                    if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                    {
                                        attendanceRecord.ROTMin = (short)((hour + 1) * 60);
                                    }
                                    else
                                    {
                                        attendanceRecord.ROTMin = (short)((hour) * 60);
                                    }
                                }
                            }
                            else
                            {
                                if (_workHours < otPolicy.PerDayROTStartLimitHour)
                                {
                                    attendanceRecord.ROTMin = 0;
                                }
                                else
                                {
                                    int policyOTLimitMin = (int)(otPolicy.PerDayROTEndLimitHour * 60.0);
                                    attendanceRecord.ROTMin = (short)policyOTLimitMin;
                                }
                            }
                        }
                        else
                        {
                            attendanceRecord.WorkMin = 0;
                            attendanceRecord.ExtraMin = (short)mins.TotalMinutes;
                        }
                        if (attendanceRecord.ROTMin > 0)
                        {
                            attendanceRecord.StatusROT = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[R-OT]";
                        }
                        #endregion
                    }
                    else
                    {

                        attendanceRecord.StatusAB = false;
                        attendanceRecord.StatusP = true;
                        attendanceRecord.ExtraMin = 0;
                        #region -- Margins--
                        //Calculate Late IN, Compare margin with Shift Late In
                        if (attendanceRecord.TimeIn.Value.TimeOfDay > attendanceRecord.DutyTime)
                        {
                            TimeSpan lateMinsSpan = (TimeSpan)(attendanceRecord.TimeIn.Value.TimeOfDay - attendanceRecord.DutyTime);
                            if (lateMinsSpan.Minutes > shift.LateIn)
                            {

                                attendanceRecord.LateIn = (short)lateMinsSpan.TotalMinutes;
                                attendanceRecord.StatusLI = true;
                                attendanceRecord.EarlyIn = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[LI]";
                            }
                            else
                            {
                                attendanceRecord.StatusLI = null;
                                attendanceRecord.LateIn = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusLI = null;
                            attendanceRecord.LateIn = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                        }

                        //Calculate Early In, Compare margin with Shift Early In
                        if (attendanceRecord.TimeIn.Value.TimeOfDay < attendanceRecord.DutyTime)
                        {
                            TimeSpan EarlyInMinsSpan = (TimeSpan)(attendanceRecord.DutyTime - attendanceRecord.TimeIn.Value.TimeOfDay);
                            if (EarlyInMinsSpan.TotalMinutes > shift.EarlyIn)
                            {
                                attendanceRecord.EarlyIn = (short)EarlyInMinsSpan.TotalMinutes;
                                attendanceRecord.StatusEI = true;
                                attendanceRecord.LateIn = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[EI]";
                            }
                            else
                            {
                                attendanceRecord.StatusEI = null;
                                attendanceRecord.EarlyIn = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusEI = null;
                            attendanceRecord.EarlyIn = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                        }

                        // CalculateShiftEndTime = ShiftStart + DutyHours
                        DateTime shiftEnd = ProcessSupportFunc.CalculateShiftEndTimeWithAttData(attendanceRecord.AttDate.Value, attendanceRecord.DutyTime.Value, (short)(attendanceRecord.ShifMin + attendanceRecord.BreakMin));

                        //Calculate Early Out, Compare margin with Shift Early Out
                        if (attendanceRecord.TimeOut < shiftEnd)
                        {
                            TimeSpan EarlyOutMinsSpan = (TimeSpan)(shiftEnd - attendanceRecord.TimeOut);
                            if (EarlyOutMinsSpan.TotalMinutes > shift.EarlyOut)
                            {
                                attendanceRecord.EarlyOut = (short)EarlyOutMinsSpan.TotalMinutes;
                                attendanceRecord.StatusEO = true;
                                attendanceRecord.LateOut = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[EO]";
                            }
                            else
                            {
                                attendanceRecord.StatusEO = null;
                                attendanceRecord.EarlyOut = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusEO = null;
                            attendanceRecord.EarlyOut = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                        }
                        //Calculate Late Out, Compare margin with Shift Late Out
                        if (attendanceRecord.TimeOut > shiftEnd)
                        {
                            TimeSpan LateOutMinsSpan = (TimeSpan)(attendanceRecord.TimeOut - shiftEnd);
                            if (LateOutMinsSpan.TotalMinutes > shift.LateOut)
                            {
                                attendanceRecord.LateOut = (short)LateOutMinsSpan.TotalMinutes;
                                // Late Out cannot have an early out, In case of poll at multiple times before and after shiftend
                                attendanceRecord.EarlyOut = null;
                                attendanceRecord.StatusLO = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[LO]";
                            }
                            else
                            {
                                attendanceRecord.StatusLO = null;
                                attendanceRecord.LateOut = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusLO = null;
                            attendanceRecord.LateOut = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        }
                        #endregion

                        #region -- Shift Things
                        //Subtract EarlyIn and LateOut from Work Minutes
                        if (shift.SubtractEIFromWork == true)
                        {
                            if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > shift.EarlyIn)
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.EarlyIn);
                            }
                        }
                        if (shift.SubtractLOFromWork == true)
                        {
                            if (attendanceRecord.LateOut != null && attendanceRecord.LateOut > shift.LateOut)
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.LateOut);
                            }
                        }

                        // Deduct break
                        if (attendanceRecord.DutyCode == "D")
                        {
                            //Normal
                            if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                            {
                                if (attendanceRecord.TimeIn < ts && attendanceRecord.TimeOut > te)
                                {
                                    attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.BreakMin);
                                }
                                else
                                {
                                    if (attendanceRecord.TotalShortMin > 0)
                                        attendanceRecord.TotalShortMin = (short)(attendanceRecord.TotalShortMin + attendanceRecord.BreakMin);
                                }
                            }
                        }
                        #endregion
                        #region -- OT Calculation --
                        //if (otPolicy.CalculateNOT == true && attendanceRecord.LateOut > 0)
                        //    attendanceRecord.NOTMin = (short)attendanceRecord.LateOut;
                        //else if (otPolicy.CalculateNOT == false)
                        //{
                        //    if (attendanceRecord.LateOut > 0)
                        //        attendanceRecord.ExtraMin = (short)(attendanceRecord.LateOut + attendanceRecord.ExtraMin);
                        //    if (attendanceRecord.EarlyIn > 0)
                        //        attendanceRecord.ExtraMin = (short)(attendanceRecord.EarlyIn + attendanceRecord.ExtraMin);
                        //}
                        short totalOTMins = 0;
                        if (otPolicy.CalculateNOT == true)
                        {
                            if (otPolicy.AddEIinOT == true)
                            {
                                if (attendanceRecord.EarlyIn > 0)
                                {
                                    totalOTMins = (short)attendanceRecord.EarlyIn;
                                }
                                if (attendanceRecord.LateOut > 0)
                                {
                                    totalOTMins = (short)(attendanceRecord.LateOut + totalOTMins);
                                }
                            }
                            else
                            {
                                totalOTMins = (short)attendanceRecord.LateOut;
                            }
                        }
                        else
                        {
                            //attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                        }
                        // Check OT with Work Minutes
                        if (totalOTMins > 0)
                        {
                            //if (attendanceRecord.WorkMin < attendanceRecord.ShifMin)
                            //{
                            //    totalOTMins = (short)(totalOTMins - (short)(attendanceRecord.ShifMin - attendanceRecord.WorkMin)); 
                            //}
                            float otHour = (float)(totalOTMins / 60.0);
                            if (otHour < otPolicy.PerDayOTStartLimitHour)
                            {
                                attendanceRecord.NOTMin = 0;
                            }
                            else if (otHour >= otPolicy.PerDayOTStartLimitHour && otHour <= otPolicy.PerDayOTStartLimitHour)
                            {
                                if (otPolicy.MinMinutesForOneHour == 0)
                                {
                                    attendanceRecord.NOTMin = (short)totalOTMins;
                                }
                                else
                                {
                                    if (totalOTMins < otPolicy.MinMinutesForOneHour)
                                        attendanceRecord.NOTMin = 0;
                                    else if (totalOTMins >= otPolicy.MinMinutesForOneHour && totalOTMins <= 61)
                                    {
                                        attendanceRecord.NOTMin = 60;
                                    }
                                    else
                                    {
                                        if (totalOTMins > 0)
                                        {
                                            int hour = (int)(totalOTMins / 60);
                                            int min = hour * 60;
                                            int remainingmin = (int)totalOTMins - min;
                                            if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                            {
                                                attendanceRecord.NOTMin = (short)((hour + 1) * 60);
                                            }
                                            else
                                            {
                                                attendanceRecord.NOTMin = (short)min;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (otHour >= otPolicy.PerDayOTEndLimitHour)
                            {

                                int policyOTLimitMin = (int)(otPolicy.PerDayOTEndLimitHour * 60.0);
                                attendanceRecord.NOTMin = (short)policyOTLimitMin;
                            }

                            if (attendanceRecord.NOTMin > 0)
                            {
                                attendanceRecord.StatusOT = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[N-OT]";
                            }
                        }
                        else
                        {

                        }
                        #endregion
                        #region --- Half Absent and Short Time ---
                        if (attendanceRecord.StatusHL == true)
                        {
                            attendanceRecord.TotalShortMin = 0;
                            attendanceRecord.EarlyOut = 0;
                            attendanceRecord.LateIn = 0;
                            attendanceRecord.LateOut = 0;
                            attendanceRecord.NOTMin = 0;
                            attendanceRecord.ExtraMin = 0;
                            attendanceRecord.TotalShortMin = 0;
                            attendanceRecord.StatusLI = false;
                            attendanceRecord.StatusEO = false;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                            attendanceRecord.PDays = 0.5;
                            attendanceRecord.AbDays = 0;
                            attendanceRecord.LeaveDays = 0.5;
                            // update if lateout

                        }
                        else
                        {
                            attendanceRecord.PDays = 1;
                            attendanceRecord.AbDays = 0;
                            attendanceRecord.LeaveDays = 0;
                            short totalshortMin = 0;
                            if (attendanceRecord.LateIn > 0)
                                totalshortMin = (short)attendanceRecord.LateIn;
                            if (attendanceRecord.EarlyOut > 0)
                                totalshortMin = (short)(totalshortMin + attendanceRecord.EarlyOut);
                            attendanceRecord.TotalShortMin = totalshortMin;
                            int marginForST = 10;
                            if (shift.LateIn > 0)
                                marginForST = shift.LateIn;
                            if (attendanceRecord.WorkMin < (attendanceRecord.ShifMin - marginForST))
                            {
                                attendanceRecord.TotalShortMin = (Int16)(attendanceRecord.TotalShortMin + (attendanceRecord.ShifMin - (attendanceRecord.WorkMin + totalshortMin)));
                            }
                            if (otPolicy.CalculateNOT == true)
                            {
                                if (attendanceRecord.NOTMin > 0)
                                {
                                    //if (attendanceRecord.TotalShortMin > 0)
                                    //    attendanceRecord.ApprovedOT = (short)(attendanceRecord.NOTMin - attendanceRecord.TotalShortMin);
                                }
                            }
                        }

                        #endregion
                        #region -- Mark Absent --
                        //Mark Absent if less than 4 hours
                        if (attendanceRecord.AttDate.Value.DayOfWeek != DayOfWeek.Friday && attendanceRecord.StatusDO != true && attendanceRecord.StatusGZ != true)
                        {
                            if (attendanceRecord.StatusHL != true)
                            {
                                short MinShiftMin = (short)shift.MinHrs;
                                if (attendanceRecord.WorkMin < MinShiftMin)
                                {
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                                    attendanceRecord.StatusAB = true;
                                    attendanceRecord.StatusP = false;
                                    attendanceRecord.PDays = 0;
                                    attendanceRecord.AbDays = 1;
                                    attendanceRecord.LeaveDays = 0;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[Absent]";
                                }
                                else
                                {
                                    attendanceRecord.StatusAB = false;
                                    attendanceRecord.StatusP = true;
                                    if (attendanceRecord.StatusHL == true)
                                    {
                                        attendanceRecord.PDays = 0.5;
                                        attendanceRecord.AbDays = 0;
                                        attendanceRecord.LeaveDays = 0.5;
                                    }
                                    else
                                    {
                                        attendanceRecord.PDays = 1;
                                        attendanceRecord.AbDays = 0;
                                        attendanceRecord.LeaveDays = 0;
                                    }
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                                }
                            }
                        }
                        #endregion

                        //RoundOff Work Minutes
                        if (shift.RoundOffWorkMin == true)
                        {
                            if (attendanceRecord.WorkMin >= (attendanceRecord.ShifMin - shift.LateIn) && (attendanceRecord.WorkMin <= ((attendanceRecord.ShifMin) + shift.LateIn)))
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.ShifMin);
                            }

                            if (attendanceRecord.WorkMin > 0 && attendanceRecord.StatusHL != true)
                            {
                                if (attendanceRecord.ShifMin <= attendanceRecord.WorkMin + attendanceRecord.TotalShortMin)
                                {
                                    attendanceRecord.WorkMin = attendanceRecord.ShifMin;
                                }
                            }
                            if (attendanceRecord.WorkMin > 0 && attendanceRecord.StatusHL == true)
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.ShifMin);
                            }
                        }
                    }
                    #region -- Break for GZ, Rest and Normal Day
                    //GZ Break
                    //if (attendanceRecord.DutyCode == "G")
                    //{
                    //    if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                    //    {
                    //        if (attendanceRecord.TimeIn < ts && attendanceRecord.TimeOut > te)
                    //        {
                    //            if (attendanceRecord.GZOTMin > 0)
                    //            {
                    //                attendanceRecord.GZOTMin = (short)(attendanceRecord.GZOTMin - attendanceRecord.BreakMin);
                    //            }
                    //            if (attendanceRecord.ExtraMin > 0)
                    //            {
                    //                attendanceRecord.ExtraMin = (short)(attendanceRecord.ExtraMin - attendanceRecord.BreakMin);
                    //            }
                    //        }
                    //    }
                    //}
                    //Rest
                    //else if (attendanceRecord.DutyCode == "R")
                    //{
                    //    if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                    //    {
                    //        if (attendanceRecord.TimeIn < ts && attendanceRecord.TimeOut > te)
                    //        {
                    //            if (attendanceRecord.OTMin > 0)
                    //            {
                    //                attendanceRecord.OTMin = (short)(attendanceRecord.OTMin - attendanceRecord.BreakMin);
                    //            }
                    //            if (attendanceRecord.ExtraMin > 0)
                    //            {
                    //                attendanceRecord.ExtraMin = (short)(attendanceRecord.ExtraMin - attendanceRecord.BreakMin);
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
    public class MyShift
    {
        public int ShftID { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DayOff1 { get; set; }
        public int DayOff2 { get; set; }
        public Int16 MonMin { get; set; }
        public Int16 TueMin { get; set; }
        public Int16 WedMin { get; set; }
        public Int16 ThuMin { get; set; }
        public Int16 FriMin { get; set; }
        public Int16 SatMin { get; set; }
        public Int16 SunMin { get; set; }
        public Int16 LateIn { get; set; }
        public Int16 EarlyIn { get; set; }
        public Int16 EarlyOut { get; set; }
        public Int16 LateOut { get; set; }
        public Int16 OverTimeMin { get; set; }
        public Int16 MinHrs { get; set; }
        public bool HasBreak { get; set; }
        public Int16 BreakMin { get; set; }
        public Int16 HalfDayBreakMin { get; set; }
        public bool GZDays { get; set; }
        public bool OpenShift { get; set; }
        public bool RoundOffWorkMin { get; set; }
        public bool SubtractLOFromWork { get; set; }
        public bool SubtractEIFromWork { get; set; }
        public bool PresentAtIN { get; set; }
        public bool CalDiffOnly { get; set; }
    }
}