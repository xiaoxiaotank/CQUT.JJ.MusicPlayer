var columnDefs = [
    {
        headerName: "序号",
        field: "sId",
        width: 80,
        filter: 'agNumberColumnFilter'
    },
    {
        headerName: "姓名",
        field: "name",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "外文名",
        field: "foreignName",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "国籍",
        field: "nationality",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "创建者",
        field: "creatorName",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "修改者",
        field: "menderName",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "下架者",
        field: "unpublisherName",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
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
    onGridReady: async function (params) {
        params.api.sizeColumnsToFit();

        $(window).on('resize', function () {
            setTimeout(function () {
                params.api.sizeColumnsToFit();
            })
        })

        var data = await GetRowData();
        params.api.setRowData(data);

        addPageSizeSelector();
    }
};


document.addEventListener("DOMContentLoaded", function () {
    $("#singerGrid").height(getContentHeight());
    var eGridDiv = document.querySelector('#singerGrid');
    new agGrid.Grid(eGridDiv, gridOptions);

    $("#createSinger").on('click', function () {
        $.ajax({
            type: 'get',
            url: '/Admin/Singer/Create',
            success: function (data) {
                $("#modal").html(data);
            }
        })
    })

    $("#exportToExcel").on('click', function () {
        var params = {
            skipHeader: false,          //如果不希望第一行是列标题名称，则设置为true。
            columnGroups: true,         //设置为true以包含标题列分组。
            skipFooters: false,         //设置为true仅在分组时才跳过页脚。没有影响，如果不分组，或者如果不使用页脚分组。
            skipGroups: false,          //如果对行进行分组，则设置为true以跳过行组页眉和页脚。没有影响，如果不分组行。
            skipPinnedTop: false,
            skipPinnedBottom: false,
            allColumns: false,          //如果为true，则所有列将按照它们在columnDefs中出现的顺序导出。否则，仅导出网格中当前显示的列，并按此顺序导出。
            onlySelected: false,        //只导出选定的行。
            fileName: '未发布歌唱家清单',       //用作文件名的字符串。如果缺少，将使用文件名'export.xls'。
            sheetName: 'MusicList'
        };
        gridOptions.api.exportDataAsExcel(params);
    })
});

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



function createFlagImg(flag) {
}
//页码从1开始
async function GetRowData() {
    return await
        $.ajax({
            type: "get",
            url: "/Admin/Singer/GetUnpublishedSingers",
            success: function (data) {
                return data;
            }
        })
}

function getContextMenuItems(params) {
    var result = [
        {
            name: '发布',
            action: function () {
                $.ajax({
                    type: "post",
                    url: "/Admin/Singer/Publish",
                    data: { "id": params.node.data.id },
                    success: function (data) {
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
                })
            }
        },
        'separator',
        {
            name: '编辑基本信息',
            action: function () {
                $.ajax({
                    type: "get",
                    url: "/Admin/Singer/UpdateBasic",
                    data: { "id": params.node.data.id },
                    success: function (data) {
                        showAjaxGetRequestData(data);
                    }
                })
            },
            //cssClasses: ['redFont', 'bold']
        },
        {
            name: '删除',
            action: function () {
                $.ajax({
                    type: "get",
                    url: "/Admin/Singer/Delete",
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


function afterCreateSinger(data) {
    if (data.isSuccessed) {
        success_prompt(data.message);
        $("#my-modal").modal("hide");
        data.jsonObject.value.sId = gridOptions.api.paginationGetRowCount() + 1;
        gridOptions.api.updateRowData({ add: [data.jsonObject.value] });
    } else {
        fail_prompt(data.message);
    }
}

function afterUpdateBasicSinger(data) {
    if (data.isSuccessed) {
        success_prompt(data.message);
        $("#my-modal").modal("hide");
        gridOptions.api.forEachNode(function (node) {
            if (node.data.id === data.jsonObject.value.id) {
                node.data.menderId = data.jsonObject.value.menderId;
                node.data.menderName = data.jsonObject.value.menderName;
                node.data.foreignName = data.jsonObject.value.foreignName;
                node.data.nationality = data.jsonObject.value.nationality;
                node.data.lastModificationTime = data.jsonObject.value.lastModificationTime
                gridOptions.api.updateRowData({ update: [node] });
                return;
            }
        });
    } else {
        fail_prompt(data.message);
    }
}

function afterDeleteSinger(data) {
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


