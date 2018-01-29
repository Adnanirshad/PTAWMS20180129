using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public static class MyHelper
    {
        public static void SaveAuditLog(int _userID, short _form, short _operation, DateTime _date)
        {
            //using (var ctx = new HRM_IKAN())
            //{
            //    AuditLog auditEntry = new AuditLog();
            //    auditEntry.AuditUserID = _userID;
            //    auditEntry.FormID = _form;
            //    auditEntry.OperationID = _operation;
            //    auditEntry.AuditDateTime = _date;
            //    ctx.AuditLogs.Add(auditEntry);
            //    ctx.SaveChanges();
            //}
        }
        public static TimeSpan ConvertTime(string p)
        {
            try
            {
                string hour = "";
                string min = "";
                int count = 0;
                int chunkSize = 2;
                int stringLength = 4;

                for (int i = 0; i < stringLength; i += chunkSize)
                {
                    count++;
                    if (count == 1)
                    {
                        hour = p.Substring(i, chunkSize);
                    }
                    if (count == 2)
                    {
                        min = p.Substring(i, chunkSize);
                    }
                    if (i + chunkSize > stringLength)
                    {
                        chunkSize = stringLength - i;
                    }
                }
                TimeSpan _currentTime = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), 00);
                return _currentTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay;
            }
        }

        public enum ReportName
        {
            Daily = 1,
            Leave,
            Monthly,
            Audit,
            ManualAtt,
            Employee,
            Detail,
            Summary,
            Grpah
        }
        //public static bool UserHasValuesInSession(FiltersModel fm)
        //{
        //    bool check = false;
        //    if (fm.CompanyFilter.Count > 0)
        //        check = true;
        //    if (fm.LocationFilter.Count > 0)
        //        check = true;
        //    //if (fm.DivisionFilter.Count > 0)
        //    //    check = true;
        //    if (fm.ShiftFilter.Count > 0)
        //        check = true;
        //    if (fm.DepartmentFilter.Count > 0)
        //        check = true;
        //    if (fm.SectionFilter.Count > 0)
        //        check = true;
        //    if (fm.TypeFilter.Count > 0)
        //        check = true;
        //    //if (fm.CrewFilter.Count > 0)
        //    //    check = true;
        //    if (fm.EmployeeFilter.Count > 0)
        //        check = true;
        //    return check;
        //}
    }

}