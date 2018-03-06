// Write your JavaScript code.
function isJsonFormat(data) {
    return typeof (data) == "object"
        && Object.prototype.toString.call(data).toLowerCase() == "[object object]"
        && !data.length;     
}


function refreshMenu(keywords) {
    $.ajax({
        type: "get",
        url: "/Admin/Menu/RefreshMenu",
        data: { "keywords": keywords },
        success: function (msg) {
            $("#sidebardiv").html(msg);
        }
    });
}

$(function () {
    //自己写的固定菜单栏
    var url = location.pathname;
    $('.sidebar-menu a').each(function () {
        var href = $(this).attr('href');
        if (url === href) {
            $(this).parents('li').eq(1).addClass("active");
            $(this).parents('ul').first().css("display", "block");
            $(this).css("color", "white");
        }
    });


    $("#search").bind('input propertychange', function () {
        var keywords = $("#search").val();
        if (keywords === "") {
            refreshMenu("");
        }
    })
    $("#search-btn").click(function () {
        var keywords = $("#search").val();
        refreshMenu(keywords);
    })
})