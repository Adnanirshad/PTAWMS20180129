using PTAWMS.Areas.Attendance.BusinessLogic;
using PTAWMS.Helper;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSLibrary;

namespace PTAWMS.Reports.Filters
{
    public partial class StepThree : System.Web.UI.Page
    {
        private HRMEntities da = new HRMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewSection("");
                BindGridViewDesignation("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
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
                SaveSectionIDs();
                SaveDesignationIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDesignation, Session["FiltersModel"] as FiltersModel, "Designation");
            }
        }
        protected void ButtonSearchSection_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            BindGridViewSection(tbSearch_Section.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
        }
        protected void ButtonSearchDesignation_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDesignationIDs();
            BindGridViewDesignation(tbSearch_Designation.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDesignation, Session["FiltersModel"] as FiltersModel, "Designation");
        }
        protected void GridViewSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();

            //change page index
            GridViewSection.PageIndex = e.NewPageIndex;
            BindGridViewSection(tbSearch_Section.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
        }
        protected void GridViewDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDesignationIDs();

            //change page index
            GridViewDesignation.PageIndex = e.NewPageIndex;
            BindGridViewDesignation(tbSearch_Designation.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDesignation, Session["FiltersModel"] as FiltersModel, "Designation");
        }
        private void SaveSectionIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
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

            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Company");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Location");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Division");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDesignation, Session["FiltersModel"] as FiltersModel, "Designation");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Department");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Type");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Crew");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Employee");

            if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
            {
                hidDateFilter.Value = "0";
            }
        }
        #endregion
        private void SaveDesignationIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDesignation, Session["FiltersModel"] as FiltersModel, "Designation");
            Session["FiltersModel"] = FM;
        }
        private void BindGridViewSection(string search)
        {
            using (var db = new HRMEntities())
            {
                FiltersModel fm = Session["FiltersModel"] as FiltersModel;
                List<ViewHRSection> _View = new List<ViewHRSection>();
                List<ViewHRSection> _TempView = new List<ViewHRSection>();
                ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
                List<ViewHRSection> secList = new List<ViewHRSection>();
                secList = db.ViewHRSections.Where(aa => aa.Status == true).ToList();
                if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "H" || LoggedInUser.UserType == "E" || LoggedInUser.HRModule == true)
                {
                    _View = secList.ToList();
                }
                else
                {
                    List<EmpView> emps = new List<EmpView>();
                    emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                    List<short?> secids = emps.Select(aa => aa.SectionID).Distinct().ToList();
                    List<ViewHRSection> tempSecs = new List<ViewHRSection>();
                    foreach (var secid in secids)
                    {
                        tempSecs.AddRange(secList.Where(aa => aa.SecID == secid).ToList());
                    }
                    _View = tempSecs.ToList();
                }
                
                if (fm.DepartmentFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var dept in fm.DepartmentFilter)
                    {
                        short _deptID = Convert.ToInt16(dept.ID);
                        _TempView.AddRange(_View.Where(aa => aa.DeptID == _deptID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                if (fm.LocationFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var dept in fm.LocationFilter)
                    {
                        short _deptID = Convert.ToInt16(dept.ID);
                        _TempView.AddRange(_View.Where(aa => aa.LocID == _deptID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                GridViewSection.DataSource = _View.Where(aa => aa.SectionName.ToUpper().Contains(search.ToUpper())).OrderBy(aa=>aa.SectionName).ToList();
                GridViewSection.DataBind();
            }
        }       
        private void BindGridViewDesignation(string search)
        {
            using (var db = new HRMEntities())
            {
                FiltersModel fm = Session["FiltersModel"] as FiltersModel;

                ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
                List<HR_Designation> desigList = new List<HR_Designation>();
                desigList = db.HR_Designation.ToList();
                List<HR_Designation> tempList = new List<HR_Designation>();
                if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "H" || LoggedInUser.UserType == "E" || LoggedInUser.HRModule == true)
                {
                    tempList = desigList.ToList();
                }
                else
                {
                    List<EmpView> emps = new List<EmpView>();
                    emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                    List<short?> desigIds = emps.Select(aa => aa.DesgID).Distinct().ToList();
                    foreach (var secid in desigIds)
                    {
                        tempList.AddRange(desigList.Where(aa => aa.DesgID == secid).ToList());
                    }
                }
                List<DesignationCommonModel> desigCommonList = new List<DesignationCommonModel>();

                foreach (var desig in tempList.Select(aa => aa.OCommonName).Distinct().ToList())
                {
                    if (desig != null && desig != "")
                    {
                        DesignationCommonModel dc = new DesignationCommonModel();
                        dc.DesigID = desig;
                        dc.DesigName = desig;
                        desigCommonList.Add(dc);
                    }
                }
                GridViewDesignation.DataSource = desigCommonList.Where(aa => aa.DesigName.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.DesigName).ToList();
                GridViewDesignation.DataBind();
            }
        }
        #region Navigation Buttons
        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            SaveDateSession();
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            SaveDesignationIDs();
            // Go to the next page
            string url = "~/Reports/Filters/StepFourFilter.aspx";
            Response.Redirect(url);
        }
        protected void ButtonSkip_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            // Go to the next page
            string url = "~/Filters/DeptFilter.aspx";
            Response.Redirect(url);
        }
        protected void ButtonFinish_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            SaveDesignationIDs();
            // Go to the next page
            string url = "~/Reports/ReportContainer.aspx";
            Response.Redirect(url);
        }
        #endregion
        protected void GridViewSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewSection.PageIndex + 1) + " of " + GridViewSection.PageCount;
            }
        }
        protected void GridViewDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDesignation.PageIndex + 1) + " of " + GridViewDesignation.PageCount;
            }
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
        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveSectionIDs();
            SaveDesignationIDs();
            Response.Redirect(path);
        }
        protected void btnGenerateHRReport(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveSectionIDs();
            SaveDesignationIDs();

            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && hidDateFilter.Value == "1")
            {
                NavigationCommonCalls("~/Reports/Filters/AttReportsHome.aspx?reportname=HRMS_PTA_Worker&dateFilter=true");
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
            SaveSectionIDs();
            SaveDesignationIDs();
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
    public class DesignationCommonModel
    {
        public string DesigID{ get; set; }
        public string DesigName { get; set; }
    }
}