// Write your JavaScript code.
function isJsonFormat(data) {
    return typeof (data) == "object"
        && Object.prototype.toString.call(data).toLowerCase() == "[object object]"
        && !data.length;     
}


var prompts = [];
/**
 * 弹出式提示框，默认3秒自动消失
 * @param message 提示信息
 * @param style 提示样式，有alert-success、alert-danger、alert-warning、alert-info
 * @param time 消失时间
 */
var prompt = function (iconClass, message, style, time) {
    style = (style === undefined) ? 'alert-success' : style;
    time = (time === undefined) ? 3000 : time;
    var p = $('<div>')
        .appendTo('body')
        .addClass('alert ' + style)
        .html('<span class="glyphicon ' + iconClass + ' icon-prompt"></span>' +
        message);
    for (var i = 0; i < prompts.length; i++) {
        var promptBlock = prompts[i];
        if (promptBlock) {
            promptBlock.stop(false, true).animate({ marginBottom: (i + 1) * (promptBlock.outerHeight() + 10) + "px" });
        }
    }
    p.css("marginBottom", -(p.outerHeight() + 10) + "px")
        .css("display", "block")
        .stop(true, true).animate({ marginBottom: "0" }, "normal", "swing", function () {
            prompts.unshift(p);
        });
    setTimeout(function () {
        p.fadeOut(500, function () {
            prompts.pop(this);
            this.remove();
        });
    }, time)
};


// 成功提示
var success_prompt = function (message, time) {
    prompt("glyphicon-ok", message, 'alert-success', time);
};

// 失败提示
var fail_prompt = function (message, time) {
    prompt("glyphicon-remove", message, 'alert-danger', time);
};

// 提醒
var warning_prompt = function (message, time) {
    prompt(" glyphicon-warning-sign", message, 'alert-warning', time);
};

// 信息提示
var info_prompt = function (message, time) {
    prompt("glyphicon-bullhorn", message, 'alert-info', time);
};