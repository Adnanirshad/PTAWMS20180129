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
    
    public partial class AssetAllocation_t
    {
        public int AllocationID { get; set; }
        public int AllocationTypeID { get; set; }
        public int AllocatedToID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public string Remarks { get; set; }
        public int AssetID { get; set; }
        public int OCID { get; set; }
        public int lastAllocationID { get; set; }
    
        public virtual AllocatedTo_t AllocatedTo_t { get; set; }
        public virtual AllocationType_t AllocationType_t { get; set; }
        public virtual Asset_t Asset_t { get; set; }
        public virtual AssetOpCondition_t AssetOpCondition_t { get; set; }
    }
}