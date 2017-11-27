var ins = (function (f) {
	$(function () {
		f.init();
	});
	return f;
})({
	ws : null,
	init: function () {
		this.ws = new WebSocket("ws://127.0.0.1:19999/");
        this.ws.onopen = function(message) {
			console.log("Server Connection");
			var message = JSON.stringify({key:"init", data:null});
			ins.ws.send(message);
        };
        this.ws.onclose = function(message) {
			console.log("Server Disconnect");
        };
        this.ws.onerror = function(message) {
			console.log("Server Error");
        };
        this.ws.onmessage = function(message) {
			function template(item) {
				return "<tr id='Key"+ item.Key +"' class='datalist'>"
						+ "<td>" + item.Key + "</td>"
						+ "<td>" + item.Code + "</td>"
						+ "<td>" + item.Id + "</td>"
						+ "<td>" + item.Starttime + "</td>"
						+ "<td class='ping'>" + item.Pingtime + "</td>"
						//+ "<td>"+item.Status+"</td>"
						+ "</tr>";
			}
			if(message.data === "") {
				return;
			}
			var node = JSON.parse(message.data);
			if(node.key === "init") {
				var list = JSON.parse(node.data);
				if (list.length === 0) {
					$(".noData").show();
				} else {
					$(".noData").hide();
				}
				for (var i in list) {
					var temp = template(list[i]);
					$("#status_table > tbody").append(temp);
				}
			} else if (node.key === "insert") {
				$(".noData").hide();
				$("#status_table > tbody").append(template(JSON.parse(node.data)));
			} else if (node.key === "ping") {
				var temp = JSON.parse(node.data);
				$('#Key'+temp.Key+" .ping").text(temp.Pingtime);
			} else if (node.key === "remove") {
				var temp = JSON.parse(node.data);
				$('#Key'+temp.Key).remove();
				if($(".datalist").length == 0) {
					$(".noData").show();
				}
			} else if (node.key === "restart") {
				var temp = JSON.parse(node.data);
				$('#Key'+temp.Key).remove();
				$("#status_table > tbody").append(template(temp));
			} else {
				console.log(message);
			}
        };
		$(document).on("click", "#manual", function () {
			$("#manual").toggleClass("hide");
			$("#manual_panel").toggleClass("hide");
		})
		$(document).on("click", "#manual_close", function () {
			$("#manual").toggleClass("hide");
			$("#manual_panel").toggleClass("hide");
		});
		$(document).on("click", "#manual_scraping", function () {
			if($("#manual_code").val().trim() === "") {
				ins.message("스크래핑 사이트를 선택하여 주십시오.");
				return;
			}
			if($("#manual_id").val().trim() === "") {
				ins.message("아이디가 없습니다.");
				return;
			}
			if($("#manual_pwd").val().trim() === "") {
				ins.message("패스워드가 없습니다.");
				return;
			}
			ins.send("start", "Code=" + $("#manual_code").val() + "&Id=" + $("#manual_id").val() + "&Pw=" + $("#manual_pwd").val());
			$("#manual").toggleClass("hide");
			$("#manual_panel").toggleClass("hide");
		});
	},
	logger: function (value) {
		ins.send("log",value);
	},
	send: function(nkey, ndata) {
		ins.ws.send(JSON.stringify({key:nkey, data:ndata}));
	},
	message: function(value) {
		$(".message").html(value);
		if(value !== "") {
			setTimeout(ins.message, 5000, "");
		}
	}
});