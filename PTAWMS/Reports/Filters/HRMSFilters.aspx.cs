using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSLibrary;

namespace PTAWMS.Reports.Filters
{
    public partial class HRMSFilters : System.Web.UI.Page
    {
        private HRMEntities da = new HRMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewQualification("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
            }
            else
            {
                SaveQualificationIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Qualification");
            }
        }
        protected void ButtonSearchEmployee_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveQualificationIDs();
            BindGridViewQualification(tbSearch_Employe1.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Qualification");
        }
        protected void GridViewEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveQualificationIDs();

            //change page index
            GridViewEmployee.PageIndex = e.NewPageIndex;
            BindGridViewQualification(tbSearch_Employe1.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Qualification");
        }

        #region --DeleteAll Filters--
        protected void ButtonDeleteAll_Click(object sender, EventArgs e)
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            list[1] = DateTime.Today.ToString("yyyy-MM-dd");
            dateFrom.Value = list[0];
            dateTo.Value = list[1];
            Session["ReportSession"] = list;
            WMSLibrary.Filters filtersHelper = new WMSLibrary.Filters();
            Session["FiltersModel"] = filtersHelper.DeleteAllFilters(Session["FiltersModel"] as FiltersModel);

            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Qualification");


        }
        #endregion
        private void SaveQualificationIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Qualification");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewQualification(string search)
        {
            using (var db = new HRMEntities())
            {
                
                GridViewEmployee.DataSource = db.HR_Emp_Qualification.ToList();
                GridViewEmployee.DataBind();
            }
        }


        protected void GridViewEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewEmployee.PageIndex + 1) + " of " + GridViewEmployee.PageCount;
            }
        }

        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }
        public DateTime DateFrom
        {
            get
            {
                if (dateFrom.Value == "")
                    return DateTime.Today.Date.AddDays(-1);
                else
                    return DateTime.Parse(dateFrom.Value);
            }
        }
        public DateTime DateTo
        {
            get
            {
                if (dateTo.Value == "")
                    return DateTime.Today.Date.AddDays(-1);
                else
                    return DateTime.Parse(dateTo.Value);
            }
        }


        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveQualificationIDs();
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
        protected void btnStepSeven_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepSeven.aspx");
        }
        protected void btnStepSix_Click(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveQualificationIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            Response.Redirect("~/Reports/Filters/AttendanceReport.aspx");
            //}
        }
        protected void btnStepOnePre_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/Filters/StepOnePre.aspx");
            //}
        }

        #endregion
    }
}