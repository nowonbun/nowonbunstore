/**
 * 
 */
function SendAjax(name, data, method) {
    $(".lodding").removeClass("lodding-off");
    $.ajax({
        url: name,
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data, textStatus, jqXHR) {
        	 setTimeout(method, 1, data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            
        },
        complete: function (jqXHR, textStatus) {
            
        }
    });
}
$(function() {
	
});

//var GoogleAuth;
//
//$(function () {
//	$('#logout').click(function() {
//		GoogleAuth.signOut();
//		GoogleAuth.disconnect();
//		//redirect login page
//		location.href="login.html";
//    });
//});
//function initialize(){
//	gapi.load('client:auth2', function(){
//		gapi.client.init({
//			'clientId' : '707897103672-cc0vse6qgq1o3gic1a2ml6ielv0e2nsa.apps.googleusercontent.com',
//			'scope' : SCOPE
//		}).then(function() {
//			GoogleAuth = gapi.auth2.getAuthInstance();
//			var user = GoogleAuth.currentUser.get();
//			if (!user.hasGrantedScopes(SCOPE)) {
//				//redirect login page
//				location.href="login.html"
//				return;
//			}
//		});
//	});
//}