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
    
    public partial class AMS2ESSP
    {
        public int AssetID { get; set; }
        public string Asset { get; set; }
        public string BarCode { get; set; }
        public string Sr_Reg_No { get; set; }
        public System.DateTime Allocation_Date { get; set; }
        public int AllocatedToID { get; set; }
        public string Employee_No { get; set; }
    }
}