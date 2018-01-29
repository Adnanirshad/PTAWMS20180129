function ModalSave() {
    //var table = $('#example').DataTable();

    //$('#example tbody').on('click', 'tr', function () {
    //    if ($(this).hasClass('selected')) {
    //        $(this).removeClass('selected');
    //    }
    //    else {
    //        table.$('tr.selected').removeClass('selected');
    //        $(this).addClass('selected');
    //    }
    //});

    //$('#button').click(function () {
    //    table.row('.selected').remove().draw(false);
    //});
    //$('#btnsave').click(function () {
    //    var values = $('input:checked').map(function () {
    //        return $(this).val();
    //    }).get();
    //var ID = values[1];
    $('#dynamic-table tbody').on('dblclick', 'td', function () {
        var id = ($(this).parent().find('td').html().trim());
        var ID = id;
        //alert(ID);

        //alert(ID);
       // $('#myModal').modal('hide');
        $("#myModal").on('hidden.bs.modal', function () {
            $(this).data('bs.modal', null);
        });
        $('#myModal').modal('hide');
        if (ID != '') {
            $.ajax({
                url: '/WMS/Attendance/EmployeeJobCard/Create/' + ID,
                type: "GET",
                cache: false,
                data: { ID: ID }
            }).done(function (result) {
                $('body').load("/WMS/Attendance/EmployeeJobCard/Create/" + ID)
            });
        }
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