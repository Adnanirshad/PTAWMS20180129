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
    
    public partial class Att_JobCardDetail
    {
        public string EmpDate { get; set; }
        public long JobDataID { get; set; }
        public int EmpID { get; set; }
        public Nullable<System.DateTime> Dated { get; set; }
        public Nullable<short> WrkCardID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> TimeIn { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public Nullable<short> WorkMin { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<short> OtherValue { get; set; }
        public Nullable<int> JCAppID { get; set; }
    
        public virtual HR_Employee HR_Employee { get; set; }
    }
}