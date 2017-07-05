/**
 * 
 */
$(function(){
	SendAjax("Test","",index);
})

function index(data){
	console.log(data);
	$("#test").html(data[0].TEST);
}