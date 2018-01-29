using PTAWMS.Areas.Attendance.BusinessLogic.AttendaceHelper;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PTAWMS.Areas.Attendance.Controllers
{
    public class BulkJobCardController : Controller
    {
        //
        // GET: /Attendance/JobCard/
        public ActionResult Index()
        {
            try
            {
                VMJCApplication vmDemo = new VMJCApplication();
                vmDemo.DivEmployees = db.HR_Department.ToList();
                vmDemo.LocEmployees = db.HR_Location.ToList();
                vmDemo.TypeEmployees = db.HR_EmpType.ToList();
                vmDemo.DeptEmployees = db.HR_Department.ToList();
                vmDemo.SecEmployees = db.HR_Section.ToList();
                vmDemo.ShiftEmployees = db.Att_Shift.ToList();
                ViewBag.JobCardType = new SelectList(db.Att_JobCard.OrderBy(aa => aa.JCName).ToList(), "JCID", "JCName");
                return View(vmDemo);
            }
            catch (Exception)
            {

                throw;
            }
            //
            //ViewBag.RosterType = new SelectList(db.Att_RosterType.OrderBy(s => s.Name), "ID", "Name");
            //ViewBag.ShiftList = new SelectList(db.Att_Shift.OrderBy(s => s.ShiftName), "ShftID", "ShiftName");
            //ViewBag.LocationList = new SelectList(db.HR_Location.OrderBy(s => s.LocationName), "LocID", "LocationName");
            //ViewBag.GroupList = new SelectList(db.HR_Group.OrderBy(s => s.GroupName), "GrpID", "GroupName");
            //ViewBag.DivisionList = new SelectList(db.HR_Division.OrderBy(s => s.DivisionName), "DivID", "DivisionName");
            //ViewBag.DepartmentList = new SelectList(Company.GetDepartmentsWithDivision(db.HR_Department.OrderBy(s => s.DepartmentName).ToList()), "DeptID", "DepartmentName");
            //ViewBag.SectionList = new SelectList(Company.GetSectionsWithDeptDivision(db.HR_Section.OrderBy(s => s.SectionName).ToList()), "SecID", "SectionName");
            //return View();
        }
        HRMEntities db = new HRMEntities();
        [HttpPost]
        public ActionResult SelectEmployee(FormCollection form)
        {
            try
            {
                string ErrorMessage = "";
                List<EmpView> emps = new List<EmpView>();
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
                List<EmpView> ViewEmps = new List<EmpView>();
                List<EmpView> tempEmps = new List<EmpView>();
                VMJCApplication vm = new VMJCApplication();
                ViewEmps = emps.ToList();
                vm.CardType = Request.Form["JobCardType"].ToString();
                vm.DateEnded = Convert.ToDateTime(Request.Form["JobDateTo"]);
                vm.JCValue = Convert.ToInt32(Request.Form["JCValue"]);
                vm.DateStarted = Convert.ToDateTime(Request.Form["JobDateFrom"]);
                if (Request.Form["SelectionRB"].ToString() == "rbAll")
                {
                    ViewEmps = emps.ToList();
                    vm.JCCriteria = "A";
                }
                else
                {
                    var checkedDivs = form.GetValues("cbDivisions");
                    List<HR_Department> divs = new List<HR_Department>();
                    divs = db.HR_Department.ToList();
                    if (checkedDivs != null)
                    {
                        List<HR_Department> tempDivs = new List<HR_Department>();
                        foreach (var item in checkedDivs)
                        {
                            short id = Convert.ToInt16(item);
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.DepartmentID == id).ToList());
                            tempDivs.Add(divs.First(aa => aa.DeptID == id));
                            vm.CriteriaData = id;
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.DivEmployees = tempDivs;
                        vm.JCCriteria = "V";
                       // vm.CriteriaData = divs
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    var checkedLoc = form.GetValues("cbLocation");
                    List<HR_Location> locs = new List<HR_Location>();
                    locs = db.HR_Location.ToList();
                    if (checkedLoc != null)
                    {
                        List<HR_Location> tempLocs = new List<HR_Location>();
                        foreach (var item in checkedLoc)
                        {
                            short id = Convert.ToInt16(item);
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.LocID == id).ToList());
                            tempLocs.Add(locs.First(aa => aa.LocID == id));
                            vm.CriteriaData = id;
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.LocEmployees = tempLocs;
                        vm.JCCriteria = "L";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    //
                    var checkedType = form.GetValues("cbType");
                    List<HR_EmpType> types = new List<HR_EmpType>();
                    types = db.HR_EmpType.ToList();
                    if (checkedType != null)
                    {
                        List<HR_EmpType> tempTypes = new List<HR_EmpType>();
                        foreach (var item in checkedType)
                        {
                            short id = Convert.ToInt16(item);
                            string name = types.First(aa => aa.TypID == id).TypeName;
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.TypeName == name).ToList());
                            tempTypes.Add(types.First(aa => aa.TypID == id));
                            vm.CriteriaData = id;
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.TypeEmployees = tempTypes;
                        vm.JCCriteria = "T";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    //
                    var checkedDept = form.GetValues("cbDept");
                    List<HR_Department> depts = new List<HR_Department>();
                    depts = db.HR_Department.ToList();
                    if (checkedDept != null)
                    {
                        List<HR_Department> tempDepts = new List<HR_Department>();
                        foreach (var item in checkedDept)
                        {
                            short id = Convert.ToInt16(item);
                            //string name = depts.First(aa => aa.DeptID == id).DeptID;
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.DeptID == id).ToList());
                            tempDepts.Add(depts.First(aa => aa.DeptID == id));
                            vm.CriteriaData = id;
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.DeptEmployees = tempDepts;
                        vm.JCCriteria = "D";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    //
                    var checkedSec = form.GetValues("cbSec");
                    List<HR_Section> secs = new List<HR_Section>();
                    secs = db.HR_Section.ToList();
                    if (checkedSec != null)
                    {
                        List<HR_Section> tempSecs = new List<HR_Section>();
                        foreach (var item in checkedSec)
                        {
                            short id = Convert.ToInt16(item);
                            //string name = depts.First(aa => aa.DeptID == id).DeptID;
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.SectionID == id).ToList());
                            tempSecs.Add(secs.First(aa => aa.SecID == id));
                            vm.CriteriaData = id;
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.SecEmployees = tempSecs;
                        vm.JCCriteria = "S";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                        tempEmps.Clear();
                    }
                    //
                    var checkedShift = form.GetValues("cbShift");
                    List<Att_Shift> shifts = new List<Att_Shift>();
                    shifts = db.Att_Shift.ToList();
                    if (checkedShift != null)
                    {
                        List<Att_Shift> tempShifts = new List<Att_Shift>();
                        tempEmps.Clear();
                        foreach (var item in checkedShift)
                        {
                            short id = Convert.ToInt16(item);
                            //string name = depts.First(aa => aa.DeptID == id).DeptID;
                            tempEmps.AddRange(ViewEmps.Where(aa => aa.ShftID == id).ToList());
                            tempShifts.Add(shifts.First(aa => aa.ShftID == id));
                            vm.CriteriaData = id;
                        }
                        ViewEmps = tempEmps.ToList();
                        vm.ShiftEmployees = tempShifts;
                        vm.JCCriteria = "H";
                    }
                    else
                    {
                        tempEmps = ViewEmps.ToList();
                    }
                }
                //First Save Job Card Application 
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                Att_JobCardApp jobCardApp = new Att_JobCardApp();
                jobCardApp.JCTypeID = Convert.ToInt16(vm.CardType);
                jobCardApp.DateCreated = DateTime.Now;
                jobCardApp.UserID = LoggedInUser.UserID;
                jobCardApp.DateStarted = Convert.ToDateTime(Request.Form["JobDateFrom"]);
                jobCardApp.DateEnded = Convert.ToDateTime(Request.Form["JobDateTo"]);
                jobCardApp.Status = false;
                jobCardApp.JobCardCriteria = vm.JCCriteria;
                jobCardApp.CriteriaData = vm.CriteriaData;
                //jobCardApp.WorkMin = (short)vm.JCValue;
                db.Att_JobCardApp.Add(jobCardApp);
                if (db.SaveChanges() > 0)
                {

                }
                vm.Employees = ViewEmps;
                vm.JCAppID = jobCardApp.JobCardAppID;
                int jcID = Convert.ToInt32(vm.CardType);
                vm.CardName = db.Att_JobCard.First(aa => aa.JCID == jcID).JCName;
                if (ErrorMessage == "")
                    return View(vm);
                else
                {
                    VMJCApplication vmDemo = new VMJCApplication();
                    vmDemo.DivEmployees = db.HR_Department.ToList();
                    vmDemo.LocEmployees = db.HR_Location.ToList();
                    vmDemo.TypeEmployees = db.HR_EmpType.ToList();
                    vmDemo.DeptEmployees = db.HR_Department.ToList();
                    vmDemo.SecEmployees = db.HR_Section.ToList();
                    vmDemo.ShiftEmployees = db.Att_Shift.ToList();
                    ViewBag.JobCardType = new SelectList(db.Att_JobCard.OrderBy(aa => aa.JCName).ToList(), "JCID", "JCName");
                    return View("Index", vmDemo);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EditAttJobCard()
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            try
            {
                string _EmpNo = "";
                List<HR_Employee> _Emp = new List<HR_Employee>();
                short _WorkCardID = Convert.ToInt16(Request.Form["JobCardType"].ToString());
                //First Save Job Card Application
                Att_JobCardApp jobCardApp = new Att_JobCardApp();
                jobCardApp.JCTypeID = _WorkCardID;
                jobCardApp.DateCreated = DateTime.Now;
                jobCardApp.UserID = LoggedInUser.UserID;
                jobCardApp.DateStarted = Convert.ToDateTime(Request.Form["JobDateFrom"]);
                jobCardApp.DateEnded = Convert.ToDateTime(Request.Form["JobDateTo"]);
                jobCardApp.Status = false;

                switch (Request.Form["RosterSelectionRB"].ToString())
                {
                    case "rbAll":
                        jobCardApp.CriteriaData = 0;
                        jobCardApp.JobCardCriteria = "A";
                        db.Att_JobCardApp.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddAtt_JobCardAppToJobCardData();
                        }
                        break;
                    case "rbShift":
                        jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["ShiftList"].ToString());
                        jobCardApp.JobCardCriteria = "H";
                        db.Att_JobCardApp.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddAtt_JobCardAppToJobCardData();
                        }
                        break;
                    case "rbLocation":
                        jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["LocationList"].ToString());
                        jobCardApp.JobCardCriteria = "L";
                        db.Att_JobCardApp.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddAtt_JobCardAppToJobCardData();
                        }
                        break;
                    case "rbDivision":
                        jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["DivisionList"].ToString());
                        jobCardApp.JobCardCriteria = "V";
                        db.Att_JobCardApp.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddAtt_JobCardAppToJobCardData();
                        }
                        break;
                    case "rbDepartment":
                        jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["DepartmentList"].ToString());
                        jobCardApp.JobCardCriteria = "D";
                        db.Att_JobCardApp.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddAtt_JobCardAppToJobCardData();
                        }
                        break;
                    case "rbSection":
                        jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["SectionList"].ToString());
                        jobCardApp.JobCardCriteria = "S";
                        db.Att_JobCardApp.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddAtt_JobCardAppToJobCardData();
                        }
                        break;
                    case "rbEmployee":
                        _EmpNo = Request.Form["EmpNo"];
                        _Emp = db.HR_Employee.Where(aa => aa.EmpNo == _EmpNo).ToList();
                        if (_Emp.Count > 0)
                        {
                            jobCardApp.CriteriaData = _Emp.FirstOrDefault().EmployeeID;
                            jobCardApp.JobCardCriteria = "E";
                            db.Att_JobCardApp.Add(jobCardApp);
                            if (db.SaveChanges() > 0)
                            {
                                AddAtt_JobCardAppToJobCardData();
                            }
                        }
                        break;
                }

                //Add Job Card to JobCardData and Mark Legends in Attendance Data if attendance Created
                Session["EditAttendanceDate"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                ViewBag.JobCardType = new SelectList(db.Att_JobCard, "JCID", "JCName");
                ViewBag.RosterType = new SelectList(db.Att_RosterType.OrderBy(s => s.Name), "ID", "Name");
                ViewBag.ShiftList = new SelectList(db.Att_Shift.OrderBy(s => s.ShiftName), "ShftID", "ShiftName");
                ViewBag.LocationList = new SelectList(db.HR_Location.OrderBy(s => s.LocationName), "LocID", "LocationName");
                ViewBag.DivisionList = new SelectList(db.HR_Department.OrderBy(s => s.DepartmentName), "DeptID", "DepartmentName");
                ViewBag.CMessage = "Job Card Created sucessfully";
                ViewData["datef"] = Session["EditAttendanceDate"].ToString();
                ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                return View("Index");
            }
            catch (Exception)
            {
                Session["EditAttendanceDate"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                ViewBag.JobCardType = new SelectList(db.Att_JobCard, "JCID", "JCName");
                ViewBag.RosterType = new SelectList(db.Att_RosterType.OrderBy(s => s.Name), "ID", "Name");
                ViewBag.ShiftList = new SelectList(db.Att_Shift.OrderBy(s => s.ShiftName), "ShftID", "ShiftName");
                ViewBag.LocationList = new SelectList(db.HR_Location.OrderBy(s => s.LocationName), "LocID", "LocationName");
                ViewBag.DivisionList = new SelectList(db.HR_Department.OrderBy(s => s.DepartmentName), "DeptID", "DepartmentName");
                ViewBag.CMessage = "Job Card Created sucessfully";
                ViewData["datef"] = Session["EditAttendanceDate"].ToString();
                ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                return View("Index");
            }
        }

        private bool ValidateJobCard(DateTime dateStart, short CardType)
        {
            try
            {
                bool check = false;
                using (var ctx = new HRMEntities())
                {
                    List<Att_JobCardApp> jcApp = new List<Att_JobCardApp>();
                    if (ctx.Att_JobCardApp.Where(aa => aa.DateStarted == dateStart && aa.JCTypeID == CardType).Count() > 0)
                        check = true;
                    ctx.Dispose();
                }
                return check;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult AddEmployeeForJC(FormCollection form)
        {
            try
            {
                List<EmpView> emps = new List<EmpView>();
                List<EmpView> SelectedEmps = new List<EmpView>();
                var checkedEmps = form.GetValues("cbEmployee");
                int JcAppID = Convert.ToInt32(Request.Form["JCAppID"]);
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
                Att_JobCardApp jcApp = db.Att_JobCardApp.First(aa => aa.JobCardAppID == JcAppID);
                foreach (var item in checkedEmps)
                {
                    int empid = Convert.ToInt32(item);
                    AddJobCardData(emps.First(aa => aa.EmployeeID == empid), jcApp);
                }

                return RedirectToAction("ListOfJobCardApp");
            }
            catch (Exception)
            {

                throw;
            }
        }
        //Add Job Card To Job Card Data
        private void AddAtt_JobCardAppToJobCardData()
        {
            try
            {
                using (var ctx = new HRMEntities())
                {
                    List<Att_JobCardApp> _jobCardApp = new List<Att_JobCardApp>();
                    _jobCardApp = ctx.Att_JobCardApp.Where(aa => aa.Status == false).ToList();
                    List<EmpView> _Emp = new List<EmpView>();
                    List<Att_DailyAttendance> attdatas = new List<Att_DailyAttendance>();
                    foreach (var jcApp in _jobCardApp)
                    {
                        jcApp.Status = true;
                        switch (jcApp.JobCardCriteria)
                        {
                            case "A":
                                _Emp = ctx.EmpViews.Where(aa => aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                            case "H":
                                short _shiftID = Convert.ToByte(jcApp.CriteriaData);
                                _Emp = ctx.EmpViews.Where(aa => aa.ShftID == _shiftID && aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                            case "L":
                                short _LocID = Convert.ToByte(jcApp.CriteriaData);
                                _Emp = ctx.EmpViews.Where(aa => aa.ShftID == _LocID && aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                            case "V":
                                short _divID = Convert.ToByte(jcApp.CriteriaData);
                                _Emp = ctx.EmpViews.Where(aa => aa.ShftID == _divID && aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                            case "D":
                                short _deptID = Convert.ToByte(jcApp.CriteriaData);
                                _Emp = ctx.EmpViews.Where(aa => aa.ShftID == _deptID && aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                            case "S":
                                short _secID = Convert.ToByte(jcApp.CriteriaData);
                                _Emp = ctx.EmpViews.Where(aa => aa.SectionID == _secID && aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                            case "E":
                                int _EmpID = (int)jcApp.CriteriaData;
                                _Emp = ctx.EmpViews.Where(aa => aa.EmployeeID == _EmpID && aa.Status == "Active" && aa.ProcessAttendance == true).ToList();
                                break;
                        }
                        attdatas = db.Att_DailyAttendance.Where(aa => aa.AttDate >= jcApp.DateStarted && aa.AttDate <= jcApp.DateEnded).ToList();
                        foreach (var selectedEmp in _Emp)
                        {
                            AddJobCardData(selectedEmp, jcApp);
                        }

                        jcApp.Status = true;
                    }
                    ctx.SaveChanges();
                    ctx.Dispose();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void AddJobCardData(EmpView _selEmp, Att_JobCardApp jcApp)
        {
            try
            {
                int _empID = _selEmp.EmployeeID;
                string _empDate = "";
                int _userID = (int)jcApp.UserID;
                DateTime _Date = (DateTime)jcApp.DateStarted;
                while (_Date <= jcApp.DateEnded)
                {
                    _empDate = _empID + _Date.ToString("yyMMdd");
                    if (jcApp.JCTypeID == 9 || jcApp.JCTypeID == 10)
                    {
                        AddJobCardDataToDatabase(_empDate, _empID, _Date, _userID, jcApp);
                    }
                    else
                    {
                        AddJobCardDataToDatabase(_empDate, _empID, _Date, _userID, jcApp);
                    }
                    _Date = _Date.AddDays(1);
                }
                ProcessSupportFunc.ProcessAttendanceRequest((DateTime)jcApp.DateStarted, (DateTime)jcApp.DateEnded, _selEmp.EmployeeID, _selEmp.EmpNo);


            }
            catch (Exception)
            {

                throw;
            }
        }
        private bool AddJobCardDataToDatabase(string _empDate, int _empID, DateTime _currentDate, int _userID, Att_JobCardApp jcApp)
        {
            bool check = false;
            try
            {
                Att_JobCardDetail _jobCardEmp = new Att_JobCardDetail();
               // jcApp.CriteriaData = 
                _jobCardEmp.EmpDate = _empDate;
                _jobCardEmp.EmpID = _empID;
                _jobCardEmp.Dated = _currentDate;
                _jobCardEmp.WrkCardID = jcApp.JCTypeID;
                _jobCardEmp.DateCreated = DateTime.Now;
               // _jobCardEmp.WorkMin = jcApp.WorkMin;
                _jobCardEmp.JCAppID = jcApp.JobCardAppID;
               // _jobCardEmp.OtherValue = jcApp.OtherValue;
                db.Att_JobCardDetail.Add(_jobCardEmp);
                if (db.SaveChanges() > 0)
                {
                    check = true;
                }
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }
        //private void AddJobCardData(EmpView _selEmp, Att_JobCardApp jcApp, List<Att_DailyAttendance> attDatas)
        //{
        //    try
        //    {
        //        int _empID = _selEmp.EmployeeID;
        //        string _empDate = "";
        //        int _userID = (int)jcApp.UserID;
        //        DateTime _Date = (DateTime)jcApp.DateStarted;
        //        while (_Date <= jcApp.DateEnded)
        //        {
        //            _empDate = _empID + _Date.ToString("yyMMdd");
        //            if (jcApp.JCTypeID == 9 || jcApp.JCTypeID == 10)
        //            {
        //                if (attDatas.Where(aa => aa.EmpID == _selEmp.EmployeeID && aa.WorkMin > 0 && aa.EmpDate == _empDate).Count() > 0)
        //                    AddJobCardDataToDatabase(_empDate, _empID, _Date, _userID, jcApp);
        //            }
        //            else
        //            {
        //                AddJobCardDataToDatabase(_empDate, _empID, _Date, _userID, jcApp);
        //            }
        //            _Date = _Date.AddDays(1);
        //        }
        //        foreach (var item in db.Att_JobCardDetail.Where(aa => aa.JCAppID == jcApp.JobCardAppID).ToList())
        //        {
        //            if (item.WorkMin == null)
        //            {
        //                if (item.WrkCardID == 1 || item.WrkCardID == 7 || item.WrkCardID == 8)
        //                {
        //                    HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == item.EmpID);
        //                    item.WorkMin = emp.Att_Shift.MonMin;
        //                }
        //                else if (item.WrkCardID == 9 || item.WrkCardID == 10)
        //                {

        //                }
        //                else
        //                    item.WorkMin = 0;
        //            }
        //            switch (item.WrkCardID)
        //            {
        //                case 1://Present
        //                    AddJCNorrmalDayAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 2://Absent
        //                    AddJCAbsentToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 3://Rest
        //                    AddJCDayOffToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 4://Public Holiday
        //                    AddJCGZDayToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 5://Remove rest
        //                    RemoveRestFromAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 6://Remove GZ
        //                    RemoveGZDayFromAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 7://Official Duty
        //                    AddJCODDayToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 8://Official Visit
        //                    AddJCOVDayToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 9://Late In Relaxation
        //                    AddJCLateInRelToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //                case 10://Early Out Relaxation
        //                    AddJCEarlyOutReToAttData(item.EmpDate, (int)item.EmpID, (DateTime)item.Dated, (short)item.WrkCardID, (short)item.WorkMin);
        //                    break;
        //            }
        //        }
        //        ProcessSupportFunc.ProcessAttendanceRequestMonthly(new DateTime(jcApp.DateStarted.Value.Year, jcApp.DateStarted.Value.Month, 1), DateTime.Today, _selEmp.EmployeeID.ToString());


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //private bool AddJobCardDataToDatabase(string _empDate, int _empID, DateTime _currentDate, int _userID, Att_JobCardApp jcApp)
        //{
        //    bool check = false;
        //    try
        //    {
        //        Att_JobCardDetail _jobCardEmp = new Att_JobCardDetail();
        //        _jobCardEmp.EmpDate = _empDate;
        //        _jobCardEmp.EmpID = _empID;
        //        _jobCardEmp.Dated = _currentDate;
        //        _jobCardEmp.WrkCardID = jcApp.JCTypeID;
        //        _jobCardEmp.DateCreated = DateTime.Now;
        //        _jobCardEmp.WorkMin = jcApp.WorkMin;
        //        _jobCardEmp.JCAppID = jcApp.JobCardAppID;
        //        _jobCardEmp.OtherValue = jcApp.OtherValue;
        //        db.Att_JobCardDetail.Add(_jobCardEmp);
        //        if (db.SaveChanges() > 0)
        //        {
        //            check = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        check = false;
        //    }
        //    return check;
        //}

        #region --Job Cards - Att_DailyAttendance ---
        private bool AddJCNorrmalDayAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "D";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = true;
                            _attdata.WorkMin = WorkMin;
                            _attdata.ShifMin = WorkMin;
                            _attdata.PDays = 1;
                            _attdata.AbDays = 0;
                            _attdata.LeaveDays = 0;
                            _attdata.Remarks = "[Present]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                            //_attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                        else
                        {
                            if (_attdata.StatusHL == true)
                            {
                                _attdata.DutyCode = "D";
                                _attdata.StatusAB = null;
                                _attdata.StatusDO = null;
                                _attdata.StatusLeave = null;
                                //_attdata.StatusHL = null;
                                _attdata.StatusGZ = null;
                                _attdata.StatusGZOT = null;
                                _attdata.StatusOD = null;
                                _attdata.StatusOT = null;
                                _attdata.StatusSL = null;
                                _attdata.StatusP = true;
                                _attdata.WorkMin = WorkMin;
                                _attdata.ShifMin = WorkMin;
                                _attdata.PDays = 0.5;
                                _attdata.AbDays = 0;
                                _attdata.LeaveDays = 0.5;
                                _attdata.Remarks = "[Present]";
                                //_attdata.TimeIn = null;
                                //_attdata.TimeOut = null;
                                _attdata.EarlyIn = null;
                                _attdata.EarlyOut = null;
                                _attdata.LateIn = null;
                                _attdata.LateOut = null;
                                //_attdata.OTMin = null;
                                _attdata.StatusEI = null;
                                _attdata.StatusEO = null;
                                _attdata.StatusLI = null;
                                _attdata.StatusLO = null;
                                context.SaveChanges();
                                if (context.SaveChanges() > 0)
                                    check = true;
                            }
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }
        private bool AddJCAbsentToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "D";
                            _attdata.StatusAB = true;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.WorkMin = 0;
                            _attdata.LeaveDays = 0;
                            _attdata.PDays = 0;
                            _attdata.AbDays = 1;
                            _attdata.ShifMin = WorkMin;
                            _attdata.Remarks = "[Absent]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                            //_attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }
        private bool AddJCDayOffToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //Day Off
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "R";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = true;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = false;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.StatusHL = null;
                            _attdata.PDays = 0;
                            _attdata.AbDays = 0;
                            _attdata.LeaveDays = 0;
                            _attdata.WorkMin = 0;
                            _attdata.ShifMin = 0;
                            _attdata.Remarks = "[DO]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                            //_attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }
        private bool AddJCGZDayToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //GZ Holiday
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "G";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = true;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.PDays = 0;
                            _attdata.AbDays = 0;
                            _attdata.LeaveDays = 0;
                            _attdata.WorkMin = 0;
                            _attdata.ShifMin = 0;
                            _attdata.Remarks = "[GZ]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                            //_attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }

        private bool RemoveRestFromAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //GZ Holiday
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            if (_attdata.StatusHL == true)
                            {
                                _attdata.AbDays = 0.5;
                                _attdata.LeaveDays = 0.5;
                                _attdata.PDays = 0;
                            }
                            else
                            {
                                _attdata.AbDays = 1;
                                _attdata.LeaveDays = 0;
                                _attdata.PDays = 0;
                            }
                            _attdata.DutyCode = "D";
                            _attdata.StatusAB = true;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.WorkMin = 0;
                            _attdata.PDays = 0;
                            _attdata.ShifMin = WorkMin;
                            _attdata.Remarks = "[Absent]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                           // _attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }
        private bool RemoveGZDayFromAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //GZ Holiday
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "D";
                            _attdata.StatusAB = true;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.WorkMin = 0;
                            _attdata.ShifMin = WorkMin;
                            _attdata.Remarks = "[Absent]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                           // _attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }

        private bool AddJCODDayToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {

            bool check = false;
            try
            {
                //Official Duty
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "O";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = true;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.WorkMin = WorkMin;
                            _attdata.ShifMin = WorkMin;
                            _attdata.Remarks = "[OD]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                           // _attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }
        private bool AddJCOVDayToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {

            bool check = false;
            try
            {
                //Official Duty
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "O";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = true;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = null;
                            _attdata.WorkMin = WorkMin;
                            _attdata.ShifMin = WorkMin;
                            _attdata.Remarks = "[OV]";
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateIn = null;
                            _attdata.LateOut = null;
                           // _attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return check;
        }
        private bool AddJCLateInRelToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "D";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = true;
                            _attdata.ShifMin = (short)(_attdata.ShifMin - WorkMin);
                            _attdata.LateIn = (short)(_attdata.LateIn - WorkMin);
                            if (_attdata.LateIn < 0)
                                _attdata.LateIn = 0;
                            _attdata.TotalShortMin = (short)(_attdata.TotalShortMin - WorkMin);
                            if (_attdata.TotalShortMin < 0)
                                _attdata.TotalShortMin = 0;
                            _attdata.PDays = 1;
                            _attdata.AbDays = 0;
                            _attdata.LeaveDays = 0;
                            _attdata.Remarks = _attdata.Remarks.Replace("[LI]", "");
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateOut = null;
                           // _attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                        else
                        {
                            if (_attdata.StatusHL == true)
                            {
                                _attdata.StatusAB = null;
                                _attdata.StatusDO = null;
                                _attdata.StatusLeave = null;
                                //_attdata.StatusHL = null;
                                _attdata.StatusGZ = null;
                                _attdata.StatusGZOT = null;
                                _attdata.StatusOD = null;
                                _attdata.StatusOT = null;
                                _attdata.StatusSL = null;
                                _attdata.StatusP = true;
                                _attdata.ShifMin = (short)(_attdata.ShifMin - WorkMin);
                                _attdata.LateIn = (short)(_attdata.LateIn - WorkMin);
                                if (_attdata.LateIn < 0)
                                    _attdata.LateIn = 0;
                                _attdata.TotalShortMin = (short)(_attdata.TotalShortMin - WorkMin);
                                if (_attdata.TotalShortMin < 0)
                                    _attdata.TotalShortMin = 0;
                                _attdata.PDays = 0.5;
                                _attdata.AbDays = 0;
                                _attdata.LeaveDays = 0.5;
                                _attdata.Remarks = _attdata.Remarks.Replace("[LI]", "");
                                //_attdata.TimeIn = null;
                                //_attdata.TimeOut = null;
                                _attdata.EarlyIn = null;
                                _attdata.EarlyOut = null;
                                _attdata.LateIn = null;
                                _attdata.LateOut = null;
                               // _attdata.OTMin = null;
                                _attdata.StatusEI = null;
                                _attdata.StatusEO = null;
                                _attdata.StatusLI = null;
                                _attdata.StatusLO = null;
                                context.SaveChanges();
                                if (context.SaveChanges() > 0)
                                    check = true;
                            }
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }
        private bool AddJCEarlyOutReToAttData(string _empDate, int _empID, DateTime _Date, short _WorkCardID, short WorkMin)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new HRMEntities())
                {
                    Att_DailyAttendance _attdata = context.Att_DailyAttendance.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    Att_JobCard _jcCard = context.Att_JobCard.FirstOrDefault(aa => aa.JCID == _WorkCardID);
                    if (_attdata != null)
                    {
                        if (_attdata.DutyCode != "L")
                        {
                            _attdata.DutyCode = "D";
                            _attdata.StatusAB = null;
                            _attdata.StatusDO = null;
                            _attdata.StatusLeave = null;
                            _attdata.StatusHL = null;
                            _attdata.StatusGZ = null;
                            _attdata.StatusGZOT = null;
                            _attdata.StatusOD = null;
                            _attdata.StatusOT = null;
                            _attdata.StatusSL = null;
                            _attdata.StatusP = true;
                            _attdata.ShifMin = (short)(_attdata.ShifMin - WorkMin);
                            _attdata.EarlyOut = (short)(_attdata.EarlyOut - WorkMin);
                            if (_attdata.EarlyOut < 0)
                                _attdata.EarlyOut = 0;
                            _attdata.TotalShortMin = (short)(_attdata.TotalShortMin - WorkMin);
                            if (_attdata.TotalShortMin < 0)
                                _attdata.TotalShortMin = 0;
                            _attdata.PDays = 1;
                            _attdata.AbDays = 0;
                            _attdata.LeaveDays = 0;
                            _attdata.Remarks = _attdata.Remarks.Replace("[EO]", "");
                            _attdata.TimeIn = null;
                            _attdata.TimeOut = null;
                            _attdata.EarlyIn = null;
                            _attdata.EarlyOut = null;
                            _attdata.LateOut = null;
                           // _attdata.OTMin = null;
                            _attdata.StatusEI = null;
                            _attdata.StatusEO = null;
                            _attdata.StatusLI = null;
                            _attdata.StatusLO = null;
                            context.SaveChanges();
                            if (context.SaveChanges() > 0)
                                check = true;
                        }
                        else
                        {
                            if (_attdata.StatusHL == true)
                            {
                                _attdata.StatusAB = null;
                                _attdata.StatusDO = null;
                                _attdata.StatusLeave = null;
                                //_attdata.StatusHL = null;
                                _attdata.StatusGZ = null;
                                _attdata.StatusGZOT = null;
                                _attdata.StatusOD = null;
                                _attdata.StatusOT = null;
                                _attdata.StatusSL = null;
                                _attdata.StatusP = true;
                                _attdata.ShifMin = (short)(_attdata.ShifMin - WorkMin);
                                _attdata.EarlyOut = (short)(_attdata.EarlyOut - WorkMin);
                                if (_attdata.EarlyOut < 0)
                                    _attdata.EarlyOut = 0;
                                _attdata.TotalShortMin = (short)(_attdata.TotalShortMin - WorkMin);
                                if (_attdata.TotalShortMin < 0)
                                    _attdata.TotalShortMin = 0;
                                _attdata.PDays = 0.5;
                                _attdata.AbDays = 0;
                                _attdata.LeaveDays = 0.5;
                                _attdata.Remarks = _attdata.Remarks.Replace("[EO]", "");
                                //_attdata.TimeIn = null;
                                //_attdata.TimeOut = null;
                                _attdata.EarlyIn = null;
                                _attdata.EarlyOut = null;
                                _attdata.LateIn = null;
                                _attdata.LateOut = null;
                               // _attdata.OTMin = null;
                                _attdata.StatusEI = null;
                                _attdata.StatusEO = null;
                                _attdata.StatusLI = null;
                                _attdata.StatusLO = null;
                                context.SaveChanges();
                                if (context.SaveChanges() > 0)
                                    check = true;
                            }
                        }
                    }
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }
        #endregion

        #region Job cArd List
        public ActionResult ListOfJobCardApp(FormCollection form, string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateStartedSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateEndedSortParm = sortOrder == "designation" ? "designation_desc" : "designation";
                ViewBag.CardTypeParm = sortOrder == "location" ? "location_desc" : "location";
                ViewBag.JCCriteriaSortParm = sortOrder == "section" ? "section_desc" : "section";
                ViewBag.CriteriaDataSortParm = sortOrder == "wing" ? "wing_desc" : "wing";
                ViewBag.ShiftSortParm = sortOrder == "shift" ? "shift_desc" : "shift";
                ViewBag.UserSortParm = sortOrder == "type" ? "type_desc" : "type";

                List<VMJCAppRecordList> _JCAppList = new List<VMJCAppRecordList>();
                _JCAppList = GetJCApp(_JCAppList);
                _JCAppList = _JCAppList.OrderByDescending(aa => aa.JCAppID).ToList();
                //List<EmpView> emps = new List<EmpView>();
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;


                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(_JCAppList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<VMJCAppRecordList> GetJCApp(List<VMJCAppRecordList> _JCAppList)
        {
            List<Att_JobCardApp> jobcardsApps = new List<Att_JobCardApp>();
            jobcardsApps = db.Att_JobCardApp.ToList();
            List<User> users = new List<Models.User>();
            users = db.Users.ToList();
            List<Att_Shift> shifts = db.Att_Shift.ToList();
            List<HR_Location> locations = db.HR_Location.ToList();
            List<HR_Department> divisions = db.HR_Department.ToList();
            List<HR_Department> departments = db.HR_Department.ToList();
            List<HR_Section> sections = db.HR_Section.ToList();
            List<HR_EmpType> emptype = db.HR_EmpType.ToList();
            List<HR_Employee> emps = db.HR_Employee.ToList();
            foreach (var item in jobcardsApps)
            {
                try
                {
                    VMJCAppRecordList _JCApplication = new VMJCAppRecordList();
                    _JCApplication.JCAppID = item.JobCardAppID;
                    _JCApplication.DateStarted = item.DateStarted;
                    _JCApplication.DateEnded = item.DateEnded;
                    _JCApplication.CardType = item.Att_JobCard.JCName;
                    _JCApplication.User = users.First(aa => aa.UserID == item.UserID).UserName;
                    switch (item.JobCardCriteria)
                    {
                        case "A"://all 
                            _JCApplication.CriteriaData = "All";
                            _JCApplication.JCCriteria = "All";
                            break;
                        case "H"://all shift
                            _JCApplication.CriteriaData = shifts.FirstOrDefault(aa => aa.ShftID == item.CriteriaData).ShiftName;
                            _JCApplication.JCCriteria = "Shift";
                            break;
                        case "L"://all loc
                            _JCApplication.CriteriaData = locations.FirstOrDefault(aa => aa.LocID == item.CriteriaData).LocationName;
                            _JCApplication.JCCriteria = "Location";
                            break;
                        case "D"://div
                            _JCApplication.CriteriaData = divisions.FirstOrDefault(aa => aa.DeptID == item.CriteriaData).DepartmentName;
                            _JCApplication.JCCriteria = "Division";
                            break;
                        case "S"://dept
                            _JCApplication.CriteriaData = sections.FirstOrDefault(aa => aa.SecID == item.CriteriaData).SectionName;
                            _JCApplication.JCCriteria = "Department";
                            break;
                        case "E"://emp
                            _JCApplication.CriteriaData = emps.FirstOrDefault(aa => aa.EmployeeID == item.CriteriaData).FullName;
                            _JCApplication.JCCriteria = "Emp";
                            break;
                        case "T"://emp
                            _JCApplication.CriteriaData = emptype.FirstOrDefault(aa => aa.TypID == item.CriteriaData).TypeName;
                            _JCApplication.JCCriteria = "EmpType";
                            break;
                    }
                    _JCAppList.Add(_JCApplication);
                }
                catch (Exception ex)
                {
                }
            }
            return _JCAppList;
        }
        public ActionResult Delete(int ID)
        {
            try
            {
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_JobCardApp jc_app = db.Att_JobCardApp.Find(ID);
                if (jc_app == null)
                {
                    return HttpNotFound();
                }
                List<Att_JobCardDetail> jc_Details = new List<Att_JobCardDetail>();
                jc_Details = db.Att_JobCardDetail.Where(aa => aa.JCAppID == ID).ToList();
                foreach (var item in jc_Details)
                {
                    db.Att_JobCardDetail.Remove(item);
                    db.SaveChanges();
                }
                db.Att_JobCardApp.Remove(jc_app);
                db.SaveChanges();
                return RedirectToAction("ListOfJobCardApp");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DeleteJCDetail(int ID)
        {
            try
            {
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Att_JobCardDetail jc_detail = db.Att_JobCardDetail.Find(ID);
                if (jc_detail == null)
                {
                    return HttpNotFound();
                }
                int jcAppID = (int)jc_detail.JCAppID;
                db.Att_JobCardDetail.Remove(jc_detail);
                db.SaveChanges();
                return RedirectToAction("Details", new { ID = jcAppID });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Details(int ID)
        {
            try
            {
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<ViewJobCardEmp> jc_app = db.ViewJobCardEmps.Where(aa => aa.JCAppID == ID).ToList();
                if (jc_app == null)
                {
                    return HttpNotFound();
                }
                return View("ListOfJobCardDetail", jc_app);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
    public class VMJCAppRecordList
    {
        public int JCAppID { get; set; }
        public Nullable<System.DateTime> DateStarted { get; set; }
        public Nullable<System.DateTime> DateEnded { get; set; }
        public string JCCriteria { get; set; }
        public string CriteriaData { get; set; }
        public string CardType { get; set; }
        public string User { get; set; }
    }
    public class VMJCApplication
    {
        public int JCAppID { get; set; }
        public Nullable<System.DateTime> DateStarted { get; set; }
        public Nullable<System.DateTime> DateEnded { get; set; }
        public string JCCriteria { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public int JCValue { get; set; }
        public int CriteriaData { get; set; }
        public List<HR_Department> DivEmployees { get; set; }
        public List<HR_Location> LocEmployees { get; set; }
        public List<HR_EmpType> TypeEmployees { get; set; }
        public List<Att_Shift> ShiftEmployees { get; set; }
       // public List<HR_Group> GroupEmployees { get; set; }
        public List<HR_Department> DeptEmployees { get; set; }
        public List<HR_Section> SecEmployees { get; set; }
        public List<EmpView> Employees { get; set; }
    }
}