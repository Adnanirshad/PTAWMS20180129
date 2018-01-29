$(document).ready(function () {

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
};