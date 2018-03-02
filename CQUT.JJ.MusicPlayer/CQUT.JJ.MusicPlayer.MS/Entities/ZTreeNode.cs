using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Entities
{
    public class ZTreeNode
    {
        /// <summary>
        /// 节点的标识属性，对应的是启用简单数据格式时idKey对应的属性名，并不一定是id,如果setting中定义的idKey:"zId",那么此处就是zId
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 节点parentId属性，命名规则同id
        /// </summary>
        public int pId { get; set; }

        /// <summary>
        /// 结点显示的文本
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 节点是否勾选，ztree配置启用复选框时有效
        /// </summary>
        public bool? @checked { get; set; }

        /// <summary>
        /// 节点是否展开
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// 节点的图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 节点展开式的图标
        /// </summary>
        public string iconOpen { get; set; }

        /// <summary>
        /// 节点折叠时的图标
        /// </summary>
        public string iconClose { get; set; }

        /// <summary>
        /// 得到该节点所有孩子节点，直接下级，若要得到所有下属层级节点，需要自己写递归得到
        /// </summary>
        public List<ZTreeNode> children { get; set; }
    }
}
