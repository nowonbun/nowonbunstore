$(function() {
	$("#birth").datepicker({
		dateFormat : "yy-mm-dd",
		changeMonth: true,
		changeYear: true,
		yearRange: "-100:+0"
		
	});
	$("input[type=submit]").bind("click", function() {
		$("td.label").removeClass("checked");
		if($.trim($("#pid").val()) === ''){
			$("#Lpid").addClass("checked");
			$("#pid").focus();
			$("#errorMsg").html("IDを入力してください。");
			return false;
		}
		if($.trim($("#idchecked").val()) === '0'){
			$("#Lpid").addClass("checked");
			$("#pid").focus();
			$("#errorMsg").html("IDを重複チェックをしてください。");
			return false;
		}
		if($.trim($("#pwd").val()) === ''){
			$("#Lpwd").addClass("checked");
			$("#pwd").focus();
			$("#errorMsg").html("パスワードを入力してください。");
			return false;
		}
		if($.trim($("#rpwd").val()) === ''){
			$("#Lrpwd").addClass("checked");
			$("#rpwd").focus();
			$("#errorMsg").html("パスワード確認を入力してください。");
			return false;
		}
		if($.trim($("#pwd").val()) !== $.trim($("#rpwd").val())){
			$("#Lpwd").addClass("checked");
			$("#Lrpwd").addClass("checked");
			$("#rpwd").focus();
			$("#errorMsg").html("パスワードとパスワード確認が一致しません。");
			return false;
		}
		if($.trim($("#name").val()) === ''){
			$("#Lname").addClass("checked");
			$("#name").focus();
			$("#errorMsg").html("お名前を入力してください。");
			return false;
		}
		if($.trim($("#birth").val()) === ''){
			$("#Lbirth").addClass("checked");
			$("#birth").focus();
			$("#errorMsg").html("誕生日を入力してください。");
			return false;
		}
		return true;
	});
	
	$("#checkerID").bind("click",function(){
		$("td.label").removeClass("checked");
		if($.trim($("#pid").val()) === ''){
			$("#Lpid").addClass("checked");
			$("#pid").focus();
			$("#errorMsg").html("IDを入力してください。");
			return false;
		}
		$.ajax({
	        url: "/newApplyIdCheck.php",
	        type: "POST",
	        dataType: "json",
	        data: "pid="+$("#pid").val(),
	        success: function (data, textStatus, jqXHR) {
	        	if(data.result === "ok"){
	        		$("#errorMsg").html("IDを使えます。");
	        		$("#idchecked").val("1");
	        		$("#pid").prop("readonly","readonly");
	        		$("#pid").addClass("disabled");
	        		return;
	        	}else{
	        		$("#errorMsg").html("IDを使っています。他のIDを使ってください。");
	        	}
	        },
	        error: function (jqXHR, textStatus, errorThrown) {
	        	$("#errorMsg").html("AJAX通信中エラーが発生しました。");
	        },
	        complete: function (jqXHR, textStatus) {
	            
	        }
	    });
	});
	$("#cancel").bind("click", function() {
		location.href = "/";
	});
});