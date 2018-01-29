using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.App_Start
{
    public static class AuditManager
    {
        public static string GetIPAddress()
        {

            try
            {
                return HttpContext.Current.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public enum AuditOperation
        {
            Add = 1,
            Edit=2,
            Delete=3,
            Submit =4,
            Approved=5,
            Reject=6,
            Comment = 7
        }
        public enum AuditForm
        {
            User=1,
            UserRole=2,
            Section=3,
            Department=4,
            Designation=5,
            Type=6,
            Grade=7,
            Location=8,
            Shift=9,
            Holiday=10,
            Device=11,
            Att_Setting=12,
            OT_Policy=13,
            OT_Period=14,
            OT_Budget=15,
            OT_Credit=16,
            OT_Debit=17,
            Att_Process=18,
            Edit_Attendance=19,
            Job_Cards=20,
            Visitor_Entry=21,
            FinYear =22,
            Pre_Job_History = 41,
            Appreciation = 42,
            Warning = 43,
            Training = 44,
            Leave = 45
        }
        public static void SaveAuditLog(int _userID, short _form, short _operation, DateTime _date,int PID)
        {
            try
            {
                using (var ctx = new HRMEntities())
                {
                    Audit_Log auditEntry = new Audit_Log();
                    auditEntry.AuditUserID = _userID;
                    auditEntry.FormID = _form;
                    auditEntry.OperationID = _operation;
                    auditEntry.AuditDateTime = _date;
                    auditEntry.IPAddress = GetIPAddress();
                    auditEntry.PID = PID;
                    ctx.Audit_Log.Add(auditEntry);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}