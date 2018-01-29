using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMSLibrary;

namespace PTAWMS.Reports.BusinessLogic
{
    public class HRRptFilter
    {
        public static List<EmpView> ReportsFilterImplementation(FiltersModel fm, List<EmpView> _TempViewList, List<EmpView> _ViewList)
        {
            //for location
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocID == _locID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shifts
            if (fm.ShiftFilter.Count > 0)
            {
                foreach (var shift in fm.ShiftFilter)
                {
                    short _shiftID = Convert.ToInt16(shift.ID);
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShftID == _shiftID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();


            _TempViewList.Clear();

            //for type
            if (fm.TypeFilter.Count > 0)
            {
                foreach (var type in fm.TypeFilter)
                {
                    short _typeID = Convert.ToInt16(type.ID);
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.TypID == _typeID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();


            ////for division
            //if (fm.DivisionFilter.Count > 0)
            //{
            //    foreach (var div in fm.DivisionFilter)
            //    {
            //        short _divID = Convert.ToInt16(div.ID);
            //        _TempViewList.AddRange(_ViewList.Where(aa => aa.D == _divID).ToList());
            //    }
            //    _ViewList = _TempViewList.ToList();
            //}
            //else
            //    _TempViewList = _ViewList.ToList();
            //_TempViewList.Clear();

            //for department
            //if (fm.DepartmentFilter.Count > 0)
            //{
            //    foreach (var dept in fm.DepartmentFilter)
            //    {
            //        short _deptID = Convert.ToInt16(dept.ID);
            //        _TempViewList.AddRange(_ViewList.Where(aa => aa.DeptID == _deptID).ToList());
            //    }
            //    _ViewList = _TempViewList.ToList();
            //}
            //else
            //    _TempViewList = _ViewList.ToList();
            //_TempViewList.Clear();

            //Employee
            if (fm.EmployeeFilter.Count > 0)
            {
                foreach (var emp in fm.EmployeeFilter)
                {
                    int _empID = Convert.ToInt32(emp.ID);
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmployeeID == _empID).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();


            return _ViewList;
        }
    }
}