var Buffer;
var FileLimitLenth = 4095;
$(function () {
    var webSocket = new WebSocket("ws://127.0.0.1:80");
    var reader = new FileReader();

    var chatMessage = $("#chatMessage");
    var chat = $("#chat");
    var chatId = $("#chatid");
    var chatBtn = $("#chatBtn");
    var storeBtn = $("#storeBtn");
    var storeFile = $("#storefile");
    var fileobj = $("#fileobj");

    var selectAll = $("select");
    var textAreaAll = $("textarea");
    var inputAll = $("input");

    webSocket.onopen = function (message) {
        SetMessage("Server Connect...");
        var data = new Uint8Array(1);
        data[0] = 0x0C;
        webSocket.send(data);
        UnsetDisabled();
    }
    webSocket.onclose = function (message) {
        SetMessage("Server Disconnect...");
        SetDisabled();
    }
    webSocket.onerror = function (message) {
        SetMessage("Server Error...");
		EndProgress();
        SetDisabled();
    }
    webSocket.onmessage = function (message) {
        var data = JSON.parse(message.data);
        if (data.type == 0) {
            SetMessage(data.message);
        } else if (data.type == 1) {
            var temp = "";
            for (var i = 0; i < data.list.length; i++) {
                temp += "<option value='" + data.list[i] + "'>" + data.list[i] + "</option>";
            }
            storeFile.html(temp);
        }
    }
    var SetMessage = function (data) {
        var val = chatMessage.val();
        chatMessage.focus();
        chatMessage.val(val + data + "\n");
        chatMessage.scrollTop(9999999999);
        chat.focus();
    }
    var SendMessage = function () {
        var message = chatId.val() + ")" + chat.val();
        chat.val("");
        webSocket.send(message);
    }
    var SetDisabled = function () {
        selectAll.prop("disabled", "disabled");
        textAreaAll.prop("disabled", "disabled");
        inputAll.prop("disabled", "disabled");
    }
    var UnsetDisabled = function () {
        selectAll.prop("disabled", "");
        textAreaAll.prop("disabled", "");
        inputAll.prop("disabled", "");
    }

    chatBtn.on("click", function (event) {
        SendMessage();
    });
    storeFile.on("dbclick", function (event) {
        var index = storeFile.prop("selectedIndex");
        location.href = "/download?" + $("#storefile>option:nth-child(" + (index + 1) + ")").val();
    });
    chat.on("keydown", function (event) {
        if (event.keyCode == 13) {
            SendMessage();
        }
    });
    storeBtn.on("click", function () {
        var file = fileobj[0].files[0];
        reader.readAsArrayBuffer(file);
    });

    reader.onload = function (e) {
        var file = fileobj[0].files[0];
		var	fileSocket = new WebSocket("ws://127.0.0.1:80");
		fileSocket.onopen = function (message) {
			setTimeout(SendFileHeader, 10, fileSocket, file, new Uint8Array(e.currentTarget.result));
		}
		fileSocket.onclose = function (message) {
			var data = new Uint8Array(1);
		    data[0] = 0x0D;
	        webSocket.send(data);
		}
		fileSocket.onmessage = function (message) {}
		fileSocket.onerror = function (message) {}
    }
    SetDisabled();
});
function SetProgress(index,length)
{
	location.href="#top";
	if(!$("body").hasClass("progress")){
		$("body").addClass("progress");
		$("div.progressBarlayout").addClass("on");
	}
	$("#progressBar > span").html(index + " / "+length);
	var rate = (index/length) *100;
	$("div.progressBarlayout > div.progress > div").css("width",rate+"%");
}
function EndProgress(){
	$("body").removeClass("progress");
	$("div.progressBarlayout").removeClass("on");
}
function SendFileHeader(fileSocket, file, filedata) {
    var header = new Uint8Array(260);
    var count = Math.floor(file.size / FileLimitLenth);
    for (var i = 0; i < 260; i++) {
        header[i] = 0x20;
    }
    header[0] = 0x0A;
    header = BitConverter(header, 1, file.size);
    header = GetFileName(header, 5, file.name);
    fileSocket.send(header);
	SetProgress(0,count);
    setTimeout(SendFileBody, 10, fileSocket, file, filedata, 0, count);
}
function SendFileBody(fileSocket, file, filedata, peek, count) {
    var index = Math.floor(peek / FileLimitLenth);
    if (index < count) {
        var data = new Uint8Array(FileLimitLenth + 1);
        data[0] = 0x0B;
        data = ArrayCopy(filedata, peek, data, 1, FileLimitLenth);
		fileSocket.send(data);
		peek += FileLimitLenth;
		SetProgress(index,count);
		setTimeout(SendFileBody, 10, fileSocket, file, filedata, peek, count);
    } else {
		remain = file.size%FileLimitLenth;
		var data = new Uint8Array(remain +1);
		data[0] = 0x0B;
		data = ArrayCopy(filedata,peek,data,1,remain);
		fileSocket.send(data);
		EndProgress();
		fileSocket.close();
    }
}
function ArrayCopy(source, sourceIdx, destination, destinationIdx, length) {
    for (var i = sourceIdx, j = destinationIdx; i < sourceIdx + length; i++, j++) {
        destination[j] = source[i];
    }
    return destination;
}
function GetFileName(bin, index, val) {
    var charList = toUTF8Array(val);
    for (var i = 0; i < charList.length; i++) {
        bin[i + index] = charList[i];
    }
    return bin;
}
function toUTF8Array(str) {
    var utf8 = [];
    for (var i = 0; i < str.length; i++) {
        var charcode = str.charCodeAt(i);
        if (charcode < 0x80) {
            utf8.push(charcode);
        } else if (charcode < 0x800) {
            utf8.push(0xc0 | (charcode >> 6), 0x80 | (charcode & 0x3f));
        } else if (charcode < 0xd800 || charcode >= 0xe000) {
            utf8.push(0xe0 | (charcode >> 12), 0x80 | ((charcode >> 6) & 0x3f), 0x80 | (charcode & 0x3f));
        } else {
            i++;
            charcode = 0x10000 + (((charcode & 0x3ff) << 10) | (str.charCodeAt(i) & 0x3ff));
            utf8.push(0xf0 | (charcode >> 18), 0x80 | ((charcode >> 12) & 0x3f), 0x80 | ((charcode >> 6) & 0x3f), 0x80 | (charcode & 0x3f));
        }
    }
    return utf8;
}
function BitConverter(bin, index, val) {
    bin[index + 3] = val >>> 24;
    bin[index + 2] = val >>> 16;
    bin[index + 1] = val >>> 8;
    bin[index + 0] = val;
    return bin;
}