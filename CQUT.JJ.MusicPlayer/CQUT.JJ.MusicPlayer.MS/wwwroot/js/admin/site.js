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
    //固定菜单栏的方式
    var url = location.pathname;
    $('.sidebar-menu a').each(function () {
        var href = $(this).attr('href');
        if (url === href) {
            $(this).parents('li').addClass("active");
            $(this).parents('ul').addClass("menu-open");
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

function getContentHeight() {
    return $('.content-wrapper').height()
        - $('.main-footer').height()
        - $('.navbar').height() - 1;
}