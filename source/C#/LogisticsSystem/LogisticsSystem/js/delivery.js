/**************수주서 등록***********/
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
//수주서 회사 선택
function orderCompSearch() {
    if ($("#companyidx").val() != "") {
        loading();
        $.ajax({
            url: '/Delivery/orderSelect',
            type: "POST",
            data: "idx=" + $("#companyidx").val(),
            dataType: "json",
            success: function (data) {
                $("#ordername").val(data.customerName);
                $("#companySecurityNumber").val(data.customerSecurityNumber);
                $("#companyAddress").val(data.customerAddress);
                $("#companyNumber").val(data.customerNumber);
                $("#companyFax").val(data.customerFax);
                $("#ordersaveplace").val(data.customerAddress);
                loadingout();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                loadingout();
                location.href = "/Home/Error";
            }
        });
    } else {
        $("#ordername").val("");
        $("#companySecurityNumber").val("");
        $("#companyAddress").val("");
        $("#companyNumber").val("");
        $("#companyFax").val("");
        $("#ordersaveplace").val("");
    }
}
//상품 셀렉트시 관련 정보 취득
function getProduct(idx) {
    loading();
    if ($("#productType_" + idx).val() != "") {
        $("#subNumber_" + idx).html(idx + 1);
        $("#productmount_" + idx).val("0");
        $("#productmount_" + idx).attr("readonly", false);
        priceCalcul(idx);
        $.ajax({
            //발주서꺼 사용
            url: '/Obtain/ProductSelect',
            type: "POST",
            data: "idx=" + $("#productType_" + idx).val(),
            dataType: "json",
            success: function (data) {
                $("#productspec_" + idx).val(data.productspec);
                $("#productspec_disp_" + idx).val(data.projectDispSpec);
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
        $("#productspec_disp_" + idx).val("");
        priceCalcul(idx);
        loadingout();
    }
}
function priceCalcul(idx) {
    mount = unNumberFormat($("#productmount_" + idx).val());
    price = unNumberFormat($("#productprice_" + idx).val());
    $("#producttotal_" + idx).val(numberFormat(mount * price));
    totalCalcul();
}
function totalCalcul() {
    var totalprice = 0;
    for (i = 0; i < 15; i++) {
        price = Number(unNumberFormat($("#producttotal_" + i).val()));
        totalprice += price;
    }
    $("#totalamount").val(numberFormat(totalprice));
    $("#obtainprice").val(numberFormat(totalprice));
    $("#money").val(numberFormat(totalprice));
}
function GoSubmit() {
    document.forms[0].action = "/Delivery/Input";
    document.forms[0].submit();
}
function GoBack() {
    document.forms[0].action = "/Delivery/DeliveryOrder?key=BACK";
    document.forms[0].submit();
}
//***********************수주서 승인리스트
function orderApproveListSearch(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Delivery/ListApproveSearch',
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
function listTemplate(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##OrderNumber##/gi, pNode.ordernumber);
    dom = dom.replace(/##OrderComp##/gi, pNode.ordername);
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
        url: '/Delivery/SubSearch',
        type: "POST",
        data: "idx=" + idx,
        dataType: "json",
        success: function (data) {
            $("#line_" + idx).children().remove();
            for (i = 0; i < data.count; i++) {
                var subdom = $("#templatecontents").html();
                subdom = subdom.replace(/##number##/gi, i + 1);
                subdom = subdom.replace(/##productName##/gi, data["item" + i].productName);
                subdom = subdom.replace(/##productSpec##/gi, data["item" + i].productspec_disp);
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
    $("#submitForm").attr("action", "/Delivery/ApprovePage");
    $("#submitForm").submit();
}
function returnPage(type) {
    if (type == 1) location.href = "/Delivery/DeliveryApproveList";
    if (type == 2) location.href = "/Delivery/DeliveryOrderList";
    if (type == 3) location.href = "/Delivery/DeliveryCheckList";
    if (type == 4) location.href = "/Delivery/DeliveryBillList";
}
function approve() {
    document.forms[0].action = "/Delivery/Approve?key=approve";
    document.forms[0].submit();
}
function approveCancel() {
    document.forms[0].action = "/Delivery/Approve?key=cancel";
    document.forms[0].submit();
}
//***************************** 수주 리스트
function orderSearchList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Delivery/ListSearch',
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
function listTemplate2(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##OrderNumber##/gi, pNode.ordernumber);
    dom = dom.replace(/##OrderComp##/gi, pNode.ordername);
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
    $("#submitForm").attr("action", "/Delivery/DeliveryOrderView");
    $("#submitForm").submit();
}
//*******************납품확인서
function DeliveryCheckList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Delivery/DeliveryCheckSearch',
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='DeliveryCheckList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='DeliveryCheckList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='DeliveryCheckList(" + nextpage + ");return false;'>▶</a>&nbsp;";
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
    dom = dom.replace(/##OrderComp##/gi, pNode.orderCompany);
    dom = dom.replace(/##OrderDate##/gi, pNode.orderSavedateString);
    dom = dom.replace(/##OrderCreater##/gi, pNode.creater);
    dom = dom.replace(/##OrderCreateDate##/gi, pNode.createdateString);
    $("#listData").append(dom);
}
function DiliveyView(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Delivery/DeliveryView");
    $("#submitForm").submit();
}
//**************************정산서
function DeliveryBillList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Delivery/BillSearch',
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='DeliveryBillList(" + prepage + ");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='DeliveryBillList(" + i + ");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='DeliveryBillList(" + nextpage + ");return false;'>▶</a>&nbsp;";
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
    dom = dom.replace(/##OrderComp##/gi, pNode.ordercompany);
    dom = dom.replace(/##OrderBillDate##/gi, pNode.billdateString);
    dom = dom.replace(/##OrderBillMoney##/gi, numberFormat(pNode.billtotal));
    dom = dom.replace(/##OrderCreater##/gi, pNode.creater);
    dom = dom.replace(/##OrderCreateDate##/gi, pNode.createdateString);
    $("#listData").append(dom);
}
function BillView(idx) {
    $("#idxForm").val(idx);
    $("#submitForm").attr("action", "/Delivery/BillView");
    $("#submitForm").submit();
}