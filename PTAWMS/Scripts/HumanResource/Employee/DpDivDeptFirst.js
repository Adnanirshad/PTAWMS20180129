function LoadDivDeptFirstTme() {
    var urls = LoadUrl('EmpLoyee/DivisionList');
    var URLDiv = urls;
    var BAvalue = $('#BusinessAreaID').val();
    $.getJSON(URLDiv + '/' + BAvalue, function (data) {
        var selectedItemdID = document.getElementById("selectedDivisionIdHidden").value;
        var items;
        $('#DivisionID').empty();
        $.each(data, function (i, state) {
            if (state.Value == selectedItemdID)
                items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DeptID').html(items);
        //Load Department
        $('#SectionID').empty();
        var urls = LoadUrl('EmpLoyee/SectionList');
        var URL = urls;
        //var URL = '/Emp/SectionList';
        var convalue = $('#selectedDeptIDHidden').val();
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
            $('#DeptID').html(items);
        });

        //Load Section           
    });


};
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
