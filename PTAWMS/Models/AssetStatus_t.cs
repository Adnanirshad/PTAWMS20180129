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
    
    public partial class AssetStatus_t
    {
        public AssetStatus_t()
        {
            this.Asset_t = new HashSet<Asset_t>();
        }
    
        public int ID { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<Asset_t> Asset_t { get; set; }
    }
}