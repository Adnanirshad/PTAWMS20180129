using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSLibrary;

namespace PTAWMS.Reports.Filters
{
    public partial class StepFive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            Response.Redirect(path);
        }
        protected void btnStepOne_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepOne.aspx");
        }

        protected void btnStepTwo_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepTwo.aspx");
        }

        protected void btnStepThree_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepThree.aspx");
        }

        protected void btnStepFour_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepFour.aspx");
        }

        protected void btnStepFive_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepFive.aspx");
        }

        protected void btnStepSix_Click(object sender, EventArgs e)
        {


            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            Response.Redirect("~/Reports/Filters/AttendanceReport.aspx");
            //}
        }

        protected void btnHRQStep_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/HRQStep.aspx");
        }


        #endregion
    }
}