//*********발주서 등록
$(function () {
    $("#createdate").datepicker({
        dateFormat: 'yy-mm-dd'
    });
    $("#perioddate").datepicker({
        dateFormat: 'yy-mm-dd'
    });
    $("#moneydate").datepicker({
        dateFormat: 'yy-mm-dd'
    });
    $("#moneyorderdate").datepicker({
        dateFormat: 'yy-mm-dd'
    });
});
//상품 셀렉트시 관련 정보 취득
function getProduct(idx) {
    loading();
    if ($("#productType_" + idx).val() != "") {
        $("#subNumber_" + idx).html(idx + 1);
        $("#productmount_" + idx).val("0");
        $("#productmount_" + idx).attr("readonly", false);
        priceCalcul(idx);
        $.ajax({
            url: '/Obtain/ProductSelect',
            type: "POST",
            data: "idx=" + $("#productType_" + idx).val(),
            dataType: "json",
            success: function (data) {
                $("#productspec_" + idx).val(data.productspec);
                $("#productprice_" + idx).val(numberFormat(data.productCost));
                loadingout();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                loadingout();
                location.href = "/Home/Error";
            }
        });
    } else {
        $("#subNumber_" + idx).html("");
        $("#productmount_" + idx).val("");
        $("#productmount_" + idx).attr("readonly", true);
        $("#productspec_" + idx).val("");
        $("#productprice_" + idx).val("");
        priceCalcul(idx);
        loadingout();
    }
}
//가격계산
function priceCalcul(idx) {
    mount = unNumberFormat($("#productmount_" + idx).val());
    price = unNumberFormat($("#productprice_" + idx).val());
    $("#producttotal_" + idx).val(numberFormat(mount * price));
    totalCalcul();
}
//전체가격계산
function totalCalcul() {
    var totalprice = 0;
    for (i = 0; i < 10; i++) {
        price = Number(unNumberFormat($("#producttotal_" + i).val()));
        totalprice += price;
    }
    $("#totalamount").val(numberFormat(totalprice));
    $("#obtainprice").val(numberFormat(totalprice));
    $("#money").val(numberFormat(totalprice));
}
function GoSubmit() {
    document.forms[0].action = "/Obtain/Input";
    document.forms[0].submit();
}
function GoBack() {
    document.forms[0].action = "/Obtain/Order?key=BACK";
    document.forms[0].submit();
}

/*************발주승인************/
//검색
function orderApproveListSearch(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Obtain/ListCheckSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listApproveTemplate(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='orderApproveListSearch(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='orderApproveListSearch(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='orderApproveListSearch(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            $("#idxCollection").val(data["idxcollection"]);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//템플릿
function listApproveTemplate(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##OrderNumber##/gi, pNode.ordernumber);
    dom = dom.replace(/##OrderComp##/gi, pNode.inordername);
    dom = dom.replace(/##OrderDate##/gi, pNode.orderdateString);
    dom = dom.replace(/##OrderDeliveryDate##/gi, pNode.ordersavedateString);
    dom = dom.replace(/##OrderCreater##/gi, pNode.creater);
    dom = dom.replace(/##OrderCreateDate##/gi, pNode.createdateString);
    $("#listData").append(dom);
}
//서브메뉴 오픈
function subSearchView(idx) {
    //일단 List가 없기때문에
    list = $("#idxCollection").val().split(',');
    for (i = 0; i < list.length; i++) {
        $("#line_" + list[i]).children().remove();
    }
    $("#templateSubcontents").children().remove();
    $("#line_" + idx).append($("#loadingImage").html());
    subSearch(idx);
}
//서브메뉴 검색
function subSearch(idx) {
    $.ajax({
        url: '/Obtain/SubSearch',
        type: "POST",
        data: "idx=" + idx,
        dataType: "json",
        success: function (data) {
            $("#line_" + idx).children().remove();
            for (i = 0; i < data.count; i++) {
                var subdom = $("#templatecontents").html();
                subdom = subdom.replace(/##number##/gi, i + 1);
                subdom = subdom.replace(/##productName##/gi, data["item" + i].productName);
                subdom = subdom.replace(/##productSpec##/gi, data["item" + i].productSpec);
                subdom = subdom.replace(/##productAmount##/gi, numberFormat(data["item" + i].productAmount));
                subdom = subdom.replace(/##productPrice##/gi, numberFormat(data["item" + i].productPrice));
                subdom = subdom.replace(/##productTotal##/gi, numberFormat(data["item" + i].productMoney));
                $("#templateSubcontents").append(subdom);
            }
            var dom = $("#templateSub").html();
            dom = dom.replace(/##idx##/gi, idx);
            $("#line_" + idx).append(dom);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function orderapprove(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Obtain/ApprovePage");
    $("#submitForm").submit();
}
function returnPage(type) {
    if (type == 1) location.href = "/Obtain/OrderApproveList";
    if (type == 2) location.href = "/Obtain/OrderList";
}
function approve() {
    document.forms[0].action = "/Obtain/Approve?key=approve";
    document.forms[0].submit();
}
function approveCancel() {
    document.forms[0].action = "/Obtain/Approve?key=cancel";
    document.forms[0].submit();
}

//******************발주리스트*****************/
function orderSearchList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Obtain/ListSearch',
        type: "POST",
        data: "page=" + page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            for (var i = 0; i < data["count"]; i++) {
                listSearchTemplate(data["item" + i]);
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='orderSearchList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='orderSearchList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='orderSearchList(" + nextpage + ");return false;'>▶</a>&nbsp;";
            $("#paging").html(dom);
            $("#idxCollection").val(data["idxcollection"]);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
function listSearchTemplate(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##OrderNumber##/gi, pNode.ordernumber);
    dom = dom.replace(/##OrderComp##/gi, pNode.inordername);
    dom = dom.replace(/##OrderDate##/gi, pNode.orderdateString);
    dom = dom.replace(/##OrderDeliveryDate##/gi, pNode.ordersavedateString);
    dom = dom.replace(/##OrderCreater##/gi, pNode.creater);
    dom = dom.replace(/##OrderCreateDate##/gi, pNode.createdateString);
    dom = dom.replace(/##OrderState##/gi, pNode.stateDisp);
    if (pNode.state == "0") dom = dom.replace(/##statecolor##/gi, "yellow");
    else if (pNode.state == "1") dom = dom.replace(/##statecolor##/gi, "red");
    else if (pNode.state == "2") dom = dom.replace(/##statecolor##/gi, "#00ffc9");
    $("#listData").append(dom);
}
function orderview(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Obtain/OrderView");
    $("#submitForm").submit();
}
function pdfCreate() {
    document.forms[0].action = "/Obtain/PDFCreate";
    document.forms[0].submit();
}
//회사검색
function getCompany() {
    loading();
    $.ajax({
        url: '/Obtain/CompanySelect',
        type: "POST",
        data: "idx=" + $("#deliverycomp").val(),
        dataType: "json",
        success: function (data) {
            $("#inorderaddress").val(data.customerAddress);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}