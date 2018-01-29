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
    
    public partial class EmpView
    {
        public Nullable<short> LocID { get; set; }
        public string LocationName { get; set; }
        public string SectionName { get; set; }
        public Nullable<short> SecID { get; set; }
        public Nullable<short> DesgID { get; set; }
        public string DesignationName { get; set; }
        public Nullable<short> TypID { get; set; }
        public string TypeName { get; set; }
        public Nullable<short> ShftID { get; set; }
        public string ShiftName { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<byte> DayOff1 { get; set; }
        public Nullable<short> GrdID { get; set; }
        public string GradeName { get; set; }
        public int EmployeeID { get; set; }
        public string EmpNo { get; set; }
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
        public string Country { get; set; }
        public string LandLine { get; set; }
        public string MobileNo { get; set; }
        public string EmergencyContactNo { get; set; }
        public string EmailID { get; set; }
        public string OfficialContactNo { get; set; }
        public string OfficialEmailID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LeavingDate { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string CardNo { get; set; }
        public string PinCode { get; set; }
        public Nullable<System.DateTime> ValidDate { get; set; }
        public Nullable<bool> FaceTemp { get; set; }
        public Nullable<int> FPTemp { get; set; }
        public Nullable<bool> ProcessAttendance { get; set; }
        public int FPID { get; set; }
        public Nullable<int> ReportingToID { get; set; }
        public Nullable<short> DepartmentID { get; set; }
        public Nullable<short> DeptID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<short> OTPolicyID { get; set; }
        public string OTPolicyName { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<bool> CalculateNOT { get; set; }
        public Nullable<bool> CalculateGZOT { get; set; }
        public Nullable<bool> CalculateRestOT { get; set; }
        public Nullable<double> PerDayOTStartLimitHour { get; set; }
        public Nullable<double> PerDayOTEndLimitHour { get; set; }
        public Nullable<double> PerDayROTStartLimitHour { get; set; }
        public Nullable<double> PerDayROTEndLimitHour { get; set; }
        public Nullable<double> PerDayGOTStartLimitHour { get; set; }
        public Nullable<double> PerDayGOTEndLimitHour { get; set; }
        public Nullable<short> MinMinutesForOneHour { get; set; }
        public Nullable<byte> DaysInMonth { get; set; }
        public Nullable<byte> DaysInWeek { get; set; }
        public Nullable<bool> AddEIinOT { get; set; }
        public Nullable<short> SectionID { get; set; }
        public Nullable<double> NormalOTAmount { get; set; }
        public Nullable<double> RestOTAmount { get; set; }
        public Nullable<double> GZOTAmount { get; set; }
        public string OCommonName { get; set; }
        public string Address2 { get; set; }
        public string DomicileCity { get; set; }
        public string DomicileProvince { get; set; }
        public string GrdName { get; set; }
        public string ScaleName { get; set; }
        public string BankAccount { get; set; }
        public string OTYPENAME { get; set; }
    }
}
