using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public class ModelChooseEmp
    {
        //public List<HR_Division> DivEmployees { get; set; }
        public List<HR_Location> LocEmployees { get; set; }
        public List<HR_EmpType> TypeEmployees { get; set; }
        public List<ViewHRSection> SecEmployees { get; set; }
        public List<HR_Grade> GradeEmployees { get; set; }
        public int PayrollPeriodID { get; set; }
        public string PayrollPeriodName { get; set; }
    }
    public class ModelChooseEmpSelect
    {
        // public List<ViewAttMonthlySummary> Emps { get; set; }
        public int PayrollPeriodID { get; set; }
    }
}