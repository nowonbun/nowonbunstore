//<script src="https://code.jquery.com/jquery-1.12.4.min.js" integrity="sha256-ZosEbRLbNQzLpnKIkEdrPv7lOy9C27hHQ+Xp8a4MxAQ=" crossorigin="anonymous"></script>
//<script async defer src="https://apis.google.com/js/api.js" onload="this.onload=function(){};initialize()" onreadystatechange="if (this.readyState === 'complete') this.onload()">
var GoogleAuth;
var SCOPE = 'https://www.googleapis.com/auth/userinfo.profile';
$(function () {
	$('#sign-in-button').click(function() {
		GoogleAuth.signOut();
		GoogleAuth.disconnect();
		GoogleAuth.signIn();
    });
	$('#sign-out-button').click(function() {
		GoogleAuth.signOut();
		GoogleAuth.disconnect();
    });
	$('#revoke-access-button').click(function() {
		GoogleAuth.signOut();
		GoogleAuth.disconnect();
	}); 
});
function initializeLogin(){
	gapi.load('client:auth2', function(){
		gapi.client.init({
			'clientId' : '669579175828-u05p2nvva9f0acdu1ujrhkahn6dat5n1.apps.googleusercontent.com',
			'scope' : SCOPE
		}).then(function() {
			GoogleAuth = gapi.auth2.getAuthInstance();
			GoogleAuth.isSignedIn.listen(function(isSignedIn){
				var StandardAISEmail = 'ais-info.co.jp';
				var user = GoogleAuth.currentUser.get();
				//var isAuthorized = user.hasGrantedScopes(SCOPE);
				
				//AIS社員のメールか確認
				if(StandardAISEmail != user.w3.U3.slice(-StandardAISEmail.length)) {
					GoogleAuth.signOut();
					GoogleAuth.disconnect();
					alert("AISの社員以外は接続できません。");
					$('#sign-in-button').css('display', 'inline-block');
				    $('#sign-out-button').css('display', 'none');
				    $('#revoke-access-button').css('display', 'none');
				} else if(user.hasGrantedScopes(SCOPE)){
					//AJAX
					var userToken = user.w3.Eea;
					var userName = user.w3.ig;
					var userEmail = user.w3.U3;
					var obj = {
							email: userEmail,
							token: userToken,
							name: userName
						};
					var user = GoogleAuth.currentUser.get();
					console.log(user);
					SendAjax("Login", obj, redirect);
//					$('#sign-in-button').css('display', 'none');
//				    $('#sign-out-button').css('display', 'inline-block');
//				    $('#revoke-access-button').css('display', 'inline-block');
				} else if(!user.hasGrantedScopes(SCOPE)){
//					$('#sign-in-button').css('display', 'inline-block');
//				    $('#sign-out-button').css('display', 'none');
//				    $('#revoke-access-button').css('display', 'none');
				}
				var user = GoogleAuth.currentUser.get();
				console.log(user);
			});
		});
	});
}

function redirect (data) {
	if (data.Result == 0 ) {
		location.href="main.html";
		return;
	}else if(data.Error == 1){
		alert("エラー１が発生しました。");
	}else if(data.Error == 2){
		alert("エラー２が発生しました。");
	}else if(data.Error == 3){
		alert("エラー３が発生しました。");
	}
}

