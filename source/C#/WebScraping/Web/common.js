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
						+ "<td>" + item.Pingtime + "</td>"
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
			} else if(node.key === "remove"){
				$("#Key"+node.data).remove();
			} else  if(node.key === "insert") {
				$(".noData").hide();
				$("#status_table > tbody").append(template(JSON.parse(node.data)));
			}else {
				console.log(message);
			}
        };
		/*
        function sendMessage(){
            var message = document.getElementById("textMessage");
            messageTextArea.value += "Send to Server => "+message.value+"\n";
            webSocket.send(message.value);
            message.value = "";
        }
        function disconnect(){
            webSocket.close();
        }*/

		/*$(document).on("click", "#refresh_btn", function () {
			$("#reflesh_view").html($("#reflesh_init").val());
			ins.receive();
			
		});*/
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
	/*
	,
	restart: function () {
		$.ajax({
			url: "./Restart",
			type: "GET"
		});
	},
	refresh: function () {
		this.receive();
		$("#reflesh_view").html($("#reflesh_init").val());
		setTimeout(this.tick, 1000, this);
	},
	receive: function () {
		function now() {
			function format(time) {
				var temp = time + "";
				if (temp.length == 1) {
					return "0" + temp;
				}
				return temp;
			}
			var dt = new Date();
			return dt.getFullYear() + "/" +
				format((dt.getMonth() + 1)) + "/" +
				format(dt.getDate()) + " " +
				format(dt.getHours()) + ":" +
				format(dt.getMinutes()) + ":" +
				format(dt.getSeconds());
		}
		function initTable() {
			$("#status_table > tbody").html("");
		}
		initTable();
		$.ajax({
			url: "./List",
			type: "GET",
			dataType: "json",
			success: function (data, textStatus, jqXHR) {
				if (data.length === 0) {
					$("#status_table > tbody").append("<tr><td colspan='5' class='noData'>Not excuted</td></tr>");
				}
				for (var i in data) {
					var temp = "<tr>"
						+ "<td>" + data[i].Key + "</td>"
						+ "<td>" + data[i].Code + "</td>"
						+ "<td>" + data[i].Id + "</td>"
						+ "<td>" + data[i].Starttime + "</td>"
						+ "<td>" + data[i].Pingtime + "</td>"
						//+ "<td>"+data[i].Status+"</td>"
						+ "</tr>";
					$("#status_table > tbody").append(temp);
				}
				$("#refreshTime").html(now());
			}
		});
	},
	tick: function () {
		var temp = Number($("#reflesh_view").html()) - 1;
		if (temp < 0) {
			ins.refresh();
		} else {
			$("#reflesh_view").html(temp);
			setTimeout(ins.tick, 1000);
		}
	},
	logger: function (str) {
		$.ajax({
			url: ".Log?" + str,
			type: "GET"
		});
	}*/
});