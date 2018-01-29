using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRM_IKAN.Models.DataValidation
{
    [MetadataType(typeof(HR_Employee))]
    partial class Employee
    {
        // Add logic to the generated class in here.
        public int FullName { get; set; }
        
    }
}