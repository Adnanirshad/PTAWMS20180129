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
    
    public partial class Att_Leave
    {
        public int LVID { get; set; }
        public Nullable<int> HREmpID { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public string LeaveTypeID { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public Nullable<int> OLvID { get; set; }
        public Nullable<double> Days { get; set; }
    }
}
