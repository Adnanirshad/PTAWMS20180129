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
    
    public partial class VMS_VisitSMS
    {
        public int PID { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string MobileNo { get; set; }
        public string VisitorName { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string VCompany { get; set; }
        public Nullable<int> VisitID { get; set; }
        public string INOUTStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> TimeIN { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public string SMSStutus { get; set; }
        public string Remarks { get; set; }
    }
}
