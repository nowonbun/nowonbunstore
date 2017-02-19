/*onResize*/
var w_width = 0;
$(window).resize(function () {
    if (w_width == 0) {
        w_width = $(window).width();
        return;
    }
    if (w_width != $(window).width()) {
        w_width = $(window).width();
        ClearMobile();
    }
});
/* init*/
$(function () {
    /* The folding event*/
    $("div.title-data").bind("click", function () {
        $(this).children().children("span.glyphicon-plus").toggleClass("off");
        $(this).children().children("span.glyphicon-minus").toggleClass("off");
        $(this).next().toggleClass("mobile-off");
    });

    /* The register button event and popup*/
    $("input#apply_mobile").bind("click", function () {
        $("html").addClass("fixed");

        $("div.layout").removeClass("off");
        $("div.apply").removeClass("off");
        $("div.apply").removeClass("height380");
        $("div.apply").addClass("height340");

        $("div.apply-area.mobile-private").removeClass("off");
        $("div.modify-area.mobile-private").addClass("off");

        $("input[type=text]#householdContent_mobile").focus();

        $("#householdYear_mobile").val($("#householdYear").val());
        $("#householdMonth_mobile").val($("#householdMonth").val());
    });

    //apply
    $("input[type=button]#applySubmit_mobile").bind("click", function () {
        if (!ValidateMobile()) {
            return;
        }
        var formdata = $("#apply_form_mobile").serialize();
        ClearMobile();
        SendAjax("Apply", formdata, ApplyDataMobile);
    });

    //modify
    $("input[type=button]#modifySubmit_mobile").bind("click", function () {
        if (!ValidateMobile()) {
            return;
        }
        var formdata = $("#apply_form_mobile").serialize();
        ClearMobile();
        SendAjax("Modify", formdata, ModifyDataMobile);
    });

    //delete
    $("input[type=button]#deleteSubmit_mobile").bind("click", function () {
        var formdata = $("#apply_form_mobile").serialize();
        ClearMobile();
        SendAjax("Delete", formdata, DeleteDataMobile);
    });

    /* The popup close button event*/
    $("div.apply > div.title > span.glyphicon.glyphicon-remove").bind("click", function () {
        ClearMobile();
    });

    //calculater
    $("input[type=button]#calc_mobile").bind("click", function () {
        $("div.calc").removeClass("off");
        InitCalc(true);
    });

    $("div.layout > div.calc > div.calcmain > div.title  > span").bind("click", function () {
        InitCalc(false);
        $("div.calc").addClass("off");
    });

    $("input[type=button]#calc_add").bind("click", function () {
        AddCalcNumber();
    });

    $("input[type=button]#calc_subtract").bind("click", function () {
        SubtractCalcNumber();
    });

    $("input[type=button]#calc_multiply").bind("click", function () {
        MultiplyCalcNumber();
    });

    $("input[type=button]#calc_division").bind("click", function () {
        DivisionCalcNumber();
    });

    $("input[type=button]#calc_clear").bind("click", function () {
        InitCalc(true);
    });

    $("#calc_result").bind("click", function () {
        var buffer = $("input[type=tel]#calc_sum").val();
        $("#householdPrice_mobile").val(buffer);
        InitCalc(false);
        $("div.calc").addClass("off");
    });

    $("#householdCategory_mobile").bind("change", function () {
        ChangeHouseholdTypeMobile();
    });

    ChangeHouseholdTypeMobile();
    InitMobile();

});

function InitCalc(focus) {
    $("input[type=tel]#calc").val("");
    $("input[type=tel]#calc_sum").val(0);
    if (focus) {
        $("input[type=tel]#calc").focus();
    }
}

function SetCalc(val) {
    $("input[type=tel]#calc").val("");
    $("input[type=tel]#calc_sum").val(val);
    $("input[type=tel]#calc").focus();
}

function AddCalcNumber() {
    var a = parseInt($("input[type=tel]#calc").val());
    var b = parseInt($("input[type=tel]#calc_sum").val());
    if (isNaN(a)) {
        a = 0;
    }
    if (isNaN(b)) {
        b = 0;
    }
    SetCalc(a+b);
}

function SubtractCalcNumber() {
    var a = parseInt($("input[type=tel]#calc").val());
    var b = parseInt($("input[type=tel]#calc_sum").val());
    if (isNaN(a)) {
        a = 0;
    }
    if (isNaN(b)) {
        b = 0;
    }
    var buffer = b - a;
    if (buffer < 0) {
        buffer = 0;
    }
    SetCalc(buffer);
}

