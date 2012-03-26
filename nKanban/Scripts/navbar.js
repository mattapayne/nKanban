$(function () {
    $('.dropdown-toggle').dropdown();
    $("#logout-link").click(function (e) {
        e.preventDefault();
        $("#logout-form").submit();
    });
});