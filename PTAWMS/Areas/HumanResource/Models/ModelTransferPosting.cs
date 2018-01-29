using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.Models
{
    public class ModelTransferPosting
    {

        public int ID { get; set; }

        [Display(Name = "From Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Display(Name = "Designation")]
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Designation { get; set; }

        [Display(Name = "Report To")]
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string ReportTo { get; set; }

        [Display(Name = "Station")]
        [Required]
        [StringLength(500, MinimumLength = 0)]
        public string Station { get; set; }

        [Display(Name = "Division")]
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Division { get; set; }

        [Display(Name = "Department")]
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Department { get; set; }

    }
}