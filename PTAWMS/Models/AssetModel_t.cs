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
    
    public partial class AssetModel_t
    {
        public AssetModel_t()
        {
            this.AssetFamily_t = new HashSet<AssetFamily_t>();
            this.AssetType_t = new HashSet<AssetType_t>();
        }
    
        public int MODID { get; set; }
        public int CatID { get; set; }
        public int MAKID { get; set; }
        public string Name { get; set; }
        public int CLSID { get; set; }
        public int OUID { get; set; }
        public int SOUID { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual AssetCategory_t AssetCategory_t { get; set; }
        public virtual AssetClass_t AssetClass_t { get; set; }
        public virtual ICollection<AssetFamily_t> AssetFamily_t { get; set; }
        public virtual AssetMake_t AssetMake_t { get; set; }
        public virtual SOUs_t SOUs_t { get; set; }
        public virtual OUs_t OUs_t { get; set; }
        public virtual ICollection<AssetType_t> AssetType_t { get; set; }
    }
}
