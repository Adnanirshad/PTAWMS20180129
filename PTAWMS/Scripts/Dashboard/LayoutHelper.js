function LoadReports(obj) {
    var urls = LoadUrl('Reports/Filters/StepTwo.aspx/SaveReportSession');
    $.ajax({
        type: "POST",
        url: urls,
        data: "{ id: '" + obj + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            urls = LoadUrl('Reports/Filters/AttendanceReport.aspx');
            window.location.href = urls;
        },
        error: function (response) {
            alert("error");
            alert(response);
        },
        failure: function (response) {
            alert("failure");
            alert(response);
        }
    });
}
function LoadMenuState(selectedMenu) {
    $("nav nav-list menu_section .nav").find(".active").removeClass("active");
    $("nav nav-list menu_section .nav").find(".open").removeClass("open");


    if (selectedMenu == "Home") {
        $("#liDashboard").addClass('active')
    }
    else if (selectedMenu == "AttendenceRpt") {
        $("#liAttendenceRpt").addClass('active')
        $("#liReports").addClass('open')
        //$("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRMSRpt") {
        $("#liHRMSRpt").addClass('active')
        $("#liReports").addClass('open')
        //$("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRPreJobHistory") {
        $("#liHRPreJobHistory").addClass('active')
        $("#liPendingRequest").addClass('open')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRAppreciation") {
        $("#liHRAppreciation").addClass('active')
        $("#liPendingRequest").addClass('open')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRWarning") {
        $("#liHRWarning").addClass('active')
        $("#liPendingRequest").addClass('open')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRTraining") {
        $("#liHRTraining").addClass('active')
        $("#liPendingRequest").addClass('open')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRQualifications") {
        $("#liHRQualification").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRDegree") {
        $("#liHRDegree").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "HRInstitute") {
        $("#liHRInstitute").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "Employee") {
        $("#liEmployee").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "Department") {
        $("#liDepartment").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "Designation") {
        $("#liDesignation").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "EmpType") {
        $("#liEmpType").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "Location") {
        $("#liLocation").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "Section") {
        $("#liSection").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "User") {
        $("#liUser").addClass('active')
        $("#liMCompany").addClass('open')
    }
    else if (selectedMenu == "UserTypes") {
        $("#liUserRole").addClass('active')
        $("#liMCompany").addClass('open')
    }// Attendance
    else if (selectedMenu == "Shift") {
        $("#liShift").addClass('active')
        $("#liMAttendance").addClass('open')
    }
    else if (selectedMenu == "Holiday") {
        $("#liHoliday").addClass('active')
        $("#liMAttendance").addClass('open')
    }
    else if (selectedMenu == "AttManual") {
        $("#liAttManual").addClass('active')
        $("#liMAttendance").addClass('open')
    }
    else if (selectedMenu == "ProcessRequest") {
        $("#liProcessRequest").addClass('active')
        $("#liMAttendance").addClass('open')
    }
    else if (selectedMenu == "Reader") {
        $("#liReader").addClass('active')
        $("#liMAttendance").addClass('open')
    }
    else if (selectedMenu == "Settings") {
        $("#liSettings").addClass('active')
        $("#liMAttendance").addClass('open')
    }// OT Settings
    else if (selectedMenu == "OTPolicy") {
        $("#liOTPolicy").addClass('active')
        $("#liMOTSetting").addClass('open')
    }
    else if (selectedMenu == "PRPeriod") {
        $("#liPRPeriod").addClass('active')
        $("#liMOTSetting").addClass('open')
    }
    else if (selectedMenu == "OTDiv") {
        $("#liOTDiv").addClass('active')
        $("#liMOTSetting").addClass('open')
    }
    else if (selectedMenu == "OTCredit") {
        $("#liOTCredit").addClass('active')
        $("#liMOTSetting").addClass('open')
    }
    else if (selectedMenu == "OTDebit") {
        $("#liOTDebit").addClass('active')
        $("#liMOTSetting").addClass('open')
    }// Job cards
    else if (selectedMenu == "EmployeeJobCard") {
        $("#liEmpJobCard").addClass('active')
        $("#liMJobCard").addClass('open')
    }
    else if (selectedMenu == "JobCard") {
        $("#liNewJobCard").addClass('active')
        $("#liMJobCard").addClass('open')
    }
    else if (selectedMenu == "PendingJobCards") {
        $("#liPendingJobCard").addClass('active')
        $("#liMJobCard").addClass('open')
    }
    else if (selectedMenu == "JobCardHistory") {
        $("#liJobCardHistory").addClass('active')
        $("#liMJobCard").addClass('open')
    }// Visitor
    else if (selectedMenu == "ScheduleVisitor") {
        $("#liVisitorRequest").addClass('active')
        $("#liMVisitor").addClass('open')
    }
    else if (selectedMenu == "PendingVRequest") {
        $("#liPendingVisitor").addClass('active')
        $("#liMVisitor").addClass('open')
    }// Overtime
    else if (selectedMenu == "SOTDeptList") {
        $("#liSOTDeptList").addClass('active')
        $("#liMOTDesk").addClass('open')
    }
    else if (selectedMenu == "ROTDeptList") {
        $("#liROTDeptList").addClass('active')
        $("#liMOTDesk").addClass('open')
    }
    else if (selectedMenu == "AOTDeptList") {
        $("#liAOTDeptList").addClass('active')
        $("#liMOTDesk").addClass('open')
    }
    else if (selectedMenu == "HROTDeptList") {
        $("#liHROTDeptList").addClass('active')
        $("#liMOTDesk").addClass('open')
    }
    else if (selectedMenu == "HAOTDeptList") {
        $("#liHAOTDeptList").addClass('active')
        $("#liMOTDesk").addClass('open')
    }


}
function btnShortCutClick(obj) {
    //window.applicationBaseUrl = @Html.Raw(HttpUtility.JavaScriptStringEncode(Url.Content("~/"), true));
    if (obj == "Dashboard") {
        var urls = LoadUrl('Home/Index');
        window.location.replace(urls);
    }
    else if (obj == "JobCard") {
        var urls = LoadUrl('Attendance/JobCard/Create');
        window.location.replace(urls);
    }
    else if (obj == "Visitor") {
        var urls = LoadUrl('Attendance/ScheduleVisitor/Index');
        window.location.replace(urls);
    }
    else if (obj == "Setting") {
        var urls = LoadUrl('Home/ChangePassword');
        window.location.replace(urls);
    }
}