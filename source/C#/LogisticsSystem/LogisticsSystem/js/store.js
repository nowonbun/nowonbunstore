//*********************입고등록
function getProduct() {
    if ($("#productType").val() != "") {
        loading();
        $.ajax({
            url: '/Store/ProductSelect',
            type: "POST",
            data: "idx=" + $("#productType").val(),
            dataType: "json",
            success: function (data) {
                $("#productprice").val(numberFormat(data.productCost));
                totalcalcul();
                loadingout();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                loadingout();
                location.href = "/Home/Error";
            }
        });
    } else {
        $("#productprice").val("");
    }
}
function totalcalcul() {
    var amount = $("#productAmount").val();
    var price = $("#productprice").val();
    amount = unNumberFormat(amount);
    price = unNumberFormat(price);
    $("#totalprice").val(numberFormat(amount*price));
}
function GoSubmit(type) {
    if (type == 1) {
        document.forms[0].action = "/Store/ApplyInsert";
        document.forms[0].submit();
    } else if (type == 2) {
        document.forms[0].action = "/Store/ReleaseInsert";
        document.forms[0].submit();
    }
}
function GoBack(type) {
    if (type == 1) {
        document.forms[0].action = "/Store/ApplyAdd?key=BACK";
        document.forms[0].submit();
    } else if (type == 2) {
        document.forms[0].action = "/Store/ReleaseAdd?key=BACK";
        document.forms[0].submit();
    }
}
//**************************************입고승인페이지
function ApplyCheckList(page) {
    loading();
    $.ajax({
        url: '/Store/ApplyListSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listTemplate1(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ApplyCheckList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ApplyCheckList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ApplyCheckList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function listTemplate1(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##ProductName##/gi, pNode.productname);
    dom = dom.replace(/##ProductAmount##/gi, numberFormat(pNode.productAmount));
    dom = dom.replace(/##ProductPrice##/gi, numberFormat(pNode.productbuyPrice));
    dom = dom.replace(/##ProductMoney##/gi, numberFormat(pNode.productAmount * pNode.productbuyPrice));
    dom = dom.replace(/##ProductState##/gi, pNode.state);
    $("#listData").append(dom);
}
function applyApprove(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Store/ApplyApprovePage");
    $("#submitForm").submit();
}
function OrderPageOpen() {
    $("#orderPage").show();
    $("#openBtn").hide();
}
function returnPage(type) {
    if (type == 1) location.href = "/Store/ApplyCheckList";
    if (type == 2) location.href = "/Store/ApplyList";
    if (type == 3) location.href = "/Store/ReleaseCheckList";
    if (type == 4) location.href = "/Store/ReleaseList";
}
function applyapproveOK() {
    document.forms[0].action = "/Store/ApplyApprove?key=approve";
    document.forms[0].submit();
}
function applyapproveCancel() {
    document.forms[0].action = "/Store/ApplyApprove?key=cancel";
    document.forms[0].submit();
}
//*******************************입고검색리스트
function ApplyList(page) {
    loading();
    $.ajax({
        url: '/Store/ApplyListearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listTemplate2(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ApplyList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ApplyList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ApplyList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function listTemplate2(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##ProductName##/gi, pNode.productname);
    dom = dom.replace(/##ProductAmount##/gi, numberFormat(pNode.productAmount));
    dom = dom.replace(/##ProductPrice##/gi, numberFormat(pNode.productbuyPrice));
    dom = dom.replace(/##ProductMoney##/gi, numberFormat(pNode.productAmount * pNode.productbuyPrice));
    dom = dom.replace(/##ProductState##/gi, pNode.stateDisp);
    if (pNode.state == "1") dom = dom.replace(/##statecolor##/gi, "yellow");
    else if (pNode.state == "5") dom = dom.replace(/##statecolor##/gi, "red");
    else if (pNode.state == "2") dom = dom.replace(/##statecolor##/gi, "#00ffc9");
    $("#listData").append(dom);
}
function listview(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Store/ApprovePage");
    $("#submitForm").submit();
}
//**************************출고승인페이지
function ReleaseCheckList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Store/ReleaseApproveSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listTemplate3(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ReleaseCheckList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ReleaseCheckList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ReleaseCheckList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function listTemplate3(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##ProductName##/gi, pNode.productname);
    dom = dom.replace(/##ProductAmount##/gi, numberFormat(pNode.productAmount));
    dom = dom.replace(/##ProductPrice##/gi, numberFormat(pNode.productSellPrice));
    dom = dom.replace(/##ProductMoney##/gi, numberFormat(pNode.productAmount * pNode.productSellPrice));
    $("#listData").append(dom);
}
function ReleaseApprove(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Store/ReleaseApprovePage");
    $("#submitForm").submit();
}
function releaseapproveOK() {
    document.forms[0].action = "/Store/ReleaseApprove?key=approve";
    document.forms[0].submit();
}
function releaseapproveCancel() {
    document.forms[0].action = "/Store/ReleaseApprove?key=cancel";
    document.forms[0].submit();
}
//***********************출고리스트
function ReleaseList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Store/ReleaseListSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listTemplate4(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ReleaseList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ReleaseList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ReleaseList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function listTemplate4(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##ProductName##/gi, pNode.productname);
    dom = dom.replace(/##ProductAmount##/gi, numberFormat(pNode.productAmount));
    dom = dom.replace(/##ProductPrice##/gi, numberFormat(pNode.productSellPrice));
    dom = dom.replace(/##ProductMoney##/gi, numberFormat(pNode.productAmount * pNode.productSellPrice));
    dom = dom.replace(/##ProductState##/gi, pNode.stateDisp);
    if (pNode.state == "3") dom = dom.replace(/##statecolor##/gi, "yellow");
    else if (pNode.state == "6") dom = dom.replace(/##statecolor##/gi, "red");
    else if (pNode.state == "4") dom = dom.replace(/##statecolor##/gi, "#00ffc9");
    $("#listData").append(dom);
}
function releaselistview(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Store/ReleasePage");
    $("#submitForm").submit();
}
//*********************입출고 수불입
function StoreList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Store/StoreListSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listTemplate5(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='StoreList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='StoreList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='StoreList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function listTemplate5(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##ProductName##/gi, pNode.productname);
    dom = dom.replace(/##ProductType##/gi, pNode.producttype);
    dom = dom.replace(/##ProductInput##/gi, numberFormat(pNode.productInput));
    dom = dom.replace(/##ProductOutput##/gi, numberFormat(pNode.productOutput));
    dom = dom.replace(/##ProductMoney##/gi, numberFormat(pNode.productmoney));
    dom = dom.replace(/##ProductDate##/gi, pNode.createdateString);
    $("#listData").append(dom);
}