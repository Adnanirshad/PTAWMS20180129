$(document).ready(function () {
    var urls = LoadUrl('EmpLoyee/DivisionList');
    var URLDiv = urls;
    var BAvalue = $('#BusinessAreaID').val();
    $.getJSON(URLDiv + '/' + BAvalue, function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DivisionID').html(items);
        //Load Department
        $('#DeptID').empty();
        var urls = LoadUrl('EmpLoyee/DeptList');
        var URL = urls;
        var convalue = $('#DivisionID').val();
        $.getJSON(URL + '/' + convalue, function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#DeptID').html(items);
            //Load Section
            $('#SectionID').empty();
            var urls = LoadUrl('EmpLoyee/SectionList');
            var URL = urls;
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
        //BA Changes
        $('#BusinessAreaID').change(function () {
            $('#DivisionID').empty();
            var urls = LoadUrl('EmpLoyee/DivisionList');
            var URL = urls;
            $.getJSON(URL + '/' + $('#BusinessAreaID').val(), function (data) {
                var items;
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#DivisionID').html(items);
                //Load Department
                $('#DeptID').empty();
                var urls = LoadUrl('EmpLoyee/DeptList');
                var URL = urls;
                var convalue = $('#DivisionID').val();
                $.getJSON(URL + '/' + convalue, function (data) {
                    var items;
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                        // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
                    });
                    $('#DeptID').html(items);
                    // Load Section
                    $('#SectionID').empty();
                    var urls = LoadUrl('EmpLoyee/SectionList');
                    var URL = urls;
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
            });
        });

        $('#DivisionID').change(function () {
            $('#DeptID').empty();
            var urls = LoadUrl('EmpLoyee/DeptList');
            var URL = urls;
            $.getJSON(URL + '/' + $('#DivisionID').val(), function (data) {
                var items;
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#DeptID').html(items);
                // Load Section
                $('#SectionID').empty();
                var urls = LoadUrl('EmpLoyee/SectionList');
                var URL = urls;
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
        });

        //Dept Change
        $('#DeptID').change(function () {
            // Load Section
            $('#SectionID').empty();
            var urls = LoadUrl('EmpLoyee/SectionList');
            var URL = urls;
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


    });

});
function fileCheck(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#blah')
                .attr('src', e.target.result)
                .width(90)
                .height(90);
            document.getElementById("blah").style.marginTop = "20px";
            document.getElementById("blah").style.marginLeft = "30px";
        };

        reader.readAsDataURL(input.files[0]);
    }
}
