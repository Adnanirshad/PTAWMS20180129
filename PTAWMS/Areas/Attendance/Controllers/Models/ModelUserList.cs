using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTAWMS.Areas.HumanResource.Models
{
    public class ModelUserList
    {
        public int LEAVEID { get; set; }
        public Nullable<int> EMPLOYEEID { get; set; }
        public string APPDATE { get; set; }
        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public string LEAVETYPE { get; set; }
        public Nullable<double> DUELEAVE { get; set; }
        public string REASON { get; set; }
        public string STATIONLEAVE { get; set; }
        public string ADDRESS { get; set; }
        public string TELNO { get; set; }
        //public string SUBSTITUTENAME { get; set; }

        public Nullable<int> SupervisorID { get; set; }
        public string SUBSTITUTENAME { get; set; }
        public SelectList SUBSTITUTENAMELIST { get; set; }
        public string SUBSTITUTEEMAIL { get; set; }
        public string STATUS { get; set; } 
        public int LeaveRecID { get; set; }
        public Nullable<int> RECID { get; set; }
        //public string REFERNAME { get; set; }

        public int REFERNAMEID { get; set; }
        public string REFERNAME { get; set; }
        public SelectList REFERNAMELIST { get; set; }
        public string REFEREMAIL { get; set; }
        public string DECISION { get; set; }
        public string COMMENTS { get; set; }
        public string ONDESKSINCE { get; set; }
        public string ACTIONDATE { get; set; }

    }


}