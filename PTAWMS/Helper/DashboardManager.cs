using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Helper
{
    public static class DashboardManager
    {
        public static List<DMParentModel> GetDataForDeptVMSummary(List<ViewVisitEmp> visitList, List<short?> secIds)
        {
            List<DMParentModel> dmList = new List<DMParentModel>();
            foreach (var secid in secIds)
            {
                if (visitList.Where(aa => aa.SecID == secid).Count() > 0)
                {
                    DMParentModel dmObj = new DMParentModel();
                    dmObj.ID = (int)secid;
                    dmObj.Name = visitList.Where(aa => aa.SecID == secid).First().SectionName;
                    dmObj.Count = visitList.Where(aa => aa.SecID == secid).Count();
                    dmList.Add(dmObj);
                }
            }
            return dmList.OrderByDescending(aa=>aa.Count).ToList();
        }

        internal static List<DMParentModel> GetDataForEmpVMSummary(List<ViewVisitEmp> visitList, List<int> empIds)
        {
            List<DMParentModel> dmList = new List<DMParentModel>();
            foreach (var empid in empIds)
            {
                if (visitList.Where(aa => aa.EmployeeID == empid).Count() > 0)
                {
                    DMParentModel dmObj = new DMParentModel();
                    dmObj.ID = (int)visitList.Where(aa => aa.EmployeeID == empid).First().EmployeeID;
                    dmObj.Name = visitList.Where(aa => aa.EmployeeID == empid).First().FullName;
                    dmObj.Count = visitList.Where(aa => aa.EmployeeID == empid).Count();
                    dmList.Add(dmObj);
                }
            }
            return dmList.OrderByDescending(aa => aa.Count).ToList();
        }

        internal static DMTMSParentModel GetDataForTMSummary(List<ViewAttData> AttList,DMTMSParentModel dmModel)
        {
            List<DMTMSParentList> dmList = new List<DMTMSParentList>();
            foreach (var date in AttList.Select(aa => aa.AttDate).Distinct())
            {
                if (date.Value.DayOfWeek!=DayOfWeek.Saturday && date.Value.DayOfWeek!=DayOfWeek.Sunday)
                {
                    DMTMSParentList dmObj = new DMTMSParentList();
                    dmObj.Name = date.Value.ToString("dd-MM-yy");
                    dmObj.CountEI = (int)(AttList.Where(aa => aa.AttDate == date && aa.EarlyIn > 0).Count());
                    dmObj.CountEO = (int)(AttList.Where(aa => aa.AttDate == date && aa.EarlyOut > 0).Count());
                    dmObj.CountLI = (int)(AttList.Where(aa => aa.AttDate == date && aa.LateIn > 0).Count());
                    dmObj.CountLO = (int)(AttList.Where(aa => aa.AttDate == date && aa.LateOut > 0).Count());
                    dmList.Add(dmObj); 
                }
            }
            dmModel.ChildList = dmList.ToList();
            dmModel.LateIn = dmList.Sum(aa => aa.CountLI);
            dmModel.LateOut = dmList.Sum(aa => aa.CountLO);
            dmModel.EarlyIn = dmList.Sum(aa => aa.CountEI);
            dmModel.EarlyOut = dmList.Sum(aa => aa.CountEO);
            return dmModel;
        }

        internal static List<ViewAttData> GetAttDataForSpecificEmp(List<ViewAttData> AttList, List<EmpView> emps)
        {
            List<ViewAttData> List = new List<ViewAttData>();
            foreach (var emp in emps.ToList())
            {
                List.AddRange(AttList.Where(aa=>aa.EmpID==emp.EmployeeID).ToList());
            }
            return List;
        }

        internal static List<DMParentModel> GetDataForEmpVMSummary(List<ViewVisitEmp> visitList)
        {
            List<DMParentModel> dmList = new List<DMParentModel>();
            if (visitList.Count > 0)
            {
                foreach (var visitorid in visitList.Select(aa=>aa.VisitorID).Distinct())
                {
                    DMParentModel dmObj = new DMParentModel();
                    dmObj.ID = (int)visitList.Where(aa => aa.VisitorID == visitorid).First().VisitorID;
                    dmObj.Name = visitList.Where(aa => aa.VisitorID == visitorid).First().VName;
                    dmObj.Count = visitList.Where(aa => aa.VisitorID == visitorid).Count();
                    dmList.Add(dmObj);
                }
            }
            return dmList.OrderByDescending(aa => aa.Count).ToList();
        }

        internal static DMTMSParentModel GetDataAttendanceEmployee(List<ViewAttData> AttList, DMTMSParentModel dmModel)
        {
            List<DMTMSParentList> dmList = new List<DMTMSParentList>();
            foreach (var date in AttList.Select(aa => aa.AttDate).Distinct())
            {
                if (date.Value.DayOfWeek != DayOfWeek.Saturday && date.Value.DayOfWeek != DayOfWeek.Sunday)
                {
                    DMTMSParentList dmObj = new DMTMSParentList();
                    dmObj.Name = date.Value.ToString("dd-MM-yy");
                    dmObj.CountEI = (int)(AttList.Where(aa => aa.AttDate == date && aa.EarlyIn > 0).Sum(aa=>aa.EarlyIn));
                    dmObj.CountEO = (int)(AttList.Where(aa => aa.AttDate == date && aa.EarlyOut > 0).Sum(aa => aa.EarlyOut));
                    dmObj.CountLI = (int)(AttList.Where(aa => aa.AttDate == date && aa.LateIn > 0).Sum(aa => aa.LateIn));
                    dmObj.CountLO = (int)(AttList.Where(aa => aa.AttDate == date && aa.LateOut > 0).Sum(aa => aa.LateOut));
                    dmList.Add(dmObj);
                }
            }
            dmModel.ChildList = dmList.ToList();
            dmModel.LateIn = dmList.Sum(aa => aa.CountLI);
            dmModel.LateOut = dmList.Sum(aa => aa.CountLO);
            dmModel.EarlyIn = dmList.Sum(aa => aa.CountEI);
            dmModel.EarlyOut = dmList.Sum(aa => aa.CountEO);
            return dmModel;
        }

        internal static List<ViewVisitEmp> GetVisitDataForSpecificEmp(List<ViewVisitEmp> visitList, List<EmpView> emps)
        {
            List<ViewVisitEmp> List = new List<ViewVisitEmp>();
            foreach (var emp in emps.ToList())
            {
                List.AddRange(visitList.Where(aa => aa.EmpID == emp.EmployeeID).ToList());
            }
            return List;
        }

        internal static DMPieChartParentModel GetDataForPieChartDivision(List<ViewAttData> AttList, DMPieChartParentModel vm, string GraphType)
        {
            List<DMParentModel> dmList = new List<DMParentModel>();
            foreach (var deptID in AttList.Select(aa => aa.DeptID).Distinct().ToList())
            {
                if (AttList.Where(aa => aa.DeptID == deptID).Count() > 0)
                {
                    DMParentModel dmObj = new DMParentModel();
                    dmObj.ID = (int)deptID;
                    dmObj.Name = AttList.Where(aa => aa.DeptID == deptID).First().DepartmentName;
                    dmObj.Count = AttList.Where(aa => aa.DeptID == deptID).Count();
                    dmList.Add(dmObj);
                }
            }
            vm.ChildList = dmList.OrderByDescending(aa => aa.Count).ToList();
            return vm;
        }

        internal static DMPieChartParentModel GetDataForPieChartDept(List<ViewAttData> AttList, DMPieChartParentModel vm, string GraphType)
        {
            List<DMParentModel> dmList = new List<DMParentModel>();
            foreach (var secId in AttList.Select(aa => aa.SecID).Distinct().ToList())
            {
                if (AttList.Where(aa => aa.SecID == secId).Count() > 0)
                {
                    DMParentModel dmObj = new DMParentModel();
                    dmObj.ID = (int)secId;
                    dmObj.Name = AttList.Where(aa => aa.SecID == secId).First().SectionName;
                    dmObj.Count = AttList.Where(aa => aa.SecID == secId).Count();
                    dmList.Add(dmObj);
                }
            }
            vm.ChildList = dmList.OrderByDescending(aa => aa.Count).ToList();
            return vm;
        }
    }
}