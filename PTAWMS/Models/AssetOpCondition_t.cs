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
    
    public partial class AssetOpCondition_t
    {
        public AssetOpCondition_t()
        {
            this.AssetAllocation_t = new HashSet<AssetAllocation_t>();
        }
    
        public int OCID { get; set; }
        public string OCName { get; set; }
    
        public virtual ICollection<AssetAllocation_t> AssetAllocation_t { get; set; }
    }
}