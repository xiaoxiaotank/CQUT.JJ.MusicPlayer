var menuTreeId = "menuTree";
var currentNodeId = null;
var menuTreeObj = null;
var oldName = "";

var setting = {
    view: {
        selectedMulti: false, //设置是否能够同时选中多个节点
        showIcon: true,  //设置是否显示节点图标
        showLine: true,  //设置是否显示节点与节点之间的连线
        showTitle: true,  //设置是否显示节点的title提示信息
    },
    check: {
        enable: false,                   //设置是否显示checkbox复选框
        //chkStyle: "checkbox",
        //chkboxType: { "Y": "ps", "N": "ps" },               
        //checkable: true
    },
    data: {
        simpleData: {
            enable: true, //设置是否启用简单数据格式（zTree支持标准数据格式跟简单数据格式，上面例子中是标准数据格式）
            idKey: "id",  //设置启用简单数据格式时id对应的属性名称
            pidKey: "pId" //设置启用简单数据格式时parentId对应的属性名称,ztree根据id及pid层级关系构建树结构
        }
    },
    edit: { //此属性添加后，树才可以被拖拽，需要提前引用exedit.js
        enable: true,
        showRenameBtn: false,
        showRemoveBtn: false,
        editNameSelectAll: true,
        drag: {
            prev: false,
            next: false,
        }
    },
    callback: {
        onClick: menuItemOnClick,   //定义节点单击事件回调函数
        onRightClick: onRightClick, //定义节点右键单击事件回调函数
        beforeRename: beforeRename, //定义节点重新编辑成功前回调函数，一般用于节点编辑时判断输入的节点名称是否合法
        onRename: onRename,         //beforeRename返回true执行
        beforeDrop: beforeDrop,     //拖拽操作结束前执行
        //onDblClick: onDblClick,  //定义节点双击事件回调函数
        //onCheck: onCheck    //定义节点复选框选中或取消选中事件的回调函数
    },
    async: {
        enable: true,      //设置启用异步加载
        type: "post",      //异步加载类型:post和get
        contentType: "application/json", //定义ajax提交参数的参数类型，一般为json格式
        url: "/Admin/Menu/GetMenuByParentId",    //定义数据请求路径
        autoParam: ["id=id", "name=name", "pId=pId"] //定义提交时参数的名称，=号前面标识节点属性，后面标识提交时json数据中参数的名称
    }
};


$(function () {     
    $.fn.zTree.init($("#" + menuTreeId), setting);
    menuTreeObj = getMenuTreeObj();

    $("#createMenu").click(function () {
        var parentId = currentNodeId;
        $.ajax({
            type: "get",
            url: "/Admin/Menu/CreateMenuItem",
            data: { "parentId": parentId },
            success: function (msg) {
                $("#modal").html(msg);               
            }
        })
    }) 

    $("#renameMenu").click(function () {
        var renameNode = menuTreeObj.getNodeByParam("id", currentNodeId);
        menuTreeObj.editName(renameNode);
        $("#rMenu").css({ "visibility": "hidden" }); //设置右键菜单不可见  
    }) 

    $("#deleteMenu").click(function () {
        var id = currentNodeId;
        $.ajax({
            type: "get",
            url: "/Admin/Menu/DeleteMenuItem",
            data: { "id": id },
            success: function (msg) {
                $("#modal").html(msg);
            }
        })
    }) 

    $("#editMenu").click(function () {
        var id = currentNodeId;
        $.ajax({
            type: "get",
            url: "/Admin/Menu/UpdateMenuItem",
            data: { "id": id },
            success: function (msg) {
                $("#modal").html(msg);                                
            }
        });
    })
})

function getMenuTreeObj() {
    return $.fn.zTree.getZTreeObj(menuTreeId);
}


function menuItemOnClick(event, treeId, treeNode) {
    alert(treeNode.id + " ," + treeNode.name + "," + treeNode.pId);
};

// 在ztree上的右击事件  
function onRightClick(event, menuTreeIdName, treeNode) {
    currentNodeId = treeNode.id;
    if (!treeNode && event.target.tagName.toLowerCase() != "button" && $(event.target).parents("a").length == 0) {
        showRMenu("root", event.clientX, event.clientY);
    }
    else if (treeNode && !treeNode.noR) {
        showRMenu("node", event.clientX, event.clientY);
    }
}

