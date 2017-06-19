var common = (function(m) {
	$(function() {
		m.init();
	});
	return m;
})({
	menuIndex : 0,
	init : function() {
		this.menuload();
		this.resize();
		$(window).resize(common.resize);
	},
	resize : function() {
		$(".main").height($(window).height() - 70);
	},
	menuload : function() {
		$("aside.menu").load("./Menu");
	},
	mainNavigate : function(tab,url) {
		console.log("click");
		var dom = $("div.tab").html();
		dom += "<span class='tabItem' id='tabItem"+common.menuIndex+"'>"+tab+"</span>";
		$("div.tab").html(dom);
		$(".tabItem").on({
			click: function(){
				var id = $(this).prop("id");
				common.tabnavigate("tabContents"+id.replace("tabItem",""));
			}
		});
		$(".tabContents").each(function(){
			$(this).addClass("disp-off");
		});
		$("div.content").append("<div class='tabContents' id='tabContents"+common.menuIndex+"'></div>");
		$("#tabContents"+common.menuIndex).load(url);
		common.menuIndex++;
		menu.mobileMenuClose();
	},
	tabnavigate : function(tabName){
		$(".tabContents").each(function(){
			$(this).addClass("disp-off");
		});
		$("#"+tabName).removeClass("disp-off");
	},
	isMobile : function() {
		if ($(window).width() <= 480) {
			return true;
		}
		return false;
	},
	isTablet : function() {
		if ($(window).width() >= 481 && $(window).width() >= 768) {
			return true;
		}
		return false;
	},
	isPc : function() {
		if ($(window).width() >= 769) {
			return true;
		}
		return false;
	},
	getPlatform : function() {
		if (common.isMobile()) {
			return "mobile";
		}
		if (common.isTablet()) {
			return "tablet";
		}
		if (common.isPc()) {
			return "pc";
		}
	}
});