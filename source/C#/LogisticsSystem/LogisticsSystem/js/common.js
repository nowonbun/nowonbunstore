$(function () {
    loadingSetting();
    $.cookie('SessionID', $("#SessionID").val());
});
//로딩화면 세팅
function loadingSetting() {
    var docHeight = $(document).height() - 100;
    var winHeight = $(window).height() - 100;
    var loadingPageSrc = '<div id="loading" align="center" valign="middle" style="display:none;" >' +
                            '<table width="100%" height=' + winHeight + ' border=0>' +
                            '<tr><td align=center valign=middle>' +
                            '<img src="/Image/Loading.gif" />' +
                            '</td></tr>' +
                            '</table>' +
                            '</div>';
    $(document.body).prepend(loadingPageSrc);
    $("#loading").css({
        opacity: '0.5',
        position: 'absolute',
        top: '0',
        width: '99%',
        height: docHeight,
        background: '#FFF',
        display: '',
        zIndex: '1'
    });
    $("#loading").hide();
}
/*Ajax로딩 화면 띄우기*/
function loading() {
    $("#loading").show();
}
/*Ajax로딩 화면 감추기*/
function loadingout() {
    $("#loading").hide();
}
function numOnly(event) {
    if (event.keyCode == 9 || event.keyCode == 116) {
        return true;
    }
    return !String.fromCharCode(event.keyCode).match(/[^0-9a-i^\`\b\r]/);
}
function PhonenumOnly(event) {
    if (event.keyCode == 9 || event.keyCode == 116 || event.keyCode == 189 || event.keyCode == 109) {
        return true;
    }
    return !String.fromCharCode(event.keyCode).match(/[^0-9a-i^\`\b\r]/);
}
function keyup(obj) {
    if (obj.value == "") {
        return;
    }
    var pBuffer = unNumberFormat(obj.value);
    pBuffer = Number(pBuffer) + "";
    obj.value = numberFormat(pBuffer);
}
function keyupNumberOnly(obj) {
    if (obj.value == "") {
        return;
    }
}
function numberFormat(num) {
    num = num + "";
    var pattern = /(-?[0-9]+)([0-9]{3})/;
    while (pattern.test(num)) {
        num = num.replace(pattern, "$1,$2");
    }
    return num;
}
function unNumberFormat(num) {
    return (num.replace(/\,/g, ""));
}
function sumNumber(elementName) {
    var data1 = $("#" + elementName + "NotTax").val();
    var data2 = $("#" + elementName + "Tax").val();

    data1 = unNumberFormat(data1);
    data2 = unNumberFormat(data2);
    data1 = Number(data1);
    data2 = Number(data2);
    var sumdata = data1 + data2;
    $("#" + elementName).val(numberFormat(sumdata+""));
}
function LangChange(pType) {
    $("#Lang").val(pType);
    document.menuPost.submit();
}
function ErrorMessageBox(code) {
    $.ajax({
        url: '/Home/ErrorMsg',
        type: "GET",
        dataType: "text",
        data: "code=" + code,
        success: function (data) {
            alert(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
function popup(url,width,height,option) {
    var foc = window.open(url, "LogisticPopup", "width=" + width + ", height=" + height + ","+option);
    foc.focus();
}