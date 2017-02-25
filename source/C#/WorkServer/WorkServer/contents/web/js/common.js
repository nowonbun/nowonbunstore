var Buffer;
$(function () {

    var webSocket = new WebSocket("ws://127.0.0.1:80");
    var chatMessage = $("#chatMessage");
    var chat = $("#chat");
    var chatId = $("#chatid");
    var chatBtn = $("#chatBtn");
    var storeFile = $("#storefile");
    var selectAll = $("select");
    var textAreaAll = $("textarea");
    var inputAll = $("input");

    webSocket.onopen = function (message) {
        SetMessage("Server Connect...");
        var data = new Uint8Array(1);
        data[0] = 7;
        webSocket.send(data);
        UnsetDisabled();
    }
    webSocket.onclose = function (message) {
        SetMessage("Server Disconnect...");
        SetDisabled();
    }
    webSocket.onerror = function (message) {
        SetMessage("Server Error...");
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

    SetDisabled();
});
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