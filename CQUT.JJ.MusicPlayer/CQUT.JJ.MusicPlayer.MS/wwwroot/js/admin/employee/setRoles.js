var columnDefs = [
    {
        headerName: "序号",
        field: "sId",
        width: 40,
        filter: 'agNumberColumnFilter',
    },
    {
        headerName: "角色名",
        field: "roleName",
        filter: 'agTextColumnFilter',
        cellClass: 'floatLeft',
        cellRenderer: 'attributeRenderer'
    },   
];


var gridOptionsOfRole = {
    columnDefs: columnDefs,                                 //列定义
    enableSorting: true,                                    //排序
    enableFilter: true,                                     //过滤器
    pagination: true,                                       //分页
    rowSelection: 'multiple',                               //多选
    rowMultiSelectWithClick: true,                          //点击多选模式
    paginationPageSize: 10,                                 //page size
    paginationNumberFormatter: function (params) {
        return '[' + params.value.toLocaleString() + ']';   //页码格式
    },
    localeText: localeText,                                 //本地化
    components: {
        attributeRenderer: attributeRenderer,
    },
    onGridReady: async function (params) {
        params.api.sizeColumnsToFit();
        $("#roleGrid").height("300px");

        var data = await GetRowData();
        params.api.setRowData(data);

        selectOwnedRoles();

        addPageSizeSelector();
    }
};

$(function () {    
    var eGridDiv = document.querySelector('#roleGrid');
    new agGrid.Grid(eGridDiv, gridOptionsOfRole);

    $("#setRolesBtn").on('click', function () {
        var roleIds = [];
        var selectedNodes = gridOptionsOfRole.api.getSelectedNodes();
        for (node of selectedNodes) {
            roleIds.push(node.data.id);
        }
        var id = GetUserId();
        var data = {
            "id": id,
            "roleIds": roleIds,
        };

        $.ajax({
            type: 'post',
            url: "/Admin/Employee/SetRoles",
            data: data,
            success: function (data) {
                if (data) {
                    if (data.isSuccessed) {
                        success_prompt(data.message);
                        gridOptions.api.forEachNode(function (node) {
                            if (node.data.id == id) {
                                console.log(node);
                                node.data.roleNames = data.jsonObject.value;
                                gridOptions.api.redrawRows(node);
                            }
                        })
                    } else {
                        fail_prompt(data.message);
                    }
                }
            },
        });
    })
});

function attributeRenderer(params) {
    if (params.data.isDefault) {
        var element = document.createElement("span");
        element.appendChild(document.createTextNode(params.value));
        var brand = createBrand("label-info", "默认角色")
        element.appendChild(brand);
        return element;
    }

    return params.value;
}

function createBrand(className, text) {
    var labelElement = document.createElement("span");
    labelElement.className += "label label-brand " + className;
    labelElement.appendChild(document.createTextNode(text));
    return labelElement;
}

async function GetRowData() {
    var id = GetUserId();
    return await
        $.ajax({
            type: "get",
            url: "/Admin/Employee/GetUserRolesByUserId",
            data: { "id": id},
            success: function (data) {
                return data;
            }
        })
}

function selectOwnedRoles() {
    gridOptionsOfRole.api.forEachNode(function (node) {
        if (node.data.hasOwned) {
            node.setSelected(true);
        }
    });
}

function GetUserId() {
    return $("#roleGrid").attr("data-id");
}