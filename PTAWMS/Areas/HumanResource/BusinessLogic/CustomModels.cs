using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTAWMS.Areas.HumanResource.BusinessLogic
{
    public class CustomModels
    {
    }
    public class ModelEmpProfileIndex
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string FullName { get; set; }
    }
    public class ModelEmpJobDetail
    {
        public int EmpID { get; set; }
        public int? BusinessAreaID { get; set; }
        public int? DivsionID { get; set; }
        public int? DepartmentID { get; set; }
        public int? SectionID { get; set; }
        public int? CategoryID { get; set; }
        public int? EmpTypeID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PermanentDate { get; set; }
        public int? LocationID { get; set; }
        public int? GradeID { get; set; }
        public int? DesignationID { get; set; }
        public int? CMDesignationID { get; set; }
        public int? GroupID { get; set; }
        public int? JobTitleID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOJ { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LeavingDate { get; set; }
        public string ReasonToLeave { get; set; }
        public bool Clereance { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClearenceDate { get; set; }
        public bool SAPIntegrated { get; set; }
        public int? SAPID { get; set; }
    }
    //public class ModelEmpPersonal
    //{
    //    public int EmpID { get; set; }
    //    public string FirstName { get; set; }
    //    public string MiddleName { get; set; }
    //    public string LastName { get; set; }
    //    public string OldIKNo { get; set; }
    //    public string FathersName { get; set; }
    //    public string MothersName { get; set; }
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime CNICExpireDate { get; set; }
    //    public string CNICNo { get; set; }
    //    public string PassportNo { get; set; }
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime DOB { get; set; }
    //    public string PECNo { get; set; }
    //    public string Gender { get; set; }
    //    public string BloodGroup { get; set; }
    //    public string MaritalStatus { get; set; }
    //    public string Religion { get; set; }
    //    public string Nationality { get; set; }
    //}
    //public class ModelEmpContact
    //{
    //    public int EmpID { get; set; }
    //    public string Address { get; set; }
    //    public string City { get; set; }
    //    public string Country { get; set; }
    //    public string LandLine { get; set; }
    //    public string MobileNo { get; set; }
    //    public string EmergencyContactNo { get; set; }
    //    public string EmailID { get; set; }
    //    public string OfficialContactNo { get; set; }
    //    public string OfficialEmailID { get; set; }
    //}
    //public class ModelEmpAttendance
    //{
    //    public int EmpID { get; set; }
    //    public short ShiftID { get; set; }
    //    public int OvertimePolicyID { get; set; }
    //    public string PinCode { get; set; }
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime ValidDate { get; set; }
    //    public bool? ProcessAttendance { get; set; }
    //    public bool? ProcessOnlyMonth { get; set; }
    //    public bool? FPTemp { get; set; }
    //    public bool? FaceTemp { get; set; }
    //    public string CardNo { get; set; }
    //    public short? LeavePolicyA { get; set; }
    //    public short? LeavePolicyB { get; set; }
    //    public short? LeavePolicyC { get; set; }
    //    public short? LeavePolicyD { get; set; }
    //    public short? LeavePolicyE { get; set; }
    //    public short? LeavePolicyF { get; set; }
    //    public short? LeavePolicyG { get; set; }
    //    public short? LeavePolicyH { get; set; }
    //    public short? LeavePolicyI { get; set; }
    //    public short? LeavePolicyJ { get; set; }
    //    public short? LeavePolicyK { get; set; }
    //    public short? LeavePolicyL { get; set; }
    //}
    //public class ModelEmpSalary
    //{
    //    public int EmpID { get; set; }
    //    public double? GrossSalary { get; set; }
    //    public string ParentEmpNo { get; set; }
    //    public bool? TaxableBonus { get; set; }
    //    public bool? EOBIContri { get; set; }
    //    public string EOBINo { get; set; }
    //    public bool? PFContri { get; set; }
    //    public double? PFAmount { get; set; }
    //    public double? PFMonthly { get; set; }
    //    public bool? OvertimeCashable { get; set; }
    //    public bool? HasAccount { get; set; }
    //    public short? BankID { get; set; }
    //    public int? BranchID { get; set; }
    //    public int? CompAcountID { get; set; }
    //    public string AccountNo { get; set; }
    //    public string SidatID { get; set; }
    //    public string BranchCode { get; set; }
    //    public int? FirstPPeriodID { get; set; }
    //    public bool? HasPayroll { get; set; }
    //    public string PaymentMode { get; set; }

    //}
    //public class ModelEmpTax
    //{
    //    public int EmpID { get; set; }
    //    public bool? Taxable { get; set; }
    //    public bool? TaxableBonus { get; set; }
    //    public string TaxCriteria { get; set; }
    //    public double? YearlyIncome { get; set; }
    //    public double? TaxExceedingAmount { get; set; }
    //    public double? FixedAmount { get; set; }
    //    public double? TaxOnExceedingAmount { get; set; }
    //    public double? TotalTaxPayable { get; set; }
    //    public double? AgeExemption { get; set; }
    //    public double? TaxDebitAmount { get; set; }
    //    public double? TaxCreditAmount { get; set; }
    //    public double? CFExemption { get; set; }
    //    public double? TaxAfterReturn { get; set; }
    //    public double? TaxPaid { get; set; }
    //    public double? TaxMonthly { get; set; }
    //    public double? TotalMonths { get; set; }
    //    public string StartPeriod { get; set; }
    //}
    //public class ModelEmpCreate
    //{
    //    public int EmpID { get; set; }
    //    public string EmpNo { get; set; }
    //    public string FirstName { get; set; }
    //    public string MiddleName { get; set; }
    //    public string LastName { get; set; }
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime DOJ { get; set; }
    //    public bool HasPayroll { get; set; }
    //    public int FirstPPeriodID { get; set; }
    //    public string Gender { get; set; }
    //    public string BloodGroup { get; set; }
    //    public string MaritalStatus { get; set; }
    //    public string Religion { get; set; }
    //    public string Nationality { get; set; }
    //    public int LocationID { get; set; }
    //}


    public class ModelEmpPersonal
    {
        [Display(Name = "Employee Number")]
        public int EmpID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Old IK Number")]
        public string OldIKNo { get; set; }
        [Display(Name = "Father Name")]
        public string FathersName { get; set; }
        [Display(Name = "Mother Name")]
        public string MothersName { get; set; }
        [Display(Name = "CNIC Expire date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CNICExpireDate { get; set; }
        [Display(Name = "CNIC Number")]
        public string CNICNo { get; set; }
        [Display(Name = "Passport Number")]
        public string PassportNo { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        [Display(Name = "PEC Number")]
        public string PECNo { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Religion")]
        public string Religion { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }
        [Display(Name = "Domicile")]
        public string DomicileCity { get; set; }
        [Display(Name = "Domicile Province")]
        public string DomicileProvince { get; set; }
        [Display(Name = "NTN Number")]
        public string NTN { get; set; }
        [Display(Name = "Current Designation")]
        public string Designation { get; set; }
        [Display(Name = "Current Grade")]
        public string Grade { get; set; }
        [Display(Name = "Employment")]
        public string Employment { get; set; }
        [Display(Name = "Mode of Appointment")]
        public string Category { get; set; }
        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AppointmentDate { get; set; }
        [Display(Name = "Confirmation Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ConfirmationDate { get; set; }
        [Display(Name = "Retirement/Contract Termination Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RetirementDate { get; set; }
        [Display(Name = "Organization Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoiningDate { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }
        [Display(Name = "Fax Number")]
        public string FaxNo { get; set; }
        [Display(Name = "Home Telephone")]
        public string HomeLandLineNo { get; set; }
        [Display(Name = "Office Telephone")]
        public string OfficeLandLine { get; set; }
        [Display(Name = "Emergency Contact Number")]
        public string EmergencyContactNo { get; set; }
        [Display(Name = "ReportToID")]
        public string ReportToID { get; set; }
        [Display(Name = "Report To")]
        public string ReportTo { get; set; }
        [Display(Name = "Department")]
        public string Section { get; set; }
        [Display(Name = "OverTime Policy")]
        public string OverTimePolicy { get; set; }
        [Display(Name = "Office Card No.")]
        public string OfficeCardNo { get; set; }
        [Display(Name = "Station")]
        public string Location { get; set; }
        [Display(Name = "Divison")]
        public string Divison { get; set; }
        [Display(Name = "Room No.")]
        public string RoomNo { get; set; }
        [Display(Name = "Extension No.")]
        public string ExtNo { get; set; }
    }
    public class ModelEmpContact
    {
        public int EmpID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string LandLine { get; set; }
        public string MobileNo { get; set; }
        public string EmergencyContactNo { get; set; }
        public string EmailID { get; set; }
        public string OfficialContactNo { get; set; }
        public string OfficialEmailID { get; set; }
    }
    public class ModelEmpAttendance
    {
        public int EmpID { get; set; }
        public short ShiftID { get; set; }
        public int OvertimePolicyID { get; set; }
        public string PinCode { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidDate { get; set; }
        public bool? ProcessAttendance { get; set; }
        public bool? ProcessOnlyMonth { get; set; }
        public bool? FPTemp { get; set; }
        public bool? FaceTemp { get; set; }
        public string CardNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public float TaxRate{ get; set; }
        public string BankAccount { get; set; }
        public string HomeAdd { get; set; }
        public short? LeavePolicyA { get; set; }
        public short? LeavePolicyB { get; set; }
        public short? LeavePolicyC { get; set; }
        public short? LeavePolicyD { get; set; }
        public short? LeavePolicyE { get; set; }
        public short? LeavePolicyF { get; set; }
        public short? LeavePolicyG { get; set; }
        public short? LeavePolicyH { get; set; }
        public short? LeavePolicyI { get; set; }
        public short? LeavePolicyJ { get; set; }
        public short? LeavePolicyK { get; set; }
        public short? LeavePolicyL { get; set; }
    }
    public class ModelEmpSalary
    {
        public int EmpID { get; set; }
        public double? GrossSalary { get; set; }
        public string ParentEmpNo { get; set; }
        public bool? TaxableBonus { get; set; }
        public bool? EOBIContri { get; set; }
        public string EOBINo { get; set; }
        public bool? PFContri { get; set; }
        public double? PFAmount { get; set; }
        public double? PFMonthly { get; set; }
        public bool? OvertimeCashable { get; set; }
        public bool? HasAccount { get; set; }
        public short? BankID { get; set; }
        public int? BranchID { get; set; }
        public int? CompAcountID { get; set; }
        public string AccountNo { get; set; }
        public string SidatID { get; set; }
        public string BranchCode { get; set; }
        public int? FirstPPeriodID { get; set; }
        public bool? HasPayroll { get; set; }
        public string PaymentMode { get; set; }

    }
    public class ModelEmpTax
    {
        public int EmpID { get; set; }
        public bool? Taxable { get; set; }
        public bool? TaxableBonus { get; set; }
        public string TaxCriteria { get; set; }
        public double? YearlyIncome { get; set; }
        public double? TaxExceedingAmount { get; set; }
        public double? FixedAmount { get; set; }
        public double? TaxOnExceedingAmount { get; set; }
        public double? TotalTaxPayable { get; set; }
        public double? AgeExemption { get; set; }
        public double? TaxDebitAmount { get; set; }
        public double? TaxCreditAmount { get; set; }
        public double? CFExemption { get; set; }
        public double? TaxAfterReturn { get; set; }
        public double? TaxPaid { get; set; }
        public double? TaxMonthly { get; set; }
        public double? TotalMonths { get; set; }
        public string StartPeriod { get; set; }
    }
    public class ModelEmpCreate
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOJ { get; set; }
        public bool HasPayroll { get; set; }
        public int FirstPPeriodID { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public int LocationID { get; set; }
    }
    public class ModelEmpDependents
    {
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Relationship")]
        public int RelationshipID { get; set; }
        public SelectList RelationshipList { get; set; }
        [Display(Name = "Medical Facility Allowed")]
        public bool MedicalFacilityAllowed { get; set; }
        [Display(Name = "Provident Fund")]
        public bool ProvidentFund { get; set; }
        [Display(Name = "Benevolent Fund")]
        public bool NominationsBenevolentFund { get; set; }
        [Display(Name = "Graduity")]
        public bool Graduity { get; set; }
        [Display(Name = "Death Compensation")]
        public bool DeathCompensation { get; set; }
        [Display(Name = "GTA")]
        public bool GTA { get; set; }
        [Display(Name = "CPF")]
        public bool CPF { get; set; }

        [Display(Name = "Medical Facility Allowed")]
        public string MedicalFacilityAllowedStatus { get; set; }
        [Display(Name = "Provident Fund")]
        public string ProvidentFundStatus { get; set; }
        [Display(Name = "Benevolent Fund")]
        public string NominationsBenevolentFundStatus { get; set; }
        [Display(Name = "Graduity")]
        public string GraduityStatus { get; set; }
        [Display(Name = "Death Compensation")]
        public string DeathCompensationStatus { get; set; }
        [Display(Name = "GTA")]
        public string GTAStatus { get; set; }
        [Display(Name = "CPF")]
        public string CPFStatus { get; set; }
    }
    public class ModelPerformanceHistory
    {
        public int ID { get; set; }

        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        [Display(Name = "Appraisal Year")]
        public string AppraisalYear { get; set; }
        [Display(Name = "Final PMS Rating")]
        public string FinalPMSRating { get; set; }
        [Display(Name = "CSO")]
        public string CSO { get; set; }
        [Display(Name = "Provident Fund")]
        public string ProvidentFund { get; set; }

    }
    public class ModelExperienceHistory
    {
        public int ID { get; set; }
        [Display(Name = "From Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string ToDate { get; set; }
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        [Display(Name = "Organisation")]
        public string Organisation { get; set; }
        [Display(Name = "Job Details")]
        public string JobDescription { get; set; }
        [Display(Name = "Organisation Address")]
        public string OrgAddress { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Path")]
        public string ExperiencePath { get; set; }
    }
    public class ModelEmpQualification
    {
        public int ID { get; set; }

        public string DegreeName { get; set; }

        public int InstituteID { get; set; }
        public string Institute { get; set; }
        public SelectList InstituteList { get; set; }

        [Display(Name = "Start Date")]
        //[Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Specialization")]
        //[Required]
        [StringLength(500, MinimumLength = 0)]
        public string Specialization { get; set; }

        [Display(Name = "Division/Grade")]
        //[Required]
        [StringLength(20, MinimumLength = 0)]
        public string Grade { get; set; }

        [Display(Name = "Session Start")]
        //[Required]
        [StringLength(20, MinimumLength = 0)]
        public string SessionStart { get; set; }

        [Display(Name = "Session End")]
        //[Required]
        [StringLength(20, MinimumLength = 0)]
        public string SessionEnd { get; set; }

        public int DegreeID { get; set; }
        public SelectList DegreesList { get; set; }

    }


    public partial class ModelEmpPendingQualification
    {
        public int PQualificationID { get; set; }
        public int EmployeeID { get; set; }
        public int DegreeID { get; set; }
        public int InstituteID { get; set; }
        public string Institute { get; set; }
        public string DegreeName { get; set; }
        public SelectList InstituteList { get; set; }
        public string StartSession { get; set; }
        public string EndSession { get; set; }
        public string Specialization { get; set; }
        public string Grade { get; set; }
        public int StatusID { get; set; }
        public int QualificationID { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }
        [Display(Name = "Session Start")]
        //[Required]
        [StringLength(20, MinimumLength = 0)]
        public string SessionStart { get; set; }

        [Display(Name = "Session End")]
        //[Required]
        [StringLength(20, MinimumLength = 0)]
        public string SessionEnd { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
    }
}