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
    
    public partial class Att_MonthlyOTEntry
    {
        public int PMOTID { get; set; }
        public Nullable<int> PayrollPeriodID { get; set; }
        public string StatusID { get; set; }
        public Nullable<double> NormalOT { get; set; }
        public Nullable<double> NormalOTAmount { get; set; }
        public Nullable<double> RestOT { get; set; }
        public Nullable<double> RestOTAmount { get; set; }
        public Nullable<double> GZOT { get; set; }
        public Nullable<double> GZOTAmount { get; set; }
        public Nullable<double> TotalOT { get; set; }
        public Nullable<int> AMOTID { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<int> ForwardToID { get; set; }
    
        public virtual PR_PayrollPeriod PR_PayrollPeriod { get; set; }
    }
}
