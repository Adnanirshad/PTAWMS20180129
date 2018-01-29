using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using PTAWMS.Areas.Attendance.BusinessLogic;
using HRM_IKAN.Authentication;
using PTAWMS.Areas.Attendance.BusinessLogic.AttendaceHelper;

namespace PTAWMS.Areas.Attendance.Controllers
{
    [CustomControllerAttributes]
    public class AttManualController : Controller
    {
        private HRMEntities db = new HRMEntities();
        // GET: /EditAttendance/

        public ActionResult Index()
        {
            try
            {
                if (Session["EditAttendanceDate"] == null)
                {
                    ViewData["datef"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    ViewData["datef"] = Session["EditAttendanceDate"].ToString();
                }
                //User LoggedInUser = Session["LoggedUser"] as User;
                ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewBag.JobCardType = new SelectList(db.Att_JobCard, "JCID", "JCName");
                ViewBag.RosterType = new SelectList(db.Att_RosterType.OrderBy(s => s.Name), "ID", "Name");
                ViewBag.ShiftList = new SelectList(db.Att_Shift.OrderBy(s => s.ShiftName), "ShftID", "ShiftName");
                ViewBag.LocationList = new SelectList(db.HR_Location.OrderBy(s => s.LocationName), "LocID", "LocationName");
                ViewBag.Message = "";
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        // Load Attendance Details of Selected Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form)
        {
            try
            {
                User LoggedInUser = Session["LoggedUser"] as User;
                //ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s => s.WorkCardName), "WorkCardID", "WorkCardName");
                ViewBag.ShiftList = new SelectList(db.Att_Shift.OrderBy(s => s.ShiftName), "ShftID", "ShiftName");
                //ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s => s.CrewName), "CrewID", "CrewName");
                //ViewBag.CompanyID = new SelectList(db.Companies.OrderBy(s => s.CompName), "CompID", "CompName", LoggedInUser.CompanyID);
                //ViewBag.CompanyIDJobCard = new SelectList(db.Companies, "CompID", "CompName");
                ViewBag.SectionList = new SelectList(db.HR_Section.OrderBy(s => s.SectionName), "SecID", "SectionName");
                ViewData["datef"] = Convert.ToDateTime(Request.Form["DateFrom"].ToString()).ToString("yyyy-MM-dd");
                ViewBag.DesignationID = new SelectList(db.HR_Designation.OrderBy(s => s.DesignationName), "DesignationID", "DesignationName");
                ViewData["datef"] = Request.Form["DateFrom"].ToString();
                if (Request.Form["EmpNo"].ToString() != "" && Request.Form["DateFrom"].ToString() != "")
                {
                    string _EmpNo = Request.Form["EmpNo"].ToString();
                    DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                    Session["EditAttendanceDate"] = Request.Form["DateFrom"].ToString();
                    //var _CompId = Request.Form["CompanyID"];
                    //int compID = Convert.ToInt32(_CompId); 
                    Att_DailyAttendance _attData = new Att_DailyAttendance();
                    List<HR_Employee> _Emp = new List<HR_Employee>();
                    int EmpID = 0;
                    //_Emp = db.HR_Employee.Where(aa => aa.EmpNo == _EmpNo && aa.CompanyID ==compID && aa.Status==true).ToList();
                    _Emp = db.HR_Employee.Where(aa => aa.EmpNo == _EmpNo).ToList();

                    if (_Emp.Count > 0)
                        EmpID = _Emp.FirstOrDefault().EmployeeID;
                    _attData = db.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpID == EmpID && aa.AttDate == _AttDataFrom);
                    if (_attData != null)
                    {
                        List<Att_DeviceData> _Polls = new List<Att_DeviceData>();
                        string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
                        _Polls = db.Att_DeviceData.Where(aa => aa.EntDate == _AttDataFrom && aa.EmpID == _attData.EmpID).OrderBy(a => a.EntTime).ToList();
                        ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                        ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                        ViewBag.EmpID = new SelectList(db.HR_Employee.OrderBy(s => s.FullName), "EmpID", "EmpNo", _attData.EmpID);
                        Session["NEmpNo"] = _attData.EmpID;
                        ViewBag.SucessMessage = "";
                        if (_attData.WorkMin != null)
                            ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
                        if (_attData.LateOut != null)
                            ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                        if (_attData.LateIn != null)
                            ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                        if (_attData.EarlyOut != null)
                            ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                        if (_attData.EarlyIn != null)
                            ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);                      
                        if (_attData.GZOTMin != null)
                            ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                        return View(_attData);
                    }
                    else
                        return View("Index");
                }
                else
                    return View("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }

        }
        //[HttpPost]
        //public ActionResult EditMultipleEntries(FormCollection form)
        //{
        //    //string _EmpNo = Request.Form["EmpNoM"].ToString();
        //    //DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFromM"].ToString());
        //    //DateTime _AttDataTo = Convert.ToDateTime(Request.Form["DateToM"].ToString());

        //    //    return View(CalculateRosterFields(_RosterType, _StartDate, _WorkMin, _DutyTime, Criteria, RosterCriteriaValue, _Shift, ra.RotaApplD));

        //    //    //return View("Index");

        //}

        //Add New Times and Process Attendance of Particular Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditData([Bind(Include = "EmpDate,AttDate,EmpNo,EmpID,DutyCode,DutyTime,TimeIn,TimeOut,WorkMin,LateIn,LateOut,EarlyIn,EarlyOut,OTMin,GZOTMin,BreakMin,SLMin,StatusP,StatusAB,StatusLI,StatusLO,StatusEI,StatusEO,StatusOT,StatusGZOT,StatusGZ,StatusDO,StatusHD,StatusSL,StatusOD,StatusLeave,StatusMN,StatusIN,StatusBreak,ShifMin,ShfSplit,ProcessIn,Remarks,Tin0,Tout0,Tin1,Tout1,Tin2,Tout2,Tin3,Tout3,Tin4,Tout4,Tin5,Tout5,Tin6,Tout6,Tin7,Tout7,Tin8,Tout8,Tin9,Tout9,Tin10,Tout10,Tin11,Tout11,Tin12,Tout12,Tin13,Tout13,Tin14,Tout14,Tin15,Tout15")] Att_DailyAttendance _attData, FormCollection form, string NewDutyCode)
        {
            // User LoggedInUser = Session["LoggedUser"] as User;
            string _EmpDate = _attData.EmpDate;
            //ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
            ViewBag.ShiftList = new SelectList(db.Att_Shift.OrderBy(s => s.ShiftName), "ShiftID", "ShiftName");
            //ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
            ViewBag.SectionList = new SelectList(db.HR_Section.OrderBy(s => s.SectionName), "SectionID", "SectionName");
            //ViewBag.CompanyID = new SelectList(db.Companies, "CompID", "CompName", LoggedInUser.CompanyID);
            //ViewBag.CompanyIDJobCard = new SelectList(db.Companies, "CompID", "CompName", LoggedInUser.CompanyID);
            ViewBag.DesignationID = new SelectList(db.HR_Designation.OrderBy(s => s.DesignationName), "DesignationID", "DesignationName");
            try
            {
                string STimeIn = form["Inhours"].ToString();
                if (STimeIn.Count() < 4)
                    STimeIn = "0" + STimeIn;
                string STimeOut = form["OutHour"].ToString();
                if (STimeOut.Count() < 4)
                    STimeOut = "0" + STimeOut;
                string STimeInH = STimeIn.Substring(0, 2);
                string STimeInM = STimeIn.Substring(2, 2);
                string STimeOutH = STimeOut.Substring(0, 2);
                string STimeOutM = STimeOut.Substring(2, 2);
                string DutyTime = form["DutyTime"].ToString();
                string Remarks = form["NewRemarks"].ToString();
                string SDutyH = DutyTime.Substring(0, 2);
                string SDutyM = DutyTime.Substring(2, 2);
                string ShiftMinString = form["ShiftMinHidden"].ToString();
                if (TimeValid(STimeIn, STimeOut))
                {
                    TimeSpan _TimeIn = new TimeSpan(Convert.ToInt16(STimeInH), Convert.ToInt16(STimeInM), 0);
                    TimeSpan _TimeOut = new TimeSpan(Convert.ToInt16(STimeOutH), Convert.ToInt16(STimeOutM), 0);
                    TimeSpan _DutyTime = Convert.ToDateTime(form["DutyTime"].ToString()).TimeOfDay;
                    //TimeSpan _DutyTime = new TimeSpan(Convert.ToInt16(SDutyH), Convert.ToInt16(SDutyM), 0);
                    TimeSpan _ThresHoldTimeS = new TimeSpan(14, 00, 00);
                    TimeSpan _ThresHoldTimeE = new TimeSpan(06, 00, 00);
                    string date = Request.Form["Attdate"].ToString();
                    DateTime _AttDate = Convert.ToDateTime(date);
                    short ShiftMins = Convert.ToInt16(ShiftMinString);
                    DateTime _NewTimeIn = new DateTime();
                    DateTime _NewTimeOut = new DateTime();
                    _NewTimeIn = _AttDate + _TimeIn;
                    if (_TimeOut < _TimeIn)
                    {
                        _NewTimeOut = _AttDate.AddDays(1) + _TimeOut;
                    }
                    else
                    {
                        _NewTimeOut = _AttDate + _TimeOut;
                    }
                    int _UserID = Convert.ToInt32(Session["LoggedUserID"].ToString());

                    ManualAttendanceProcess _pma = new ManualAttendanceProcess(_EmpDate, "", false, _NewTimeIn, _NewTimeOut, NewDutyCode, _UserID, _DutyTime, Remarks, ShiftMins);
                    List<Att_DeviceData> _Polls = new List<Att_DeviceData>();
                    _Polls = db.Att_DeviceData.Where(aa => aa.EntDate == _AttDate && aa.EmpID == _attData.EmpID).OrderBy(a => a.EntTime).ToList();
                    ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                    ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                    //_attData = db.Att_DeviceData.First(aa => aa.EmpDate == _EmpDate);
                    Att_DailyAttendance _myAttData = new Att_DailyAttendance();
                    using (var ctx = new HRMEntities())
                    {
                        _myAttData = db.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _EmpDate);
                    }
                    ViewBag.SucessMessage = "Attendance record updated.";
                    if (_myAttData.WorkMin != null)
                        ViewBag.WorkMin = TimeSpan.FromMinutes((double)_myAttData.WorkMin);
                    if (_myAttData.LateOut != null)
                        ViewBag.LateOut = TimeSpan.FromMinutes((double)_myAttData.LateOut);
                    if (_myAttData.LateIn != null)
                        ViewBag.LateIn = TimeSpan.FromMinutes((double)_myAttData.LateIn);
                    if (_myAttData.EarlyOut != null)
                        ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_myAttData.EarlyOut);
                    if (_myAttData.EarlyIn != null)
                        ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_myAttData.EarlyIn);
                    if (_myAttData.StatusGZOT == true)
                        ViewBag.GZOT = TimeSpan.FromMinutes((double)_myAttData.GZOTMin);


                    return View("Edit", _myAttData);
                }
                else
                {
                    ViewBag.SucessMessage = "New Time In and New Time out is not valid";
                    //_attData = db.Att_DeviceData.First(aa => aa.EmpDate == _EmpDate);
                    return View(_attData);
                }

            }
            catch (Exception ex)
            {
                ViewBag.SucessMessage = "An error occured while saving Entry";
                //_attData = db.Att_DeviceData.First(aa => aa.EmpDate == _EmpDate);
                List<Att_DeviceData> _Polls = new List<Att_DeviceData>();
                _Polls = db.Att_DeviceData.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
                ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                return View(_attData);
            }
        }
        private bool TimeValid(string STimeIn, string STimeOut)
        {
            try
            {
                if (STimeIn.Count() == 4 && STimeOut.Count() == 4)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult NextEntry(FormCollection form)
        {
            try
            {
                //ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
                ViewData["datef"] = Convert.ToDateTime(Session["EditAttendanceDate"]).ToString("yyyy-MM-dd");
                int _EmpID = Convert.ToInt32(Session["NEmpNo"]);
                if (Session["NEmpNo"] != null)
                {
                    DateTime _AttDataFrom = Convert.ToDateTime(ViewData["datef"].ToString()).AddDays(1);
                    Att_DailyAttendance _attData = new Att_DailyAttendance();
                    _attData = db.Att_DailyAttendance.First(aa => aa.EmpID == _EmpID && aa.AttDate == _AttDataFrom);
                    if (_attData != null)
                    {
                        Session["EditAttendanceDate"] = Convert.ToDateTime(ViewData["datef"]).AddDays(1);
                        ViewBag.EmpID = new SelectList(db.HR_Employee.OrderBy(s => s.FullName), "EmpID", "EmpNo", _attData.EmpID);
                        List<Att_DeviceData> _Polls = new List<Att_DeviceData>();
                        string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
                        _Polls = db.Att_DeviceData.Where(aa => aa.EntDate == _AttDataFrom && aa.EmpID == _attData.EmpID).OrderBy(a => a.EntTime).ToList();
                        ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                        ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                        ViewBag.SucessMessage = "";
                        if (_attData.WorkMin != null)
                            ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
                        if (_attData.LateOut != null)
                            ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                        if (_attData.LateIn != null)
                            ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                        if (_attData.EarlyOut != null)
                            ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                        if (_attData.EarlyIn != null)
                            ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
                        if (_attData.NOTMin != null)
                            ViewBag.OT = TimeSpan.FromMinutes((double)_attData.NOTMin);
                        if (_attData.StatusGZOT == true)
                            ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                        return View("Edit", _attData);
                    }
                    else
                        return View("Index");
                }
                else
                    return View("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }
        }
        public ActionResult PreviousEntry()
        {
            try
            {
                ViewData["datef"] = Convert.ToDateTime(Session["EditAttendanceDate"]).ToString("yyyy-MM-dd");
                int _EmpID = Convert.ToInt32(Session["NEmpNo"]);
                if (_EmpID != null)
                {
                    DateTime _AttDataFrom = Convert.ToDateTime(ViewData["datef"].ToString()).AddDays(-1);
                    Att_DailyAttendance _attData = new Att_DailyAttendance();
                    _attData = db.Att_DailyAttendance.First(aa => aa.EmpID == _EmpID && aa.AttDate == _AttDataFrom);
                    if (_attData != null)
                    {
                        Session["EditAttendanceDate"] = Convert.ToDateTime(ViewData["datef"]).AddDays(-1);
                        ViewBag.EmpID = new SelectList(db.HR_Employee.OrderBy(s => s.EmpNo), "EmpID", "EmpNo", _attData.EmpID);
                        ViewBag.SucessMessage = "";
                        List<Att_DeviceData> _Polls = new List<Att_DeviceData>();
                        string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
                        _Polls = db.Att_DeviceData.Where(aa => aa.EntDate == _AttDataFrom && aa.EmpID == _attData.EmpID).OrderBy(a => a.EntTime).ToList();
                        ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                        ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                        if (_attData.WorkMin != null)
                            ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
                        if (_attData.LateOut != null)
                            ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                        if (_attData.LateIn != null)
                            ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                        if (_attData.EarlyOut != null)
                            ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                        if (_attData.EarlyIn != null)
                            ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
                        if (_attData.NOTMin != null)
                            ViewBag.OT = TimeSpan.FromMinutes((double)_attData.NOTMin);
                        if (_attData.StatusGZOT == true)
                            ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                        return View("Edit", _attData);
                    }
                    else
                        return View("Index");
                }
                else
                    return View("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }
        }

        #region -- Edit Entries for Single Employee---
        //Load Edit Entries View
        public ActionResult EditMultipleEntries()
        {
            try
            {
                if (Request.Form["EmpNo"].ToString() != "" && Request.Form["DateFrom"].ToString() != "" && Request.Form["DateTo"].ToString() != "")
                {
                    string _EmpNo = Request.Form["EmpNo"].ToString();
                    DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                    DateTime _AttDataTo = Convert.ToDateTime(Request.Form["DateTo"].ToString());
                    List<Att_DailyAttendance> dailyAttendance = new List<Att_DailyAttendance>();
                    dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.EmpNo == _EmpNo && aa.AttDate >= _AttDataFrom && aa.AttDate <= _AttDataTo).ToList();
                    if (dailyAttendance.Count > 0)
                        return View(EditAttManager.GetAttendanceAttributes(dailyAttendance, _AttDataFrom, _AttDataTo));
                    else
                        return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Save Edit Attendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEditEntries(AttEditSingleEmployee Model)
        {
            try
            {
                bool edited = false;
                int _UserID = Convert.ToInt32(Session["LoggedUserID"].ToString());
                int empID = Convert.ToInt32(Request.Form["empID"].ToString());
                DateTime dtFrom = Convert.ToDateTime(Request.Form["dateFrom"].ToString());
                DateTime dtTo = Convert.ToDateTime(Request.Form["dateTo"].ToString());
                List<Att_DailyAttendance> oldAttendance = new List<Att_DailyAttendance>();
                oldAttendance = db.Att_DailyAttendance.Where(aa => aa.EmpID == empID && aa.AttDate >= dtFrom && aa.AttDate <= dtTo).ToList();
                for (int i = 0; i < oldAttendance.Count; i++)
                {
                    string empDate = Request.Form["EmpDate" + i.ToString()].ToString();
                    Att_DailyAttendance att = oldAttendance.First(aa => aa.EmpDate == empDate);
                    string DutyCode = Request.Form["DutyCode" + i.ToString()].ToString();
                    string DutyTime = Request.Form["DutyTime" + i.ToString()].ToString();
                    string ShiftTime = Request.Form["ShiftTime" + i.ToString()].ToString();
                    //string TimeIn = Request.Form["TimeIn" + i.ToString()].ToString();
                    //string TimeOut = Request.Form["TimeOut" + i.ToString()].ToString();
                    string TIn1 = Request.Form["TimeIn1" + i.ToString()].ToString();
                    string TOut1 = Request.Form["TimeOut1" + i.ToString()].ToString();
                    string TIn2 = Request.Form["TimeIn2" + i.ToString()].ToString();
                    string TOut2 = Request.Form["TimeOut2" + i.ToString()].ToString();
                    string TIn3 = Request.Form["TimeIn3" + i.ToString()].ToString();
                    string TOut3 = Request.Form["TimeOut3" + i.ToString()].ToString();
                    string Remarks = Request.Form["Remarks" + i.ToString()].ToString();
                    EditAttendanceList editlist = EditAttManager.GetEditAttendanceList(empDate, DutyCode, DutyTime, ShiftTime, TIn1, TOut1, TIn2, TOut2, TIn3, TOut3, Remarks);
                    if (EditAttManager.CheckRecordIsEdited(att, editlist))
                    {
                        edited = true;
                        DateTime _NewTimeIn = new DateTime();
                        DateTime _NewTimeOut = new DateTime();
                        DateTime? _NewTimeIn1;
                        DateTime? _NewTimeOut1;
                        DateTime? _NewTimeIn2;
                        DateTime? _NewTimeOut2;
                        DateTime? _NewTimeIn3;
                        DateTime? _NewTimeOut3;
                        _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);

                        if (editlist.TimeIn != null && editlist.TimeOut != null)
                        {
                            if (editlist.TimeIn1 != null)
                                _NewTimeIn1 = (DateTime)(att.AttDate + editlist.TimeIn1);
                            else
                                _NewTimeIn1 = null;
                            if (editlist.TimeIn2 != null)
                                _NewTimeIn2 = (DateTime)(att.AttDate + editlist.TimeIn2);
                            else
                                _NewTimeIn2 = null;
                            if (editlist.TimeIn3 != null)
                                _NewTimeIn3 = (DateTime)(att.AttDate + editlist.TimeIn3);
                            else
                                _NewTimeIn3 = null;
                            if (editlist.TimeOut1 != null)
                                _NewTimeOut1 = (DateTime)(att.AttDate + editlist.TimeOut1);
                            else
                                _NewTimeOut1 = null;
                            if (editlist.TimeOut2 != null)
                                _NewTimeOut2 = (DateTime)(att.AttDate + editlist.TimeOut2);
                            else
                                _NewTimeOut2 = null;
                            if (editlist.TimeOut3 != null)
                                _NewTimeOut3 = (DateTime)(att.AttDate + editlist.TimeOut3);
                            else
                                _NewTimeOut3 = null;
                            if (editlist.TimeOut < editlist.TimeIn)
                            {
                                _NewTimeOut = att.AttDate.Value.AddDays(1) + editlist.TimeOut;
                            }
                            else
                            {
                                _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                            }
                            if (editlist.TimeOut1 < editlist.TimeIn1)
                            {
                                _NewTimeOut1 = att.AttDate.Value.AddDays(1) + editlist.TimeOut1;
                            }
                            if (editlist.TimeOut2 < editlist.TimeIn2)
                            {
                                _NewTimeOut2 = att.AttDate.Value.AddDays(1) + editlist.TimeOut2;
                            }
                            if (editlist.TimeOut3 < editlist.TimeIn3)
                            {
                                _NewTimeOut3 = att.AttDate.Value.AddDays(1) + editlist.TimeOut3;
                            }
                            ManualAttendanceProcess _pma = new ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, _UserID, editlist.DutyTime, "", (short)editlist.ShiftTime.TotalMinutes, _NewTimeIn1, _NewTimeOut1, _NewTimeIn2, _NewTimeOut2, _NewTimeIn3, _NewTimeOut3, Remarks);
                        }
                        else
                        {
                            if (editlist.TimeIn.TotalMinutes > 0)
                                _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);
                            if (editlist.TimeOut.TotalMinutes > 0)
                                _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                            ManualAttendanceProcess _pma = new ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, _UserID, editlist.DutyTime, "", (short)editlist.ShiftTime.TotalMinutes);
                        }
                    }
                    else
                    {

                    }
                }
                if (edited == true)
                {
                    DateTime dt = oldAttendance.OrderByDescending(aa => aa.AttDate).FirstOrDefault().AttDate.Value;
                    //Process Monthly if date is greater than 20 from start
                    ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(dt.Year, dt.Month, 1), DateTime.Today, oldAttendance.FirstOrDefault().EmpID.ToString());
                }
                using (var ctx = new HRMEntities())
                {
                    List<Att_DailyAttendance> dailyAttendance = new List<Att_DailyAttendance>();
                    dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.EmpID == empID && aa.AttDate >= dtFrom && aa.AttDate <= dtTo).ToList();
                    return View("EditMultipleEntries", EditAttManager.GetAttendanceAttributes(dailyAttendance, dtFrom, dtTo));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region -- Edit Entries for DateWise---
        //Load Edit Entries View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDateWiseEntries()
        {
            try
            {
                if (Request.Form["DateFrom"].ToString() != "")
                {
                    DateTime _AttDataTo = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                    int selectedID = 0;
                    string Criteria = "";
                    List<Att_DailyAttendance> dailyAttendance = new List<Att_DailyAttendance>();
                    switch (Request.Form["RosterSelectionRB"].ToString())
                    {
                        case "rbAll":
                            dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbAll";
                            break;
                        case "rbShift":
                            selectedID = Convert.ToInt32(Request.Form["ShiftList"].ToString());
                            dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.HR_Employee.ShiftID == selectedID && aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbShift";
                            break;
                        case "rbLocation":
                            selectedID = Convert.ToInt32(Request.Form["LocationList"].ToString());
                            dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.HR_Employee.LocationID == selectedID && aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbLocation";
                            break;

                        //case "rbGroup":
                        //    selectedID = Convert.ToInt32(Request.Form["GroupList"].ToString());
                        //    dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.HR_Employee.GroupID == selectedID && aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                        //    Criteria = "rbGroup";
                        //    break;
                        //case "rbDivision":
                        //    selectedID = Convert.ToInt32(Request.Form["DivisionList"].ToString());
                        //    dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.HR_Employee.HR_Section.HR_Department.DivsionID == selectedID && aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                        //    Criteria = "rbDivision";
                        //    break;
                        case "rbDepartment":
                            selectedID = Convert.ToInt32(Request.Form["DepartmentList"].ToString());
                            dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.HR_Employee.HR_Section.DepartmentID == selectedID && aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbDepartment";
                            break;
                        case "rbSection":
                            selectedID = Convert.ToInt32(Request.Form["SectionList"].ToString());
                            dailyAttendance = db.Att_DailyAttendance.Where(aa => aa.HR_Employee.SectionID == selectedID && aa.AttDate == _AttDataTo).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbSection";
                            break;
                    }
                    if (dailyAttendance.Count > 0)
                        return View(EditAttManager.GetAttendanceAttributesDateWise(dailyAttendance, _AttDataTo, Criteria, selectedID));
                    else
                        return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //Save Edit Attendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEditEntriesDateWise(AttEditSingleEmployee Model)
        {
            try
            {
                int _UserID = Convert.ToInt32(Session["LoggedUserID"].ToString());
                int count = Convert.ToInt32(Request.Form["Count"].ToString());
                DateTime dt = Convert.ToDateTime(Request.Form["Date"].ToString());
                string Criteria = Request.Form["Criteria"].ToString();
                int CriteriaData = Convert.ToInt32(Request.Form["CriteriaData"].ToString());
                List<Att_DailyAttendance> oldAttendance = new List<Att_DailyAttendance>();
                oldAttendance = db.Att_DailyAttendance.Where(aa => aa.AttDate == dt).ToList();
                string Message = "";
                for (int i = 0; i < count; i++)
                {
                    int EmpID = Convert.ToInt32(Request.Form["Row" + i.ToString()].ToString());
                    string empDate = Request.Form["EmpDate" + i.ToString()].ToString();
                    Att_DailyAttendance att = oldAttendance.First(aa => aa.EmpDate == empDate);
                    string DutyCode = Request.Form["DutyCode" + i.ToString()].ToString();
                    string DutyTime = Request.Form["DutyTime" + i.ToString()].ToString();
                    string ShiftTime = Request.Form["ShiftTime" + i.ToString()].ToString();
                    //string TimeIn = Request.Form["TimeIn" + i.ToString()].ToString();
                    //string TimeOut = Request.Form["TimeOut" + i.ToString()].ToString();
                    string TIn1 = Request.Form["TimeIn1" + i.ToString()].ToString();
                    string TOut1 = Request.Form["TimeOut1" + i.ToString()].ToString();
                    string TIn2 = Request.Form["TimeIn2" + i.ToString()].ToString();
                    string TOut2 = Request.Form["TimeOut2" + i.ToString()].ToString();
                    string TIn3 = Request.Form["TimeIn3" + i.ToString()].ToString();
                    string TOut3 = Request.Form["TimeOut3" + i.ToString()].ToString();
                    string Remarks = Request.Form["Remarks" + i.ToString()].ToString();
                    EditAttendanceList editlist = EditAttManager.GetEditAttendanceList(empDate, DutyCode, DutyTime, ShiftTime, TIn1, TOut1, TIn2, TOut2, TIn3, TOut3, Remarks);

                    if (EditAttManager.CheckRecordIsEdited(att, editlist))
                    {
                        DateTime _NewTimeIn = new DateTime();
                        DateTime _NewTimeOut = new DateTime();
                        DateTime? _NewTimeIn1;
                        DateTime? _NewTimeOut1;
                        DateTime? _NewTimeIn2;
                        DateTime? _NewTimeOut2;
                        DateTime? _NewTimeIn3;
                        DateTime? _NewTimeOut3;
                        _NewTimeIn = (DateTime)(att.AttDate + editlist.TimeIn);

                        if (editlist.TimeIn != null && editlist.TimeOut != null)
                        {
                            if (editlist.TimeIn1 != null)
                                _NewTimeIn1 = (DateTime)(att.AttDate + editlist.TimeIn1);
                            else
                                _NewTimeIn1 = null;
                            if (editlist.TimeIn2 != null)
                                _NewTimeIn2 = (DateTime)(att.AttDate + editlist.TimeIn2);
                            else
                                _NewTimeIn2 = null;
                            if (editlist.TimeIn3 != null)
                                _NewTimeIn3 = (DateTime)(att.AttDate + editlist.TimeIn3);
                            else
                                _NewTimeIn3 = null;
                            if (editlist.TimeOut1 != null)
                                _NewTimeOut1 = (DateTime)(att.AttDate + editlist.TimeOut1);
                            else
                                _NewTimeOut1 = null;
                            if (editlist.TimeOut2 != null)
                                _NewTimeOut2 = (DateTime)(att.AttDate + editlist.TimeOut2);
                            else
                                _NewTimeOut2 = null;
                            if (editlist.TimeOut3 != null)
                                _NewTimeOut3 = (DateTime)(att.AttDate + editlist.TimeOut3);
                            else
                                _NewTimeOut3 = null;
                            if (editlist.TimeOut < editlist.TimeIn)
                            {
                                _NewTimeOut = att.AttDate.Value.AddDays(1) + editlist.TimeOut;
                            }
                            else
                            {
                                _NewTimeOut = (DateTime)(att.AttDate + editlist.TimeOut);
                            }
                            if (editlist.TimeOut1 < editlist.TimeIn1)
                            {
                                _NewTimeOut1 = att.AttDate.Value.AddDays(1) + editlist.TimeOut1;
                            }
                            if (editlist.TimeOut2 < editlist.TimeIn2)
                            {
                                _NewTimeOut2 = att.AttDate.Value.AddDays(1) + editlist.TimeOut2;
                            }
                            if (editlist.TimeOut3 < editlist.TimeIn3)
                            {
                                _NewTimeOut3 = att.AttDate.Value.AddDays(1) + editlist.TimeOut3;
                            }
                            ManualAttendanceProcess _pma = new ManualAttendanceProcess(editlist.EmpDate, "", false, _NewTimeIn, _NewTimeOut, editlist.DutyCode, _UserID, editlist.DutyTime, "", (short)editlist.ShiftTime.TotalMinutes, _NewTimeIn1, _NewTimeOut1, _NewTimeIn2, _NewTimeOut2, _NewTimeIn3, _NewTimeOut3, Remarks);
                        }
                    }
                    else
                    {

                    }
                }
                using (var ctx = new HRMEntities())
                {
                    List<Att_DailyAttendance> dailyAttendance = new List<Att_DailyAttendance>();
                    switch (Criteria)
                    {
                        case "rbAll":
                            dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbAll";
                            break;
                        case "rbShift":
                            dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.HR_Employee.ShiftID == CriteriaData && aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbShift";
                            break;
                        case "rbLocation":
                            dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.HR_Employee.LocationID == CriteriaData && aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbLocation";
                            break;

                        //case "rbGroup":
                        //    dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.HR_Employee.GroupID == CriteriaData && aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                        //    Criteria = "rbGroup";
                        //    break;
                        //case "rbDivision":
                        //    dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.HR_Employee.HR_Section.HR_Department.DivsionID == CriteriaData && aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                        //    Criteria = "rbDivision";
                        //    break;
                        case "rbDepartment":
                            dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.HR_Employee.HR_Section.DepartmentID == CriteriaData && aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbDepartment";
                            break;
                        case "rbSection":
                            dailyAttendance = ctx.Att_DailyAttendance.Where(aa => aa.HR_Employee.SectionID == CriteriaData && aa.AttDate == dt).OrderByDescending(aa => aa.EmpID).ToList();
                            Criteria = "rbSection";
                            break;
                    }
                    return View("EditDateWiseEntries", EditAttManager.GetAttendanceAttributesDateWise(dailyAttendance, dt, Criteria, CriteriaData));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

    }
}
