using PTAWMS.Helper;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSLibrary;

namespace PTAWMS.Reports.Filters
{
    public partial class StepOne : System.Web.UI.Page
    {
        private HRMEntities da = new HRMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewDepartment("");
                BindGridViewLocation("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
                if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
                {
                    hidDateFilter.Value = "1";
                }
                else if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
                    hidDateFilter.Value = "0";
            }
            else
            {
                SaveDepartmentIDs();
                SaveLocationIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            }
        }
        protected void ButtonSearchLocation_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveLocationIDs();
            BindGridViewLocation(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
        }
        protected void ButtonSearchDepartment_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();
            BindGridViewDepartment(TextBoxSearchDepartment.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
        }
        protected void GridViewDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();

            //change page index
            GridViewDepartment.PageIndex = e.NewPageIndex;
            BindGridViewDepartment(TextBoxSearchDepartment.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
        }

        protected void GridViewLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveLocationIDs();

            //change page index
            GridViewLocation.PageIndex = e.NewPageIndex;
            BindGridViewLocation(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
        }
        private void SaveDepartmentIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
            Session["FiltersModel"] = FM;
        }
        private void SaveLocationIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            Session["FiltersModel"] = FM;
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

            //WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Company");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewCity, Session["FiltersModel"] as FiltersModel, "Location");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Type");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
            {
                hidDateFilter.Value = "0";
            }

        }
        #endregion
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
        private void BindGridViewDepartment(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<HR_Department> _View = new List<HR_Department>();
            List<HR_Department> _TempView = new List<HR_Department>();
            ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForReportsDepartment(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select * from HR_Department where Status=1 order by DepartmentName asc");
            _View = dt.ToList<HR_Department>();
            GridViewDepartment.DataSource = _View.Where(aa => aa.DepartmentName.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.DepartmentName).ToList();
            GridViewDepartment.DataBind();
        }

        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }

        private void BindGridViewLocation(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<HR_Location> _View = new List<HR_Location>();
            List<HR_Location> _TempView = new List<HR_Location>();
            ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForLocReport(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select * from HR_Location" + query + " LocationName");
            _View = dt.ToList<HR_Location>();
            GridViewLocation.DataSource = _View.Where(aa => aa.LocationName.ToUpper().Contains(search.ToUpper()) && aa.Status == true).OrderBy(aa => aa.LocationName).ToList();
            GridViewLocation.DataBind();
        }


        protected void GridViewDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDepartment.PageIndex + 1) + " of " + GridViewDepartment.PageCount;
            }
        }

        protected void GridViewLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewLocation.PageIndex + 1) + " of " + GridViewLocation.PageCount;
            }
        }

        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveLocationIDs();

            Response.Redirect(path);
        }
        protected void btnStepOne_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepOne.aspx");
        }

        protected void btnStepTwo_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/StepTwo.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepTwo.aspx");
        }

        protected void btnStepThree_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/StepThree.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepThree.aspx");
        }

        protected void btnStepFour_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/StepFour.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepFour.aspx");
        }

        protected void btnStepFive_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/StepFive.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepFive.aspx");
        }
        protected void btnStepSeven_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepSeven.aspx");
        }
        protected void btnHRQStep_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/HRQStep.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/HRQStep.aspx");
        }
        protected void btnStepSix_Click(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveLocationIDs();

            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/AttendanceReport.aspx?dateFilter=true");
            }
            else
                Response.Redirect("~/Reports/Filters/AttendanceReport.aspx");
            //}
        }

        protected void btnStepOnePre_Click(object sender, EventArgs e)
        {
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            Response.Redirect("~/Reports/Filters/StepOnePre.aspx");
            //}
        }
        #endregion
        [WebMethod(EnableSession = true)]
        public static string DeleteSingleFilter(string id, string parentid)
        {
            FiltersModel fml = new FiltersModel();
            fml = HttpContext.Current.Session["FiltersModel"] as FiltersModel;
            fml = WMSLibrary.Filters.DeleteSingleFilter(fml, id, parentid);
            return DateTime.Now.ToString();
        }


        protected void btnGenerateHRReport(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveLocationIDs();

            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                Response.Redirect("~/Reports/RptLoader/AttReportsHome.aspx?reportname=HRMS_PTA_Worker&dateFilter=true");
            }
            else
                Response.Redirect("~/Reports/RptLoader/AttReportsHome.aspx?reportname=HRMS_PTA_Worker");
            //}
        }

    }
}