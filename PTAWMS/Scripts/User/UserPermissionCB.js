$(document).ready(function () {
   // $("#hide").hide();
    if (document.getElementById('HRModule').checked) {
        $("#HRModuleDiv").show();
    } else {
        $("#HRModuleDiv").hide();
    }
    $('#HRModule').change(function () {
        if ($(this).is(":checked")) {
            if ($(this).is(":checked")) {
                $("#HRModuleDiv").show();
            }
            else {
                $("#HRModuleDiv").hide();
            }
        }
        else {
            $("#HRModuleDiv").hide();
        }
    });
    // employee
    if (document.getElementById('MHREmployee').checked) {
        $("#HREmpProfileDiv").show();
    } else {
        $("#HREmpProfileDiv").hide();
    }
    $('#MHREmployee').change(function () {
        if ($(this).is(":checked")) {
            $("#HREmpProfileDiv").show();
        }
        else {
            $("#HREmpProfileDiv").hide();
        }
    });
    // Attendance
    if (document.getElementById('AttendanceModule').checked) {
        $("#AttendanceModuleDiv").show();
    } else {
        $("#AttendanceModuleDiv").hide();
    }
    $('#AttendanceModule').change(function () {
        if ($(this).is(":checked")) {
            $("#AttendanceModuleDiv").show();
        }
        else {
            $("#AttendanceModuleDiv").hide();
        }
    });
    //Visitor
    if (document.getElementById('VisitorModule').checked) {
        $("#VisitorModuleDiv").show();
    } else {
        $("#VisitorModuleDiv").hide();
    }
    $('#VisitorModule').change(function () {
        if ($(this).is(":checked")) {
            $("#VisitorModuleDiv").show();
        }
        else {
            $("#VisitorModuleDiv").hide();
        }
    });
    // Leaves
    //if (document.getElementById('MAttLeaves').checked) {
    //    $("#LeaveDiv").show();
    //} else {
    //    $("#LeaveDiv").hide();
    //}
    //$('#MAttLeaves').change(function () {
    //    if ($(this).is(":checked")) {
    //        $("#LeaveDiv").show();
    //    }
    //    else {
    //        $("#LeaveDiv").hide();
    //    }
    //});

    // Advance Salary
    if (document.getElementById('PRAdvance').checked) {
        $("#PRAdvanceDiv").show();
    } else {
        $("#PRAdvanceDiv").hide();
    }
    $('#PRAdvance').change(function () {
        if ($(this).is(":checked")) {
            $("#PRAdvanceDiv").show();
        }
        else {
            $("#PRAdvanceDiv").hide();
        }
    });

    // Loan
    if (document.getElementById('PRLoan').checked) {
        $("#PRLoanDiv").show();
    } else {
        $("#PRLoanDiv").hide();
    }
    $('#PRLoan').change(function () {
        if ($(this).is(":checked")) {
            $("#PRLoanDiv").show();
        }
        else {
            $("#PRLoanDiv").hide();
        }
    });

    // Payroll Period
    if (document.getElementById('PRPeriod').checked) {
        $("#PRPeriodDiv").show();
    } else {
        $("#PRPeriodDiv").hide();
    }
    $('#PRPeriod').change(function () {
        if ($(this).is(":checked")) {
            $("#PRPeriodDiv").show();
        }
        else {
            $("#PRPeriodDiv").hide();
        }
    });
});


