$(document).ready(function () {

    $("#DivDivision").hide();
    $("#DivLocation").show();
    $("#DivType").hide();
    $("#DivEmployee").hide();
    $("#DivGrade").hide();
    $("#DivSection").hide();
    var test = $("input[name$='SelectionRB']:checked").val();
    if (test == "rbAll") {
        $("#DivDivision").hide();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivEmployee").hide();
        $("#DivGrade").hide();
        $("#DivSection").hide();
    }
    if (test == "rbDivision") {
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivEmployee").hide();
        $("#DivDivision").show();
        $("#DivGrade").hide();
        $("#DivSection").hide();
    }
    if (test == "rbLocation") {
        $("#DivDivision").hide();
        $("#DivType").hide();
        $("#DivEmployee").hide();
        $("#DivLocation").show();
        $("#DivGrade").hide();
        $("#DivSection").hide();
    }
    if (test == "rbType") {
        $("#DivDivision").hide();
        $("#DivLocation").hide();
        $("#DivEmployee").hide();
        $("#DivType").show();
        $("#DivGrade").hide();
        $("#DivSection").hide();
    }
    if (test == "rbSection") {
        $("#DivDivision").hide();
        $("#DivLocation").hide();
        $("#DivEmployee").hide();
        $("#DivType").hide();
        $("#DivGrade").hide();
        $("#DivSection").show();
    }
    if (test == "rbGrade") {
        $("#DivDivision").hide();
        $("#DivLocation").hide();
        $("#DivEmployee").hide();
        $("#DivType").hide();
        $("#DivGrade").show();
        $("#DivSection").hide();
    }
    if (test == "rbEmployee") {
        $("#DivDivision").hide();
        $("#DivLocation").hide();
        $("#DivEmployee").show();
        $("#DivType").hide();
        $("#DivGrade").hide();
        $("#DivSection").hide();
    }
    $("input[name$='SelectionRB']").click(function () {
        var test = $(this).val();
        if (test == "rbAll") {
            $("#DivDivision").hide();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivEmployee").hide();
            $("#DivGrade").hide();
            $("#DivSection").hide();
        }
        if (test == "rbDivision") {
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivEmployee").hide();
            $("#DivDivision").show();
            $("#DivGrade").hide();
            $("#DivSection").hide();
        }
        if (test == "rbLocation") {
            $("#DivDivision").hide();
            $("#DivType").hide();
            $("#DivEmployee").hide();
            $("#DivLocation").show();
            $("#DivGrade").hide();
            $("#DivSection").hide();
        }
        if (test == "rbSRange") {
            $("#DivDivision").hide();
            $("#DivLocation").hide();
            $("#DivEmployee").hide();
            $("#DivType").show();
            $("#DivGrade").hide();
            $("#DivSection").hide();
        }
        if (test == "rbSection") {
            $("#DivDivision").hide();
            $("#DivLocation").hide();
            $("#DivEmployee").hide();
            $("#DivType").hide();
            $("#DivGrade").hide();
            $("#DivSection").show();
        }
        if (test == "rbGrade") {
            $("#DivDivision").hide();
            $("#DivLocation").hide();
            $("#DivEmployee").hide();
            $("#DivType").hide();
            $("#DivGrade").show();
            $("#DivSection").hide();
        }
        if (test == "rbEmployee") {
            $("#DivDivision").hide();
            $("#DivLocation").hide();
            $("#DivEmployee").show();
            $("#DivType").hide();
            $("#DivGrade").hide();
            $("#DivSection").hide();
        }
    });

});

$(document).ready(function () {
    document.getElementById("ELName").innerHTML = "Name: No Selected Employee";
    document.getElementById("ELDesignation").innerHTML = "Designation: No Selected Employee";
    document.getElementById("ELSection").innerHTML = "Section: No Selected Employee";
    document.getElementById("ELType").innerHTML = "Type: No Selected Employee";
    document.getElementById("ELJoin").innerHTML = "Join Date: No Selected Employee";
    $('#buttonId').click(function () {
        var EmpNo = document.getElementById("EmpNo").value;
        var urls = LoadUrl('MonthDataEditor/GetEmpInfo');
        var URL3 = urls;
        $.getJSON(URL3, { EmpNo: EmpNo }, function (data) {
            var values = data.split('@');
            document.getElementById("ELName").innerHTML = values[0];
            document.getElementById("ELDesignation").innerHTML = values[1];
            document.getElementById("ELSection").innerHTML = values[2];
            document.getElementById("ELType").innerHTML = values[3];
            document.getElementById("ELJoin").innerHTML = values[4];
        });

    });
});