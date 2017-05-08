$(function () {

    //sign out
    $("input[type=button].signout").bind("click", function () {
        location.href = "/Home/Signout";
    });
    //date-left
    $("span.main-date-arrow.glyphicon-arrow-left").bind("click", function () {
        AddMonth(-1);
        Search();
    });
    //date-right
    $("span.main-date-arrow.glyphicon-arrow-right").bind("click", function () {
        AddMonth(1);
        Search();
    });
    $("div.main-date > select, select#searchDaySelect, select#searchTypeSelect").bind("change", function () {
        Search();
    });
    $("input[type=button]#searchInit").bind("click", function () {
        $("select#searchDaySelect,select#searchTypeSelect").val("");
        Search();
    });

    $("input[type=tel]").keydown(function (data, handler) {
        if (data.key === "Backspace") {
            return true;
        }
        if (data.key.match(/[0-9]/) == null) {
            return false;
        } else {
            return true;
        }
    });

    Init();
    Search();
});

function Init() {
    var date = new Date();
    $("#householdYear").val(date.getFullYear());
    $("#householdMonth").val(date.getMonth() + 1);
    $("#householdDay_pc").val(date.getDate());
    $("#householdDay_mobile").val(date.getDate());
}

function MoneyInit() {
    $("table.table-data.table-data1 > tbody").html($("div.template > table.template-data1-nothing > tbody").html());
    $("table.table-data.table-data2 > tbody").html($("div.template > table.template-data2-nothing > tbody").html());
    $("table.table-data.table-data3 > tbody").html($("div.template > table.template-data3-nothing > tbody").html());
    ViewTotal("span#totalMoney1", 0, "0");
    ViewTotal("span#totalMoney2", 0, "0");
    ViewTotal("span#totalMoney3", 0, "0");
}

function ViewTotal(name, val, valStr) {
    $(name).html(ConvertMoneyStr(val, valStr));
    $(name).removeClass("money-plus");
    $(name).removeClass("money-minus");
    $(name).removeClass("money-zero");
    if (val < 0) {
        $(name).addClass("money-minus");
    } else if (val > 0) {
        $(name).addClass("money-plus");
    } else {
        $(name).addClass("money-zero");
    }
}

function ConvertMoneyStr(val, valStr) {
    return "￥" + valStr.replace("-", "");
}

function Search() {
    var year = parseInt($("#householdYear").val());
    var month = parseInt($("#householdMonth").val());
    var day = parseInt($("#searchDaySelect").val());
    if (isNaN(day)) {
        day = "";
    }
    var type = $("#searchTypeSelect").val();
    MoneyInit();
    SendAjax("Search", "year=" + year + "&month=" + month + "&day=" + day + "&type=" + type, SearchData);
}

function AddMonth(addmonth) {
    var year = parseInt($("#householdYear").val());
    var month = parseInt($("#householdMonth").val());
    month += addmonth;
    if (month < 1) {
        month += 12;
        year--;
    } else if (month > 12) {
        month -= 12;
        year++;
    }
    //householdDay
    $("#householdYear").val(year);
    $("#householdMonth").val(month);
}

