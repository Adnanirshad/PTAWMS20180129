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
    
    public partial class Vendor_t
    {
        public Vendor_t()
        {
            this.AssetPurchase_t = new HashSet<AssetPurchase_t>();
            this.AssetRepair_t = new HashSet<AssetRepair_t>();
        }
    
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContactDetails { get; set; }
    
        public virtual ICollection<AssetPurchase_t> AssetPurchase_t { get; set; }
        public virtual ICollection<AssetRepair_t> AssetRepair_t { get; set; }
    }
}
