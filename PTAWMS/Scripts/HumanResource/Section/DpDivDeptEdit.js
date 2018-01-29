$(document).ready(function () {

    var urls = LoadUrl('Section/DeptList');
    var URL = urls;
    // var URL = '/Emp/SectionList';
    var convalue = $('#DivisionID').val();
    $.getJSON(URL + '/' + convalue, function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DepartmentID').html(items);

        $('#DivisionID').change(function () {
            $('#DepartmentID').empty();
            var urls = LoadUrl('Section/DeptList');
            var URL = urls;
            // var URL = '/Emp/DepartmentList';
            $.getJSON(URL + '/' + $('#DivisionID').val(), function (data) {
                var items;
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#DepartmentID').html(items);


            });
        });



    });

});