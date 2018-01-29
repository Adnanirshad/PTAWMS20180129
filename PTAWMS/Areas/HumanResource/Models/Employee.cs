using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.Models
{
    public class ModelEmployee
    {

    # region Personal_Information_Fields



        public int EmployeeID { get; set; }

//asdfasdfasdfasf
        public string EmpNo { get; set; }
        public int FPID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Full Name")]
        public string FathersName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Date of Birth")]
        public Nullable<System.DateTime> DOB { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "CNIC No.")]
        public string CNICNo { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Religion")]
        public string Religion { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Domicile City")]
        public string DomicileCity { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Domicile Province")]
        public string DomicileProvince { get; set; }
        //[Required(ErrorMessage = "*")]
        [Display(Name = "NTN No.")]
        public string NTN { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Present Address")]
        public string Address { get; set; }

        #endregion



        //public string BloodGroup { get; set; }
        //public Nullable<int> EmpImage { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        //public string LandLine { get; set; }
        //public string MobileNo { get; set; }
        //public string EmergencyContactNo { get; set; }
        //public string EmailID { get; set; }
        //public string OfficialContactNo { get; set; }
        //public string OfficialEmailID { get; set; }
        //public string Status { get; set; }
        //public Nullable<System.DateTime> LeavingDate { get; set; }
        //public Nullable<short> DesignationID { get; set; }
        //public Nullable<short> LocationID { get; set; }
        //public Nullable<short> SectionID { get; set; }
        //public Nullable<short> DeptID { get; set; }
        //public Nullable<short> GradeID { get; set; }
        //public Nullable<System.DateTime> DOJ { get; set; }
        //public Nullable<int> ReportingToID { get; set; }
        //public Nullable<short> EmpTypeID { get; set; }
        //public Nullable<short> ShiftID { get; set; }
        //public string CardNo { get; set; }
        //public string PinCode { get; set; }
        //public Nullable<System.DateTime> ValidDate { get; set; }
        //public Nullable<bool> FaceTemp { get; set; }
        //public Nullable<int> FPTemp { get; set; }
        //public Nullable<bool> ProcessAttendance { get; set; }
        //public Nullable<short> OTPolicyID { get; set; }
    }
}