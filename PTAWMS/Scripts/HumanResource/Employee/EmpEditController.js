$(document).ready(function () {

    $("#divLoading").show();
    $('#btnOpenPersonal').addClass('active');
    var ID = document.getElementById("EmpID").value;
    if (ID != '') {
        var urls = LoadUrl('HumanResource/Employee/EPPersonal');
        $.ajax({
            url: urls,
            type: "GET",
            cache: false,
            data: { ID: ID }
        }).done(function (result) {

            $("#divLoading").hide();
            $('#partialcontainer').html(result);
        });
    }
    /// Job Details
    $('#btnOpenJobDetails').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('#btnOpenJobDetails').addClass('active');
        $('#btnOpenPersonal').removeClass('active');
        $('#btnOpenContact').removeClass('active');
        $('#btnOpenAttendance').removeClass('active');
        $('#btnOpenSalary').removeClass('active');
        $('#btnOpenTax').removeClass('active');
        var PrID = document.getElementById("EmpID").value;
        if (PrID != '') {
            var urls = LoadUrl('HumanResource/Employee/EPJobDetail');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: PrID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
                ForEmpEdit();
            });
        }
    });
    /// Personal
    $('#btnOpenPersonal').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('#btnOpenJobDetails').removeClass('active');
        $('#btnOpenPersonal').addClass('active');
        $('#btnOpenContact').removeClass('active');
        $('#btnOpenAttendance').removeClass('active');
        $('#btnOpenSalary').removeClass('active');
        $('#btnOpenTax').removeClass('active');
        var PrID = document.getElementById("EmpID").value;
        if (PrID != '') {
            var urls = LoadUrl('HumanResource/Employee/EPPersonal');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: PrID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
            });
        }
    });
    /// Contact
    $('#btnOpenContact').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('#btnOpenJobDetails').removeClass('active');
        $('#btnOpenPersonal').removeClass('active');
        $('#btnOpenContact').addClass('active');
        $('#btnOpenAttendance').removeClass('active');
        $('#btnOpenSalary').removeClass('active');
        $('#btnOpenTax').removeClass('active');
        var PrID = document.getElementById("EmpID").value;
        if (PrID != '') {
            var urls = LoadUrl('HumanResource/Employee/EPContact');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: PrID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
            });
        }
    });
    /// Attendance
    $('#btnOpenAttendance').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('#btnOpenJobDetails').removeClass('active');
        $('#btnOpenPersonal').removeClass('active');
        $('#btnOpenContact').removeClass('active');
        $('#btnOpenAttendance').addClass('active');
        $('#btnOpenSalary').removeClass('active');
        $('#btnOpenTax').removeClass('active');
        var PrID = document.getElementById("EmpID").value;
        if (PrID != '') {
            var urls = LoadUrl('HumanResource/Employee/EPAttendance');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: PrID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
            });
        }
    });
    /// Salary
    $('#btnOpenSalary').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('#btnOpenJobDetails').removeClass('active');
        $('#btnOpenPersonal').removeClass('active');
        $('#btnOpenContact').removeClass('active');
        $('#btnOpenAttendance').removeClass('active');
        $('#btnOpenSalary').addClass('active');
        $('#btnOpenTax').removeClass('active');
        var PrID = document.getElementById("EmpID").value;
        if (PrID != '') {
            var urls = LoadUrl('HumanResource/Employee/EPSalary');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: PrID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
            });
        }
    });
    /// Tax
    $('#btnOpenTax').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('#btnOpenJobDetails').removeClass('active');
        $('#btnOpenPersonal').removeClass('active');
        $('#btnOpenContact').removeClass('active');
        $('#btnOpenAttendance').removeClass('active');
        $('#btnOpenSalary').removeClass('active');
        $('#btnOpenTax').addClass('active');
        var PrID = document.getElementById("EmpID").value;
        if (PrID != '') {
            var urls = LoadUrl('HumanResource/Employee/EPTax');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: PrID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
            });
        }
    });


    //$('#SaveEmpPersonalInfo').on("click", function getIdFunction() {
    //    var empID = document.getElementById("EmpID").value;
    //    if (PrID != '') {
    //        $.ajax({
    //            url: '/WMS/HumanResource/Employee/EPPersonal',
    //            type: "POST",
    //            cache: false,
    //            data: { ID: empID },
    //            success: function (result) {
    //                $('#partialcontainer').html(result);
    //            },
    //            error: function () {
    //                alert("Failed");
    //            }
    //        });
    //    }
    //});


});

