using PTAWMS.Areas.Attendance.BusinessLogic.AttendaceHelper;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public class ManualAttendanceProcess
    {
        HRMEntities context = new HRMEntities();
        Att_DailyAttendance _OldAttData = new Att_DailyAttendance();
        Att_DailyAttendance _NewAttData = new Att_DailyAttendance();
        Att_ManualEdit _ManualEditData = new Att_ManualEdit();

        //Replace New TimeIn and Out with Old TimeIN and Out in Attendance Data
        public ManualAttendanceProcess(string EmpDate, string JobCardName, bool JobCardStatus, DateTime NewTimeIn, DateTime NewTimeOut, string NewDutyCode, int _UserID, TimeSpan _NewDutyTime, string _Remarks, short _ShiftMins)
        {
            _OldAttData = context.Att_DailyAttendance.First(aa => aa.EmpDate == EmpDate);
            if (_OldAttData != null)
            {
                if (JobCardStatus == false)
                {
                    SaveOldAttData(_OldAttData, _UserID);
                    if (SaveNewAttData(NewTimeIn, NewTimeOut, NewDutyCode, _NewDutyTime, _Remarks, _ShiftMins))
                    {
                        _OldAttData.TimeIn = NewTimeIn;
                        _OldAttData.TimeOut = NewTimeOut;
                        _OldAttData.Tin0 = NewTimeIn;
                        _OldAttData.Tout0 = NewTimeOut;
                        _OldAttData.DutyCode = NewDutyCode;
                        _OldAttData.DutyTime = _NewDutyTime;
                        _OldAttData.WorkMin = 0;
                        _OldAttData.GZOTMin = 0;
                        _OldAttData.NOTMin = 0;
                        _OldAttData.ROTMin = 0;
                        _OldAttData.LateIn = 0;
                        _OldAttData.EarlyIn = 0;
                        _OldAttData.EarlyOut = 0;
                        _OldAttData.LateOut = 0;
                        _OldAttData.ExtraMin = 0;
                        _OldAttData.TotalShortMin = 0;
                        _OldAttData.StatusAB = false;
                        _OldAttData.StatusP = true;
                        _OldAttData.StatusEO = null;
                        _OldAttData.StatusEI = null;
                        _OldAttData.StatusOT = null;
                        _OldAttData.StatusLI = null;
                        _OldAttData.StatusLO = null;
                        _OldAttData.StatusDO = null;
                        _OldAttData.StatusGZOT = null;
                        _OldAttData.StatusLeave = null;
                        _OldAttData.ExtraMin = null;
                        _OldAttData.StatusMN = true;
                        _OldAttData.ShifMin = _ShiftMins;
                        _OldAttData.Remarks = "";
                        if (NewDutyCode == "G")
                            _OldAttData.StatusGZ = true;
                        else
                            _OldAttData.StatusGZ = false;
                        if (NewDutyCode == "R")
                            _OldAttData.StatusDO = true;
                        else
                            _OldAttData.StatusDO = false;
                        if (_Remarks != "")
                            _OldAttData.Remarks = "[" + _Remarks + "]";
                        _OldAttData.StatusLeave = null;
                        ProcessDailyAttendance(_OldAttData);
                    }
                }
            }
        }
        public ManualAttendanceProcess(string EmpDate, string JobCardName, bool JobCardStatus, DateTime NewTimeIn, DateTime NewTimeOut, string NewDutyCode, int _UserID, TimeSpan _NewDutyTime, string _Remarks, short _ShiftMins, DateTime? NewTin1, DateTime? NewTout1, DateTime? NewTin2, DateTime? NewTout2, DateTime? NewTin3, DateTime? NewTout3, string Remarks)
        {
            _OldAttData = context.Att_DailyAttendance.First(aa => aa.EmpDate == EmpDate);
            if (_OldAttData != null)
            {
                if (JobCardStatus == false)
                {
                    SaveOldAttData(_OldAttData, _UserID);
                    if (SaveNewAttData(NewTimeIn, NewTimeOut, NewDutyCode, _NewDutyTime, _Remarks, _ShiftMins, NewTin1, NewTout1, NewTin2, NewTout2, NewTin3, NewTout3))
                    {

                        if (NewTimeIn != NewTimeOut)
                        {
                            _OldAttData.TimeIn = NewTimeIn;
                            _OldAttData.TimeOut = NewTimeOut;
                        }
                        else
                        {
                            _OldAttData.TimeIn = null;
                            _OldAttData.TimeOut = null;
                        }
                        if (NewTin1 != null)
                            _OldAttData.Tin0 = NewTin1;
                        else
                            _OldAttData.Tin0 = null;
                        if (NewTout1 != null)
                            _OldAttData.Tout0 = NewTout1;
                        else
                            _OldAttData.Tout0 = null;
                        if (NewTin2 != null)
                            _OldAttData.Tin1 = NewTin2;
                        else
                            _OldAttData.Tin1 = null;
                        if (NewTout2 != null)
                            _OldAttData.Tout1 = NewTout2;
                        else
                            _OldAttData.Tout1 = null;
                        if (NewTin3 != null)
                            _OldAttData.Tin2 = NewTin3;
                        else
                            _OldAttData.Tin2 = null;
                        if (NewTout3 != null)
                            _OldAttData.Tout2 = NewTout3;
                        else
                            _OldAttData.Tout2 = null;
                        _OldAttData.DutyCode = NewDutyCode;
                        _OldAttData.DutyTime = _NewDutyTime;
                        _OldAttData.WorkMin = 0;
                        _OldAttData.PDays = 0;
                        _OldAttData.AbDays = 0;
                        _OldAttData.LeaveDays = 0;
                        _OldAttData.LateIn = 0;
                        _OldAttData.LateOut = 0;
                        _OldAttData.EarlyIn = 0;
                        _OldAttData.EarlyOut = 0;
                        _OldAttData.NOTMin = 0;
                        _OldAttData.GZOTMin = 0;
                        _OldAttData.TotalShortMin = 0;
                        _OldAttData.SLMin = 0;
                        _OldAttData.ExtraMin = 0;
                        _OldAttData.StatusP = false;
                        _OldAttData.StatusAB = false;
                        _OldAttData.StatusLI = false;
                        _OldAttData.StatusLO = false;
                        _OldAttData.StatusEI = false;
                        _OldAttData.StatusEO = false;
                        _OldAttData.StatusOT = false;
                        _OldAttData.StatusGZOT = false;
                        _OldAttData.StatusGZ = false;
                        _OldAttData.StatusDO = false;
                        _OldAttData.StatusSL = false;
                        _OldAttData.StatusOD = false;
                        _OldAttData.StatusHL = false;
                        _OldAttData.StatusLeave = false;
                        _OldAttData.StatusMN = true;
                        _OldAttData.StatusIN = false;
                        _OldAttData.ShifMin = _ShiftMins;
                        _OldAttData.Remarks = Remarks;
                        if (NewDutyCode == "G")
                            _OldAttData.StatusGZ = true;
                        else
                            _OldAttData.StatusGZ = false;
                        if (NewDutyCode == "R")
                            _OldAttData.StatusDO = true;
                        else
                            _OldAttData.StatusDO = false;
                        if (_Remarks != "")
                            _OldAttData.Remarks = "[" + _Remarks + "]";
                        _OldAttData.StatusLeave = null;
                        ProcessDailyAttendance(_OldAttData);
                        context.SaveChanges();
                    }
                }
            }
        }

        //Save Old and New Attendance Data in Manual Attendance Table
        private bool SaveNewAttData(DateTime _NewTimeIn, DateTime _NewTimeOut, string _NewDutyCode, TimeSpan _NewDutyTime, string _remarks, short _ShiftMins)
        {
            bool check = false;
            _ManualEditData.NewTimeIn = _NewTimeIn;
            _ManualEditData.NewTimeOut = _NewTimeOut;
            _ManualEditData.NewDutyCode = _NewDutyCode;
            _ManualEditData.EditDateTime = DateTime.Now;
            _ManualEditData.NewDutyTime = _NewDutyTime;
            _ManualEditData.NewRemarks = "[" + _remarks + "]";
            _ManualEditData.NewShiftMin = _ShiftMins;
            try
            {
                context.Att_ManualEdit.Add(_ManualEditData);
                context.SaveChanges();
                check = true;
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }
        private bool SaveNewAttData(DateTime _NewTimeIn, DateTime _NewTimeOut, string _NewDutyCode, TimeSpan _NewDutyTime, string _remarks, short _ShiftMins, DateTime? NewTin1, DateTime? NewTout1, DateTime? NewTin2, DateTime? NewTout2, DateTime? NewTin3, DateTime? NewTout3)
        {
            bool check = false;
            if (_NewTimeIn != _NewTimeOut)
            {
                _ManualEditData.NewTimeIn = _NewTimeIn;
                _ManualEditData.NewTimeOut = _NewTimeOut;
                if (NewTin1 != null)
                    _ManualEditData.NewTin1 = NewTin1;
                if (NewTout1 != null)
                    _ManualEditData.NewTout1 = NewTout1;
                if (NewTin2 != null)
                    _ManualEditData.NewTin2 = NewTin2;
                if (NewTout2 != null)
                    _ManualEditData.NewTout2 = NewTout2;
                if (NewTin3 != null)
                    _ManualEditData.NewTin3 = NewTin3;
                if (NewTout3 != null)
                    _ManualEditData.NewTout3 = NewTout3;
            }
            _ManualEditData.NewDutyCode = _NewDutyCode;
            _ManualEditData.EditDateTime = DateTime.Now;
            _ManualEditData.NewDutyTime = _NewDutyTime;
            _ManualEditData.NewRemarks = "[" + _remarks + "]";
            _ManualEditData.NewShiftMin = _ShiftMins;
            try
            {
                context.Att_ManualEdit.Add(_ManualEditData);
                context.SaveChanges();
                check = true;
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        private void SaveOldAttData(Att_DailyAttendance _OldAttData, int Userid)
        {
            try
            {
                _ManualEditData.OldDutyCode = _OldAttData.DutyCode;
                _ManualEditData.OldTimeIn = _OldAttData.TimeIn;
                _ManualEditData.OldTimeOut = _OldAttData.TimeOut;
                _ManualEditData.EmpDate = _OldAttData.EmpDate;
                _ManualEditData.UserID = Userid;
                _ManualEditData.EditDateTime = DateTime.Now;
                _ManualEditData.EmpID = _OldAttData.EmpID;
                _ManualEditData.OldRemarks = _OldAttData.Remarks;
            }
            catch (Exception ex)
            {

            }
        }

        //Work Times calculation controller
        public void ProcessDailyAttendance(Att_DailyAttendance _attData)
        {
            try
            {
                Att_DailyAttendance attendanceRecord = _attData;
                HR_Employee employee = attendanceRecord.HR_Employee;
                List<Att_ShiftChngedEmp> _shiftEmpCh = new List<Att_ShiftChngedEmp>();
                _shiftEmpCh = context.Att_ShiftChngedEmp.ToList();
                List<Att_OutPass> _OutPasses = new List<Att_OutPass>();
                _OutPasses = context.Att_OutPass.Where(aa => aa.EmpID == _attData.EmpID && aa.Dated == _attData.AttDate.Value).ToList();
                List<Att_Shift> shifts = new List<Att_Shift>();
                shifts = context.Att_Shift.ToList();
                List<Att_ShiftChanged> cshifts = new List<Att_ShiftChanged>();
                cshifts = context.Att_ShiftChanged.ToList();
                if (_attData.StatusLeave == true)
                    _attData.ShifMin = 0;
                //If TimeIn and TimeOut are not null, then calculate other Atributes
                if (_attData.TimeIn != null && _attData.TimeOut != null)
                {
                    Att_Shift _shift = ProcessSupportFunc.GetEmployeeChangedShift(_attData.HR_Employee, _shiftEmpCh.Where(aa => aa.EmpID == _attData.EmpID).ToList(), _attData.AttDate.Value, shifts);
                    MyShift shift = ProcessSupportFunc.GetEmployeeShift(_shift);
                    if (_attData.StatusHL == true)
                    {
                        _attData.ShifMin = ProcessSupportFunc.CalculateShiftMinutes(shift, _attData.AttDate.Value.DayOfWeek);
                        _attData.ShifMin = (short)(_attData.ShifMin / 2);
                    }
                    //If TimeIn = TimeOut then calculate according to DutyCode
                    if (_attData.TimeIn == _attData.TimeOut)
                    {
                        CalculateInEqualToOut(_attData);
                    }
                    else
                    {
                        if (_attData.DutyTime == new TimeSpan(0, 0, 0))
                        {
                            //CalculateWorkMins.CalculateOpenShiftTimes(_attData, shift, _attData.HR_Employee.Att_OTPolicy);
                        }
                        Att_OutPass aop = new Att_OutPass();
                        if (_OutPasses.Where(aa => aa.Dated == _attData.AttDate && aa.EmpID == _attData.EmpID).Count() > 0)
                            aop = _OutPasses.First(aa => aa.Dated == _attData.AttDate && aa.EmpID == _attData.EmpID);
                        //CalculateWorkMins.CalculateShiftTimes(_attData, shift, _attData.HR_Employee.Att_OTPolicy, aop);
                    }
                }
                else
                {
                    CalculateInEqualToOut(_attData);
                }
            }
            catch (Exception ex)
            {
            }

            context.SaveChanges();
        }

        TimeSpan OpenShiftThresholdStart = new TimeSpan(17, 00, 00);
        TimeSpan OpenShiftThresholdEnd = new TimeSpan(11, 00, 00);

        private void CalculateInEqualToOut(Att_DailyAttendance attendanceRecord)
        {
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[G-OT]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[R-OT]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Manual]", "");
            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[M]", "");
            switch (attendanceRecord.DutyCode)
            {
                case "G":
                    attendanceRecord.StatusAB = false;
                    attendanceRecord.StatusGZ = true;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.NOTMin = 0;
                    attendanceRecord.PDays = 0;
                    attendanceRecord.AbDays = 0;
                    attendanceRecord.LeaveDays = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[GZ][M]";
                    break;
                case "R":
                    attendanceRecord.StatusAB = false;
                    attendanceRecord.StatusGZ = false;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.ROTMin = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    attendanceRecord.StatusDO = true;
                    attendanceRecord.PDays = 0;
                    attendanceRecord.AbDays = 0;
                    attendanceRecord.LeaveDays = 0;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[DO][M]";
                    break;
                case "D":
                    if (attendanceRecord.StatusLeave == true)
                    {
                        attendanceRecord.AbDays = 0;
                        attendanceRecord.PDays = 0;
                        attendanceRecord.LeaveDays = 1;
                        attendanceRecord.StatusGZ = false;
                        attendanceRecord.WorkMin = 0;
                        attendanceRecord.EarlyIn = 0;
                        attendanceRecord.EarlyOut = 0;
                        attendanceRecord.LateIn = 0;
                        attendanceRecord.LateOut = 0;
                        attendanceRecord.NOTMin = 0;
                        attendanceRecord.GZOTMin = 0;
                        attendanceRecord.StatusGZOT = false;
                        attendanceRecord.TimeIn = null;
                        attendanceRecord.TimeOut = null;
                        attendanceRecord.StatusDO = false;
                        attendanceRecord.StatusP = false;
                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[M]";
                    }
                    else if (attendanceRecord.StatusHL == true)
                    {
                        attendanceRecord.AbDays = 0.5;
                        attendanceRecord.PDays = 0;
                        attendanceRecord.LeaveDays = 0.5;
                        attendanceRecord.StatusHL = true;
                        attendanceRecord.StatusAB = true;
                        attendanceRecord.StatusGZ = false;
                        attendanceRecord.WorkMin = 0;
                        attendanceRecord.EarlyIn = 0;
                        attendanceRecord.EarlyOut = 0;
                        attendanceRecord.LateIn = 0;
                        attendanceRecord.LateOut = 0;
                        attendanceRecord.NOTMin = 0;
                        attendanceRecord.GZOTMin = 0;
                        attendanceRecord.StatusGZOT = false;
                        attendanceRecord.TimeIn = null;
                        attendanceRecord.TimeOut = null;
                        attendanceRecord.StatusDO = false;
                        attendanceRecord.StatusP = false;
                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[HA][M]";
                    }
                    else
                    {
                        attendanceRecord.AbDays = 1;
                        attendanceRecord.PDays = 0;
                        attendanceRecord.LeaveDays = 0;
                        attendanceRecord.StatusAB = true;
                        attendanceRecord.StatusGZ = false;
                        attendanceRecord.WorkMin = 0;
                        attendanceRecord.EarlyIn = 0;
                        attendanceRecord.EarlyOut = 0;
                        attendanceRecord.LateIn = 0;
                        attendanceRecord.LateOut = 0;
                        attendanceRecord.NOTMin = 0;
                        attendanceRecord.GZOTMin = 0;
                        attendanceRecord.StatusGZOT = false;
                        attendanceRecord.TimeIn = null;
                        attendanceRecord.TimeOut = null;
                        attendanceRecord.StatusDO = false;
                        attendanceRecord.StatusP = false;
                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[Absent][M]";
                    }
                    break;
            }
        }

    }
}