/**
 * 
 */
$(function() {
	$("#peopleNumber").bind("change", function() {
		$("div.member input.hasDatepicker").removeClass("hasDatepicker");
		var setlinecount = Number($("#peopleNumber").val());
		var linecount = $("div.member > table > tbody").children("TR").size();
		if (setlinecount === linecount) {
			return;
		}
		if (setlinecount > linecount) {
			setlinecount = setlinecount - linecount;
			var temp = $("tbody#addMemberTemplate").html();
			for(var i=0;i<setlinecount;i++){
				$("div.member > table > tbody").append(temp);
			}
			console.log($("input.birth"));
			$("div.member input.birth").each(function(){
				console.log(this);
				$(this).prop("readonly","readonly");
				$(this).datepicker({
					dateFormat : "yy-mm-dd",
					changeMonth: true,
					changeYear: true,
					yearRange: "-100:+0"
				});
			})
			return;
		}
		if (setlinecount < linecount) {
			setlinecount = linecount - setlinecount;
			for (var i = 0; i < setlinecount; i++) {
				$("div.member > table > tbody").children("TR")[(linecount-1)-i].remove();
			}
			return;
		}
	});
	$("input[type=submit]").bind("click",function(){
		$("div.member input[type=text].inputcheck").removeClass("redcolor");
		$("#errorMsg").html("");
		var valicheck = true;
		$("div.member input[type=text].inputcheck").each(function(){
			if(!valicheck){
				return;
			}
			var value = $(this).val();
			if($.trim(value) === ''){
				var name = $(this).prop("alt");
				$(this).focus();
				$(this).addClass("redcolor");
				$("#errorMsg").html(name + "を入力してください。");
				valicheck = false;
			}
		});
		return valicheck;
	});
});