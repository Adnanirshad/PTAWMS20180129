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
    
    public partial class HR_LeaveReq
    {
        public int LEAVEID { get; set; }
        public Nullable<int> EMPLOYEEID { get; set; }
        public Nullable<System.DateTime> APPDATE { get; set; }
        public Nullable<System.DateTime> FROMDATE { get; set; }
        public Nullable<System.DateTime> TODATE { get; set; }
        public string LEAVETYPE { get; set; }
        public Nullable<double> DUELEAVE { get; set; }
        public string REASON { get; set; }
        public string STATIONLEAVE { get; set; }
        public string ADDRESS { get; set; }
        public string TELNO { get; set; }
        public string SUBSTITUTENAME { get; set; }
        public string SUBSTITUTEEMAIL { get; set; }
        public string STATUS { get; set; }
    }
}
