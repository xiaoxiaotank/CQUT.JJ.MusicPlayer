$(function () {
    $("#singer").change(function () {
        $(".album").hide();
        $(".album").attr("name", null);
        var index = $("#singer>option:selected").index();
        var selectedAlbum = $(".album").eq(index);
        selectedAlbum.show();
        selectedAlbum.attr("name", "AlbumId");
    });
    $("#singer").change();
})