var menu = (function(m) {
	$(function() {
		m.init();
	});
	return m;
})({
	state : null,
	init : function() {
		state = common.getPlatform();
		$('ul > li > ul').addClass('disp-off');
		this.menuRolling();
		this.showInit();
		$(window).resize(menu.menuResize);
		$("div.contents").before().on(
				{
					click : function() {
						if (!common.isPc()
								&& $("#menucheck").prop("checked") === true) {
							menu.hide();
						}
					}
				});
	},
	showInit : function() {
		$("#idcheck").prop("checked", "");
		if (common.isPc()) {
			menu.show();
		} else {
			menu.hide();
		}
	},
	menuResize : function() {
		if (common.getPlatform() !== state) {
			state = common.getPlatform();
			menu.showInit();
		}
	},
	menuRolling : function() {
		$(".menu > ul > li").on({
			click : function() {
				$(this).children("ul").toggleClass("disp-off");
			}
		});
		$(".menu > ul > li > ul > li").on({
			click : function() {
				return false;
			}
		});
	},
	mobileMenuClose : function() {
		if (!common.isPc()) {
			menu.hide();
		}
	},
	hide : function() {
		$("#menucheck").prop("checked", "");
	},
	show : function() {
		$("#menucheck").prop("checked", "checked");
	}
});