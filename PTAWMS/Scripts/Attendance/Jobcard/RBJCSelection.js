//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {
    $("#DivDivision").hide();
    $("#DivDept").hide();
    $("#DivSec").hide();
    $("#DivLocation").hide();
    $("#DivGroup").hide();
    $("#DivType").hide();
    $("#DivShift").hide();
    $("#JCValueDiv").hide();
    var test = $("input[name$='SelectionRB']:checked").val();
    if (test == "rbAll") {
        $("#DivDivision").hide();
        $("#DivDept").hide();
        $("#DivSec").hide();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivShift").hide();
        $("#DivGroup").hide();
    }
    if (test == "rbShift") {
        $("#DivDivision").hide();
        $("#DivDept").hide();
        $("#DivSec").hide();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivShift").show();
        $("#DivGroup").hide();
    } if (test == "rbLocation") {
        $("#DivDivision").hide();
        $("#DivDept").hide();
        $("#DivSec").hide();
        $("#DivLocation").show();
        $("#DivType").hide();
        $("#DivShift").hide();
        $("#DivGroup").hide();
    }
    if (test == "rbGroup") {
        $("#DivDivision").hide();
        $("#DivDept").hide();
        $("#DivSec").hide();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivShift").hide();
        $("#DivGroup").show();
    }
    if (test == "rbDivision") {
        $("#DivDivision").show();
        $("#DivDept").hide();
        $("#DivSec").hide();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivShift").hide();
        $("#DivGroup").hide();
    }
    if (test == "rbDepartment") {
        $("#DivDivision").hide();
        $("#DivDept").show();
        $("#DivSec").hide();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivShift").hide();
        $("#DivGroup").hide();
    }
    if (test == "rbType") {
        $("#DivDivision").hide();
        $("#DivDept").hide();
        $("#DivSec").hide();
        $("#DivLocation").hide();
        $("#DivType").show();
        $("#DivShift").hide();
        $("#DivGroup").hide();
    }
    if (test == "rbSection") {
        $("#DivDivision").hide();
        $("#DivDept").hide();
        $("#DivSec").show();
        $("#DivLocation").hide();
        $("#DivType").hide();
        $("#DivShift").hide();
        $("#DivGroup").hide();
    }
    $("input[name$='SelectionRB']").click(function () {
        var test = $(this).val();
        if (test == "rbAll") {
            $("#DivDivision").hide();
            $("#DivDept").hide();
            $("#DivSec").hide();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivShift").hide();
            $("#DivGroup").hide();
        }
        if (test == "rbType") {
            $("#DivDivision").hide();
            $("#DivDept").hide();
            $("#DivSec").hide();
            $("#DivLocation").hide();
            $("#DivType").show();
            $("#DivShift").hide();
            $("#DivGroup").hide();
        }
        if (test == "rbShift") {
            $("#DivDivision").hide();
            $("#DivDept").hide();
            $("#DivSec").hide();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivShift").show();
            $("#DivGroup").hide();
        } if (test == "rbLocation") {
            $("#DivDivision").hide();
            $("#DivDept").hide();
            $("#DivSec").hide();
            $("#DivLocation").show();
            $("#DivType").hide();
            $("#DivShift").hide();
            $("#DivGroup").hide();
        }
        if (test == "rbGroup") {
            $("#DivDivision").hide();
            $("#DivDept").hide();
            $("#DivSec").hide();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivShift").hide();
            $("#DivGroup").show();
        }
        if (test == "rbDivision") {
            $("#DivDivision").show();
            $("#DivDept").hide();
            $("#DivSec").hide();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivShift").hide();
            $("#DivGroup").hide();
        }
        if (test == "rbDept") {
            $("#DivDivision").hide();
            $("#DivDept").show();
            $("#DivSec").hide();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivShift").hide();
            $("#DivGroup").hide();
        }
        if (test == "rbSection") {
            $("#DivDivision").hide();
            $("#DivDept").hide();
            $("#DivSec").show();
            $("#DivLocation").hide();
            $("#DivType").hide();
            $("#DivShift").hide();
            $("#DivGroup").hide();
        }
    });

    $("#JobCardType").change(function () {
        var val = this.value;
        if (val == 9 || val == 10) {
            $("#JCValueDiv").show();
        }
    });
});