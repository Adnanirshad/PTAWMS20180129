$(document).ready(function () {

    var theme = init_echarts();
    var text = '{ "employees" : [' +
 '{ "firstName":"John" , "lastName":"Doe" },' +
 '{ "firstName":"Anna" , "lastName":"Smith" },' +
 '{ "firstName":"Peter" , "lastName":"Jones" } ]}';
    var obj = JSON.parse(text);
    renderPieTimeInChart(theme, obj);
    renderPieCharTimeOut(theme, obj);
});