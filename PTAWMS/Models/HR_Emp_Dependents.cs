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
    
    public partial class HR_Emp_Dependents
    {
        public int DependentID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string Name { get; set; }
        public bool MedicalFacilityAllowed { get; set; }
        public bool ProvidentFundAllowed { get; set; }
        public bool BenevolentFundAllowed { get; set; }
        public bool Graduity { get; set; }
        public bool DeathCompensation { get; set; }
        public bool CTF { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string DocumentPath { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public string Relationship { get; set; }
        public string DOB { get; set; }
        public string Date1 { get; set; }
        public string Source_of_Data { get; set; }
        public string SEQ { get; set; }
    }
}