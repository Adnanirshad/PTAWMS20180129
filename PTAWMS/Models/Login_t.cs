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
    
    public partial class Login_t
    {
        public Login_t()
        {
            this.Asset_t = new HashSet<Asset_t>();
            this.AssetPurchase_t = new HashSet<AssetPurchase_t>();
            this.AssetPurchase_t1 = new HashSet<AssetPurchase_t>();
        }
    
        public int ID { get; set; }
        public string LoginID { get; set; }
        public string LoginPwd { get; set; }
        public string UserType { get; set; }
        public int OUID { get; set; }
        public int SOUID { get; set; }
    
        public virtual ICollection<Asset_t> Asset_t { get; set; }
        public virtual ICollection<AssetPurchase_t> AssetPurchase_t { get; set; }
        public virtual ICollection<AssetPurchase_t> AssetPurchase_t1 { get; set; }
        public virtual SOUs_t SOUs_t { get; set; }
        public virtual OUs_t OUs_t { get; set; }
    }
}