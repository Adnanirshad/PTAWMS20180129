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
    
    public partial class VMS_CreateVisitorSMS
    {
        public int ID { get; set; }
        public string FirstLineForIN { get; set; }
        public string SeconfLineForIN { get; set; }
        public string ThirdAForIN { get; set; }
        public string ThirdBForIN { get; set; }
        public string ThirdCForIN { get; set; }
        public string ForthLineForIN { get; set; }
        public string FirstLineForOUT { get; set; }
        public string SeconfLineForOUT { get; set; }
        public string ThirdAForOUT { get; set; }
        public string ThirdBForOUT { get; set; }
        public string ThirdCForOUT { get; set; }
        public string ForthLineForOUT { get; set; }
        public Nullable<bool> InMessage { get; set; }
        public Nullable<bool> OutMessage { get; set; }
    }
}
