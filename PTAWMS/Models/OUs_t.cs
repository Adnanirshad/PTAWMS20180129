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
    
    public partial class OUs_t
    {
        public OUs_t()
        {
            this.AllocatedTo_t = new HashSet<AllocatedTo_t>();
            this.AllocationType_t = new HashSet<AllocationType_t>();
            this.AssetCategory_t = new HashSet<AssetCategory_t>();
            this.AssetClass_t = new HashSet<AssetClass_t>();
            this.AssetFamily_t = new HashSet<AssetFamily_t>();
            this.AssetMake_t = new HashSet<AssetMake_t>();
            this.AssetModel_t = new HashSet<AssetModel_t>();
            this.AssetPurchase_t = new HashSet<AssetPurchase_t>();
            this.AssetRepair_t = new HashSet<AssetRepair_t>();
            this.AssetRetirement_t = new HashSet<AssetRetirement_t>();
            this.AssetRetirementCriteria_t = new HashSet<AssetRetirementCriteria_t>();
            this.AssetType_t = new HashSet<AssetType_t>();
            this.Login_t = new HashSet<Login_t>();
            this.SOUs_t = new HashSet<SOUs_t>();
        }
    
        public int OUID { get; set; }
        public string OpUnit { get; set; }
    
        public virtual ICollection<AllocatedTo_t> AllocatedTo_t { get; set; }
        public virtual ICollection<AllocationType_t> AllocationType_t { get; set; }
        public virtual ICollection<AssetCategory_t> AssetCategory_t { get; set; }
        public virtual ICollection<AssetClass_t> AssetClass_t { get; set; }
        public virtual ICollection<AssetFamily_t> AssetFamily_t { get; set; }
        public virtual ICollection<AssetMake_t> AssetMake_t { get; set; }
        public virtual ICollection<AssetModel_t> AssetModel_t { get; set; }
        public virtual ICollection<AssetPurchase_t> AssetPurchase_t { get; set; }
        public virtual ICollection<AssetRepair_t> AssetRepair_t { get; set; }
        public virtual ICollection<AssetRetirement_t> AssetRetirement_t { get; set; }
        public virtual ICollection<AssetRetirementCriteria_t> AssetRetirementCriteria_t { get; set; }
        public virtual ICollection<AssetType_t> AssetType_t { get; set; }
        public virtual ICollection<Login_t> Login_t { get; set; }
        public virtual ICollection<SOUs_t> SOUs_t { get; set; }
    }
}