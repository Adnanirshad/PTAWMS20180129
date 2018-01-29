$(document).ready(function () {

    $("#divLoading").show();
    $('li a.active').removeClass('active');
    //$('#btnOpenPersonal').addClass('active');
    
    var ID = document.getElementById("EmpID").value;
   
    //if (ID != '') {
    //    $.ajax({
    //        url: '/WMS/HumanResource/Profile/EmpPersonalDetails',
    //        type: "GET",
    //        cache: false,
    //        data: { ID: ID }
    //    }).done(function (result) {
    //        $("#divLoading").hide();
    //        $('#partialcontainer').html(result);
    //    });
    //}
    /// Personal
    $('#btnOpenPersonal').on("click", function getIdFunction() {
        
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenPersonal').parents('li').addClass('active');

        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open"); 

        $("#liProfile").addClass('active');
        $("#liPersonalInfo").addClass('open');

        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EmpPersonalDetails');
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
    });
   
    /// Attendance
    $('#btnOpenAttendance').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('li a.active').removeClass('active');
        $('#btnOpenAttendance').addClass('active');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EPAttendance');
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
    });
    
    

    /// Qualification
    $('#btnOpenQualification').on("click", function getIdFunction() {
        console.log('Calling btnOpenQualification');
        console.log(window.location.pathname);
        console.log(window.applicationBaseUrl);
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenQualification').parents('li').addClass('active');
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");
        $("#liQualification").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPQualification');
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
    });

    /// Dependents
    $('#btnOpenDependents').on("click", function getIdFunction() {
        console.log('Calling btnOpenDependents');
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenDependents').parents('li').addClass('active');  
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liDependent").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPDependents');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: ID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
                // ForEmpEdit();
            });
        }
    });

    /// Performance Histroy
    $('#btnOpenPerformanceHistory').on("click", function getIdFunction() {
        console.log('Calling btnOpenPerformanceHistory');
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenPerformanceHistory').parents('li').addClass('active');         
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liDependent").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPPerformanceHistory');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: ID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
                // ForEmpEdit();
            });
        }
    });

    /// Experience
    $('#btnOpenExperience').on("click", function getIdFunction() {
        console.log('Calling btnOpenExperience');
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenExperience').parents('li').addClass('active');   
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liPrejobhistory").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPExperienceHistory');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: ID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
                // ForEmpEdit();
            });
        }
    });

    /// Posting Transfer
    $('#btnOpenPostingTransfer').on("click", function getIdFunction() {
        console.log('Calling btnOpenPostingTransfer');
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenPostingTransfer').parents('li').addClass('active');               
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liTransfer").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPPostingTransfer');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: ID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
                // ForEmpEdit();
            });
        }
    });

    /// Assets
    $('#btnOpenAssets').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenAssets').parents('li').addClass('active');   
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liAsset").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/GetAssets');
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
    });

    /// Promotion
    $('#btnOpenPromotion').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenPromotion').parents('li').addClass('active');
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liPromotion").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPPromotion');
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
    });

    /// Appreciation
    $('#btnOpenAppreciation').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenAppreciation').parents('li').addClass('active');
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liAppreciation").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPAppreciation');
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
    });

    /// Warning
    $('#btnOpenWarning').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenWarning').parents('li').addClass('active');
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liWarning").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPWarning');
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
    });


    /// Training
    $('#btnOpenTraining').on("click", function getIdFunction() {
        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenTraining').parents('li').addClass('active');
        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liTraining").addClass('active');
        $("#liPersonalInfo").addClass('open');
        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPTraining');
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
    });

    /// Fund
    $('#btnOpenFundAdvance').on("click", function getIdFunction() {
        console.log('Calling btnOpenPostingTransfer');
        $("#divLoading").show();
        $('li a.active').removeClass('active');
        $('#btnOpenFundAdvance').addClass('active');        

        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPFunds');
            $.ajax({
                url: urls,
                type: "GET",
                cache: false,
                data: { ID: ID }
            }).done(function (result) {
                $("#divLoading").hide();
                $('#partialcontainer').html(result);
                // ForEmpEdit();
            });
        }
    });

    $('#btnOpenLeaves').on("click", function getIdFunction() {

        $("#divLoading").show();
        $('li').removeClass('active');
        $('#btnOpenLeaves').parents('li').addClass('active');

        $("nav nav-list menu_section .nav").find(".active").removeClass("active");
        $("nav nav-list menu_section .nav").find(".open").removeClass("open");

        $("#liLeave").addClass('active');
        $("#liPersonalInfo").addClass('open');

        var ID = document.getElementById("EmpID").value;
        if (ID != '') {
            var urls = LoadUrl('HumanResource/Profile/EMPLeaves');
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
    });

    //$('#SaveEmpPersonalInfo').on("click", function getIdFunction() {
    //    var empID = document.getElementById("EmpID").value;
    //    if (ID != '') {
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
            success: function () {
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
                var ID = document.getElementById("EmpID").value;
                var urls = LoadUrl('HumanResource/Employee/EPPersonal');
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                    data: { ID: ID }
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