function SendAjax(name, data, method) {
    $(".lodding").removeClass("lodding-off");
    $.ajax({
        url: "/AJAX/" + name,
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data, textStatus, jqXHR) {
            if (data.result === "SIGNERROR") {
                SetErrorMsg(ERROR0004, 0);
                location.href = "/Home/Index";
            }
            if (data.result === "NG") {
                SetErrorMsg(data.error, 0);
            }
            if (data.result === "OK") {
                setTimeout(method, 1, data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            SetErrorMsg(ERROR0000, 0);
        },
        complete: function (jqXHR, textStatus) {
            $(".lodding").addClass("lodding-off");
        }
    });
}

function SearchData(data) {
    console.log(data);
    var list;
    var val = data.DATA;
    ViewTotal("span#totalMoney1", val.TotalAmountNum, val.TotalAmount);

    $("#Income").html(ConvertMoneyStr(val.IncomeAmountNum, val.IncomeAmount));
    $("#expend").html(ConvertMoneyStr(val.ExpendAmountNum, val.ExpendAmount));

    list = val.TotalList;
    if (list.length > 0) {
        $("table.table-data.table-data1 > tbody").html("");
        for (i = 0; i < list.length; i++) {
            var item = list[i];
            var dom = $("div.template > table.template-data1 > tbody").html();
            dom = dom.replace(/##DATA##/gi, CreateData(item));
            if (item.Day === "--") {
                dom = dom.replace(/##HOVER##/gi, "class='credit'");
            } else {
                dom = dom.replace(/##HOVER##/gi, "");
            }
            dom = dom.replace(/##DATE##/gi, item.Day);
            dom = dom.replace(/##CATEGORY##/gi, item.Category);
            dom = dom.replace(/##TYPE##/gi, item.Type);
            dom = dom.replace(/##CONTENTS##/gi, item.Content);
            dom = dom.replace(/##PRICE##/gi, ConvertMoneyStr(item.PriceNum, item.Price));
            if (item.PriceNum < 0) {
                dom = dom.replace(/##CLASS##/gi, "money-minus");
            } else if (item.PriceNum > 0) {
                dom = dom.replace(/##CLASS##/gi, "money-plus");
            } else {
                dom = dom.replace(/##CLASS##/gi, "money-zero");
            }
            $("table.table-data.table-data1 > tbody").append(dom);
        }
    }
    ViewTotal("span#totalMoney2", val.AccountAmountNum, val.AccountAmount);
    list = val.AccountList;
    if (list.length > 0) {
        $("table.table-data.table-data2 > tbody").html("");
        for (i = 0; i < list.length; i++) {
            var item = list[i];
            var dom = $("div.template > table.template-data2 > tbody").html();
            dom = dom.replace(/##DATE##/gi, item.Day);
            dom = dom.replace(/##TYPE##/gi, item.Type);
            dom = dom.replace(/##CONTENTS##/gi, item.Content);
            dom = dom.replace(/##PRICE##/gi, ConvertMoneyStr(item.PriceNum, item.Price));
            if (item.PriceNum < 0) {
                dom = dom.replace(/##CLASS##/gi, "money-minus");
            } else if (item.PriceNum > 0) {
                dom = dom.replace(/##CLASS##/gi, "money-plus");
            } else {
                dom = dom.replace(/##CLASS##/gi, "money-zero");
            }
            $("table.table-data.table-data2 > tbody").append(dom);
        }
    }
    ViewTotal("span#totalMoney3", val.CreditAmountNum, val.CreditAmount);
    list = val.CreditList;
    if (list.length > 0) {
        $("table.table-data.table-data3 > tbody").html("");
        for (i = 0; i < list.length; i++) {
            var item = list[i];
            var dom = $("div.template > table.template-data3 > tbody").html();
            dom = dom.replace(/##DATE##/gi, item.Day);
            dom = dom.replace(/##TYPE##/gi, item.Type);
            dom = dom.replace(/##CONTENTS##/gi, item.Content);
            dom = dom.replace(/##PRICE##/gi, ConvertMoneyStr(item.PriceNum, item.Price));
            if (item.PriceNum < 0) {
                dom = dom.replace(/##CLASS##/gi, "money-minus");
            } else if (item.PriceNum > 0) {
                dom = dom.replace(/##CLASS##/gi, "money-plus");
            } else {
                dom = dom.replace(/##CLASS##/gi, "money-zero");
            }
            $("table.table-data.table-data3 > tbody").append(dom);
        }
    }

    SetEvent();
    SetEventPc();
    SetEventMobile();
}

function SetEvent() {
    $("table.table-data1 > tbody > tr:not(.credit)").on("click", function () {
        if ($(this).hasClass("nothing")) {
            return;
        }
        ClearList();
        $(this).addClass("click");
    });
}

function SetErrorMsg(msg, time) {
    $("#error").html(msg);
    if (time > 0) {
        setTimeout(SetErrorMsg, time, "", 0);
    }
}

function CreateData(val) {
    var ret = val.Idx + ",";
    ret += val.Day + ",";
    ret += val.Cd + ",";
    ret += val.Tp + ",";
    ret += val.Content + ",";
    ret += val.PriceNum + ",";
    ret += val.Pdt;
    return ret;
}

function CreateClass(val) {
    var res = val.split(",");
    this.Idx = res[0];
    this.Day = res[1];
    this.Cd = res[2];
    this.Tp = res[3];
    this.Content = res[4];
    this.Price = res[5];
    this.Pdt = res[6];
}
