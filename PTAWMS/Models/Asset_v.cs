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
    
    public partial class Asset_v
    {
        public int AssetID { get; set; }
        public int TYPID { get; set; }
        public string ClassName { get; set; }
        public string CategoryName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public string BCNo { get; set; }
        public string SrOrRegNo { get; set; }
        public int PURID { get; set; }
        public System.DateTime DoP { get; set; }
        public decimal UnitPrice { get; set; }
        public System.DateTime weDate { get; set; }
        public System.DateTime FromDate { get; set; }
        public string Status { get; set; }
        public string AllocationTypeName { get; set; }
        public string AllocatedToName { get; set; }
        public int VendorID { get; set; }
        public int AllocationTypeID { get; set; }
        public int AllocatedToID { get; set; }
        public int OCID { get; set; }
        public string OCName { get; set; }
        public System.DateTime ToDate { get; set; }
        public string VendorName { get; set; }
        public int OUID { get; set; }
        public int SOUID { get; set; }
        public int AssetStatusID { get; set; }
        public string Others { get; set; }
        public int CatID { get; set; }
        public int CLSID { get; set; }
        public string EmpNo { get; set; }
        public Nullable<int> DesignID { get; set; }
        public Nullable<int> DepartID { get; set; }
    }
}
