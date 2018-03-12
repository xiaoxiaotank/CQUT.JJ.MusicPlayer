function onPageSizeChanged(newPageSize) {
    var value = document.getElementById('page-size').value;
    gridOptions.api.paginationSetPageSize(Number(value));
}

function addPageSizeSelector() {
    $(".ag-paging-panel").prepend('<div style="flex-direction:row-reverse">' +
        '每页展示 ' +
        '<select onchange="onPageSizeChanged()" id="page-size">' +
        '<option value="20" selected>20</option>' +
        '<option value="50">50</option>' +
        '<option value="100">100</option>' +
        '</select>' +
        ' 条数据' +
        '</div>');
}
