using Microsoft.Reporting.WebForms;
using PTAWMS.Areas.Attendance.BusinessLogic;
using PTAWMS.Helper;
using PTAWMS.Models;
using PTAWMS.Reports.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSLibrary;

namespace PTAWMS.Reports.RptLoader
{
    public partial class AttReportsHome : System.Web.UI.Page
    {
        FiltersModel fm;
        String title = "";
        string _dateFrom = "";
        string reportpath = "";
        int PeriodStartID = 0;
        int PeriodEndID = 0;
        HRMEntities db = new HRMEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            String reportName = Request.QueryString["reportname"];
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            String type = Request.QueryString["type"];
            if (!Page.IsPostBack)
            {
                List<string> list = Session["ReportSession"] as List<string>;
                //dateFrom.Value = list[0];
                //dateTo.Value = list[1];
                fm = new FiltersModel();
                ViewUserEmp LoggedInUser = HttpContext.Current.Session["LoggedUser"] as ViewUserEmp;
                QueryBuilder qb = new QueryBuilder();
                // string query = qb.QueryForEmployeeReports(LoggedInUser);
                fm = Session["FiltersModel"] as FiltersModel;
                _dateFrom = list[0];
                string _dateTo = list[1];
                string PathString = "";
                Page.Server.ScriptTimeout = 3600;
                reportpath = "~/Reports";
                PeriodStartID = GetPayrollPeriodIDStart(Convert.ToDateTime(_dateFrom), db.PR_PayrollPeriod.ToList());
                PeriodEndID = GetPayrollPeriodIDEnd(Convert.ToDateTime(_dateTo), db.PR_PayrollPeriod.ToList());
                List<EmpView> emps = new List<EmpView>();
                string values = string.Empty;
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "H" && LoggedInUser.UserType != "E" && LoggedInUser.HRModule != true)
                {
                    fm = GetEmpFilterAttributeList(emps, LoggedInUser.EmployeeID, fm);
                }
                switch (reportName)
                {
                    #region ------Attendance Daily-------                    
                    case "emp_att":
                        DataTable dt3 = qb.GetValuesfromDB("select * from ViewAttData  where  (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )");
                        List<ViewAttData> _ViewAttDataDetailed2 = dt3.ToList<ViewAttData>();
                        List<ViewAttData> _TempViewAttDataDetailed2 = new List<ViewAttData>();
                        title = "Employee Attendance";
                        PathString = reportpath + "/RDLC/EmpAttSummary.rdlc";
                        //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed2, _ViewAttDataDetailed2), _dateFrom + " TO " + _dateTo);
                        break;
                    case "emp_att_chk":
                        DataTable dt4 = qb.GetValuesfromDB("select * from Att_DailyAttendance  where  (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )");
                        List<Att_DailyAttendance> _ViewAttDataDetailed4 = dt4.ToList<Att_DailyAttendance>();
                        List<Att_DailyAttendance> _TempViewAttDataDetailed4 = new List<Att_DailyAttendance>();
                        title = "Employee Attendance chrcking";
                        PathString = reportpath + "/RDLC/EmpAttSummaryCheck.rdlc";
                        //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed4, _ViewAttDataDetailed4), _dateFrom + " TO " + _dateTo);
                        break;
                    case "detailed_att":
                        DataTable dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )");
                        List<ViewAttDataDetail> _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        List<ViewAttDataDetail> _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Detailed Attendence";
                        PathString = reportpath + "/RDLC/DRdetailed.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "detailed_attPB":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Detailed Attendence";
                        PathString = reportpath + "/RDLC/DRdetailedPB.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "missing_attendance":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " ) and ((TimeIn is null and TimeOut is not null) or (TimeIn is not null and TimeOut is null) )");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Missing Attendance";
                        PathString = reportpath + "/RDLC/DRMissingAtt.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "present":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " ) and StatusP = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Present";
                        PathString = reportpath + "/RDLC/DRPresent.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "absent":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )and StatusAB = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Absent";
                        PathString = reportpath + "/RDLC/DRAbsent.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "early_in":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )and StatusEI = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Early In";
                        PathString = reportpath + "/RDLC/DREarlyIn.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "early_out":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where  (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )and StatusEO = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Early Out";
                        PathString = reportpath + "/RDLC/DREarlyOut.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "late_in":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where  (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )and StatusLI = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Late In";
                        PathString = reportpath + "/RDLC/DRLateIn.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "late_out":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where  (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )and StatusLO = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Late Out";
                        PathString = reportpath + "/RDLC/DRLateOut.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "multiple_in_out":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where   (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " ) and (Tin1 is not null or TOut1 is not null)");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Multiple In/Out";
                        PathString = reportpath + "/RDLC/DRMultipleInOut.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;
                    case "overtime":
                        dt2 = qb.GetValuesfromDB("select * from ViewAttDataDetail  where  (AttDate >= " + "'" + _dateFrom + "'" + " and AttDate <= " + "'" + _dateTo + "'" + " )and StatusOT = 1");
                        _ViewAttDataDetailed = dt2.ToList<ViewAttDataDetail>();
                        _TempViewAttDataDetailed = new List<ViewAttDataDetail>();
                        title = "Overtime";
                        PathString = reportpath + "/RDLC/DROverTime.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttDataDetailed, _ViewAttDataDetailed), _dateFrom + " TO " + _dateTo);
                        break;

                    #endregion
                    #region------Attendance Monthly--------
                    case "MonthlySummary_att":
                        string _period = Convert.ToDateTime(_dateFrom).Month.ToString() + Convert.ToDateTime(_dateFrom).Year.ToString();

                        dt2 = qb.GetValuesfromDB("select * from ViewAttMonthlySummary");
                        title = "Monthly Summary (1st to 31st)";
                        List<ViewAttMonthlySummary> _ViewAttMonthlySummary = dt2.ToList<ViewAttMonthlySummary>();
                        List<ViewAttMonthlySummary> _TempViewAttMonthlySummary = new List<ViewAttMonthlySummary>();
                        PathString = reportpath + "/RDLC/MRSummary.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttMonthlySummary, _ViewAttMonthlySummary), _dateFrom);
                        break;
                    #endregion
                    #region -- Overtime reports --
                    case "d_apc":
                        DataTable dt7 = qb.GetValuesfromDB("select * from ViewDailyOTEntry  where  (OTProcessingPeriodID >= " + "'" + PeriodStartID + "'" + " and OTProcessingPeriodID <= " + "'" + PeriodEndID + "'" + " ) and StatusID='A'");
                        List<ViewDailyOTEntry> _ViewDailyOTEntry = dt7.ToList<ViewDailyOTEntry>();
                        List<ViewDailyOTEntry> _TempViewDailyOTEntry = new List<ViewDailyOTEntry>();
                        List<PR_PayrollPeriod> periods = db.PR_PayrollPeriod.Where(aa => aa.PID >= PeriodStartID && aa.PID <= PeriodEndID).ToList();
                        string per = "";
                        foreach (var item in periods)
                        {
                            per = per + item.PName;
                        }
                        title = "Detailed Overtime Approved Claims for " + per;
                        PathString = reportpath + "/RDLC/EmpApprovedOT.rdlc";
                        //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewDailyOTEntry, _ViewDailyOTEntry), _dateFrom + " TO " + _dateTo);
                        break;
                    case "d_pending":
                        DataTable dt8 = qb.GetValuesfromDB("select * from ViewDailyOTEntry  where  (OTProcessingPeriodID >= " + "'" + PeriodStartID + "'" + " and OTProcessingPeriodID <= " + "'" + PeriodEndID + "'" + " ) and StatusID='P'");
                        List<ViewDailyOTEntry> _ViewDailyOTEntryPending = dt8.ToList<ViewDailyOTEntry>();
                        List<ViewDailyOTEntry> _TempViewDailyOTEntryPending = new List<ViewDailyOTEntry>();
                        List<PR_PayrollPeriod> period = db.PR_PayrollPeriod.Where(aa => aa.PID >= PeriodStartID && aa.PID <= PeriodEndID).ToList();
                        string pend = "";
                        foreach (var item in period)
                        {
                            per = pend + item.PName;
                        }
                        title = "Detailed Pending Overtime";
                        PathString = reportpath + "/RDLC/EmpPenndingOT.rdlc";
                        //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewDailyOTEntryPending, _ViewDailyOTEntryPending), _dateFrom + " TO " + _dateTo);
                        break;
                    case "d_all":
                        DataTable dt9 = qb.GetValuesfromDB("select * from ViewDailyOTEntry  where  (OTProcessingPeriodID >= " + "'" + PeriodStartID + "'" + " and OTProcessingPeriodID <= " + "'" + PeriodEndID + "'" + " )");
                        List<ViewDailyOTEntry> _ViewDailyOTEntryAll = dt9.ToList<ViewDailyOTEntry>();
                        List<ViewDailyOTEntry> _TempViewDailyOTEntryAll = new List<ViewDailyOTEntry>();
                        List<PR_PayrollPeriod> periodd = db.PR_PayrollPeriod.Where(aa => aa.PID >= PeriodStartID && aa.PID <= PeriodEndID).ToList();
                        string all = "";
                        foreach (var item in periodd)
                        {
                            per = all + item.PName;
                        }
                        title = "Detailed Overtime ";
                        PathString = reportpath + "/RDLC/EmpOT.rdlc";
                        //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewDailyOTEntryAll, _ViewDailyOTEntryAll), _dateFrom + " TO " + _dateTo);
                        break;
                    case "a_ocs":
                        DataTable dt = qb.GetValuesfromDB("select * from EmpView  where Status='Active'");
                        List<EmpView> _ViewPollData = dt.ToList<EmpView>();
                        List<EmpView> _TempViewPollData = new List<EmpView>();
                        title = "Summary Overtime Approved Claims";
                        PathString = reportpath + "/RDLC/MOTSummary.rdlc";
                        //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                        DateTime dtS = Convert.ToDateTime(_dateFrom);
                        DateTime dtE = Convert.ToDateTime(_dateTo);
                        _TempViewPollData = HRRptFilter.ReportsFilterImplementation(fm, _TempViewPollData, _ViewPollData);
                        List<ViewDailyOTEntry> otDatas = db.ViewDailyOTEntries.Where(aa => aa.OTProcessingPeriodID >= PeriodStartID && aa.OTProcessingPeriodID <= PeriodEndID && aa.StatusID == "A").ToList();
                        List<VMOTSummary> data = AttRptFilter.GetOTSummaryData(db.PR_PayrollPeriod.ToList(), otDatas, _dateFrom, _dateTo, _TempViewPollData);
                        LoadReportSummary(PathString, data, _dateFrom + " TO " + _dateTo);
                        break;
                    #endregion
                    #region-----Visitors Reports
                    case "dailyvisitor":
                        dt2 = qb.GetValuesfromDB("select * from ViewVisitEmp where  (VisitDate >= " + "'" + _dateFrom + "'" + " and VisitDate <= " + "'" + _dateTo + "'" + " )");
                        title = "Daily Visitor Details";
                        List<ViewVisitEmp> _ViewVisitorEmp = dt2.ToList<ViewVisitEmp>();
                        List<ViewVisitEmp> _TempViewVisitorEmp = new List<ViewVisitEmp>();
                        PathString = reportpath + "/RDLC/DailyVisitDetail.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewVisitorEmp, _ViewVisitorEmp), _dateFrom + " TO " + _dateTo);
                        break;
                    case "visitorsummary":
                        dt2 = qb.GetValuesfromDB("select * from ViewVisitEmp where  (VisitDate >= " + "'" + _dateFrom + "'" + " and VisitDate <= " + "'" + _dateTo + "'" + " )");
                        title = "Visitor Summary";
                        List<ViewVisitEmp> _ViewVisitorsummary = dt2.ToList<ViewVisitEmp>();
                        List<ViewVisitEmp> _TempViewVisitorsummary = new List<ViewVisitEmp>();
                        PathString = reportpath + "/RDLC/NewVisitEmps.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewVisitorsummary, _ViewVisitorsummary), _dateFrom + " TO " + _dateTo);
                        break;
                    case "empvisitorsummary":
                        dt2 = qb.GetValuesfromDB("select * from ViewVisitEmp where  (VisitDate >= " + "'" + _dateFrom + "'" + " and VisitDate <= " + "'" + _dateTo + "'" + " )");
                        title = "Employee Visitor Summary";
                        List<ViewVisitEmp> _ViewVisitorempsummary = dt2.ToList<ViewVisitEmp>();
                        List<ViewVisitEmp> _TempViewVisitorempsummary = new List<ViewVisitEmp>();
                        PathString = reportpath + "/RDLC/EmpVisitSummary.rdlc";
                        LoadReport(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewVisitorempsummary, _ViewVisitorempsummary), _dateFrom + " TO " + _dateTo);
                        break;
                    #endregion
                    #region HRMS Reports
                    //case "hrms_report":
                    //    List<ViewEmpQualification> _ViewHRMS = new List<ViewEmpQualification>();
                    //    _ViewHRMS = db.ViewEmpQualifications.ToList();
                    //    List<ViewEmpQualification> _TempViewHRMS = new List<ViewEmpQualification>();
                    //    title = "Employee Attendance";
                    //    PathString = reportpath + "/RDLC/HRMSReport.rdlc";
                    //    //Session["PayrollPeriodID"] = GetPayrollID(_dateFrom,_dateTo); 
                    //    LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewHRMS, _ViewHRMS), _dateFrom + " TO " + _dateTo);
                    //    break;

                    case "HRMS_Employee_Active":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active' and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                            values = "Joining Date Range: " + Convert.ToDateTime(_dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(_dateTo).ToString("dd/MM/yyyy");
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active'");//  and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        List<EmpView> _ViewAttHRMS = dt2.ToList<EmpView>();
                        List<EmpView> _TempViewAttHRMS = new List<EmpView>();
                        title = "Active Employee";
                        //PathString = reportpath + "/RDLC/HREmpDetails.rdlc";
                        PathString = "~/Reports/RDLC/HREmpDetails1.rdlc";

                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMS, _ViewAttHRMS), values);
                        break;
                    case "HRMS_PTA_Worker":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active' and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                            values = "Joining Date Range: " + Convert.ToDateTime(_dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(_dateTo).ToString("dd/MM/yyyy");
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active'");//  and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        List<EmpView> _ViewAttHRMS1 = dt2.ToList<EmpView>();
                        List<EmpView> _TempViewAttHRMS2 = new List<EmpView>();
                        title = "Active Employee";
                        //PathString = reportpath + "/RDLC/HREmpDetails.rdlc";
                        PathString = "~/Reports/RDLC/HRWorkerDetails.rdlc";

                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMS2, _ViewAttHRMS1), values);
                        break;
                    case "HRMS_Employee_Resigned":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Resigned' and (LeavingDate >= " + "'" + _dateFrom + "'" + " and LeavingDate <= " + "'" + _dateTo + "'" + " )");
                            values = "Leaving Date Range: " + Convert.ToDateTime(_dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(_dateTo).ToString("dd/MM/yyyy");
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Resigned'");//  where  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        List<EmpView> _ViewAttHRMSResigned = dt2.ToList<EmpView>();
                        List<EmpView> _TempViewAttHRMSResigned = new List<EmpView>();
                        title = "Resigned Employee";
                        //PathString = reportpath + "/RDLC/HRResignedEmpDetails.rdlc";
                        PathString = "~/Reports/RDLC/HRResignedEmpDetails.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSResigned, _ViewAttHRMSResigned), values);
                        break;
                    case "HRMS_EmpAddress_Active":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active'  and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                            values = "Joining Date Range: " + Convert.ToDateTime(_dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(_dateTo).ToString("dd/MM/yyyy");
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active'");//  where  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        List<EmpView> _ViewAttHRMSADD = dt2.ToList<EmpView>();
                        List<EmpView> _TempViewAttHRMSAdd = new List<EmpView>();
                        title = "Active Employee Address";
                        //PathString = reportpath + "/RDLC/HRActiveEmpAdd.rdlc";
                        PathString = "~/Reports/RDLC/HRActiveEmpAdd.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSAdd, _ViewAttHRMSADD), values);
                        break;
                    case "HRMS_EmpAddress_Resigned":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Resigned' and  (LeavingDate >= " + "'" + _dateFrom + "'" + " and LeavingDate <= " + "'" + _dateTo + "'" + " )");
                            values = "Leaving Date Range: " + Convert.ToDateTime(_dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(_dateTo).ToString("dd/MM/yyyy");
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Resigned'");//  where  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");

                        _ViewAttHRMSResigned = dt2.ToList<EmpView>();
                        _TempViewAttHRMSResigned = new List<EmpView>();
                        title = "Resigned Employee Address";
                        //PathString = reportpath + "/RDLC/HRResignedEmpAdd.rdlc";
                        PathString = "~/Reports/RDLC/HRResignedEmpAdd.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSResigned, _ViewAttHRMSResigned), values);
                        break;
                    case "HRMS_PTA_EMP":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active' and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                            values = "Joining Date Range: " + Convert.ToDateTime(_dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(_dateTo).ToString("dd/MM/yyyy");
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active'");//  where  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        _ViewAttHRMSResigned = dt2.ToList<EmpView>();
                        _TempViewAttHRMSResigned = new List<EmpView>();
                        title = "PTA Employee List";
                        //PathString = reportpath + "/RDLC/HRResignedEmpAdd.rdlc";
                        PathString = "~/Reports/RDLC/HRPTAEmp.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSResigned, _ViewAttHRMSResigned), values);
                        break;
                    case "HRMS_PTA_Qualif":
                        values = string.Empty;
                        if (dateFilter == "true")
                        {
                            dt2 = qb.GetValuesfromDB("select * from ViewReportQualification where Status like 'Active' and (EndSession >= " + Convert.ToDateTime(_dateFrom).Year + " and EndSession <= " + Convert.ToDateTime(_dateTo).Year + " )");
                            values = "End Session Range: " + Convert.ToDateTime(_dateFrom).Year + " To " + Convert.ToDateTime(_dateTo).Year;
                        }
                        else
                            dt2 = qb.GetValuesfromDB("select * from ViewReportQualification where Status like 'Active'");//  where  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");

                        List<ViewReportQualification> _ViewAttHRMSQualif = dt2.ToList<ViewReportQualification>();
                        List<ViewReportQualification> _TempViewAttHRMSQualif = new List<ViewReportQualification>();
                        title = "EmployeeQualification";
                        //PathString = reportpath + "/RDLC/HREmpQualification.rdlc";
                        PathString = "~/Reports/RDLC/HREmpQualification.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSQualif, _ViewAttHRMSQualif), values);
                        break;
                    case "HRMS_PTA_Hist":
                        dt2 = qb.GetValuesfromDB("select * from ViewEmpHistory where isnull([DependentDeleted],0) = 0 and isnull([ExpDeleted],0) = 0 and isnull([TrainingDeleted],0) = 0 and isnull([WarningDeleted],0) = 0 and isnull([AppreciationDeleted],0) = 0 ");

                        List<ViewEmpHistory> _ViewAttHRMSHist = dt2.ToList<ViewEmpHistory>();
                        List<ViewEmpHistory> _TempViewAttHRMSHist = new List<ViewEmpHistory>();
                        title = "EmployeeHistory";
                        PathString = "~/Reports/RDLC/HREmpHistory.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSHist, _ViewAttHRMSHist), _dateFrom + " TO " + _dateTo);
                        break;
                    case "HRMS_PTA_DeptAgg":
                        if (dateFilter == "true")
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active' and  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        else
                            dt2 = qb.GetValuesfromDB("select * from EmpView where Status like 'Active'");//  where  (DOJ >= " + "'" + _dateFrom + "'" + " and DOJ <= " + "'" + _dateTo + "'" + " )");
                        _ViewAttHRMSResigned = dt2.ToList<EmpView>();
                        _TempViewAttHRMSResigned = new List<EmpView>();
                        title = "PTA Employee Designation Summary";
                        //PathString = reportpath + "/RDLC/HRResignedEmpAdd.rdlc";
                        PathString = "~/Reports/RDLC/HREmpDesigAgg.rdlc";
                        LoadReportSummary(PathString, AttRptFilter.ReportsFilterImplementation(fm, _TempViewAttHRMSResigned, _ViewAttHRMSResigned), _dateFrom + " TO " + _dateTo);
                        break;


                        #endregion
                }
            }
        }
        private FiltersModel GetEmpFilterAttributeList(List<EmpView> emps, int? EmpID, FiltersModel fm)
        {
            List<FiltersAttributes> filterList = new List<FiltersAttributes>();
            if (fm.CMDesignationFilter.Count == 0 && fm.CompanyFilter.Count == 0 && fm.DepartmentFilter.Count == 0 && fm.EmployeeFilter.Count == 0 && fm.LocationFilter.Count == 0 && fm.ReaderFilter.Count == 0 && fm.SectionFilter.Count == 0 && fm.ShiftFilter.Count == 0 && fm.TypeFilter.Count == 0)
            {
                emps = emps.Where(aa => aa.EmployeeID == EmpID).ToList();
                foreach (var emp in emps)
                {
                    FiltersAttributes fa = new FiltersAttributes();
                    fa.ID = emp.EmployeeID.ToString();
                    fa.FilterName = emp.EmployeeID.ToString();
                    filterList.Add(fa);
                }
                fm.EmployeeFilter = filterList;
            }
            
            return fm;
        }
        private void LoadReportSummary(string path, List<ViewEmpQualification> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewEmpQualification> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            //ReportParameter rp = new ReportParameter("Date", date, false);
            //ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }
        private void LoadReportSummary(string path, List<ViewReportQualification> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewReportQualification> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            //ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("RP1", date, false);
            this.ReportViewer1.LocalReport.SetParameters(rp1);
            ReportViewer1.LocalReport.Refresh();
        }
        private void LoadReport(string path, List<ViewVisitEmp> _Employee, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewVisitEmp> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }
        private void LoadReportSummary(string path, List<ViewEmpHistory> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewEmpHistory> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            //ReportParameter rp = new ReportParameter("Date", date, false);
            //ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }


        private int GetPayrollPeriodIDStart(DateTime dateFrom, List<PR_PayrollPeriod> list)
        {
            foreach (var item in list)
            {
                if (dateFrom == item.PStartDate)
                    return item.PID;
            }
            return 0;
        }
        private int GetPayrollPeriodIDEnd(DateTime dateFrom, List<PR_PayrollPeriod> list)
        {
            foreach (var item in list)
            {
                if (dateFrom == item.PEndDate)
                    return item.PID;
            }
            return 0;
        }
        private void LoadReportSummary(string PathString, List<VMOTSummary> data, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(PathString);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<VMOTSummary> ie;
            ie = data.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }

        private void LoadReportSummary(string PathString, List<ViewDailyOTEntry> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(PathString);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewDailyOTEntry> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }

        private void LoadReportSummary(string PathString, List<Att_DailyAttendance> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(PathString);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<Att_DailyAttendance> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }

        private void LoadReportSummary(string PathString, List<ViewAttDataDetail> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(PathString);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewAttDataDetail> ie;
            ie = list.AsQueryable();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }

        private void LoadReportSummary(string path, List<EmpView> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<EmpView> ie;
            ie = list.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            //ReportParameter rp = new ReportParameter("Date", DateTime.Now.ToShortDateString(), false);
            ReportParameter rp1 = new ReportParameter("RP1", date, false);
            this.ReportViewer1.LocalReport.SetParameters(rp1);
            ReportViewer1.LocalReport.Refresh();
        }

        private void DownloadReport(List<ViewAttData> list, string p, int UserID)
        {
            string val = "";
            val = DownloadData(list, p, UserID);
            if (val != "")
            {
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + val);
                Response.WriteFile(val);
                Response.End();
            }
        }

        public string DownloadData(List<ViewAttData> attData, string Date, int UserID)
        {
            //HRMEntities db = new HRMEntities();
            //string retVal = "";
            ////lvPDF is nothing but the listview control name
            //string[] st = new string[5];
            //DirectoryInfo di = new DirectoryInfo(@"D:\");
            //if (di.Exists == false)
            //    di.Create();
            //StreamWriter sw = new StreamWriter(@"D:\" + UserID + ".xls", false);
            //sw.AutoFlush = true;
            //List<HR_Employee> emps = new List<HR_Employee>();
            //emps = db.HR_Employee.Where(aa => aa.Status == "Active").ToList();
            //sw.WriteLine("Employee Attendance Report");
            //sw.WriteLine(Date + "\n");
            //foreach (var emp in emps)
            //{
            //    if (attData.Where(aa => aa.EmpID == emp.EmployeeID).Count() > 0)
            //    {
            //        sw.WriteLine("Emp No \t\t" + emp.EmpNo + " \t\t\t" + "Emp Type" + "\t\t\t" + emp.HR_Location.LocationName);
            //        sw.WriteLine("Name \t\t" + emp.FullName + " \t\t\t " + "Section" + "\t\t\t" + emp.HR_Section.SectionName);
            //        sw.WriteLine("Designation \t\t" + emp.HR_Designation.DesignationName + " \t\t\t " + "Department" + "\t\t\t" + emp.HR_Section.HR_Department.DepartmentName);
            //        sw.WriteLine("Att Date" + "\t Duty Time" + "\t shift" + "\t Time In" + "\t Time Out" + "\t Work" + "\t L.I" + "\t L.O" + "\t E.I" + "\t E.O" + "\t O.T" + "\t S.H" + "\t Remarks");
            //        List<ViewAttData> att = new List<ViewAttData>();
            //        att = attData.Where(aa => aa.EmpID == emp.EmployeeID).ToList();
            //        foreach (var at in att)
            //        {
            //            List<string> values = new List<string>();
            //            values.Add(at.AttDate.Value.ToString("dd-MMM-yyyy"));
            //            values.Add(at.DutyTime.Value.Hours.ToString("00") + ":" + at.DutyTime.Value.Minutes.ToString("00"));
            //            values.Add(ConverToHours(at.ShifMin));
            //            values.Add(ConverToTime(at.TimeIn));
            //            values.Add(ConverToTime(at.TimeOut));
            //            values.Add(ConverToHours(at.WorkMin));
            //            values.Add(ConverToHours(at.LateIn));
            //            values.Add(ConverToHours(at.LateOut));
            //            values.Add(ConverToHours(at.EarlyIn));
            //            values.Add(ConverToHours(at.EarlyOut));
            //            values.Add(ConverToHours(at.TotalShortMin));
            //            if (at.Remarks == null)
            //                values.Add(" ");
            //            else
            //                values.Add(at.Remarks.ToString());
            //            string val = "";
            //            for (int i = 0; i < values.Count; i++)
            //            {
            //                val = val + values[i] + "\t";
            //            }
            //            sw.WriteLine(val);
            //        }
            //        string Period = "";
            //        int ActualOT = 0;
            //        float TotalDays = 0;
            //        float PaidDays = 0;
            //        float PreDays = 0;
            //        float AbDays = 0;
            //        float LeaveDays = 0;
            //        int GZDays = 0;
            //        int GZPresent = 0;
            //        int RestDays = 0;
            //        int RestPresent = 0;
            //        if (att.Count > 0)
            //            Period = att[0].AttDate.Value.Date.Month.ToString() + att[0].AttDate.Value.Date.Year.ToString("0000");
            //        if (db.Att_MonthData.Where(aa => aa.Period == Period && aa.EmpID == emp.EmployeeID).Count() > 0)
            //        {
            //            Att_MonthData attMnData = db.Att_MonthData.First(aa => aa.Period == Period && aa.EmpID == emp.EmployeeID);
            //            if (attMnData.TotalDays != null)
            //                TotalDays = (float)attMnData.TotalDays;
            //            if (attMnData.WorkDays != null)
            //                PaidDays = (float)(attMnData.WorkDays);
            //            if (attMnData.PreDays != null)
            //                PreDays = (float)(attMnData.PreDays);
            //            if (attMnData.AbDays != null)
            //                AbDays = (float)(attMnData.AbDays);
            //            if (attMnData.LeaveDays != null)
            //                LeaveDays = (float)(attMnData.LeaveDays);
            //            if (attMnData.GZDays != null)
            //                GZDays = (int)attMnData.GZDays;
            //            if (attMnData.GZPresentDays != null)
            //                GZPresent = (int)attMnData.GZPresentDays;
            //            if (attMnData.RestDays != null)
            //                RestDays = (int)attMnData.RestDays;
            //            if (attMnData.RestPresentDays != null)
            //                RestPresent = (int)attMnData.RestPresentDays;
            //        }
            //        sw.WriteLine("Actual OT\t\t" + ActualOT);
            //        sw.WriteLine("Total Days\t\t" + TotalDays + "\tPresent Days\t\t" + PreDays + "\tGZ Days\t\t" + GZDays + "\tLeaves Days\t\t" + LeaveDays);
            //        sw.WriteLine("Paid Days\t\t" + PaidDays + "\tRest Days\t\t" + RestDays + "\tGZ Present\t\t" + GZPresent + "\tLateIn Days\t\t" + att.Where(aa => aa.LateIn > 0).Count().ToString());
            //        sw.WriteLine("Absent Days\t\t" + AbDays + "\tRest Present\t\t" + RestPresent + "\tLate Sitting\t\t" + att.Where(aa => aa.LateOut > 0).Count().ToString() + "\tEarlyOut Days\t\t" + att.Where(aa => aa.EarlyOut > 0).Count().ToString());

            //    }

            //}

            //sw.Close();
            //FileInfo fil = new FileInfo(@"D:\" + UserID.ToString() + ".xls");
            //if (fil.Exists == true)
            //{
            //    retVal = "D:\\" + UserID.ToString() + ".xls";
            //}
            return "";
        }
        private string ConverToTime(DateTime? time)
        {
            string ret = "";
            if (time != null)
            {
                ret = time.Value.TimeOfDay.Hours.ToString("00") + ":" + time.Value.TimeOfDay.Minutes.ToString("00");
            }
            return ret;
        }

        private string ConverToHours(short? val)
        {
            string ret = "";
            if (val != null && val > 0)
            {
                TimeSpan ts = new TimeSpan(0, (short)val, 0);
                ret = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00");
            }
            return ret;
        }
        //Load Device_Data report();
        private void LoadReport(string PathString, List<ViewPollData> list, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(PathString);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewPollData> ie;
            ie = list.AsQueryable();

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);


            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }

        // Load leave_att report       
        // Load detailed_att report
        private void LoadReport(string path, List<ViewAttDataDetail> _Employee, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewAttDataDetail> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }
        private void LoadReport(string path, List<ViewAttData> _Employee, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewAttData> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }
        private void LoadReportSummary(string path, List<ViewAttData> _Employee, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewAttData> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }
        private void LoadReport(string path, List<ViewAttMonthlySummary> _Employee, string date)
        {
            string _Header = title;
            this.ReportViewer1.LocalReport.DisplayName = title;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewAttMonthlySummary> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Add(datasource1);

            ReportParameter rp = new ReportParameter("Date", date, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }

        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            Response.Redirect(path);
        }
        protected void btnGenerateHRReport(object sender, EventArgs e)
        {
            //if (MyHelper.UserHasValuesInSession(fm))
            //{
            Response.Redirect("~/Reports/RptLoader/AttReportsHome.aspx?reportname=HRMS_PTA_Worker");
            //}
        }
        protected void btnStepOne_Click(object sender, EventArgs e)
        {
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
            {
                NavigationCommonCalls("~/Reports/Filters/StepOne.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepOne.aspx");
        }

        protected void btnStepTwo_Click(object sender, EventArgs e)
        {
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
            {
                NavigationCommonCalls("~/Reports/Filters/StepTwo.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepTwo.aspx");
        }

        protected void btnStepThree_Click(object sender, EventArgs e)
        {
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
            {
                NavigationCommonCalls("~/Reports/Filters/StepThree.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepThree.aspx");
        }

        protected void btnStepFour_Click(object sender, EventArgs e)
        {
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
            {
                NavigationCommonCalls("~/Reports/Filters/StepFour.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/StepFour.aspx");
        }

        protected void btnStepFive_Click(object sender, EventArgs e)
        {
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
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
        protected void btnStepSix_Click(object sender, EventArgs e)
        {
            //{
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
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
        protected void btnHRQStep_Click(object sender, EventArgs e)
        {
            String dateFilter = string.IsNullOrEmpty(Request.QueryString["dateFilter"]) ? "" : Request.QueryString["dateFilter"];
            if (Convert.ToString(Session["ReportMenu"]) == "HRMS" && dateFilter == "true")
            {
                NavigationCommonCalls("~/Reports/Filters/HRQStep.aspx?dateFilter=true");
            }
            else
                NavigationCommonCalls("~/Reports/Filters/HRQStep.aspx");
        }
        #endregion
        //Load Summary Report

    }
}