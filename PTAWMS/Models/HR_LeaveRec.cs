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
    
    public partial class HR_LeaveRec
    {
        public int LeaveRecID { get; set; }
        public Nullable<int> RECID { get; set; }
        public Nullable<int> LEAVEID { get; set; }
        public string REFERNAME { get; set; }
        public string REFEREMAIL { get; set; }
        public string DECISION { get; set; }
        public string COMMENTS { get; set; }
        public string ONDESKSINCE { get; set; }
        public string ACTIONDATE { get; set; }
    }
}