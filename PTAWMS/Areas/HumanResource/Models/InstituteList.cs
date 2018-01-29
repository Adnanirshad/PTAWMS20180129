using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.Models
{
    public class InstituteList
    {
        
            public string InstituteName { get; set; }
            public int NoOfInst { get; set; }

    }

    public class ValueList
    {

        public string Name { get; set; }
        public int Total { get; set; }

    }
}