$('#UserType').change(function () {
    //var URL = '/Emp/SectionList';
    var types = $('#UserType').val();
    var urls = LoadUrl('User/GetUserType');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
        data: { types: types }
    }).done(function (result) {

        if (result[0].CanAdd == true) {
            $('#CanAdd').prop('checked', true);
        }
        else {
            $('#CanAdd').prop('checked', false);
        }
        if (result[0].CanEdit == true) {
            $('#CanEdit').prop('checked', true);
        }
        else {
            $('#CanEdit').prop('checked', false);
        }

        if (result[0].CanDelete == true) {
            $('#CanDelete').prop('checked', true);
        }
        else {
            $('#CanDelete').prop('checked', false);
        }

        if (result[0].CanView == true) {
            $('#CanView').prop('checked', true);
        }
        else {
            $('#CanView').prop('checked', false);
        }

        if (result[0].HRModule == true) {
            $('#HRModule').prop('checked', true);
            if ($('#HRModule').is(":checked")) {
                $("#HRModuleDiv").show();
            }
            else {
                $("#HRModuleDiv").hide();
            }
        }
        else {
            $('#HRModule').prop('checked', false);          
            if ($('#HRModule').is(":checked")) {
                $("#HRModuleDiv").show();
            }
            else {
                $("#HRModuleDiv").hide();
            }
        }
        if (result[0].MHRCompHierarchy == true) {
            $('#MHRCompHierarchy').prop('checked', true);
        }
        else {
            $('#MHRCompHierarchy').prop('checked', false);
        }       
        if (result[0].MUser == true) {
            $('#MUser').prop('checked', true);
        }
        else {
            $('#MUser').prop('checked', false);
        }
        if (result[0].HREmpType == true) {
            $('#HREmpType').prop('checked', true);
        }
        else {
            $('#HREmpType').prop('checked', false);
        }
        if (result[0].HRDesignation == true) {
            $('#HRDesignation').prop('checked', true);
        }
        else {
            $('#HRDesignation').prop('checked', false);
        }
        if (result[0].MGrade == true) {
            $('#MGrade').prop('checked', true);
        }
        else {
            $('#MGrade').prop('checked', false);
        }
        if (result[0].HRLocation == true) {
            $('#HRLocation').prop('checked', true);
        }
        else {
            $('#HRLocation').prop('checked', false);
        }
        if (result[0].HRDeptartment == true) {
            $('#HRDeptartment').prop('checked', true);
        }
        else {
            $('#HRDeptartment').prop('checked', false);
        }
        if (result[0].HRSection == true) {
            $('#HRSection').prop('checked', true);
        }
        else {
            $('#HRSection').prop('checked', false);
        }
        if (result[0].MHREmployee == true) {
            $('#MHREmployee').prop('checked', true);
            if ($('#MHREmployee').is(":checked")) {
                $("#HREmpProfileDiv").show();
            }
            else {
                $("#HREmpProfileDiv").hide();
            }
        }
        else {
            $('#MHREmployee').prop('checked', false);
            if ($('#MHREmployee').is(":checked")) {
                $("#HREmpProfileDiv").show();
            }
            else {
                $("#HREmpProfileDiv").hide();
            }
        }
        if (result[0].MHREmpA == true) {
            $('#MHREmpA').prop('checked', true);
        }
        else {
            $('#MHREmpA').prop('checked', false);
        }
        if (result[0].MHREmpE == true) {
            $('#MHREmpE').prop('checked', true);
        }
        else {
            $('#MHREmpE').prop('checked', false);
        }
        if (result[0].MHREmpV == true) {
            $('#MHREmpV').prop('checked', true);
        }
        else {
            $('#MHREmpV').prop('checked', false);
        }
        if (result[0].MHREmpD == true) {
            $('#MHREmpD').prop('checked', true);
        }
        else {
            $('#MHREmpD').prop('checked', false);
        }

        if (result[0].MHREmpPersonal == true) {
            $('#MHREmpPersonal').prop('checked', true);
        }
        else {
            $('#MHREmpPersonal').prop('checked', false);
        }
        if (result[0].MHREmpJob == true) {
            $('#MHREmpJob').prop('checked', true);
        }
        else {
            $('#MHREmpJob').prop('checked', false);
        }
        if (result[0].MHREmpAtt == true) {
            $('#MHREmpAtt').prop('checked', true);
        }
        else {
            $('#MHREmpAtt').prop('checked', false);
        }
        if (result[0].AttendanceModule == true) {
            $('#AttendanceModule').prop('checked', true);
            if ($('#AttendanceModule').is(":checked")) {
                $("#AttendanceModuleDiv").show();
            }
            else {
                $("#AttendanceModuleDiv").hide();
            }
        }
        else {
            $('#AttendanceModule').prop('checked', false);
            if ($('#AttendanceModule').is(":checked")) {
                $("#AttendanceModuleDiv").show();
            }
            else {
                $("#AttendanceModuleDiv").hide();
            }
        }
        if (result[0].MAttEditAttendance == true) {
            $('#MAttEditAttendance').prop('checked', true);
        }
        else {
            $('#MAttEditAttendance').prop('checked', false);
        }
        if (result[0].MAttMonthEditor == true) {
            $('#MAttMonthEditor').prop('checked', true);
        }
        else {
            $('#MAttMonthEditor').prop('checked', false);
        }
        if (result[0].MOption == true) {
            $('#MOption').prop('checked', true);
        }
        else {
            $('#MOption').prop('checked', false);
        }
        if (result[0].MAttDevice == true) {
            $('#MAttDevice').prop('checked', true);
        }
        else {
            $('#MAttDevice').prop('checked', false);
        }
        if (result[0].MAttJobCard == true) {
            $('#MAttJobCard').prop('checked', true);
        }
        else {
            $('#MAttJobCard').prop('checked', false);
        }
        if (result[0].MAttShift == true) {
            $('#MAttShift').prop('checked', true);
        }
        else {
            $('#MAttShift').prop('checked', false);
        }
        if (result[0].MAttPolicy == true) {
            $('#MAttPolicy').prop('checked', true);
        }
        else {
            $('#MAttPolicy').prop('checked', false);
        }
        if (result[0].MAttDownloadTime == true) {
            $('#MAttDownloadTime').prop('checked', true);
        }
        else {
            $('#MAttDownloadTime').prop('checked', false);
        }
        if (result[0].MAttRoster == true) {
            $('#MAttRoster').prop('checked', true);
        }
        else {
            $('#MAttRoster').prop('checked', false);
        }
        if (result[0].MAttOTPolicy == true) {
            $('#MAttOTPolicy').prop('checked', true);
        }
        else {
            $('#MAttOTPolicy').prop('checked', false);
        }
        if (result[0].MAttDeviceUtility == true) {
            $('#MAttDeviceUtility').prop('checked', true);
        }
        else {
            $('#MAttDeviceUtility').prop('checked', false);
        }
        if (result[0].OTBudget == true) {
            $('#OTBudget').prop('checked', true);
        }
        else {
            $('#OTBudget').prop('checked', false);
        }
        if (result[0].OTBudgetCreditDebit == true) {
            $('#OTBudgetCreditDebit').prop('checked', true);
        }
        else {
            $('#OTBudgetCreditDebit').prop('checked', false);
        }
        if (result[0].MAttOTCreate == true) {
            $('#MAttOTCreate').prop('checked', true);
        }
        else {
            $('#MAttOTCreate').prop('checked', false);
        }
        if (result[0].MAttOTEdit == true) {
            $('#MAttOTEdit').prop('checked', true);
        }
        else {
            $('#MAttOTEdit').prop('checked', false);
        }
        if (result[0].MAttMOT == true) {
            $('#MAttMOT').prop('checked', true);
        }
        else {
            $('#MAttMOT').prop('checked', false);
        }
        if (result[0].MAttHoliday == true) {
            $('#MAttHoliday').prop('checked', true);
        }
        else {
            $('#MAttHoliday').prop('checked', false);
        }
        if (result[0].MAttProcess == true) {
            $('#MAttProcess').prop('checked', true);
        }
        else {
            $('#MAttProcess').prop('checked', false);
        }
        if (result[0].VisitorModule == true) {
            $('#VisitorModule').prop('checked', true);
            if ($('#VisitorModule').is(":checked")) {
                $("#VisitorModuleDiv").show();
            }
            else {
                $("#VisitorModuleDiv").hide();
            }
        }
        else {
            $('#VisitorModule').prop('checked', false);
            if ($('#VisitorModule').is(":checked")) {
                $("#VisitorModuleDiv").show();
            }
            else {
                $("#VisitorModuleDiv").hide();
            }
        }
        if (result[0].VisitorApplication == true) {
            $('#VisitorApplication').prop('checked', true);
        }
        else {
            $('#VisitorApplication').prop('checked', false);
        }
        if (result[0].VisitorSupervisor == true) {
            $('#VisitorSupervisor').prop('checked', true);
        }
        else {
            $('#VisitorSupervisor').prop('checked', false);
        }
        if (result[0].VisitorSupervisor == true) {
            $('#VisitorSupervisor').prop('checked', true);
        }
        else {
            $('#VisitorSupervisor').prop('checked', false);
        }
    });

});