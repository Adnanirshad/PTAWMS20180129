function Modal() {  
    var table = $('#example').DataTable();

    $('#example tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $('#button').click(function () {
        table.row('.selected').remove().draw(false);
    });
    $('#dynamic-table tbody').on('dblclick', 'td', function () {
        //alert("adnan");
        var id = ($(this).parent().find('td').html().trim());
        var Name = ($(this).parent().find('#name').html().trim());
        var ID = id;
       // $("input[name=RecommendID]").val(ID)
        document.getElementById("RecommendID").value = ID;
        $("#RecommenderN").val(Name)
        $(function () {
            $('#modal').click();
        });
       // $('#myModal').modal('hide');
    });
}
jQuery(function ($) {
    $('#example tr').dblclick(function () {
        return false;
    }).dblclick(function () {
        alert("adnan");
        return false;
    });
});