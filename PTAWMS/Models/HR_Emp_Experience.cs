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
    
    public partial class HR_Emp_Experience
    {
        public int ExperienceID { get; set; }
        public int EmployeeID { get; set; }
        public string Designation { get; set; }
        public string OrganisationName { get; set; }
        public string OrgAddress { get; set; }
        public string OrgContactNumber { get; set; }
        public string Department { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string JobDescription { get; set; }
        public string ExperienceLetterPath { get; set; }
        public int StatusID { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
    }
}