//显示右键菜单  
function showRMenu(type, x, y) {
    $("#rMenu").css({ "top": y + "px", "left": x + "px", "visibility": "visible" }); //设置右键菜单的位置、可见  
    $("body").bind("mousedown", onBodyMouseDown);
}
//隐藏右键菜单  
function hideRMenu() {
    if (rMenu)
        $("#rMenu").css({ "visibility": "hidden" }); //设置右键菜单不可见  
    $("body").unbind("mousedown", onBodyMouseDown);
}
//鼠标按下事件  
function onBodyMouseDown(event) {
    if (!(event.target.id == "rMenu" || $(event.target).parents("#rMenu").length > 0)) {
        $("#rMenu").css({ "visibility": "hidden" });
    }
}  


//菜单树成功操作后执行
function afterMenuTreeOption(data) {   
    $("#my-modal").modal("hide");
    alert(data.message);
    if (data.isSuccessed)
        refreshMenu();
}

function refreshChildNodes(refreshParentNode) {
    var parentId = 0;
    if (refreshParentNode != null) {
        parentId = refreshParentNode.id;
        refreshParentNode.isParent = true;
        menuTreeObj.expandNode(refreshParentNode);
    }
    menuTreeObj.setting.async.url = "/Admin/Menu/GetMenuByParentId?parentId=" + parentId;
    menuTreeObj.reAsyncChildNodes(refreshParentNode, "refresh", false);
}

$("#add-root").on("click", function () {
    $.ajax({
        type: "get",
        url: "/Admin/Menu/CreateMenuItem",
        success: function (msg) {
            $("#modal").html(msg);
        }
    });
});

function createMenuItemed(data) {
    if (data.isSuccessed) {
        var refreshNode = menuTreeObj.getNodeByParam("id", currentNodeId);
        refreshChildNodes(refreshNode);
    }
    afterMenuTreeOption(data);
}

function updateMenuItemed(data) {    
    afterMenuTreeOption(data);
    if (data.isSuccessed) {
        var selectedNode = menuTreeObj.getNodeByParam("id",currentNodeId);
        var refreshNode = menuTreeObj.getNodeByParam("id",selectedNode.pId);
        refreshChildNodes(refreshNode);
    }
}

function deleteMenuItemed(data) {   
    if (data.isSuccessed){
        var currentNode = menuTreeObj.getNodeByParam("id", currentNodeId);
        var refreshNode = menuTreeObj.getNodeByParam("id", currentNode.pId);
        refreshChildNodes(refreshNode);
    }        
    afterMenuTreeOption(data);
}

function beforeRename(treeId, treeNode, newName, isCancel) {
    if (isCancel) {
        return true;
    }
        
    if (newName != null) {
        oldName = treeNode.name;
        newName = $.trim(newName);
        if (newName.length > 0 && newName.length <= 8) {
            var isSuccessed = false;
            $.ajax({
                type: "post",
                async: false,
                url: "/Admin/Menu/RenameMenuItem",
                data: { "id": treeNode.id, "header": newName },
                success: function (msg) {
                    alert(msg.message);
                    isSuccessed = msg.isSuccessed;
                    if (!isSuccessed)
                        menuTreeObj.cancelEditName(oldName);
                },
            });
            return isSuccessed;
        }
        else {
            alert("菜单名不能为空白字符且不能大于8个字符！");            
        }  
    }
    return false;
}

function onRename(event, treeId, treeNode, isCancel) {
    if (!isCancel) {
        refreshMenu();
    }
}

function beforeDrop(treeId, treeNodes, targetNode, moveType) {
    //拖拽到根节点
    moveType = "inner";
    var id = treeNodes[0].id, origParentId = treeNodes[0].pId, parentId = null;
    if (targetNode != null)
        parentId = targetNode.id;

    var isSuccessed = true;
    $.ajax({
        type: "get",
        async: false,
        url: "/Admin/Menu/MigrateMenuItem",
        data: { "id": id, "parentId": parentId },
        success: function (msg) {
            if (isJsonFormat(msg) && !msg.isSuccessed) {
                alert(msg.message);
                isSuccessed = false;
            }
            else {
                $("#modal").html(msg);
                $("#cancel").click(function () {
                    var sourceParentNode = menuTreeObj.getNodeByParam("id", origParentId);
                    refreshChildNodes(sourceParentNode);                 
                    refreshChildNodes(targetNode);
                })
            }
        }
    });
    return isSuccessed;
}

function migrateMenuItemed(data) {
    afterMenuTreeOption(data);
    if (!data.isSuccessed) {
        menuTreeObj.refresh();
    }
}


function refreshMenu(keywords) {
    $.ajax({
        type: "get",
        url: "/Admin/Menu/RefreshMenu",
        data: { "keywords": keywords },
        success: function (msg) {
            $("#sidebardiv").html(msg);
        }
    });
}

