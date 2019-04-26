$(function () {
    // Opera 8.0+
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';
    // Safari 3.0+ "[object HTMLElementConstructor]" 
    var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));
    // Internet Explorer 6-11
    var isIE = /*@cc_on!@*/false || !!document.documentMode;
    // Edge 20+
    var isEdge = !isIE && !!window.StyleMedia;
    // Chrome 1 - 71
    var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
    // Blink engine detection
    var isBlink = (isChrome || isOpera) && !!window.CSS;
    console.log("isOpera" + isOpera);
    console.log("isFirefox" + isFirefox);
    console.log("isSafari" + isSafari);
    console.log("isIE" + isIE);
    console.log("isEdge" + isEdge);
    console.log("isChrome" + isChrome);
    console.log("isBlink" + isBlink);
    if (isFirefox || isSafari || isIE || isEdge) {

    }
});
