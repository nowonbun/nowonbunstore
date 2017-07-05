/**
 * 
 */
$(function(){
	SendAjax("ReservationNow","",indexNow);
	
	$("div.popup").hide();
	
	$("section#room1 div.used > span.result").html("修正");
	$("section#room1 div.startdate > span.result").html("2017/04/13 11:00:00");
})

function indexNow(data){
	console.log(data);
	var purpose = data[0].PurposeCode;
	
	switch (purpose) {
	  case 1:
		  // 式の結果が value1 にマッチする場合に実行する文
		  $("#purpose").html("お客様ミーティング");
		  break;
	  case 2:
		  // 式の結果が value2 にマッチする場合に実行する文
		  $("#purpose").html("部署長会議");
		  break;
	  case 3:
		  // 式の結果が valueN にマッチする場合に実行する文
		  $("#purpose").html("営業会議");
		  break;
	　　case 4:
		  // 式の結果が valueN にマッチする場合に実行する文
		  $("#purpose").html("社内教育");
		  break;
	　　case 5:
		  // 式の結果が valueN にマッチする場合に実行する文
		  $("#purpose").html("チーム会議");
		  break;
	  default:
		  // 式の値にマッチするものが存在しない場合に実行する文
		  $("#purpose").html("なし");
		  break;
	}
}
