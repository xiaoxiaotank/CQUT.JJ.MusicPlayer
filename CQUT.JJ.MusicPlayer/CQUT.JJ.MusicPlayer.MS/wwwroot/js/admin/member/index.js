
var columnDefs = [
    { headerName: "编号", field: "id", hide: true },
    { headerName: "序号", field: "sId" },
    { headerName: "用户名", field: "userName" },
    { headerName: "昵称", field: "nickName" },
    { headerName: "创建日期", field: "creationTime" }
];


var gridOptions = {
    columnDefs: columnDefs,
    enableSorting: true,
    enableFilter: true,
    animateRows: true,
    pagination: true,
    paginationAutoPageSize: true,
    onGridReady: function (params) {
        params.api.sizeColumnsToFit();
    }
};


document.addEventListener("DOMContentLoaded", function () {

    var eGridDiv = document.querySelector('#memberGrid');
    new agGrid.Grid(eGridDiv, gridOptions);

    GetRowData();
    gridOptions.api.setRowData();
});

//页码从1开始
function GetRowData() {
    $.ajax({
        type: "get",
        url: "/Admin/Member/GetMembers",
        success: function (data) {
            gridOptions.api.setRowData(data);
        }
    })
}