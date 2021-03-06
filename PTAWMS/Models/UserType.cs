//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PTAWMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserType
    {
        public string PUTID { get; set; }
        public string UTypeName { get; set; }
        public Nullable<bool> CanEdit { get; set; }
        public Nullable<bool> CanDelete { get; set; }
        public Nullable<bool> CanView { get; set; }
        public Nullable<bool> CanAdd { get; set; }
        public Nullable<bool> MAttProcess { get; set; }
        public Nullable<bool> MOption { get; set; }
        public Nullable<bool> MAttDevice { get; set; }
        public Nullable<bool> MAttDeviceUtility { get; set; }
        public Nullable<bool> MAttEditAttendance { get; set; }
        public Nullable<bool> MAttJobCard { get; set; }
        public Nullable<bool> MAttShift { get; set; }
        public Nullable<bool> MAttPolicy { get; set; }
        public Nullable<bool> MAttDownloadTime { get; set; }
        public Nullable<bool> MAttHoliday { get; set; }
        public Nullable<bool> MAttOTPolicy { get; set; }
        public Nullable<bool> MAttOTCreate { get; set; }
        public Nullable<bool> MAttOTEdit { get; set; }
        public Nullable<bool> MAttLeaves { get; set; }
        public Nullable<bool> MAttRoster { get; set; }
        public Nullable<bool> MHRCompHierarchy { get; set; }
        public Nullable<bool> MUser { get; set; }
        public Nullable<bool> MGrade { get; set; }
        public Nullable<bool> MHREmpA { get; set; }
        public Nullable<bool> MHREmpE { get; set; }
        public Nullable<bool> MHREmpV { get; set; }
        public Nullable<bool> MHREmpD { get; set; }
        public Nullable<bool> MHREmployee { get; set; }
        public Nullable<bool> MHREmpPersonal { get; set; }
        public Nullable<bool> MHREmpJob { get; set; }
        public Nullable<bool> MHREmpAtt { get; set; }
        public Nullable<bool> HRModule { get; set; }
        public Nullable<bool> HREmpType { get; set; }
        public Nullable<bool> HRLocation { get; set; }
        public Nullable<bool> HRDeptartment { get; set; }
        public Nullable<bool> HRDesignation { get; set; }
        public Nullable<bool> HRSection { get; set; }
        public Nullable<bool> AttendanceModule { get; set; }
        public Nullable<int> OUserID { get; set; }
        public Nullable<bool> VisitorApplication { get; set; }
        public Nullable<bool> VisitorEntry { get; set; }
        public Nullable<bool> VisitorSupervisor { get; set; }
        public Nullable<bool> OTBudget { get; set; }
        public Nullable<bool> OTBudgetCreditDebit { get; set; }
        public Nullable<bool> VisitorModule { get; set; }
    }
}
