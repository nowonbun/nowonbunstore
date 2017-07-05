/**
 * 
 */
$(function() {
	$("#startdate").timepicker();
	$("#arrivedate").timepicker();
	$('#startdate').timepicker('setTime', new Date());
	$('#arrivedate').timepicker('setTime', new Date());
	$('#startdateButton').bind('click', function() {
		$('#startdate').timepicker('setTime', new Date());
	});
	$('#arrivedateButton').bind('click', function() {
		$('#arrivedate').timepicker('setTime', new Date());
	});
	$("#startdate").bind("keydown", function() {
		return false;
	});
	$("#arrivedate").bind("keydown", function() {
		return false;
	});
	$("input[type=tel]").bind("keydown", function() {
		console.log(event.keyCode);
		if (event.keyCode >= 48 && event.keyCode <= 57) {
			return true;
		}
		if (event.keyCode < 32) {
			return true;
		}
		return false;
	});
	$("input[type=tel]").bind("keyup", function() {
		var num = $(this).val();
		if (num === null || num === '') {
			$(this).val('0');
		}
		num = num.replace(/,/gi, '');
		num = Number(num);
		num = num.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
		$(this).val(num);
	});
	$("#checkerCode").bind("click", function() {
		$("td.label").removeClass("checked");
		if ($.trim($("#code").val()) === '') {
			$("#Lcode").addClass("checked");
			$("#code").focus();
			$("#errorMsg").html("コードを入力してください。");
			return false;
		}
		$.ajax({
			url : "/admin/addAdminProductCheck.php",
			type : "POST",
			dataType : "json",
			data : "code=" + $("#code").val(),
			success : function(data, textStatus, jqXHR) {
				if (data.result === "ok") {
					$("#errorMsg").html("コードを使えます。");
					$("#codeChecker").val("1");
					$("#code").prop("readonly", "readonly");
					$("#code").addClass("disabled");
					return;
				} else {
					$("#errorMsg").html("コードを使っています。他のコードを使ってください。");
				}
			},
			error : function(jqXHR, textStatus, errorThrown) {
				$("#errorMsg").html("AJAX通信中エラーが発生しました。");
			},
			complete : function(jqXHR, textStatus) {

			}
		});
	});
	$("#cancel").bind("click", function() {
		location.href = "/admin/adminProductList.php";
	});
	$("#startCountry").bind("change", function() {
		startLocationSelect();
	});
	$("#arriveCountry").bind("change", function() {
		endLocationSelect();
	});
	$("input[type=submit]").bind("click",function(){
		$("td.label").removeClass("checked");
		if($.trim($("#code").val()) === ''){
			$("#Lcode").addClass("checked");
			$("#code").focus();
			$("#errorMsg").html("商品コードを入力してください。");
			return false;
		}
		if($.trim($("#codeChecker").val()) === '0'){
			$("#Lcode").addClass("checked");
			$("#pcodeid").focus();
			$("#errorMsg").html("商品コードの重複チェックをしてください。");
			return false;
		}
		if($.trim($("#planname").val()) === ''){
			$("#Lplanname").addClass("checked");
			$("#planname").focus();
			$("#errorMsg").html("プラン名を入力してください。");
			return false;
		}
		if(transTimetick($.trim($("#startdate").val())) >= transTimetick($.trim($("#arrivedate").val()))){
			$("#Lstartdate").addClass("checked");
			$("#Larrivedate").addClass("checked");
			$("#errorMsg").html("出発時間と到着時間を合わせてください。");
			return false;
		}
		if(transMoneyToNumber($.trim($("#price").val())) <= 0){
			$("#Lprice").addClass("checked");
			$("#price").focus();
			$("#errorMsg").html("価格を入力してください。");
			return false;
		}
		return true;	
	});

	startLocationSelect();
});
function transMoneyToNumber(val){
	return Number(val.replace(/,/gi,""));
}
function transTimetick(val){
	var pm = false;
	if(val.indexOf("pm")!= -1){
		pm = true;
	}
	val = val.replace(/am/gi,"").replace(/pm/gi,"");
	var buffer = val.split(":");
	var hour = Number(buffer[0]);
	var min = Number(buffer[1]);
	if(pm){
		hour += 12;
	}
	return hour * 60 + min;
}
function startLocationSelect() {
	var start = $("#startCountry").val();
	var end = $("#arriveCountry").val();
	if (start !== end) {
		return;
	}
	if (start === $($("#arriveCountry").children()[0]).val()) {
		$($("#arriveCountry").children()[1]).prop("selected", "selected");
	} else {
		$($("#arriveCountry").children()[0]).prop("selected", "selected");
	}
	bindlocation($("#startCountry"), $("#startLocation"));
	bindlocation($("#arriveCountry"), $("#arriveLocation"));
}
function endLocationSelect() {
	var start = $("#startCountry").val();
	var end = $("#arriveCountry").val();
	if (start !== end) {
		return;
	}
	if (start === $($("#startCountry").children()[0]).val()) {
		$($("#startCountry").children()[1]).prop("selected", "selected");
	} else {
		$($("#startCountry").children()[0]).prop("selected", "selected");
	}
	bindlocation($("#startCountry"), $("#startLocation"));
	bindlocation($("#arriveCountry"), $("#arriveLocation"));
}
function bindlocation(target, obj) {
	var dom = $("div.template > div#locationselect > select#location_key_" + $(target).val()).html();
	$(obj).html(dom);
}
