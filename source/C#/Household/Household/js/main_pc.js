/*onResize*/
$(window).resize(function () {
    ClearPc();
});

$(function () {
    //apply
    $("input[type=button]#applySubmit_pc").bind("click", function () {
        if (!ValidatePc()) {
            return;
        }
        var formdata = $("#applyPc").serialize();
        //add
        SendAjax("Apply", formdata, ApplyDataPc);
    });

    //cancel
    $("input[type=button]#cancelSubmit_pc").bind("click", function () {
        ClearPc();
    });

    //modify
    $("input[type=button]#modifySubmit_pc").bind("click", function () {
        if (!ValidatePc()) {
            return;
        }
        var formdata = $("#applyPc").serialize();
        SendAjax("Modify", formdata, ModifyDataPc);
    });

    //delete
    $("input[type=button]#deleteSubmit_pc").bind("click", function () {
        var formdata = $("#applyPc").serialize();
        SendAjax("Delete", formdata, DeleteDataPc);
    });

    $("#householdCategory_pc").bind("change", function () {
        ChangeHouseholdTypePc();
    });

    ChangeHouseholdTypePc();
    InitPc();
});
function SetEventPc() {
    ClearPc();
    // entity click
    $("table.table-data1 > tbody > tr:not(.credit)").on("click", function () {
        $("table.table-input.pc-private div.apply-area").addClass("off");
        $("table.table-input.pc-private div.modify-area").removeClass("off");

        var v = $(this).children(0).children("input").val();
        var c = new CreateClass(v);
        $("#householdIdx_pc").val(c.Idx);
        $("#householdPdt_pc").val(c.Pdt);
        $("#householdDay_pc").val(c.Day);
        $("#householdCategory_pc").val(c.Cd);
        ChangeHouseholdTypePc();
        $("#householdType_pc").val(c.Tp);
        $("#householdContent_pc").val(c.Content);
        $("#householdPrice_pc").val(c.Price > 0 ? c.Price : c.Price * -1);
        location.href = "#top";
    });
}

function ClearPc() {
    ClearList();
    $("table.table-input.pc-private div.apply-area").removeClass("off");
    $("table.table-input.pc-private div.modify-area").addClass("off");
    $("#householdIdx_pc").val("");
    $("#householdContent_pc").val("");
    $("#householdPrice_pc").val("");
}

function ClearList() {
    $("table.table-data1 > tbody > tr").each(function () {
        $(this).removeClass("click");
    });
}

function ChangeHouseholdTypePc() {
    var name = "#select_" + $("#householdCategory_pc").val();
    var dom = $(name).html();
    $("#householdType_pc").html(dom);
}

function ApplyDataPc(data) {
    ClearPc();
    SetErrorMsg(INFO00001, 3000);
    Search();
}

function ModifyDataPc(data) {
    ClearPc();
    SetErrorMsg(INFO00002, 3000);
    Search();
}

function DeleteDataPc(data) {
    ClearPc();
    SetErrorMsg(INFO00003, 3000);
    Search();
}

function ValidatePc() {
    var month = parseInt($("#householdMonth").val());
    var date = new Date($("#householdYear").val(), month - 1, $("#householdDay_pc").val());
    if (month !== date.getMonth() + 1) {
        SetErrorMsg(ERROR0001,3000);
        return false;
    }
    var contents = $("#householdContent_pc").val();
    if (contents === null || contents.trim() === "") {
        SetErrorMsg(ERROR0002, 3000);
        return false;
    }
    var price = $("#householdPrice_pc").val();
    if (price === null || price.trim() === "") {
        SetErrorMsg(ERROR0003, 3000);
        return false;
    }
    if (parseInt(price) < 0) {
        SetErrorMsg(ERROR0005, 3000);
        return false;
    }
    return true;
}

function InitPc() {
    $("#householdCategory_pc").val("000");
    $("#householdType_pc").val("002");
}
