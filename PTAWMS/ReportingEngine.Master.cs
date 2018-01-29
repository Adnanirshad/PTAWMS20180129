using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PTAWMS
{
    public partial class ReportingEngine : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
                    lbUserName.Text = LoggedInUser.FullName;
                    img.Src = "~/Home/RetrieveUserImage/" + LoggedInUser.EmpID;
                    img.Alt = LoggedInUser.FullName;
                    hidEmpID.Value = LoggedInUser.EmpID.ToString();
                    if (HttpContext.Current.Session["ReportMenu"] == null)
                        HttpContext.Current.Session["ReportMenu"] = "Attendance";
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}