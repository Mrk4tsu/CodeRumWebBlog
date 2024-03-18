$(document).ready(function () {
    if ($.fn.metisMenu) {
        $('#side-menu').metisMenu();
    }

    $('#AlertBox').removeClass('hide');
    $('#AlertBox').delay(7000).slideUp(500);
});