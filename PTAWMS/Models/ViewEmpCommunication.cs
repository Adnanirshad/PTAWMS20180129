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
    
    public partial class ViewEmpCommunication
    {
        public int EmployeeID { get; set; }
        public int CommunicationID { get; set; }
        public int TypeID { get; set; }
        public string Comment { get; set; }
        public int RecordID { get; set; }
        public int StatusID { get; set; }
        public int ActionBy { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public string EmpName { get; set; }
        public string ActionByName { get; set; }
        public string Status { get; set; }
        public int Expr1 { get; set; }
        public Nullable<bool> HRModule { get; set; }
    }
}
