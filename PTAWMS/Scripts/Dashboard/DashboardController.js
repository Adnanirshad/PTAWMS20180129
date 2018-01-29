function LoadDivisionPieChart(graphtype) {
    var urls = LoadUrl('Home/RenderPCForDivs');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
        data: { GraphType: graphtype }
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCTMSParent').html(result);
    });
}
function LoadDepartmentPieChart(graphtype) {
    var urls = LoadUrl('Home/RenderPCForDepts');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
        data: { GraphType: graphtype }
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCTMSParent').html(result);
    });
}

function LoadspecificDeptPieChart(divid, graphtype)
{
    $("#divLoading").show();
    var urls = LoadUrl('Home/RenderPCForSpecificDivs');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
        data: { GraphType: graphtype,DeptID:divid }
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCTMSChild').html(result);
    });
}



function LoadDeptVMSummary() {
    var urls = LoadUrl('Home/PVSDeptSummary');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCDeptSummary').html(result);
    });
}
function LoadEmpVMSummary(deptid) {
    var urls = LoadUrl('Home/PVSEmpSummary');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
        data: { deptid:deptid }
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCEmpSummary').html(result);
    });
}

function LoadTMSSummary() {
    var urls = LoadUrl('Home/PTMSSummary');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false
    }).done(function (result) {
        $("#divLoading").hide();
        if (result != "OK") {
            $("#divLoading").hide();
            $('#PVCTMSParent').html(result);
        }
    });
}
function LoadEmployeeVisitor() {
    var urls = LoadUrl('Home/PVEmployeeVisitor');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false
    }).done(function (result) {
        if (result != "OK") {
            $("#divLoading").hide();
            $('#PVCEmpSummary').html(result);
        }
    });
}
function LoadEmployeeAttendance() {
    var urls = LoadUrl('Home/PVEmployeeAttendance');
    $.ajax({
        url: urls,
        type: "GET",
        cache: false
    }).done(function (result) {
        $("#divLoading").hide();
        if (result != "OK") {
            $("#divLoading").hide();
            $('#PVCTMSChild').html(result);
        }
    });
}

function RenderPieChartTMS(obj2) {
    google.charts.load("current", { packages: ["corechart"] });
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        var myArray = [];
        myArray.push(["Criteria", 'No of Employees']);
        var obj = jQuery.parseJSON(obj2)
        for (i = 0; i < obj.length; i++) {
            myArray.push([obj[i].Name, obj[i].Count]);
        }
        var data = google.visualization.arrayToDataTable(myArray);

        var options = {
            fontSize: 11,
            //title: 'My Daily Activities',
            height: 400,
            pieHole: 0.2,
        };

        var chart = new google.visualization.PieChart(document.getElementById('pieChart'));
        chart.draw(data, options);
    }
}
function RenderPieChartTMS2(obj2) {
    google.charts.load("current", { packages: ["corechart"] });
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        var myArray = [];
        myArray.push(["Criteria", 'No of Employees']);
        var obj = jQuery.parseJSON(obj2)
        for (i = 0; i < obj.length; i++) {
            myArray.push([obj[i].Name, obj[i].Count]);
        }
        var data = google.visualization.arrayToDataTable(myArray);

        var options = {
            fontSize: 11,
            //title: 'My Daily Activities',
            height: 400,
            pieHole: 0.2,
        };

        var chart = new google.visualization.PieChart(document.getElementById('pieChart2'));
        chart.draw(data, options);
    }
}
function RenderPieChart(obj) {
    google.charts.load("current", { packages: ["corechart"] });
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        var myArray = [];
        myArray.push(['Departments', 'No of Visitors']);
        for (i = 0; i < obj.length; i++) {
            myArray.push([obj[i].Name, obj[i].Count]);
        }
        var data = google.visualization.arrayToDataTable(myArray);

        var options = {
                    fontSize: 11,
            //title: 'My Daily Activities',
            height: 400,
            pieHole: 0.2,
        };

        var chart = new google.visualization.PieChart(document.getElementById('pieChartvd'));
        chart.draw(data, options);
    }
}

function RenderBarChart(obj) {

    google.charts.load('current', { 'packages': ['bar'] });
    google.charts.setOnLoadCallback(drawStuff);

    function drawStuff() {
        var myArray = [];
        myArray.push(['Employees', 'Number of Visits']);
        for (i = 0; i < obj.length; i++) {
            myArray.push([obj[i].Name, obj[i].Count]);
        }
        //alert(myArray);
        var data = new google.visualization.arrayToDataTable(myArray);
        var options = {
            title: 'Chess opening moves',          
            legend: { position: 'none' },
            chart: {
                title: ' ',
                subtitle: ''
            },
            //colors: ['#ed1c24'],
            bars: 'vertical', // Required for Material Bar Charts.
            axes: {
                x: {
                    0: { side: 'bottom', label: ' ' } // Top x-axis.
                }
            },
            bar: { groupWidth: "99%" }
        };

        var chart = new google.charts.Bar(document.getElementById('barchart'));
        chart.draw(data, options);
    };
}

function RenderLineChart(obj) {

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {
        var myArray = [];
        myArray.push(['Dates', 'Early In', 'Late In', 'Early Out', 'Late Out']);
        for (i = 0; i < obj.length; i++) {
            myArray.push([obj[i].Name, obj[i].CountEI, obj[i].CountLI, obj[i].CountEO, obj[i].CountLO]);
        }
        var data = google.visualization.arrayToDataTable(myArray);

        var options = {
            title: '',
            height:250,
            curveType: 'function',
            legend: { position: 'none' }
        };

        var chart = new google.visualization.LineChart(document.getElementById('lineChart'));

        chart.draw(data, options);
    };
}


