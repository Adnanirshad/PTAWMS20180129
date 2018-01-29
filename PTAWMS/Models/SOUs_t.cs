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
    
    public partial class SOUs_t
    {
        public SOUs_t()
        {
            this.AllocatedTo_t = new HashSet<AllocatedTo_t>();
            this.Asset_t = new HashSet<Asset_t>();
            this.AssetCategory_t = new HashSet<AssetCategory_t>();
            this.AssetClass_t = new HashSet<AssetClass_t>();
            this.AssetFamily_t = new HashSet<AssetFamily_t>();
            this.AssetMake_t = new HashSet<AssetMake_t>();
            this.AssetModel_t = new HashSet<AssetModel_t>();
            this.AssetPurchase_t = new HashSet<AssetPurchase_t>();
            this.AssetRepair_t = new HashSet<AssetRepair_t>();
            this.AssetType_t = new HashSet<AssetType_t>();
            this.Login_t = new HashSet<Login_t>();
        }
    
        public int SOUID { get; set; }
        public string SOpUnit { get; set; }
        public int OUID { get; set; }
    
        public virtual ICollection<AllocatedTo_t> AllocatedTo_t { get; set; }
        public virtual ICollection<Asset_t> Asset_t { get; set; }
        public virtual ICollection<AssetCategory_t> AssetCategory_t { get; set; }
        public virtual ICollection<AssetClass_t> AssetClass_t { get; set; }
        public virtual ICollection<AssetFamily_t> AssetFamily_t { get; set; }
        public virtual ICollection<AssetMake_t> AssetMake_t { get; set; }
        public virtual ICollection<AssetModel_t> AssetModel_t { get; set; }
        public virtual ICollection<AssetPurchase_t> AssetPurchase_t { get; set; }
        public virtual ICollection<AssetRepair_t> AssetRepair_t { get; set; }
        public virtual ICollection<AssetType_t> AssetType_t { get; set; }
        public virtual ICollection<Login_t> Login_t { get; set; }
        public virtual OUs_t OUs_t { get; set; }
    }
}
