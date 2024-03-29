﻿$(function () {

    $(document).delegate('#musicFile', 'change', function (e) {
        var file = e.currentTarget.files[0];
        var musicName = file.name;
        $("#musicName").val(musicName.substr(0, musicName.indexOf('.')));
    });  

    $("#singer").change(function () {
        $(".album").hide();
        $(".album").attr("id", null);
        var index = $("#singer>option:selected").index();
        var selectedAlbum = $(".album").eq(index);
        selectedAlbum.show();
        selectedAlbum.attr("id","selected")

    });
    $("#singer").change();

    $("#submitMusic").on("click", function () {
        var fileUpload = $("#musicFile").get(0);
        var file = fileUpload.files[0];
        var data = new FormData();
        data.append(file.name, file);
        data.append("name", $("#musicName").val());
        data.append("singerId", $("#singer").find("option:selected").val())
        data.append("albumId", $("#selected").find("option:selected").val())
        $.ajax({
            type: "POST",
            url: "/Admin/Music/Create",
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {
                afterCreateMusic(data);
            },
            error: function (msg) {
                fail_prompt(msg);
            }
        });
    })
})