using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PTAWMS.Models;
using WMSLibrary;

namespace PTAWMS.Reports.BusinessLogic
{
    public class AttRptFilter
    {
        public static List<Models.ViewAttDataDetail> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<Models.ViewAttDataDetail> _TempViewAttDataDetailed, List<Models.ViewAttDataDetail> _ViewAttDataDetailed)
        {
            //for location
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttDataDetailed.AddRange(_ViewAttDataDetailed.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttDataDetailed = _TempViewAttDataDetailed.ToList();
            }
            else
                _TempViewAttDataDetailed = _ViewAttDataDetailed.ToList();
            _TempViewAttDataDetailed.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttDataDetailed.AddRange(_ViewAttDataDetailed.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttDataDetailed = _TempViewAttDataDetailed.ToList();
            }
            else
                _TempViewAttDataDetailed = _ViewAttDataDetailed.ToList();


            _TempViewAttDataDetailed.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttDataDetailed.AddRange(_ViewAttDataDetailed.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttDataDetailed = _TempViewAttDataDetailed.ToList();
            }
            else
                _TempViewAttDataDetailed = _ViewAttDataDetailed.ToList();
            _TempViewAttDataDetailed.Clear();    
            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttDataDetailed.AddRange(_ViewAttDataDetailed.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttDataDetailed = _TempViewAttDataDetailed.ToList();
            }
            else
                _TempViewAttDataDetailed = _ViewAttDataDetailed.ToList();
            _TempViewAttDataDetailed.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttDataDetailed.AddRange(_ViewAttDataDetailed.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttDataDetailed = _TempViewAttDataDetailed.ToList();
            }
            else
                _TempViewAttDataDetailed = _ViewAttDataDetailed.ToList();
            _TempViewAttDataDetailed.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttDataDetailed.AddRange(_ViewAttDataDetailed.Where(aa => aa.EmpID == _empID).ToList());
                }
                _ViewAttDataDetailed = _TempViewAttDataDetailed.ToList();
            }
            else
                _TempViewAttDataDetailed = _ViewAttDataDetailed.ToList();
            _TempViewAttDataDetailed.Clear();


            return _ViewAttDataDetailed;
        }

        internal static List<Models.ViewAttData> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<Models.ViewAttData> _TempViewAttDataDetailed2, List<Models.ViewAttData> _ViewAttDataDetailed2)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttDataDetailed2.AddRange(_ViewAttDataDetailed2.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttDataDetailed2 = _TempViewAttDataDetailed2.ToList();
            }
            else
                _TempViewAttDataDetailed2 = _ViewAttDataDetailed2.ToList();
            _TempViewAttDataDetailed2.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttDataDetailed2.AddRange(_ViewAttDataDetailed2.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttDataDetailed2 = _TempViewAttDataDetailed2.ToList();
            }
            else
                _TempViewAttDataDetailed2 = _ViewAttDataDetailed2.ToList();


            _TempViewAttDataDetailed2.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttDataDetailed2.AddRange(_ViewAttDataDetailed2.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttDataDetailed2 = _TempViewAttDataDetailed2.ToList();
            }
            else
                _TempViewAttDataDetailed2 = _ViewAttDataDetailed2.ToList();
            _TempViewAttDataDetailed2.Clear();
            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttDataDetailed2.AddRange(_ViewAttDataDetailed2.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttDataDetailed2 = _TempViewAttDataDetailed2.ToList();
            }
            else
                _TempViewAttDataDetailed2 = _ViewAttDataDetailed2.ToList();
            _TempViewAttDataDetailed2.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttDataDetailed2.AddRange(_ViewAttDataDetailed2.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttDataDetailed2 = _TempViewAttDataDetailed2.ToList();
            }
            else
                _TempViewAttDataDetailed2 = _ViewAttDataDetailed2.ToList();
            _TempViewAttDataDetailed2.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttDataDetailed2.AddRange(_ViewAttDataDetailed2.Where(aa => aa.EmpID == _empID).ToList());
                }
                _ViewAttDataDetailed2 = _TempViewAttDataDetailed2.ToList();
            }
            else
                _TempViewAttDataDetailed2 = _ViewAttDataDetailed2.ToList();
            _TempViewAttDataDetailed2.Clear();


            return _ViewAttDataDetailed2;
        }

        internal static List<Models.ViewAttMonthlySummary> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<Models.ViewAttMonthlySummary> _TempViewAttMonthlySummary, List<Models.ViewAttMonthlySummary> _ViewAttMonthlySummary)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();


            _TempViewAttMonthlySummary.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();
            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();


            return _ViewAttMonthlySummary;
        }

        internal static List<Models.Att_DailyAttendance> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<Models.Att_DailyAttendance> _TempViewAttDataDetailed4, List<Models.Att_DailyAttendance> _ViewAttDataDetailed4)
        {            
            return _ViewAttDataDetailed4;
        }

        internal static List<Models.ViewDailyOTEntry> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<Models.ViewDailyOTEntry> _TempViewAttMonthlySummary, List<Models.ViewDailyOTEntry> _ViewAttMonthlySummary)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();


            _TempViewAttMonthlySummary.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();
            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();


            return _ViewAttMonthlySummary;
        }



        public static List<VMOTSummary> GetOTSummaryData(List<Models.PR_PayrollPeriod> periods, List<Models.ViewDailyOTEntry> otDatas, string _dateFrom, string _dateTo, List<Models.EmpView> emps)
        {
            List<VMOTSummary> list = new List<VMOTSummary>();
            foreach (var item in emps)
            {
                if (otDatas.Where(aa => aa.EmployeeID == item.EmployeeID).Count()>0)
                {
                    List<int?> pids = otDatas.Where(aa => aa.EmployeeID == item.EmployeeID).Select(aa => aa.PeriodID).Distinct().ToList();
                    foreach (var pid in pids)
                    {
                        Models.PR_PayrollPeriod pr = periods.First(aa => aa.PID == pid);
                        List<Models.ViewDailyOTEntry> tempData = otDatas.Where(aa => aa.EmployeeID == item.EmployeeID && aa.PeriodID == pid).ToList();
                        if (tempData.Count > 0)
                        {
                            VMOTSummary obj = new VMOTSummary();
                            obj.EmployeeID = item.EmployeeID;
                            obj.EmpNo = item.EmpNo;
                            obj.Name = item.FullName;
                            obj.StartDate = Convert.ToDateTime(_dateFrom);
                            obj.EndDate = Convert.ToDateTime(_dateTo);
                            obj.LocID = item.LocID;
                            obj.LocationName = item.LocationName;
                            obj.SectionName = item.SectionName;
                            obj.SecID = item.SecID;
                            obj.DesgID = item.DesgID;
                            obj.DesignationName = item.DesignationName;
                            obj.TypID = item.TypID;
                            obj.TypeName = item.FullName;
                            obj.GradeID = item.GrdID;
                            obj.GradeName = item.GradeName;
                            obj.DeptID = item.DeptID;
                            obj.DepartmentName = item.DepartmentName;
                            obj.SystemOTMinutes = (int)tempData.Sum(aa => aa.ActualOTMin);
                            obj.ApprovedOTMinutes = (int)tempData.Sum(aa => aa.ApprovedOTMin);
                            obj.OTAmount = (int)tempData.Sum(aa => aa.OTAmount);
                            obj.ApprovedBy = tempData.First().UAFullName;
                            obj.PeriodID = pr.PID;
                            obj.PeriodName = pr.PName;
                            list.Add(obj);
                        }
                    } 
                }
            }
            return list;
        }

        internal static List<Models.ViewVisitEmp> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<Models.ViewVisitEmp> _TempViewVisitorEmp, List<Models.ViewVisitEmp> _ViewVisitorEmp)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
            }
            else
                _TempViewVisitorEmp = _ViewVisitorEmp.ToList();
            _TempViewVisitorEmp.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
            }
            else
                _TempViewVisitorEmp = _ViewVisitorEmp.ToList();


            _TempViewVisitorEmp.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewVisitorEmp = _ViewVisitorEmp.ToList();
            }
            else
                _TempViewVisitorEmp = _ViewVisitorEmp.ToList();

            _TempViewVisitorEmp.Clear();
            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
            }
            else
                _TempViewVisitorEmp = _ViewVisitorEmp.ToList();
            _TempViewVisitorEmp.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
            }
            else
                _TempViewVisitorEmp = _ViewVisitorEmp.ToList();
            _TempViewVisitorEmp.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
            }
            else
                _TempViewVisitorEmp = _ViewVisitorEmp.ToList();
            _TempViewVisitorEmp.Clear();


            return _ViewVisitorEmp;
        }

        //internal static List<ViewEmpQualification> ReportsFilterImplementation(FiltersModel fm, List<ViewEmpQualification> _TempViewVisitorEmp, List<ViewEmpQualification> _ViewVisitorEmp)
        //{
        //    //for sections
        //    if (fm.QualificationFilter.Count > 0)
        //    {
        //        foreach (var sec in fm.QualificationFilter)
        //        {
        //            short _secID = Convert.ToInt16(sec.ID);
        //            _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.QualificationID == _secID).ToList());
        //        }
        //        _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
        //    }
        //    else
        //        _TempViewVisitorEmp = _ViewVisitorEmp.ToList();
        //    _TempViewVisitorEmp.Clear();

        //    //Employee
        //    if (fm.EmployeeFilter.Count > 0)
        //    {
        //        foreach (var emp in fm.EmployeeFilter)
        //        {
        //            int _empID = Convert.ToInt32(emp.ID);
        //            _TempViewVisitorEmp.AddRange(_ViewVisitorEmp.Where(aa => aa.EmployeeID == _empID).ToList());
        //        }
        //        _ViewVisitorEmp = _TempViewVisitorEmp.ToList();
        //    }
        //    else
        //        _TempViewVisitorEmp = _ViewVisitorEmp.ToList();
        //    _TempViewVisitorEmp.Clear();


        //    return _ViewVisitorEmp;
        //}

        internal static List<EmpView> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<EmpView> _TempViewAttMonthlySummary, List<EmpView> _ViewAttMonthlySummary)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();


            _TempViewAttMonthlySummary.Clear();

            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for designation
            if (fm.CMDesignationFilter.Count > 0)
            {
                foreach (var type in fm.CMDesignationFilter)
                {
                    //short _desigID = Convert.ToInt16(type.ID);
                    //_TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DesgID == _desigID).ToList());
                    string _desigName = type.FilterName.ToUpper();
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.OCommonName.ToUpper() == _desigName).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //for domicile
            if (fm.DomicileFilter.Count > 0)
            {
                foreach (var type in fm.DomicileFilter)
                {
                    string search = type.ID;
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DomicileProvince == search).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();


            return _ViewAttMonthlySummary;
        }

        internal static List<ViewReportQualification> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<ViewReportQualification> _TempViewAttMonthlySummary, List<ViewReportQualification> _ViewAttMonthlySummary)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();


            _TempViewAttMonthlySummary.Clear();

            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for Designation
            if (fm.CMDesignationFilter.Count > 0)
            {
                foreach (var type in fm.CMDesignationFilter)
                {
                    string _desigName = type.FilterName.ToUpper();
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DesignationName.ToUpper() == _desigName).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();


            //for Domicile
            //if (fm.DomicileFilter.Count > 0)
            //{
            //    foreach (var type in fm.DomicileFilter)
            //    {
            //        string search = type.ID;
            //        _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DomicileProvince == search).ToList());
            //    }
            //    _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            //}
            //else
            //    _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();


            return _ViewAttMonthlySummary;
        }

        internal static List<ViewEmpHistory> ReportsFilterImplementation(WMSLibrary.FiltersModel fm, List<ViewEmpHistory> _TempViewAttMonthlySummary, List<ViewEmpHistory> _ViewAttMonthlySummary)
        {
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();


            _TempViewAttMonthlySummary.Clear();

            //for department
            if (fm.DepartmentFilter.Count > 0)
            {
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for Designation
            if (fm.CMDesignationFilter.Count > 0)
            {
                foreach (var type in fm.CMDesignationFilter)
                {
                    string _desigName = type.FilterName.ToUpper();
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DesignationName.ToUpper() == _desigName).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //for sections
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var sec in fm.SectionFilter)
                {
                    short _secID = Convert.ToInt16(sec.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.SecID == _secID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();


            //for Domicile
            //if (fm.DomicileFilter.Count > 0)
            //{
            //    foreach (var type in fm.DomicileFilter)
            //    {
            //        string search = type.ID;
            //        _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.DomicileProvince == search).ToList());
            //    }
            //    _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            //}
            //else
            //    _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();

            _TempViewAttMonthlySummary.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewAttMonthlySummary.AddRange(_ViewAttMonthlySummary.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewAttMonthlySummary = _TempViewAttMonthlySummary.ToList();
            }
            else
                _TempViewAttMonthlySummary = _ViewAttMonthlySummary.ToList();
            _TempViewAttMonthlySummary.Clear();


            return _ViewAttMonthlySummary;
        }

    }




    public class VMMonthlyAttData
    {
        public int EmpID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float WorkingDays { get; set; }
        public float PresentDays { get; set; }
        public float AbsentDays { get; set; }
        public float Leaves { get; set; }
        public float HalfLeaves { get; set; }
        public float RestDays { get; set; }
        public float RestPresentDays { get; set; }
        public float GZDays { get; set; }
        public float GZPresentDays { get; set; }
        public float LIDays { get; set; }
        public float EODays { get; set; }
        public float LODays { get; set; }
        public float EIDays { get; set; }
        public string ShiftHour { get; set; }
        public string WorkedHours { get; set; }
        public string ActualOTHours { get; set; }
        public string ShortHours { get; set; }


    }
    public class VMOTSummary
    {

        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<short> LocID { get; set; }
        public string LocationName { get; set; }
        public string SectionName { get; set; }
        public Nullable<short> SecID { get; set; }
        public Nullable<short> DesgID { get; set; }
        public string DesignationName { get; set; }
        public Nullable<short> TypID { get; set; }
        public string TypeName { get; set; }
        public Nullable<short> GradeID { get; set; }
        public string GradeName { get; set; }
        public Nullable<short> DeptID { get; set; }
        public string DepartmentName { get; set; }
        public int SystemOTMinutes { get; set; }
        public int ApprovedOTMinutes { get; set; }
        public int OTAmount { get; set; }
        public string ApprovedBy { get; set; }
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
    }
}