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
    
    public partial class EmpDepartments_t
    {
        public EmpDepartments_t()
        {
            this.AllocatedTo_t = new HashSet<AllocatedTo_t>();
        }
    
        public int ID { get; set; }
        public string Department { get; set; }
    
        public virtual ICollection<AllocatedTo_t> AllocatedTo_t { get; set; }
    }
}