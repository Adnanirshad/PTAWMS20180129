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
    
    public partial class HR_EmpPromotions
    {
        public int PromotionID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<System.DateTime> Date_of_Promotion___Reappointment { get; set; }
        public string Details { get; set; }
        public string DocumentPath { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public string Source_of_Data { get; set; }
    }
}
