var id = 0;
permissionTreeId = "permissionTree"
var setting = {
    view: {
        selectedMulti: false, //设置是否能够同时选中多个节点
        showIcon: true,  //设置是否显示节点图标
        showLine: true,  //设置是否显示节点与节点之间的连线
        showTitle: true,  //设置是否显示节点的title提示信息
    },
    check: {
        enable: true,                   //设置是否显示checkbox复选框
        chkStyle: "checkbox",
        chkboxType: { "Y": "ps", "N": "ps" },
        checkable: true
    },
    data: {
        simpleData: {
            enable: true, //设置是否启用简单数据格式（zTree支持标准数据格式跟简单数据格式，上面例子中是标准数据格式）
            idKey: "id",  //设置启用简单数据格式时id对应的属性名称
            pidKey: "pId" //设置启用简单数据格式时parentId对应的属性名称,ztree根据id及pid层级关系构建树结构
        }
    },   
    async: {
        enable: true,      //设置启用异步加载
        type: "post",      //异步加载类型:post和get
        contentType: "application/json", //定义ajax提交参数的参数类型，一般为json格式
        autoParam: ["id=id", "name=name", "pId=pId"] //定义提交时参数的名称，=号前面标识节点属性，后面标识提交时json数据中参数的名称
    }
};

$(document).ready(function () {
    id = $('#' + permissionTreeId).attr('data-id');
    var setting = {
        view: {
            selectedMulti: false, //设置是否能够同时选中多个节点
            showIcon: true,  //设置是否显示节点图标
            showLine: true,  //设置是否显示节点与节点之间的连线
            showTitle: true,  //设置是否显示节点的title提示信息
        },
        check: {
            enable: true,                   //设置是否显示checkbox复选框
            chkStyle: "checkbox",
            chkboxType: { "Y": "ps", "N": "ps" },
            checkable: true
        },
        data: {
            simpleData: {
                enable: true, //设置是否启用简单数据格式（zTree支持标准数据格式跟简单数据格式，上面例子中是标准数据格式）
                idKey: "id",  //设置启用简单数据格式时id对应的属性名称
                pidKey: "pId" //设置启用简单数据格式时parentId对应的属性名称,ztree根据id及pid层级关系构建树结构
            }
        },
        async: {
            enable: true,      //设置启用异步加载
            type: "get",      //异步加载类型:post和get
            contentType: "application/json", //定义ajax提交参数的参数类型，一般为json格式
            url: "/Admin/Employee/GetPermissions?id=" + id,
            autoParam: ["id=id", "name=name", "pId=pId"] //定义提交时参数的名称，=号前面标识节点属性，后面标识提交时json数据中参数的名称
        }
    };
    $.fn.zTree.init($("#" + permissionTreeId), setting);

    $("#submitBtn").on('click', function () {
        var permissions = [];
        $(getPermissionTreeObj().getCheckedNodes(true)).each(function (i, n) {
            permissions.push(n.id);
        })
        var data = {
            "id": id,
            "permissionCodes": permissions,
        };
        
        $.ajax({
            type: 'post',
            url: "/Admin/Employee/Authorize",
            data: data,
            success: function (data) {
                if (data) {
                    if (data.isSuccessed) {
                        success_prompt(data.message);
                    } else {
                        fail_prompt(data.message);
                    }
                }
            },
        });
    })
})


function getPermissionTreeObj() {
    return $.fn.zTree.getZTreeObj(permissionTreeId);
}
