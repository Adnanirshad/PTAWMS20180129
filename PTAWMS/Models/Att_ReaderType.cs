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
    
    public partial class Att_ReaderType
    {
        public Att_ReaderType()
        {
            this.Att_Reader = new HashSet<Att_Reader>();
        }
    
        public byte RdrTypeID { get; set; }
        public string RdrTypeName { get; set; }
        public string RdrType { get; set; }
    
        public virtual ICollection<Att_Reader> Att_Reader { get; set; }
    }
}
