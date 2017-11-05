
var nowPage = 0;
var preTab = null;
var bufferCode = "";

$(function () {
    tabMenu($("#tab1"));
});

function tabMenu(obj) {
    if (preTab != null) {
        $(preTab).css("background-color", "#6fBBD4");
        $("#" + $(preTab).attr("id") + "content").hide();
    }
    $(obj).css("background-color", "#9a9a9a");
    $("#" + $(obj).attr("id") + "content").show();
    preTab = obj;
}
//코드생성
function CodeCreate() {
    if ($("#customerCode").val() != "") {
        ErrorMessageBox('0003');
        return true;
    } else {
        $.ajax({
            url: '/Customer/CodeCreate',
            type: "POST",
            dataType: "text",
            success: function (data) {
                $("#customerCode").val(data);
                $("#codeBtn").prop("disabled", true);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                loadingout();
                location.href = "/Home/Error";
            }
        });
    }
    return true;
}
//고객입력
function CustomerInsert() {
    if ($("#productcode").val() == "") {
        ErrorMessageBox("1000");
        return;
    }
    loading();
    var formdata = $("#customerInsertForm").serialize();
    $.ajax({
        url: "/Customer/Insert",
        type: "POST",
        dataType: "text",
        data: formdata,
        success: function (data) {
            if (data == "SESSIONOUT") {
                location.href = "/";
            } else if (data != "") {
                $("#Errormsg").html(data);
            } else {
                $("#Errormsg").html("");
                CustomerList(nowPage);
                bufferCode = "CC-" + $("#customerCode").val();
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//고객초기화
function CustomerClear() {
    CustomerInit();
    $("#ApplySet").show();
    $("#codeBtn").prop("disabled", false);
    $("#ModifySet").hide();
    $("#tab2").hide();
}
//고객리스트 (Ajax용)
function CustomerList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Customer/ListSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listTemplate(data["item" + i]);
            }
            pagecount = Math.ceil(data["totalcount"] / data["limit"]);
            prepage = (Math.floor(page / 10) * 10) + 1;
            if (prepage < 1) {
                prepage = 1;
            }
            nextpage = prepage + 10;
            if (nextpage > pagecount) {
                nextpage = pagecount;
            }
            var dom = "";
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='CustomerList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='CustomerList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='CustomerList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            if (bufferCode != "") {
                CustomerSearch(bufferCode);
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//리스트클릭시 고객정보
function CustomerSearch(code) {
    loading();
    $.ajax({
        url: '/Customer/CustomerSearch',
        type: "POST",
        data: "code=" + code,
        dataType: "json",
        success: function (data) {
            CustomerInit();
            ContentsDataSetting(data);

            $("#ApplySet").hide();
            $("#ModifySet").show();
            $("#tab2").show();
            $("#HistorySet").hide();
            bufferCode = "";
            $("#codeBtn").prop("disabled", true);
            CustomerHistoryList(1, code);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
// 히스토리 검색
function CustomerHistoryList(page, code) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Customer/HistorySearch',
        type: "POST",
        data: "page=" + page + "&code=" + code,
        dataType: "json",
        success: function (data) {
            $("#historylist").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listHistoryTemplate(data["item" + i]);
            }
            pagecount = Math.ceil(data["totalcount"] / data["limit"]);
            prepage = (Math.floor(page / 10) * 10) + 1;
            if (prepage < 1) {
                prepage = 1;
            }
            nextpage = prepage + 10;
            if (nextpage > pagecount) {
                nextpage = pagecount;
            }
            var dom = "";
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='CustomerHistoryList(" + prepage + ",\"" + code + "\");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='CustomerHistoryList(" + i + ",\"" + code + "\");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='CustomerHistoryList(" + nextpage + ",\"" + code + "\");return false;'>▶</a>&nbsp;";
            $("#historypaging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//히스토리 리스트에서 클릭시(검색시)
function CustomerHistorySearch(idx){
    loading();
    $.ajax({
        url: '/Customer/CustomerHistorySearch',
        type: "POST",
        data: "idx=" + idx,
        dataType: "json",
        success: function (data) {
            CustomerInit();
            ContentsDataSetting(data);
            $("#ApplySet").hide();
            if (data.state == 0) {
                $("#ModifySet").show();
                $("#HistorySet").hide();
            } else {
                $("#ModifySet").hide();
                $("#HistorySet").show();
            }
            bufferCode = "";
            $("#codeBtn").prop("disabled", true);
            loadingout();
            $("#tab1").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//삭제쿼리
function CustomerDelete() {
    loading();
    var code = "CC-" + $("#customerCode").val();
    $.ajax({
        url: "/Customer/Delete",
        type: "POST",
        dataType: "text",
        data: "code=" + code,
        success: function (data) {
            if (data == "SESSIONOUT") {
                location.href = "/";
            } else {
                CustomerList(nowPage);
                CustomerClear();
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
///수정 처리
function CustomerModify() {
    loading();
    var formdata = $("#customerInsertForm").serialize();
    $.ajax({
        url: "/Customer/Modify",
        type: "POST",
        dataType: "text",
        data: formdata,
        success: function (data) {
            if (data == "SESSIONOUT") {
                location.href = "/";
            } else if (data != "") {
                $("#Errormsg").html(data);
            } else {
                $("#Errormsg").html("");
                CustomerList(1);
                bufferCode = "CC-" + $("#customerCode").val();
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}

function CustomerInit() {
    $("#customerCode").val("");
    $("#customerName").val("");
    $("#customerRepresetitive").val("");
    $("#customerSecurityNumber").val("");
    $("#customerNumber").val("");
    $("#customerFax").val("");
    $("#customerPostNumber1").val("");
    $("#customerPostNumber2").val("");
    $("#customerAddress").val("");
    $("#customerEmail").val("");
    $("#customerTaxViewRepresentative").val("");
    $("#customerTaxViewerPostNumber1").val("");
    $("#customerTaxViewerPostNumber2").val("");
    $("#customerTaxViewerAddress").val("");
    $("#customerAccountbank").val("");
    $("#customerAccountbankcodename").val("");
    $("#customerAccountbankcode").val("");
    $("#customerAccountOwnerName").val("");
    $("#customerAccountNumber").val("");
    $("#customerRepressent").val("");
    $("#customerRepressentNumber").val("");
    $("#creater").val("");
    $("#createdate").val("");
    $("#other").val("");

    $("#customerType").val($('#customerType option:eq(0)').val());
    $("#customerPaymentMethod").val($('#customerPaymentMethod option:eq(0)').val());
    $("#customerTaxType").val($('#customerTaxType option:eq(0)').val());
    $("#customerTax").val($('#customerTax option:eq(0)').val());
    $("#customerGrade").val($('#customerGrade option:eq(0)').val());
}
// 고객리스트 템플릿
function listTemplate(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##customercode##/gi, pNode.customercode);
    dom = dom.replace(/##customername##/gi, pNode.customerName);
    dom = dom.replace(/##representative##/gi, pNode.customerRepresetitive);
    dom = dom.replace(/##customersecuritynumber##/gi, pNode.customerSecurityNumber);
    dom = dom.replace(/##customernumber##/gi, pNode.customerNumber);
    dom = dom.replace(/##customerfax##/gi, pNode.customerFax);
    dom = dom.replace(/##customeremail##/gi, pNode.customerEmail);
    $("#listData").append(dom);
}
// 고객정보란 세팅함수
function ContentsDataSetting(data) {
    $("#customerCode").val(data.customercode.replace(/CC-/gi, ""));
    $("#customerName").val(data.customerName);
    $("#customerRepresetitive").val(data.customerRepresetitive);
    $("#customerSecurityNumber").val(data.customerSecurityNumber);
    $("#customerNumber").val(data.customerNumber);
    $("#customerFax").val(data.customerFax);
    $("#customerPostNumber1").val(data.customerPostNumber1);
    $("#customerPostNumber2").val(data.customerPostNumber2);
    $("#customerAddress").val(data.customerAddress);
    $("#customerEmail").val(data.customerEmail);
    $("#customerTaxViewRepresentative").val(data.customerTaxViewRepresentative);
    $("#customerTaxViewerPostNumber1").val(data.customerTaxViewerPostNumber1);
    $("#customerTaxViewerPostNumber2").val(data.customerTaxViewerPostNumber2);
    $("#customerTaxViewerAddress").val(data.customerTaxViewerAddress);
    $("#customerAccountbank").val(data.customerAccountbank);
    $("#customerAccountbankcodename").val(data.customerAccountbankcodename);
    $("#customerAccountbankcode").val(data.customerAccountbankcode);
    $("#customerAccountOwnerName").val(data.customerAccountOwnerName);
    $("#customerAccountNumber").val(data.customerAccountNumber);
    $("#customerRepressent").val(data.customerRepressent);
    $("#customerRepressentNumber").val(data.customerRepressentNumber);
    $("#creater").val(data.creater);
    $("#createdate").val(data.createdateString);
    $("#other").val(data.other);

    $("#customerType").val(data.customerType);
    $("#customerPaymentMethod").val(data.customerPaymentMethod);
    $("#customerTaxType").val(data.customerTaxType);
    $("#customerTax").val(data.customerTax);
    $("#customerGrade").val(data.customerGrade);
}
//히스토리 리스트
function listHistoryTemplate(pNode) {
    var dom = $("#historytemplate").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##modifier##/gi, pNode.creater);
    dom = dom.replace(/##modifyData##/gi, pNode.createdateString);
    $("#historylist").append(dom);
}