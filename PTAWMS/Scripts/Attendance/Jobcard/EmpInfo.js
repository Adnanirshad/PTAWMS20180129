$(document).ready(function () {
   
    $('#buttonId').click(function () {
        var EmpNo = document.getElementById("EmpNo").value;
        var urls = LoadUrl('JobCard/GetEmployeeInfo');
        var URL = urls;
        $.getJSON(URL, { EmpNo: EmpNo }, function (data) {
            var values = data.split('@');
            document.getElementById("EName").innerHTML = values[0];
            document.getElementById("EFName").innerHTML = values[1];
            document.getElementById("EDesignation").innerHTML = values[2];
            document.getElementById("ESection").innerHTML = values[3];
        });      
    });
});