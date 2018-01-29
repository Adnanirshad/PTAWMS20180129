using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRM_IKAN.Models.DataValidation
{
    public class MAtt_LvApp
    {
        
    //using HRM_IKAN.Models.DataValidation;
    //using System.ComponentModel.DataAnnotations;
    //[MetadataType(typeof(MAtt_LvApp))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ToDate { get; set; }
    }
}