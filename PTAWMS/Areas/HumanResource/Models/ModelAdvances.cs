using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.Models
{
    public class ModelAdvances
    {
        public int ID { get; set; }

        [Display(Name = "Advance Type")]
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string AdvacneType { get; set; }

        [Display(Name = "Availed (Rs.)")]
        [Required]
        [DataType(DataType.Currency)]
        public Double AvailedCost { get; set; }

        [Display(Name = "Outstanding (Rs.)")]
        [Required]
        [DataType(DataType.Currency)]
        public Double OutstandingCost { get; set; }

        [Display(Name = "Loan Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LoanDate { get; set; }      

        
    }
}