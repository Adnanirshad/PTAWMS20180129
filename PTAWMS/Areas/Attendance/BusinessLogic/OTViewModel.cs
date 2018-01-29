using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.Attendance.BusinessLogic
{
    public class ModelComboBox 
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    // For Supervisor -- Department
    public class ModelRDeptPending
    {
        public int PayrollPeriodID { get; set; }
        public int LoggedUserID { get; set; }
        public List<ModelRDeptPendingList> List { get; set; }
        public int Count { get; set; }
        public int DivRemainingBudget { get; set; }
        public int TotalOTAmount { get; set; }
        public List<string> Message { get; set; }
    }
    public class ModelRDeptPendingList
    {
        public short DeptID { get; set; }
        public string DeptName { get; set; }
        public string NoOfEmps { get; set; }
        public string SystemOvertime { get; set; }
        public string ClaimOvertime { get; set; }
        public int OTAmount { get; set; }
        public int PendingAtSupervisor { get; set; }
        public int PendingAtRecommender { get; set; }
        public int PendingAtApprover { get; set; }
        public int Approved { get; set; }
    }
    // end

    // For Supervisor -- Employee wise
    public class ModelSOTEmpList
    {
        public int PayrollPeriodID { get; set; }
        public int RecommendID { get; set; }
        public string RecommenderName { get; set; }
        public int DivRemainingBudget { get; set; }
        public int TotalOTAmount { get; set; }
        public string SystemOT { get; set; }
        public string ClaimedOT { get; set; }
        public string TotalEmps { get; set; }
        public string DecisionID { get; set; }
        public int DeptID { get; set; }
        public List<ModelSOTPDList> List { get; set; }
        public int Count { get; set; }
        public List<string> Message { get; set; }
        public bool IsLate { get; set; }
        public bool Certified { get; set; }
        public string Justification { get; set; }
    }
    public class ModelSOTPDList
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public string Section { get; set; }
        public int DailyOTRequestCount { get; set; }
        public string SystemOTHours { get; set; }
        public string ClaimedOTHours { get; set; }
        public int ClaimedOTMins { get; set; }
        public int SystemOTMins { get; set; }
        public int OTAmount { get; set; }
        public string StatusRemarks { get; set; }
        public string StatusForward { get; set; }
        public int PendingAtSupervisor { get; set; }
        public int PendingAtRecommender { get; set; }
        public int PendingAtApprover { get; set; }
        public int Approved { get; set; }
        public int PeriodID { get; set; }
        public int Processed{ get; set; }
        public int OTProcessPeriodID { get; set; }
    }
    //End

    // For Supervisor -- Employee wise detail
    public class ModelSOTPEmpDetail
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public int DeptID { get; set; }
        public string PeriodName { get; set; }
        public int PeriodID { get; set; }
        public string OTPolicy { get; set; }
        public string OTPolicyDays { get; set; }
        public int RecommendID { get; set; }
        public string RecommendName { get; set; }
        public string DecisionID { get; set; }
        public List<ModelDOTEntries> List { get; set; }
        public int Count { get; set; }
        public int TotalAmount { get; set; }
        public string SystemOT { get; set; }
        public string ClaimedOT { get; set; }
        public int TotalDays { get; set; }
        public int DivRemainingBudget { get; set; }
        public List<string> Message { get; set; }
        public bool IsLate { get; set; }
        public bool Certified { get; set; }
        public string Justification { get; set; }
    }

    public class ModelDOTEntries
    {
        public string EmpDate { get; set; }
        public string Date { get; set; }
        public string TimeIN { get; set; }
        public string TimeOut { get; set; }
        public string WorkHours { get; set; }
        public string SystemOTHours { get; set; }
        public string ClaimedOTHours { get; set; }
        public int ClaimedOTMins { get; set; }
        public int SystemOTMins { get; set; }
        public int OTAmount { get; set; }
        public string StatusRemarks { get; set; }
        public string StatusForward { get; set; }
        public string StatusID { get; set; }
    }
    //end
}