function SavePersonalInfoFunction() {
    $("#SaveEmpPersonalInfo").click(function (e) {
        e.preventDefault();
        var empid = document.getElementById("EmpID").value;
        var formData = new FormData();
        var totalFiles = document.getElementById("ImageData").files.length;
        for (var i = 0; i < totalFiles; i++) {
            var _file = document.getElementById("ImageData").files[i];
            formData.append("ImageData", _file);
        }
        formData.append("EmpID", empid);
        $("#divLoading").show();
        // for Image
        var urls = LoadUrl('HumanResource/Employee/EPImage');
        $.ajax({
            url: urls,
            type: 'POST',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (res) {

            },
            error: function () {
                $("#result").text('an error occured')
            }
        });
        var urls = LoadUrl('HumanResource/Employee/EPPersonal');
        $.ajax({
            url: urls,
            type: 'POST',
            data: $("#EditEPPersonalForm").serialize(),
            success: function () {
                var PrID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPPersonal');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: PrID }
                }).done(function (res) {
                    $("#divLoading").hide();
                    $('#partialcontainer').html(res);
                });
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });

    })
}
function SaveJobDetailFunction() {
    // Section Cascade dropdown
    // Load Section on page run for first time
    $('#SectionID').empty();
    var urls = LoadUrl('EmpLoyee/SectionList');
    var URL = urls;
    //var URL = '/Emp/SectionList';
    var convalue = $('#DeptID').val();
    $.getJSON(URL + '/' + convalue, function (data) {
        var selectedItemID = document.getElementById("selectedSectionIdHidden").value;
        var items;
        $.each(data, function (i, state) {
            if (state.Value == selectedItemID)
                items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#SectionID').html(items);
    });


    $('#DeptID').change(function () {
        $('#SectionID').empty();
        var urls = LoadUrl('EmpLoyee/SectionList');
        var URL = urls;
        //var URL = '/Emp/SectionList';
        var convalue = $('#DeptID').val();
        $.getJSON(URL + '/' + convalue, function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#SectionID').html(items);
        });
    });

    // Others
    $("#SaveJobDetailsInfo").click(function (e) {
        e.preventDefault();
        $("#divLoading").show();
        var urls = LoadUrl('HumanResource/Employee/EPJobDetail');
        $.ajax({
            url: urls,
            type: 'POST',
            data: $("#EditEJobDetailForm").serialize(),
            success: function () {
                var PrID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPJobDetail');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: PrID }
                }).done(function (res) {
                    $("#divLoading").hide();
                    $('#partialcontainer').html(res);
                });
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });

    })
}

function SaveContactInfoFunction() {
    $("#SaveEmpConactInfo").click(function (e) {
        e.preventDefault();
        $("#divLoading").show();
        var urls = LoadUrl('HumanResource/Employee/EPContact');
        $.ajax({
            url: urls,
            type: 'POST',
            data: $("#EditEContactForm").serialize(),
            success: function () {
                var PrID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPContact');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: PrID }
                }).done(function (res) {
                    $("#divLoading").hide();
                    $('#partialcontainer').html(res);
                });
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });

    })
}
function SaveAttendanceFunction() {
    $("#SaveEmpAttendance").click(function (e) {
        e.preventDefault();
        $("#divLoading").show();
        var urls = LoadUrl('HumanResource/Employee/EPAttendance');
        $.ajax({
            url: urls,
            type: 'POST',
            data: $("#EditEAttendanceForm").serialize(),
            success: function () {
                var PrID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPAttendance');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: PrID }
                }).done(function (res) {
                    $("#divLoading").hide();
                    $('#partialcontainer').html(res);
                });
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });

    })
}

function SaveSalaryFunction() {
    $("#SaveEmpSalaryInfo").click(function (e) {
        e.preventDefault();
        $("#divLoading").show();
        var urls = LoadUrl('HumanResource/Employee/EPSalary');
        $.ajax({
            url: urls,
            type: 'POST',
            data: $("#EditESalaryForm").serialize(),
            success: function () {
                var PrID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPSalary');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: PrID }
                }).done(function (res) {
                    $("#divLoading").hide();
                    $('#partialcontainer').html(res);
                });
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });

    })
}
function SaveTaxFunction() {
    $("#SaveEmpTaxInfo").click(function (e) {
        e.preventDefault();
        $("#divLoading").show();
        var urls = LoadUrl('HumanResource/Profile/EMPLeaves');
        $.ajax({
            url: '/WMS/HumanResource/Employee/EPTax',
            type: 'POST',
            data: $("#EditETaxForm").serialize(),
            success: function () {
                var PrID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPTax');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: PrID }
                }).done(function (res) {
                    $("#divLoading").hide();
                    $('#partialcontainer').html(res);
                });
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });

    })
}

function ForEmpEdit() {
    var URL = '/EmpLoyee/DivisionList';
    // var URL = '/Emp/DepartmentList';
    var convalue = $('#BusinessAreaID').val();
    $.getJSON(URL + '/' + convalue, function (data) {

        $('#DivsionID').empty();
        //var URL = '/EmpLoyee/DivisionList';
        // var URL = '/Emp/SectionList';
        //var convalue = $('#BusinessAreaID').val();
        //$.getJSON(URL + '/' + convalue, function (data) {
        var selectedItemID = document.getElementById("selectedDivisionIdHidden").value;
        var items;
        $.each(data, function (i, state) {
            if (state.Value == selectedItemID)
                items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DivisionID').html(items);
        //});

        $('#DeptID').empty();
        var urls = LoadUrl('EmpLoyee/DeptList');
        var URL = urls;
        var convalue = $('#DivisionID').val();
        $.getJSON(URL + '/' + convalue, function (data) {
            var selectedItemID = document.getElementById("selectedDeptIDHidden").value;
            var items;
            $.each(data, function (i, state) {
                if (state.Value == selectedItemID)
                    items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
                else
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });

            $('#DeptID').html(items);

            $('#SectionID').empty();
            var urls = LoadUrl('EmpLoyee/SectionList');
            var URL = urls;
            // var URL = '/Emp/SectionList';
            var convalue = $('#DeptID').val();
            $.getJSON(URL + '/' + convalue, function (data) {
                var selectedItemID = document.getElementById("selectedSectionIdHidden").value;
                var items;
                $.each(data, function (i, state) {
                    if (state.Value == selectedItemID)
                        items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
                    else
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                });
                $('#SectionID').html(items);
            });
        });
    });


};