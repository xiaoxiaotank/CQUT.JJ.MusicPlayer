$(function () {
    $.ajax({
        type: "Get",
        url: "/Menu/GetMenu",
        //async: false,
        success: function (data) {
            $.fn.zTree.init($("#menuTree"), setting, data);
        }
    });
})

var setting = {
    check: {
        enable: true,
        chkStyle: "checkbox",
        chkboxType: { "Y": "ps", "N": "ps" },
        isSimpleData: true,              //数据是否采用简单 Array 格式，默认false
        treeNodeKey: "id",               //在isSimpleData格式下，当前节点id属性
        treeNodeParentKey: "pId",        //在isSimpleData格式下，当前节点的父节点id属性
        showLine: true,                  //是否显示节点间的连线
        checkable: true
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: zTreeOnClick
    }
};
//popZtree(setting);
function zTreeOnClick(event, treeId, treeNode) {
    alert(treeNode.id + " ," + treeNode.name);
};