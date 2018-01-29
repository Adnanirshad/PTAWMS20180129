using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.Models
{
    public class ModelFund
    {
        public int ID { get; set; }

        [Display(Name = "Balance Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BalanceDate { get; set; }

        [Display(Name = "Fund Own Balance")]
        [Required]
        [DataType(DataType.Currency)]
        public Double FundOwnBalance { get; set; }

        [Display(Name = "Fund Bank Balance")]
        [Required]
        [DataType(DataType.Currency)]
        public Double FundBankBalance { get; set; }

        [Display(Name = "EGF Gratuity Balance")]
        [Required]
        [DataType(DataType.Currency)]
        public Double EGFGratuityBalance { get; set; }
        
    }
}