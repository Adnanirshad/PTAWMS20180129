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
    
    public partial class AssetRetCriteriaConditions_t
    {
        public int ID { get; set; }
        public int CatID { get; set; }
        public string FirstCondition { get; set; }
        public string LogicalCondition { get; set; }
        public string SecondCondition { get; set; }
        public int CriteriaID { get; set; }
    
        public virtual AssetCategory_t AssetCategory_t { get; set; }
        public virtual AssetRetirementCriteria_t AssetRetirementCriteria_t { get; set; }
    }
}