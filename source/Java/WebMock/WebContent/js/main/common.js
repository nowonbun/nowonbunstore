var common = (function(m) {
	$(function() {
		m.init();
	});
	return m;
})({
	init : function() {
		this.menuload();
		this.resize();
		$(window).resize(common.resize);
	},
	resize : function() {
		$(".main").height($(window).height() - 72);
	},
	menuload : function() {
		$("aside.menu").load("./Menu");
	},
	mainNavigate : function(url) {
		$("div.contents").load(url);
		menu.mobileMenuClose();
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