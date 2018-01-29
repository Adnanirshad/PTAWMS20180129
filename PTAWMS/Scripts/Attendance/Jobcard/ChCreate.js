$(document).ready(function () {
    $("#timewise").hide();
    $("#datewise").hide();
    var test = $("#TimeBased").val();
    if (test == "False") {
        $('#RBMultipleDate').prop('checked', true);
        $('#RBSingleDate').prop('checked', false);
        $("#datewise").show();
        $("#timewise").hide();
    }
    else {

        $('#RBMultipleDate').prop('checked', false);
        $('#RBSingleDate').prop('checked', true);
        $("#timewise").show();
        $("#datewise").hide();
    }
    $("input[name$='SelectionRB']").click(function () {
        var test = $(this).val();
        if (test == "All") {
            $("#TimeBased").val("True");
            $("#timewise").show();
            $("#datewise").hide();
        }
        else {
            $("#TimeBased").val("False");
            $("#timewise").hide();
            $("#datewise").show();
        }
    });
});