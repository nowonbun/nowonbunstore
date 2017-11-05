var sec = 5;
$(function () {
    setTimeout("check()", 0);
});
function check() {
    $("#secView").html(sec);
    sec--;
    if (sec > -1) {
        setTimeout("check()", 1000);
    } else {
        location.href = "/";
    }
}