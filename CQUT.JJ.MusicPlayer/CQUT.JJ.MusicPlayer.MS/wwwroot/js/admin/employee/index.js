var data = [];

var columnDefs = [
    { headerName: "序号", field: "sId", width: 40, colId:"110" },
    { headerName: "用户名", field: "userName" },
    { headerName: "昵称", field: "nickName" },
    { headerName: "创建日期", field: "creationTime" }
];


var gridOptions = {
    columnDefs: columnDefs, //列定义
    enableSorting: true,    //排序
    enableFilter: true,     //过滤器
    animateRows: true,      //行动画
    pagination: true,       //分页
    paginationAutoPageSize: true, //自动确定每一页显示数据多少
    getContextMenuItems: getContextMenuItems,   //右键菜单
    allowContextMenuWithControlKey: true,      
    enableCellChangeFlash: true,    //单元格数据改变时闪一下
    onGridReady: async function (params) {
        params.api.sizeColumnsToFit();

        $(window).on('resize', function () {
            setTimeout(function () {
                params.api.sizeColumnsToFit();
            })
        })

        data = await GetRowData(params);
        params.api.setRowData(data);
    }
};

document.addEventListener("DOMContentLoaded", function () {

    var eGridDiv = document.querySelector('#employeeGrid');
    new agGrid.Grid(eGridDiv, gridOptions);

    $("#createEmployee").on('click', function () {
        $.ajax({
            type: 'get',
            url: '/Admin/Employee/Create',
            success: function (data) {
                $("#modal").html(data);
            }
        })
    })
});




function createFlagImg(flag) {
}
//页码从1开始
async function GetRowData() {
    return await
        $.ajax({
            type: "get",
            url: "/Admin/Employee/GetEmployees",
            success: function (data) {
                return data;
            }
        })
}

function getContextMenuItems(params) {
    var result = [
        {
            name: '编辑',
            action: function () {
                $.ajax({
                    type: "get",
                    url: "/Admin/Employee/Update",
                    data: { "id": params.node.data.id },
                    success: function (data) {
                        $("#modal").html(data);
                    }
                })
            },
            //cssClasses: ['redFont', 'bold']
        },
        {
            name: '权限管理',
            subMenu: [
                {
                    name: '设置权限',
                    action: function () {
                        console.log('Ireland was pressed');
                    },                    
                },
                {
                    name: '设置角色',
                    action: function () {
                        console.log('UK was pressed');
                    },                    
                },
            ]
        },
        {
            name: '删除',
            action: function () {
                $.ajax({
                    type: "get",
                    url: "/Admin/Employee/Delete",
                    data: { "id": params.node.data.id },
                    success: function (data) {
                        $("#modal").html(data);
                    }
                })
            }
        },
        //加一个横线
        'separator',      
        'copy'
    ];

    return result;
}


function afterCreateEmployee(data) {
    alert(data.message);
    if (data.isSuccessed) {
        $("#my-modal").modal("hide");
        data.jsonObject.value.sId = gridOptions.api.paginationGetRowCount() + 1;
        gridOptions.api.updateRowData({ add: [data.jsonObject.value] });
    }    
}

function afterUpdateEmployee(data) {
    alert(data.message);
    if (data.isSuccessed) {
        $("#my-modal").modal("hide");
        gridOptions.api.forEachNode(function (node) {
            if (node.data.id === data.jsonObject.value.id) {
                node.data.nickName = data.jsonObject.value.nickName;
                gridOptions.api.updateRowData({ update: [node] });
                return;
            }
        });      
    }
}

function afterDeleteEmployee(data) {
    alert(data.message);
    $("#my-modal").modal("hide");
    if (data.isSuccessed) {       
        var sId = 0;
        gridOptions.api.forEachNode(function (node) {
            if (node.data.id === data.jsonObject.value.id) {
                gridOptions.api.updateRowData({ remove: [node.data] });
                sId = node.data.sId;
            }
            else if (sId !== 0) {
                node.data.sId = sId++;
                gridOptions.api.updateRowData({ update: [node] });
            }
        });
    }
}


