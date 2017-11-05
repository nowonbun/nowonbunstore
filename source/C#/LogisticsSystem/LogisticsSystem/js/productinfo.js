
var nowPage = 0;
var bufferCode = "";
var preTab = null;
//OnLoad
$(function () {
    tabMenu($("#tab1"));
});
//탭메뉴
function tabMenu(obj) {
    if (preTab != null) {
        $(preTab).css("background-color", "#6fBBD4");
        $("#" + $(preTab).attr("id") + "content").hide();
    }
    $(obj).css("background-color", "#9a9a9a");
    $("#" + $(obj).attr("id") + "content").show();
    preTab = obj;
}
function ImageFile() {
    //$("#productImage").click();
}
function Imageupload() {
    if (typeof FormData == null) {
        ErrorMessageBox('0002');
    } else {
        var data = new FormData();
        data.append('file', $('#productImage')[0].files[0]);
        $.ajax({
            url: '/AJAX/Imageupload',
            type: "post",
            dataType: "text",
            data: data,
            processData: false,
            contentType: false,
            success: function (data, textStatus, jqXHR) {
                if (data == "OK") {
                    $("#dispImage").attr("src", "/AJAX/ImageBuffer");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                loadingout();
                location.href = "/Home/Error";
            }
        });
    }
    return true;
}
//코드생성
function CodeCreate() {
    if ($("#productcode").val() != "") {
        ErrorMessageBox('0003');
        return true;
    } else {
        $.ajax({
            url: '/Product/CodeCreate',
            type: "POST",
            dataType: "text",
            success: function (data) {
                $("#productcode").val(data);
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
//리스트 검색(Ajax)
function ProductList(page) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Product/ListSearch',
        type: "POST",
        data: "page="+page,
        dataType: "json",
        success: function (data) {
            $("#listData").children().remove();
            var datacount = data["count"];
            if (datacount > 0) {
                for (var i = 0; i < datacount; i++) {
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
                dom += "<a href='#' style='text-decoration:none;color:blue;' onclick='ProductList(" + prepage + ");return false;'>◀</a>&nbsp;";
                for (var i = 1 ; i <= pagecount; i++) {
                    if (page == i) {
                        dom += "&nbsp;<font style='text-decoration:none;color:black;'>" + i + "</font>&nbsp;";
                    } else {
                        dom += "&nbsp;<a href='#' style='text-decoration:none;color:blue;' onclick='ProductList(" + i + ");return false;'>" + i + "</a>&nbsp;";
                    }
                }
                dom += "&nbsp;<a href='#' style='text-decoration:none;color:blue;' onclick='ProductList(" + nextpage + ");return false;'>▶</a>";
                $("#paging").html(dom);
                //기존 선택 된 것이 있으면 재선택인데...(이게 Insert 용이다.)
                if (bufferCode != "") {
                    ProductSearch(bufferCode);
                }
            } else {
                $("#listData").append($("#noSearch").html());
                $("#paging").html("");
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//이게 입력용
function ProductInsert(formName, url) {
    if ($("#productcode").val() == "") {
        ErrorMessageBox("1000");
        return;
    }
    loading();
    var formdata = $("#productInsertForm").serialize();
    $.ajax({
        url: "/Product/Insert",
        type: "POST",
        dataType: "text",
        data: formdata,
        success: function (data) {
            if (data == "SESSIONOUT") {
                location.href = "/Home/Error";
            } else if (data != "") {
                $("#Errormsg").html(data);
            } else {
                $("#Errormsg").html("");
                ProductList(nowPage);
                bufferCode = "GC-" + $("#productcode").val();
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//상품 정보 넣기
function ProductSearch(code) {
    loading();
    $.ajax({
        url: '/Product/ProductSearch',
        type: "POST",
        data: "code=" + code,
        dataType: "json",
        success: function (data) {
            ProductInit();
            ContentsDataSetting(data);
            $("#ApplySet").hide();
            $("#ModifySet").show();
            $("#tab2").show();
            $("#HistorySet").hide();
            bufferCode = "";
            $("#codeBtn").prop("disabled", true);
            ProductHistoryList(1,code);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//히스토리검색
function ProductHistoryList(page, code) {
    nowPage = page;
    loading();
    $.ajax({
        url: '/Product/HistorySearch',
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
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ProductHistoryList(" + prepage + ",\"" + code + "\");return false;'>◀</a>&nbsp;";
            for (var i = 1 ; i <= pagecount; i++) {
                dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ProductHistoryList(" + i + ",\"" + code + "\");return false;'>" + i + "</a>&nbsp;";
            }
            dom += "&nbsp;<a href='#' style='text-decoration:none;' onclick='ProductHistoryList(" + nextpage + ",\"" + code + "\");return false;'>▶</a>&nbsp;";
            $("#historypaging").html(dom);
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//히스토리검색
function ProductHistorySearch(idx){
    loading();
    $.ajax({
        url: '/Product/ProductHistorySearch',
        type: "POST",
        data: "idx=" + idx,
        dataType: "json",
        success: function (data) {
            ProductInit();
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
//초기화
function ProductClear() {
    ProductInit();
    $("#ApplySet").show();
    $("#codeBtn").prop("disabled", false);
    $("#ModifySet").hide();
    $("#tab2").hide();
}
//삭제합수
function ProductDelete() {
    if (!confirm($("#deleteText").text())) {
        return;
    }
    loading();
    var code = "GC-" + $("#productcode").val();
    $.ajax({
        url: "/Product/Delete",
        type: "POST",
        dataType: "text",
        data: "code=" + code,
        success: function (data) {
            if (data == "SESSIONOUT") {
                location.href = "/";
            } else {
                ProductList(nowPage);
                ProductClear();
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}
//수정하기함수
function ProductModify() {
    if (!confirm($("#modifyText").text())) {
        return;
    }
    loading();
    var formdata = $("#productInsertForm").serialize();
    $.ajax({
        url: "/Product/Modify",
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
                ProductList(nowPage);
                bufferCode = "GC-" + $("#productcode").val();
            }
            loadingout();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            loadingout();
            location.href = "/Home/Error";
        }
    });
}


//리스트 템플릿
function listTemplate(pNode) {
    var dom = $("#template").html();
    dom = dom.replace(/##productcode##/gi, pNode.productcode);
    dom = dom.replace(/##productname##/gi, pNode.productname);
    dom = dom.replace(/##productacquirer##/gi, pNode.productAcquirer);
    dom = dom.replace(/##productmanufacturer##/gi, pNode.productManufacturer);
    dom = dom.replace(/##productcost##/gi, numberFormat(pNode.productCost));
    dom = dom.replace(/##productprice##/gi, numberFormat(pNode.productPrice));
    $("#listData").append(dom);
}
//상품정보 초기화
function ProductInit() {
    $("#productcode").val("");
    $("#productname").val("");
    $("#productAcquirer").val("");
    $("#productManufacturer").val("");
    $("#productCost").val("");
    $("#productCostNotTax").val("");
    $("#productCostTax").val("");
    $("#productRetailPrice").val("");
    $("#productRetailPriceNotTax").val("");
    $("#productRetailPriceTax").val("");
    $("#productFactoryPrice").val("");
    $("#productFactoryPriceNotTax").val("");
    $("#productFactoryPriceTax").val("");
    $("#productPrice").val("");
    $("#productPriceNotTax").val("");
    $("#productPriceTax").val("");
    $("#barcode").val("");
    $("#QRcode").val("");
    $("#creater").val("");
    $("#createdate").val("");
    $("#other").val("");

    $("#productType").val($('#productType option:eq(0)').val());
    $("#productSpec").val($('#productSpec option:eq(0)').val());
    $("#productTax").val($('#productTax option:eq(0)').val());
}
// 상품정보란 세팅함수
function ContentsDataSetting(data) {
    $("#productcode").val(data.productcode.replace(/GC-/gi, ""));
    $("#productname").val(data.productname);
    $("#productAcquirer").val(data.productAcquirer);
    $("#productManufacturer").val(data.productManufacturer);
    $("#productCost").val(numberFormat(data.productCost));
    $("#productCostNotTax").val(numberFormat(data.productCostNotTax));
    $("#productCostTax").val(numberFormat(data.productCostTax));
    $("#productRetailPrice").val(numberFormat(data.productRetailPrice));
    $("#productRetailPriceNotTax").val(numberFormat(data.productRetailPriceNotTax));
    $("#productRetailPriceTax").val(numberFormat(data.productRetailPriceTax));
    $("#productFactoryPrice").val(numberFormat(data.productFactoryPrice));
    $("#productFactoryPriceNotTax").val(numberFormat(data.productFactoryPriceNotTax));
    $("#productFactoryPriceTax").val(numberFormat(data.productFactoryPriceTax));
    $("#productPrice").val(numberFormat(data.productPrice));
    $("#productPriceNotTax").val(numberFormat(data.productPriceNotTax));
    $("#productPriceTax").val(numberFormat(data.productPriceTax));
    $("#barcode").val(data.barcode);
    $("#QRcode").val(data.QRcode);
    $("#creater").val(data.creater);
    $("#createdate").val(data.createdateString);
    $("#other").val(data.other);

    $("#productType").val(data.productType);
    $("#productSpec").val(data.productspec);
    $("#productTax").val(data.productTax);
}
//이력리스트검색
function listHistoryTemplate(pNode) {
    var dom = $("#historytemplate").html();
    dom = dom.replace(/##idx##/gi, pNode.idx);
    dom = dom.replace(/##modifier##/gi, pNode.creater);
    dom = dom.replace(/##modifyData##/gi, pNode.createdateString);
    $("#historylist").append(dom);
}