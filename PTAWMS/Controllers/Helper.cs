using PTAWMS.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PTAWMS.Controllers
{
    public class Helper
    {
        public DataTable GetPresentCount(DateTime dts, DateTime dte)
        {
            QueryBuilder qb = new QueryBuilder();
            return qb.GetValuesfromDB("SELECT DesignationName, SectionName, EmpNo, FullName, SUM(PDays) AS PDays FROM dbo.ViewAttData WHERE (AttDate >= '" + dts.ToString("yyyy-MM-dd") + "' and AttDate<='" + dte.ToString("yyyy-MM-dd") + "' and PDays>0) GROUP BY DesignationName, SectionName, EmpNo, FullName ORDER BY PDays DESC");
        }
        public DataTable GetLateInCount(DateTime dts, DateTime dte)
        {
            QueryBuilder qb = new QueryBuilder();
            return qb.GetValuesfromDB("SELECT DesignationName, SectionName, EmpNo, FullName, SUM(LateIn) AS LateInMin, Count(AttDate) AS LateInDay FROM dbo.ViewAttData WHERE (AttDate >= '" + dts.ToString("yyyy-MM-dd") + "' and AttDate<='" + dte.ToString("yyyy-MM-dd") + "' and LateIn>0) GROUP BY DesignationName, SectionName, EmpNo, FullName ORDER BY LateInDay DESC");
        }

        internal DataTable GetMultipleInOutCount(DateTime dts, DateTime dte)
        {
            QueryBuilder qb = new QueryBuilder();
            return qb.GetValuesfromDB("SELECT DesignationName, SectionName, EmpNo, FullName,  Count(AttDate) AS Days FROM dbo.ViewAttDataDetail WHERE (AttDate >= '" + dts.ToString("yyyy-MM-dd") + "' and AttDate<='" + dte.ToString("yyyy-MM-dd") + "' and Tin1 is not null) GROUP BY DesignationName, SectionName, EmpNo, FullName ORDER BY Days DESC");
        }

        internal DataTable GetAbsentCount(DateTime dts, DateTime dte)
        {
            QueryBuilder qb = new QueryBuilder();
            return qb.GetValuesfromDB("SELECT DesignationName, SectionName, EmpNo, FullName,  Count(AttDate) AS Days FROM dbo.ViewAttDataDetail WHERE (AttDate >= '" + dts.ToString("yyyy-MM-dd") + "' and AttDate<='" + dte.ToString("yyyy-MM-dd") + "' and AbDays>0) GROUP BY DesignationName, SectionName, EmpNo, FullName ORDER BY Days DESC");
        }

        internal DataTable GetLateOutCount(DateTime dts, DateTime dte)
        {
            QueryBuilder qb = new QueryBuilder();
            return qb.GetValuesfromDB("SELECT DesignationName, SectionName, EmpNo, FullName, SUM(LateOut) AS LateOutMin, Count(AttDate) AS LateOutDay FROM dbo.ViewAttData WHERE (AttDate >= '" + dts.ToString("yyyy-MM-dd") + "' and AttDate<='" + dte.ToString("yyyy-MM-dd") + "' and LateOut>0) GROUP BY DesignationName, SectionName, EmpNo, FullName ORDER BY LateOutDay DESC");
        }
    }
}