using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.Models
{
    public partial class ModalPending_Qualification
    {
        public int PQualificationID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int DegreeID { get; set; }
        public string Degree { get; set; }
        public int InstituteID { get; set; }
        public string Institute { get; set; }
        public string StartSession { get; set; }
        public string EndSession { get; set; }
        public string Specialization { get; set; }
        public string Grade { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int QualificationID { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
    }
}