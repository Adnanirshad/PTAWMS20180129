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
    
    public partial class ViewPollData
    {
        public long PollID { get; set; }
        public int EmpID { get; set; }
        public string EmpDate { get; set; }
        public Nullable<int> FpID { get; set; }
        public string CardNo { get; set; }
        public System.DateTime EntDate { get; set; }
        public System.DateTime EntTime { get; set; }
        public Nullable<byte> RdrDuty { get; set; }
        public Nullable<bool> Process { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public short RdrID { get; set; }
        public string RdrName { get; set; }
        public byte ReaderDutyCode { get; set; }
        public string IpAdd { get; set; }
        public short IpPort { get; set; }
        public byte RdrTypeID { get; set; }
        public bool Status { get; set; }
        public Nullable<short> LocID { get; set; }
        public Nullable<bool> ClearRecords { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string DesignationName { get; set; }
        public string TypeName { get; set; }
        public string ShiftName { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string EmpNo { get; set; }
    }
}
