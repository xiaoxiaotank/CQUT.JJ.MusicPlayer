using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Data.Entities
{
    public class ZTreeNode
    {
        public int id { get; set; }

        public int pId { get; set; }

        public string name { get; set; }

    //    name,  //节点显示的文本
    //checked, //节点是否勾选，ztree配置启用复选框时有效
    //open,  //节点是否展开
    //icon,  //节点的图标
    //iconOpen, //节点展开式的图标
    //iconClose, //节点折叠时的图标
    //id,   //节点的标识属性，对应的是启用简单数据格式时idKey对应的属性名，并不一定是id,如果setting中定义的idKey:"zId",那么此处就是zId
    //pId,  //节点parentId属性，命名规则同id
    //children, //得到该节点所有孩子节点，直接下级，若要得到所有下属层级节点，需要自己写递归得到
    //isParent, //判断该节点是否是父节点，一般应用中通常需要判断只有叶子节点才能进行相关操作，或者删除时判断下面是有子节点时经常用到。
    //getPath() //得到该节点的路径，即所有父节点，包括自己，此方法返回的是一个数组，通常用于创建类似面包屑导航的东西A-->B-->C 

    }
}
