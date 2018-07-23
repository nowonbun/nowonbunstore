$(function() {
	$(".button-collapse").sideNav();
	var sideNavScrollbar = document.querySelector('.custom-scrollbar');
	Ps.initialize(sideNavScrollbar);
	$(window).resize(function(){
		console.log("log");
		$("#sidenav-overlay").remove();
	});
});
