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
    public partial class StepFour : System.Web.UI.Page
    {
        private HRMEntities da = new HRMEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewEmployee("");
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
                SaveEmployeeIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
            }
        }
        protected void ButtonSearchEmployee_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveEmployeeIDs();
            BindGridViewEmployee(tbSearch_Employe1.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
        }
        protected void GridViewEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveEmployeeIDs();

            //change page index
            GridViewEmployee.PageIndex = e.NewPageIndex;
            BindGridViewEmployee(tbSearch_Employe1.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
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
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
            {
                hidDateFilter.Value = "0";
            }

        }
        #endregion
        private void SaveEmployeeIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewEmployee(string search)
        {
            using (var db =new HRMEntities())
            {
                FiltersModel fm = Session["FiltersModel"] as FiltersModel;
                List<EmpView> _View = new List<EmpView>();
                List<EmpView> _TempView = new List<EmpView>();
                ViewUserEmp  LoggedInUser= HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
                if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "H" || LoggedInUser.UserType == "E" || LoggedInUser.HRModule == true)
                {
                    _View = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
                }
                else
                {
                    List<EmpView> emps = new List<EmpView>();
                    
                    emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                    _View = emps;
                }
                if (fm.DepartmentFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var comp in fm.DepartmentFilter)
                    {
                        short _compID = Convert.ToInt16(comp.ID);
                        _TempView.AddRange(_View.Where(aa => aa.DeptID == _compID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                if (fm.SectionFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var comp in fm.SectionFilter)
                    {
                        short _compID = Convert.ToInt16(comp.ID);
                        _TempView.AddRange(_View.Where(aa => aa.SecID == _compID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                if (fm.TypeFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var comp in fm.TypeFilter)
                    {
                        short _compID = Convert.ToInt16(comp.ID);
                        _TempView.AddRange(_View.Where(aa => aa.TypID == _compID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                if (fm.CMDesignationFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var comp in fm.CMDesignationFilter)
                    {
                        string _compID = comp.ID;
                        _TempView.AddRange(_View.Where(aa => aa.OCommonName == _compID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                if (fm.LocationFilter.Count > 0)
                {
                    _TempView.Clear();
                    foreach (var comp in fm.LocationFilter)
                    {
                        short _compID = Convert.ToInt16(comp.ID);
                        _TempView.AddRange(_View.Where(aa => aa.LocID == _compID).ToList());
                    }
                    _View = _TempView.ToList();
                }
                //if (fm.CMDesignationFilter.Count > 0)
                //{
                //    _TempView.Clear();
                //    foreach (var comp in fm.CMDesignationFilter)
                //    {
                //        short _compID = Convert.ToInt16(comp.ID);
                //        _TempView.AddRange(_View.Where(aa => aa. == _compID).ToList());
                //    }
                //    _View = _TempView.ToList();
                //}
                List<EmpView> empv = new List<EmpView>();
                empv = _View.Where(aa => aa.FullName.ToUpper().Contains(search.ToUpper()) || aa.EmpNo.ToUpper().Contains(search.ToUpper())).OrderBy(aa => aa.FullName).ToList();
                GridViewEmployee.DataSource = empv;
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
            SaveEmployeeIDs();
            Response.Redirect(path);
        }
        protected void btnGenerateHRReport(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveEmployeeIDs();

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
            SaveEmployeeIDs();
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