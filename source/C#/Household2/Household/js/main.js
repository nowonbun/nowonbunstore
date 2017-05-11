_ = (function (m) {
    $(function () {
        m.signout.init();
        m.date.init();
        m.selecter.init();
        m.calculator.init();
        m.screen.init();
        m.input.init();
    });
    return m;
})({
    signout: {
        init: function () {
            $("input[type=button].signout").bind("click", function () {
                location.href = "/Home/Signout";
            });
        }
    },
    search: {
        init: function () {
            this.setting();
            this.initVal();
            $("#householdYearView").html($("#householdYear").val());
            $("#householdMonthView").html($("#householdMonth").val());
            this.search();
        },
        initVal: function () {
            var date = new Date();
            $("#householdYear").val(date.getFullYear());
            $("#householdMonth").val(date.getMonth() + 1);
            $("#householdDay_pc").val(date.getDate());
            $("#householdDay_mobile").val(date.getDate());
        },
        setting: function () {
            //date-left
            $("fa fa-chevron-circle-left").on("on", function () {
                this.addMonth(-1);
                this.search();
            });
            //date-right
            $("fa fa-chevron-circle-right").on("on", function () {
                this.addMonth(1);
                this.search();
            });
            $("div.main-date select, select#searchDaySelect, select#searchTypeSelect").on("change", function () {
                this.search();
            });
            $("#householdYear").on("change", function () {
                $("#householdYearView").html($("#householdYear").val());
            });
            $("#householdMonth").on("change", function () {
                $("#householdMonthView").html($("#householdMonth").val());
            });
            $("input[type=button]#searchInit").on("click", function () {
                $("select#searchDaySelect,select#searchTypeSelect").val("");
                this.search();
            });
        },
        search: function () {
            var year = parseInt($("#householdYear").val());
            var month = parseInt($("#householdMonth").val());
            var day = parseInt($("#searchDaySelect").val());
            if (isNaN(day)) {
                day = "";
            }
            var type = $("#searchTypeSelect").val();
            MoneyInit();
            _.fn.sendAjax("Search", "year=" + year + "&month=" + month + "&day=" + day + "&type=" + type, _.data.searchData);
        },
        addMonth: function (addmonth) {
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
    },
    data: {
        init: function () {
            this.setting();
        },
        initVal: function () {
            $("table.table-data.table-data1 > tbody").html($("div.template > table.template-data1-nothing > tbody").html());
            $("table.table-data.table-data2 > tbody").html($("div.template > table.template-data2-nothing > tbody").html());
            $("table.table-data.table-data3 > tbody").html($("div.template > table.template-data3-nothing > tbody").html());
            this.viewTotal("span#totalMoney1", 0, "0");
            this.ViewTotal("span#totalMoney2", 0, "0");
            this.ViewTotal("span#totalMoney3", 0, "0");
        },
        setting: function () {
            /* The folding event*/
            $("div.title-data").bind("click", function () {
                $(this).children().children("span.glyphicon-plus").toggleClass("off");
                $(this).children().children("span.glyphicon-minus").toggleClass("off");
                $(this).next().toggleClass("mobile-off");
            });
            $("table.table-data1 > tbody > tr:not(.credit)").on("click", function () {
                if ($(this).hasClass("nothing")) {
                    return;
                }
                ClearList();
                $(this).addClass("click");
            });
            /* The register button event and popup*/
            $("input#apply_mobile").on("click", function () {
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
            $("input[type=button]#applySubmit_mobile").on("click", function () {
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
            $("div.apply > div.title > span.remove").bind("click", function () {
                ClearMobile();
            });
        },
        viewTotal: function (name, val, valStr) {
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
        },
        searchData: function (data) {
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
        },
        SetEventMobile: function () {
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
    },
    mobilePopup: {
        init: function () {
            ChangeHouseholdTypeMobile();
            InitMobile();
        },
        setting: function () {
            $("#householdCategory_mobile").on("change", function () {
                ChangeHouseholdTypeMobile();
            });
        },
        ClearMobile: function () {
            $("html").removeClass("fixed");
            $("div.layout").addClass("off");
            $("div.apply").addClass("off");
            $("div.apply-area.mobile-private,div.modify-area.mobile-private").removeClass("off");
            $("div.calc").addClass("off");

            ClearMobileData();
        },
        ChangeHouseholdTypeMobile: function () {
            var name = "#select_" + $("#householdCategory_mobile").val();
            var dom = $(name).html();
            $("#householdType_mobile").html(dom);
        },

        InitMobile: function () {
            $("#householdCategory_mobile").val("000");
            $("#householdType_mobile").val("002");
            ClearMobileData();
        },

        ClearMobileData: function () {
            $("#householdYear_mobile").val("");
            $("#householdMonth_mobile").val("");
            $("#householdIdx_mobile").val("");
            $("#householdPdt_mobile").val("");
            $("#householdContent_mobile").val("");
            $("#householdPrice_mobile").val("");
        },

        ValidateMobile: function () {
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
        },
        ApplyDataMobile: function (data) {
            SetErrorMsg(INFO00001, 3000);
            Search();
        },
        ModifyDataMobile: function (data) {
            SetErrorMsg(INFO00002, 3000);
            Search();
        },
        DeleteDataMobile: function (data) {
            SetErrorMsg(INFO00003, 3000);
            Search();
        },
        SetErrorMobileMsg: function (msg, time) {
            $(".error_mobile").html(msg);
            if (time > 0) {
                setTimeout(SetErrorMobileMsg, time, "", 0);
            }
        }
    },
    input: {
        init: function () {
            ChangeHouseholdTypePc();
            InitPc();
        },
        setting: function () {
            //apply
            $("input[type=button]#applySubmit_pc").on("click", function () {
                if (!ValidatePc()) {
                    return;
                }
                var formdata = $("#applyPc").serialize();
                //add
                SendAjax("Apply", formdata, ApplyDataPc);
            });

            //cancel
            $("input[type=button]#cancelSubmit_pc").on("click", function () {
                ClearPc();
            });

            //modify
            $("input[type=button]#modifySubmit_pc").on("click", function () {
                if (!ValidatePc()) {
                    return;
                }
                var formdata = $("#applyPc").serialize();
                SendAjax("Modify", formdata, ModifyDataPc);
            });

            //delete
            $("input[type=button]#deleteSubmit_pc").on("click", function () {
                var formdata = $("#applyPc").serialize();
                SendAjax("Delete", formdata, DeleteDataPc);
            });

            $("#householdCategory_pc").on("change", function () {
                ChangeHouseholdTypePc();
            });
        }, SetEventPc: function () {
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
        },

        ClearPc: function () {
            ClearList();
            $("table.table-input.pc-private div.apply-area").removeClass("off");
            $("table.table-input.pc-private div.modify-area").addClass("off");
            $("#householdIdx_pc").val("");
            $("#householdContent_pc").val("");
            $("#householdPrice_pc").val("");
        },

        ClearList: function () {
            $("table.table-data1 > tbody > tr").each(function () {
                $(this).removeClass("click");
            });
        },

        ChangeHouseholdTypePc: function () {
            var name = "#select_" + $("#householdCategory_pc").val();
            var dom = $(name).html();
            $("#householdType_pc").html(dom);
        },

        ApplyDataPc: function (data) {
            ClearPc();
            SetErrorMsg(INFO00001, 3000);
            Search();
        },

        ModifyDataPc: function (data) {
            ClearPc();
            SetErrorMsg(INFO00002, 3000);
            Search();
        },

        DeleteDataPc: function (data) {
            ClearPc();
            SetErrorMsg(INFO00003, 3000);
            Search();
        },

        ValidatePc: function () {
            var month = parseInt($("#householdMonth").val());
            var date = new Date($("#householdYear").val(), month - 1, $("#householdDay_pc").val());
            if (month !== date.getMonth() + 1) {
                SetErrorMsg(ERROR0001, 3000);
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
        },

        InitPc: function () {
            $("#householdCategory_pc").val("000");
            $("#householdType_pc").val("002");
        }
    },
    calculator: {
        init: function () {
            this.setting();
        },
        setting: function () {
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
            //calculater
            $("input[type=button]#calc_mobile").on("click", function () {
                $("div.calc").removeClass("off");
                InitCalc(true);
            });

            $("div.layout > div.calc > div.calcmain > div.title  > span").on("click", function () {
                InitCalc(false);
                $("div.calc").addClass("off");
            });

            $("input[type=button]#calc_add").on("click", function () {
                AddCalcNumber();
            });

            $("input[type=button]#calc_subtract").on("click", function () {
                SubtractCalcNumber();
            });

            $("input[type=button]#calc_multiply").on("click", function () {
                MultiplyCalcNumber();
            });

            $("input[type=button]#calc_division").on("click", function () {
                DivisionCalcNumber();
            });

            $("input[type=button]#calc_clear").on("click", function () {
                InitCalc(true);
            });

            $("#calc_result").on("click", function () {
                var buffer = $("input[type=tel]#calc_sum").val();
                $("#householdPrice_mobile").val(buffer);
                InitCalc(false);
                $("div.calc").addClass("off");
            });
        },
        setCalc: function (val) {
            $("input[type=tel]#calc").val("");
            $("input[type=tel]#calc_sum").val(val);
            $("input[type=tel]#calc").focus();
        },
        InitCalc: function (focus) {
            $("input[type=tel]#calc").val("");
            $("input[type=tel]#calc_sum").val(0);
            if (focus) {
                $("input[type=tel]#calc").focus();
            }
        },
        AddCalcNumber: function () {
            var a = parseInt($("input[type=tel]#calc").val());
            var b = parseInt($("input[type=tel]#calc_sum").val());
            if (isNaN(a)) {
                a = 0;
            }
            if (isNaN(b)) {
                b = 0;
            }
            SetCalc(a + b);
        },
        SubtractCalcNumber: function () {
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
        },

        MultiplyCalcNumber: function () {
            var a = parseInt($("input[type=tel]#calc").val());
            var b = parseInt($("input[type=tel]#calc_sum").val());
            if (isNaN(a)) {
                a = 0;
            }
            if (isNaN(b)) {
                b = 0;
            }
            SetCalc(a * b);
        },

        DivisionCalcNumber: function () {
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
        },


        GetNumber: function (tag) {
            var val = parseInt($("td.calc-txt > input[type=tel]").val());
            if (isNaN(val)) {
                val = 0;
            }
            return val;
        }
    },
    screen: {
        w_width: 0,
        init: function () {
            $(window).resize(function () {
                ClearPc();
                if (this.w_width == 0) {
                    this.w_width = $(window).width();
                    return;
                }
                if (this.w_width != $(window).width()) {
                    this.w_width = $(window).width();
                    ClearMobile();
                }
            });
        }
    },
    fn: {
        convertMoneyStr: function (val, valStr) {
            return "￥" + valStr.replace("-", "");
        },
        sendAjax: function (name, data, method) {
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
        },
        setErrorMsg: function (msg, time) {
            $("#error").html(msg);
            if (time > 0) {
                setTimeout(SetErrorMsg, time, "", 0);
            }
        },
        createData: function (val) {
            var ret = val.Idx + ",";
            ret += val.Day + ",";
            ret += val.Cd + ",";
            ret += val.Tp + ",";
            ret += val.Content + ",";
            ret += val.PriceNum + ",";
            ret += val.Pdt;
            return ret;
        },
        createClass: function CreateClass(val) {
            var res = val.split(",");
            this.Idx = res[0];
            this.Day = res[1];
            this.Cd = res[2];
            this.Tp = res[3];
            this.Content = res[4];
            this.Price = res[5];
            this.Pdt = res[6];
        }
    }
});