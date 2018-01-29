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
    
    public partial class HR_Employee
    {
        public HR_Employee()
        {
            this.Att_DailyAttendance = new HashSet<Att_DailyAttendance>();
            this.Att_DeviceData = new HashSet<Att_DeviceData>();
            this.Att_EmpFace = new HashSet<Att_EmpFace>();
            this.Att_EmpFP = new HashSet<Att_EmpFP>();
            this.Att_JobCardDetail = new HashSet<Att_JobCardDetail>();
            this.Att_MonthData = new HashSet<Att_MonthData>();
            this.Att_OTDailyEntry = new HashSet<Att_OTDailyEntry>();
            this.Att_OutPass = new HashSet<Att_OutPass>();
            this.Att_ShiftChngedEmp = new HashSet<Att_ShiftChngedEmp>();
            this.Users = new HashSet<User>();
            this.VMS_VisitInfo = new HashSet<VMS_VisitInfo>();
        }
    
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
        public int FPID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string FathersName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string CNICNo { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<int> EmpImage { get; set; }
        public string Address { get; set; }
        public string LandLine { get; set; }
        public string MobileNo { get; set; }
        public string EmergencyContactNo { get; set; }
        public string PersonalEmailID { get; set; }
        public string OfficialContactNo { get; set; }
        public string OfficialEmailID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LeavingDate { get; set; }
        public Nullable<short> DesignationID { get; set; }
        public Nullable<short> LocationID { get; set; }
        public Nullable<short> SectionID { get; set; }
        public Nullable<short> DeptID { get; set; }
        public Nullable<short> GradeID { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<int> ReportingToID { get; set; }
        public Nullable<short> EmpTypeID { get; set; }
        public Nullable<short> ShiftID { get; set; }
        public string CardNo { get; set; }
        public string PinCode { get; set; }
        public Nullable<System.DateTime> ValidDate { get; set; }
        public Nullable<bool> FaceTemp { get; set; }
        public Nullable<int> FPTemp { get; set; }
        public Nullable<bool> ProcessAttendance { get; set; }
        public Nullable<short> OTPolicyID { get; set; }
        public string Address2 { get; set; }
        public Nullable<System.DateTime> RetirementDate { get; set; }
        public Nullable<System.DateTime> DateOfCommision { get; set; }
        public Nullable<System.DateTime> StationJoinDate { get; set; }
        public Nullable<System.DateTime> GovtSrvsDate { get; set; }
        public Nullable<System.DateTime> OrgJoinDate { get; set; }
        public string GrdName { get; set; }
        public string ScaleName { get; set; }
        public string EmailID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string DomicileCity { get; set; }
        public string DomicileProvince { get; set; }
        public string ExtensionNo { get; set; }
        public string RoomNo { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public string BankAccount { get; set; }
    
        public virtual ICollection<Att_DailyAttendance> Att_DailyAttendance { get; set; }
        public virtual ICollection<Att_DeviceData> Att_DeviceData { get; set; }
        public virtual ICollection<Att_EmpFace> Att_EmpFace { get; set; }
        public virtual ICollection<Att_EmpFP> Att_EmpFP { get; set; }
        public virtual ICollection<Att_JobCardDetail> Att_JobCardDetail { get; set; }
        public virtual ICollection<Att_MonthData> Att_MonthData { get; set; }
        public virtual ICollection<Att_OTDailyEntry> Att_OTDailyEntry { get; set; }
        public virtual Att_OTPolicy Att_OTPolicy { get; set; }
        public virtual ICollection<Att_OutPass> Att_OutPass { get; set; }
        public virtual Att_Shift Att_Shift { get; set; }
        public virtual ICollection<Att_ShiftChngedEmp> Att_ShiftChngedEmp { get; set; }
        public virtual HR_Department HR_Department { get; set; }
        public virtual HR_Designation HR_Designation { get; set; }
        public virtual HR_Grade HR_Grade { get; set; }
        public virtual HR_Location HR_Location { get; set; }
        public virtual HR_Section HR_Section { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<VMS_VisitInfo> VMS_VisitInfo { get; set; }
    }
}
