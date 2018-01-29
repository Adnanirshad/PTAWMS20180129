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
    public partial class StepTwo : System.Web.UI.Page
    {
        private HRMEntities da = new HRMEntities();
        [WebMethod]
        public static string SaveReportSession(string id)
        {
            HttpContext.Current.Session["ReportMenu"] = id;
            return "OK";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewType("");
                BindGridViewShift("");
                BindGridViewDomicile("");
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
                SaveTypeIDs();
                SaveShiftIDs();
                SaveDomicileIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Domicile");
            }
        }
        protected void ButtonSearchType_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveTypeIDs();
            BindGridViewType(tbSearch_Type.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
        }
        protected void ButtonSearchShift_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveShiftIDs();
            BindGridViewShift(tbSearch_Shift.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
        }
        protected void ButtonSearchDomicile_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDomicileIDs();
            BindGridViewDomicile(tbSearchDomicile.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDomicile, Session["FiltersModel"] as FiltersModel, "Domicile");
        }
        protected void GridViewType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveTypeIDs();

            //change page index
            GridViewType.PageIndex = e.NewPageIndex;
            BindGridViewType(tbSearch_Type.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
        }
        #region --DeleteAll Filters--
        protected void ButtonDeleteAll_Click(object sender, EventArgs e)
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            list[1] = DateTime.Today.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
            dateFrom.Value = list[0];
            dateTo.Value = list[1];
            WMSLibrary.Filters filtersHelper = new WMSLibrary.Filters();
            Session["FiltersModel"] = filtersHelper.DeleteAllFilters(Session["FiltersModel"] as FiltersModel);

            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Company");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Location");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Division");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Shift");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Department");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Shift");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Employee");
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
            {
                hidDateFilter.Value = "0";
            }

        }
        #endregion
        protected void GridViewShift_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveShiftIDs();

            //change page index
            GridViewShift.PageIndex = e.NewPageIndex;
            BindGridViewShift("");
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
        }

        protected void GridViewDomicile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDomicileIDs();

            //change page index
            GridViewDomicile.PageIndex = e.NewPageIndex;
            BindGridViewDomicile(tbSearchDomicile.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDomicile, Session["FiltersModel"] as FiltersModel, "Domicile");
        }

        private void SaveTypeIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            Session["FiltersModel"] = FM;
        }
        private void SaveShiftIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
            Session["FiltersModel"] = FM;
        }
        private void SaveDomicileIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDomicile, Session["FiltersModel"] as FiltersModel, "Domicile");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewType(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
            List<HR_EmpType> _View = new List<HR_EmpType>();
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForEmployeeTypeReport(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select * from HR_EmpType " + query);
            _View = dt.ToList<HR_EmpType>();
            GridViewType.DataSource = _View.Where(aa => aa.TypeName.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.TypeName).ToList();
            GridViewType.DataBind();
        }

        private void BindGridViewDomicile(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
            List<HR_Employee> _View = new List<HR_Employee>();
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForEmployeeTypeReport(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select distinct DomicileProvince from HR_Employee where isnull(DomicileProvince,'') <> ''");
            _View = dt.ToList<HR_Employee>();
            GridViewDomicile.DataSource = _View.Where(aa => aa.DomicileProvince.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.DomicileProvince).ToList();
            GridViewDomicile.DataBind();
        }

        private void BindGridViewShift(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<Att_Shift> _View = new List<Att_Shift>();
            QueryBuilder qb = new QueryBuilder();
            DataTable dt = qb.GetValuesfromDB("select * from Att_Shift ");
            _View = dt.ToList<Att_Shift>();
            GridViewShift.DataSource = _View.Where(aa => aa.ShiftName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewShift.DataBind();
        }


        protected void GridViewType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewType.PageIndex + 1) + " of " + GridViewType.PageCount;
            }
        }

        protected void GridViewShift_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewShift.PageIndex + 1) + " of " + GridViewShift.PageCount;
            }
        }

        protected void GridViewDomicile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDomicile.PageIndex + 1) + " of " + GridViewDomicile.PageCount;
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
            SaveShiftIDs();
            SaveTypeIDs();
            Response.Redirect(path);
        }

        protected void btnGenerateHRReport(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveShiftIDs();
            SaveTypeIDs();

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
        protected void btnStepOne_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/StepOne.aspx?dateFilter=true");
            }
            else
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
            SaveTypeIDs();
            SaveShiftIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                Response.Redirect("~/Reports/Filters/AttendanceReport.aspx?dateFilter=true");
            }
            else
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