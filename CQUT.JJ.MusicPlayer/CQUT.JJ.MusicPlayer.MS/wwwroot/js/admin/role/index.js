var data = [];

var columnDefs = [
    {
        headerName: "序号",
        field: "sId",
        width: 40,
        filter: 'agNumberColumnFilter'
    },
    {
        headerName: "角色名",
        field: "name",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
        cellRenderer: 'sManagerRenderer'
    },
    {
        headerName: "创建日期",
        field: "creationTime",
        filter: 'agDateColumnFilter',
        filterParams: {
            comparator: timeComparator()
        }
    },
    {
        headerName: "最近修改日期",
        field: "lastModificationTime",
        filter: 'agDateColumnFilter',
        filterParams: {
            comparator: timeComparator()
        }
    }

];


var gridOptions = {
    columnDefs: columnDefs,                                 //列定义
    enableSorting: true,                                    //排序
    enableFilter: true,                                     //过滤器
    animateRows: true,                                      //行动画
    pagination: true,                                       //分页
    //paginationAutoPageSize: true,                         //自动确定每一页显示数据多少
    paginationPageSize: 20,                                 //page size
    getContextMenuItems: getContextMenuItems,               //右键菜单
    allowContextMenuWithControlKey: true,
    enableCellChangeFlash: true,                            //单元格数据改变时闪一下
    paginationNumberFormatter: function (params) {
        return '[' + params.value.toLocaleString() + ']';   //页码格式
    },
    localeText: localeText,                                 //本地化
    excelStyles: [
        {
            id: 'floatLeft',
            alignment: {
                horizontal: 'left',
                vertical: 'center'
            },
        }
    ],
    components: {
        sManagerRenderer: sManagerRenderer,
    },
    onGridReady: async function (params) {
        params.api.sizeColumnsToFit();

        $(window).on('resize', function () {
            setTimeout(function () {
                params.api.sizeColumnsToFit();
            })
        })

        data = await GetRowData();
        params.api.setRowData(data);

        addPageSizeSelector();
    }
};


document.addEventListener("DOMContentLoaded", function () {
    $("#roleGrid").height(getContentHeight());
    var eGridDiv = document.querySelector('#roleGrid');
    new agGrid.Grid(eGridDiv, gridOptions);

    $("#createRole").on('click', function () {
        $.ajax({
            type: 'get',
            url: '/Admin/Role/Create',
            success: function (data) {
                $("#modal").html(data);
            }
        })
    })
});


function sManagerRenderer(params) {
    if (params.data.isDefault) {
        var element = document.createElement("span");
        element.appendChild(document.createTextNode(params.value));
        var brand = createBrand("label-info", "默认角色")
        element.appendChild(brand);
        return element;
    }

    return params.value;
}

function timeComparator(filterLocalDateAtMidnight, cellValue) {
    var dateAsString = cellValue;
    if (dateAsString == null) return -1;
    var dateParts = [dateAsString.substr(0, 4), dateAsString.substr(5, 2), dateAsString.substr(8, 2)];
    var cellDate = new Date(Number(dateParts[2]), Number(dateParts[1]) - 1, Number(dateParts[0]));
    //相等
    if (filterLocalDateAtMidnight.getTime() == cellDate.getTime()) {
        return 0
    }
    //小于
    if (cellDate < filterLocalDateAtMidnight) {
        return -1;
    }
    //大于
    if (cellDate > filterLocalDateAtMidnight) {
        return 1;
    }
}

/**
 * 创建标签
 * @param {string} className 类名
 * @param {string} text 文本
 */
function createBrand(className, text) {
    var labelElement = document.createElement("span");
    labelElement.className += "label label-brand " + className;
    labelElement.appendChild(document.createTextNode(text));
    return labelElement;
}


function createFlagImg(flag) {
}
//页码从1开始
async function GetRowData() {
    return await
        $.ajax({
            type: "get",
            url: "/Admin/Role/GetRoles",
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
                    url: "/Admin/Role/Update",
                    data: { "id": params.node.data.id },
                    success: function (data) {
                        showAjaxGetRequestData(data);
                    }
                })
            },
            //cssClasses: ['redFont', 'bold']
        },
        {
            name: '设置权限',
            action: function () {
                $.ajax({
                    type: "get",
                    url: "/Admin/Role/Authorize",
                    data: { "id": params.node.data.id },
                    success: function (data) {
                        showAjaxGetRequestData(data);
                    }
                })
            },
        },
        {
            name: '删除',
            action: function () {
                $.ajax({
                    type: "get",
                    url: "/Admin/Role/Delete",
                    data: { "id": params.node.data.id },
                    success: function (data) {
                        showAjaxGetRequestData(data);
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

function afterUpdateRole(data) {
    if (data.isSuccessed) {
        success_prompt(data.message);
        $("#my-modal").modal("hide");
        gridOptions.api.forEachNode(function (node) {
            if (node.data.id === data.jsonObject.value.id) {
                node.data.nickName = data.jsonObject.value.nickName;
                gridOptions.api.updateRowData({ update: [node] });
                return;
            }
        });
    } else {
        fail_prompt(data.message);
    }
}

function afterDeleteRole(data) {
    $("#my-modal").modal("hide");
    if (data.isSuccessed) {
        success_prompt(data.message);
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
    } else {
        fail_prompt(data.message);
    }
}