function MultiplyCalcNumber() {
    var a = parseInt($("input[type=tel]#calc").val());
    var b = parseInt($("input[type=tel]#calc_sum").val());
    if (isNaN(a)) {
        a = 0;
    }
    if (isNaN(b)) {
        b = 0;
    }
    SetCalc(a * b);
}

function DivisionCalcNumber() {
    var a = parseInt($("input[type=tel]#calc").val());
    var b = parseInt($("input[type=tel]#calc_sum").val());
    if (isNaN(a)) {
        a = 0;
    }
    if (isNaN(b)) {
        b = 0;
    }
    var c = b / a;
    if (isNaN(c)) {
        c = 0;
    }
    c = Math.round(c);
    SetCalc(c);
}


function GetNumber(tag) {
    var val = parseInt($("td.calc-txt > input[type=tel]").val());
    if (isNaN(val)) {
        val = 0;
    }
    return val;
}

function SetEventMobile() {
    ClearMobile();
    // entity click
    $("table.table-data1 > tbody > tr:not(.credit)").on("click", function () {

        $("html").addClass("fixed");
        $("div.layout").removeClass("off");
        $("div.apply").removeClass("off");
        $("div.apply").removeClass("height340");
        $("div.apply").addClass("height380");

        $("div.apply-area.mobile-private").addClass("off");
        $("div.modify-area.mobile-private").removeClass("off");

        $("input[type=text]#householdContent_mobile").focus();

        var v = $(this).children(0).children("input").val();
        var c = new CreateClass(v);
        $("#householdYear_mobile").val($("#householdYear").val());
        $("#householdMonth_mobile").val($("#householdMonth").val());
        $("#householdIdx_mobile").val(c.Idx);
        $("#householdPdt_mobile").val(c.Pdt);
        $("#householdDay_mobile").val(c.Day);
        $("#householdCategory_mobile").val(c.Cd);
        ChangeHouseholdTypeMobile();
        $("#householdType_mobile").val(c.Tp);
        $("#householdContent_mobile").val(c.Content);
        $("#householdPrice_mobile").val(c.Price > 0 ? c.Price : c.Price * -1);
    });
}

function ClearMobile() {
    $("html").removeClass("fixed");
    $("div.layout").addClass("off");
    $("div.apply").addClass("off");
    $("div.apply-area.mobile-private,div.modify-area.mobile-private").removeClass("off");
    $("div.calc").addClass("off");

    ClearMobileData();
}

function ChangeHouseholdTypeMobile() {
    var name = "#select_" + $("#householdCategory_mobile").val();
    var dom = $(name).html();
    $("#householdType_mobile").html(dom);
}

function InitMobile() {
    $("#householdCategory_mobile").val("000");
    $("#householdType_mobile").val("002");
    ClearMobileData();
}

function ClearMobileData() {
    $("#householdYear_mobile").val("");
    $("#householdMonth_mobile").val("");
    $("#householdIdx_mobile").val("");
    $("#householdPdt_mobile").val("");
    $("#householdContent_mobile").val("");
    $("#householdPrice_mobile").val("");
}

function ValidateMobile() {
    var month = parseInt($("#householdMonth_mobile").val());
    var date = new Date($("#householdYear_mobile").val(), month - 1, $("#householdDay_mobile").val());
    if (month !== date.getMonth() + 1) {
        SetErrorMobileMsg(ERROR0001, 3000);
        return false;
    }
    var contents = $("#householdContent_mobile").val();
    if (contents === null || contents.trim() === "") {
        SetErrorMobileMsg(ERROR0002, 3000);
        return false;
    }
    var price = $("#householdPrice_mobile").val();
    if (price === null || price.trim() === "") {
        SetErrorMobileMsg(ERROR0003, 3000);
        return false;
    }
    if (parseInt(price) < 0) {
        SetErrorMobileMsg(ERROR0005, 3000);
        return false;
    }
    return true;
}

function ApplyDataMobile(data) {
    SetErrorMsg(INFO00001, 3000);
    Search();
}

function ModifyDataMobile(data) {
    SetErrorMsg(INFO00002, 3000);
    Search();
}

function DeleteDataMobile(data) {
    SetErrorMsg(INFO00003, 3000);
    Search();
}

function SetErrorMobileMsg(msg, time) {
    $(".error_mobile").html(msg);
    if (time > 0) {
        setTimeout(SetErrorMobileMsg, time, "", 0);
    }
}