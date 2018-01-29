using PTAWMS.Helper;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSLibrary;

namespace PTAWMS.Reports.Filters
{
    public partial class HRQStep : System.Web.UI.Page
    {
        private HRMEntities da = new HRMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewDegree("");
                BindGridViewInstitute("");
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
                BindGridViewDegree();
                SaveInstituteIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Degree");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewInstitute, Session["FiltersModel"] as FiltersModel, "Institute");
            }
        }
        protected void ButtonSearchInstitute_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveInstituteIDs();
            BindGridViewInstitute(tbSearchInstitute.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewInstitute, Session["FiltersModel"] as FiltersModel, "Institute");
        }
        protected void ButtonSearchDegree_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            BindGridViewDegree();
            BindGridViewDegree(txtSearchDegree.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Degree");
        }
        protected void GridViewDegree_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            BindGridViewDegree();

            //change page index
            GridViewDegree.PageIndex = e.NewPageIndex;
            BindGridViewDegree(txtSearchDegree.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Degree");
        }

        protected void GridViewInstitute_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveInstituteIDs();

            //change page index
            GridViewInstitute.PageIndex = e.NewPageIndex;
            BindGridViewInstitute(tbSearchInstitute.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewInstitute, Session["FiltersModel"] as FiltersModel, "Institute");
        }
        private void BindGridViewDegree()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Degree");
            Session["FiltersModel"] = FM;
        }
        private void SaveInstituteIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewInstitute, Session["FiltersModel"] as FiltersModel, "Institute");
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

            //WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Company");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewCity, Session["FiltersModel"] as FiltersModel, "Location");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Type");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewInstitute, Session["FiltersModel"] as FiltersModel, "Institue");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDegree, Session["FiltersModel"] as FiltersModel, "Degree");
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
        private void BindGridViewDegree(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<HR_Emp_Qualification> _View = new List<HR_Emp_Qualification>();
            List<HR_Emp_Qualification> _TempView = new List<HR_Emp_Qualification>();
            ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForReportsDepartment(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select distinct Degree from HR_Emp_Qualification where isnull(Deleted,0)=0 and isnull(Degree,'') <> ''");
            _View = dt.ToList<HR_Emp_Qualification>();
            GridViewDegree.DataSource = _View.Where(aa => aa.Degree.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.Degree).ToList();
            GridViewDegree.DataBind();
        }

        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }

        private void BindGridViewInstitute(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<HR_Emp_Qualification> _View = new List<HR_Emp_Qualification>();
            List<HR_Emp_Qualification> _TempView = new List<HR_Emp_Qualification>();
            ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForLocReport(LoggedInUser);
            DataTable dt = new DataTable();
            if (query != "where")
                dt = qb.GetValuesfromDB("select distinct Institute from HR_Emp_Qualification where isnull(Deleted,0)=0 and isnull(Institute,'') <> ''");
            _View = dt.ToList<HR_Emp_Qualification>();
            GridViewInstitute.DataSource = _View.Where(aa => aa.Institute.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.Institute).ToList();
            GridViewInstitute.DataBind();
        }


        protected void GridViewDegree_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDegree.PageIndex + 1) + " of " + GridViewDegree.PageCount;
            }
        }

        protected void GridViewInstitute_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewInstitute.PageIndex + 1) + " of " + GridViewInstitute.PageCount;
            }
        }

        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveInstituteIDs();

            Response.Redirect(path);
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
            SaveInstituteIDs();

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
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            Response.Redirect("~/Reports/Filters/StepOnePre.aspx");
            //}
        }
        protected void btnGenerateHRReport(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveInstituteIDs();

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

        #endregion
        [WebMethod(EnableSession = true)]
        public static string DeleteSingleFilter(string id, string parentid)
        {
            FiltersModel fml = new FiltersModel();
            fml = HttpContext.Current.Session["FiltersModel"] as FiltersModel;
            fml = WMSLibrary.Filters.DeleteSingleFilter(fml, id, parentid);
            return DateTime.Now.ToString();
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveReportSession(string id)
        {
            HttpContext.Current.Session["ReportMenu"] = id;
            return "OK";
        }
    }
}