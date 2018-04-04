
var columnDefs = [
    {
        headerName: "序号",
        field: "sId",
        width: 80,
        filter: 'agNumberColumnFilter'
    },
    {
        headerName: "信息描述",
        field: "message",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "类型",
        field: "type",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "操作者",
        field: "userName",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "日志源",
        field: "source",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
    },
    {
        headerName: "日期",
        field: "dateTime",
        filter: 'agDateColumnFilter',
        filterParams: {
            comparator:function (filterLocalDateAtMidnight, cellValue){
                var dateAsString = cellValue;
                if (dateAsString == null) return -1;
                var dateParts = [dateAsString.substr(0, 4), dateAsString.substr(5, 2), dateAsString.substr(8, 2)];
                var cellDate = new Date(Number(dateParts[2]), Number(dateParts[1]) - 1, Number(dateParts[0]));

                if (filterLocalDateAtMidnight.getTime() == cellDate.getTime()) {
                    return 0
                }

                if (cellDate < filterLocalDateAtMidnight) {
                    return -1;
                }

                if (cellDate > filterLocalDateAtMidnight) {
                    return 1;
                }
            }
        }
    }
];


var gridOptions = {
    columnDefs: columnDefs,
    enableSorting: true,
    enableFilter: true,
    animateRows: true,
    pagination: true,
    paginationPageSize: 20,                                 //page size
    paginationNumberFormatter: function (params) {
        return '[' + params.value.toLocaleString() + ']';
    },
    localeText: localeText,
    rowClassRules:{
        "log-info": function (params) { return params.node.data.type == "信息" },
        "log-success": function (params) { return params.node.data.type == "成功" },
        "log-fail": function (params) { return params.node.data.type == "失败" },
        "log-warning": function (params) { return params.node.data.type == "警告" },
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
    $("#logGrid").height(getContentHeight());
    var eGridDiv = document.querySelector('#logGrid');
    new agGrid.Grid(eGridDiv, gridOptions);
});

//页码从1开始
async function GetRowData() {
    return await
        $.ajax({
            type: "get",
            url: "/Admin/LocalLog/GetLogs",
            success: function (data) {
                return data;
            }
        })
}
