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
    
    public partial class ViewEmpTraining
    {
        public int TrainingID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<double> Year_ { get; set; }
        public string Training_Course { get; set; }
        public string Training_School_Institution { get; set; }
        public string DocumentPath { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string Source_of_Data { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
        public string TrainingPlace { get; set; }
        public Nullable<bool> InternationalTraining { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
