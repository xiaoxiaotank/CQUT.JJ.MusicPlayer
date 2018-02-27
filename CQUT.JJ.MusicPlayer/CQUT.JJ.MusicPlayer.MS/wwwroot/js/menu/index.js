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
    callback: {
        onClick: menuItemOnClick,   //定义节点单击事件回调函数
        onRightClick: OnRightClick, //定义节点右键单击事件回调函数
        //beforeRename: beforeRename, //定义节点重新编辑成功前回调函数，一般用于节点编辑时判断输入的节点名称是否合法
        //onDblClick: onDblClick,  //定义节点双击事件回调函数
        //onCheck: onCheck    //定义节点复选框选中或取消选中事件的回调函数
    },
    async: {
        enable: true,      //设置启用异步加载
        type: "get",      //异步加载类型:post和get
        contentType: "application/json", //定义ajax提交参数的参数类型，一般为json格式
        url: "/Menu/GetMenu",    //定义数据请求路径
        autoParam: ["id=id", "name=name", "pId=pId"] //定义提交时参数的名称，=号前面标识节点属性，后面标识提交时json数据中参数的名称
    }
};

$(function () {
    $.fn.zTree.init($("#menuTree"), setting);
})


function menuItemOnClick(event, treeId, treeNode) {
    alert(treeNode.id + " ," + treeNode.name + "," + treeNode.pId);
};

// 在ztree上的右击事件  
function OnRightClick(event, treeId, treeNode) {
    if (!treeNode && event.target.tagName.toLowerCase() != "button" && $(event.target).parents("a").length == 0) {
        showRMenu("root", event.clientX, event.clientY);
    } else if (treeNode && !treeNode.noR) {
        showRMenu("node", event.clientX, event.clientY);
    }
}
//显示右键菜单  
function showRMenu(type, x, y) {
    $("#rMenu ul").show();
    rMenu.css({ "top": y + "px", "left": x + "px", "visibility": "visible" }); //设置右键菜单的位置、可见  
    $("body").bind("mousedown", onBodyMouseDown);
}
//隐藏右键菜单  
function hideRMenu() {
    if (rMenu) rMenu.css({ "visibility": "hidden" }); //设置右键菜单不可见  
    $("body").unbind("mousedown", onBodyMouseDown);
}
//鼠标按下事件  
function onBodyMouseDown(event) {
    if (!(event.target.id == "rMenu" || $(event.target).parents("#rMenu").length > 0)) {
        rMenu.css({ "visibility": "hidden" });
    }
}  
