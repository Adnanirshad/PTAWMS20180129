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
    
    public partial class AssetRepair_t
    {
        public int RepairID { get; set; }
        public int AssetID { get; set; }
        public System.DateTime RepairDate { get; set; }
        public decimal RepairCost { get; set; }
        public int wPeriod { get; set; }
        public string wDMY { get; set; }
        public Nullable<System.DateTime> weDate { get; set; }
        public int VendorID { get; set; }
        public string Remarks { get; set; }
        public int OUID { get; set; }
        public int SOUID { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int RTID { get; set; }
    
        public virtual Asset_t Asset_t { get; set; }
        public virtual SOUs_t SOUs_t { get; set; }
        public virtual OUs_t OUs_t { get; set; }
        public virtual AssetRepairType_t AssetRepairType_t { get; set; }
        public virtual Vendor_t Vendor_t { get; set; }
    }
}
