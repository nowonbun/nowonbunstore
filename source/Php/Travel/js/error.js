/**
 * 
 */

$(function(){
	setTimeout(setCount,1000,5);
});
function setCount(index){
	if(index === 0){
		location.href = "/login.php";
	}
	$("div.error > span").html(index+"秒後に移動します。");
	setTimeout(setCount,1000,--index);
}
