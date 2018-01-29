$(document).ready(function () {
    document.getElementById("ELName").innerHTML = "Name: No Selected Employee";
    document.getElementById("ELDesignation").innerHTML = "Designation: No Selected Employee";
    document.getElementById("ELSection").innerHTML = "Department: No Selected Employee";
    document.getElementById("ELType").innerHTML = "Type: No Selected Employee";
    document.getElementById("ELJoin").innerHTML = "Join Date: No Selected Employee";
    $('#buttonId').click(function () {
        var EmpNo = document.getElementById("EmpNo").value;
        var urls = LoadUrl('ProcessRequest/GetEmpInfo');
        var URL = urls;
        $.getJSON(URL, { EmpNo: EmpNo }, function (data) {
            var values = data.split('@');
            document.getElementById("ELName").innerHTML = values[0];
            document.getElementById("ELDesignation").innerHTML = values[1];
            document.getElementById("ELSection").innerHTML = values[2];
            document.getElementById("ELType").innerHTML = values[3];
            document.getElementById("ELJoin").innerHTML = values[4];
        });

    });
});