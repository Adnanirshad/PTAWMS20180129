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
    
    public partial class ViewEMPTransfer
    {
        public int PHEmpID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<short> ODesignationID { get; set; }
        public Nullable<short> OLocID { get; set; }
        public Nullable<short> OSectionID { get; set; }
        public Nullable<short> OSecID { get; set; }
        public Nullable<short> ODeptID { get; set; }
        public Nullable<short> OEmpTypeID { get; set; }
        public Nullable<short> OGradeID { get; set; }
        public string OGrd { get; set; }
        public Nullable<int> OReportingTo { get; set; }
        public string OScale { get; set; }
        public Nullable<System.DateTime> OStationJoinDate { get; set; }
        public Nullable<System.DateTime> OCurrentDate { get; set; }
        public Nullable<System.DateTime> ORetirementDate { get; set; }
        public Nullable<System.DateTime> ODateOfCommision { get; set; }
        public Nullable<System.DateTime> OGovtSrvsDate { get; set; }
        public Nullable<System.DateTime> OOrgJoinDate { get; set; }
        public Nullable<System.DateTime> OTerminationDate { get; set; }
        public string Remarks { get; set; }
        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public string LocationName { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string TypeName { get; set; }
    }
}
