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
    
    public partial class AssetAllocationReport_v
    {
        public int AllocatedToID { get; set; }
        public string AllocatedToName { get; set; }
        public string EmpNo { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public string Station { get; set; }
        public string AllocationTypeName { get; set; }
        public string BCNo { get; set; }
        public string SrOrRegNo { get; set; }
        public System.DateTime DoP { get; set; }
        public Nullable<int> BPS { get; set; }
        public Nullable<decimal> Expr1 { get; set; }
        public int CLSID { get; set; }
        public int CatID { get; set; }
        public int OUID { get; set; }
        public int SOUID { get; set; }
        public decimal UnitPrice { get; set; }
        public string TypeName { get; set; }
        public string Status { get; set; }
        public string OCName { get; set; }
        public System.DateTime AllocationDate { get; set; }
        public int AssetID { get; set; }
    }
}
