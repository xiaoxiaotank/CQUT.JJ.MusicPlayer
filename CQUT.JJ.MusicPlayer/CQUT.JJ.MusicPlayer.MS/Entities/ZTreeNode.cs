using Newtonsoft.Json;
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
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// 节点parentId属性，命名规则同id
        /// </summary>
        [JsonProperty(PropertyName = "pId")]
        public string ParentId { get; set; }

        /// <summary>
        /// 结点显示的文本
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 节点是否勾选，ztree配置启用复选框时有效
        /// </summary>
        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }

        /// <summary>
        /// 设置checked不可用
        /// </summary>
        [JsonProperty("chkDisabled")]
        public bool CheckDisabled { get; set; }

        /// <summary>
        /// 节点是否展开
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        public bool Open { get; set; }

        /// <summary>
        /// 节点的图标
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 节点展开式的图标
        /// </summary>
        [JsonProperty(PropertyName = "iconOpen")]
        public string IconOpen { get; set; }

        /// <summary>
        /// 节点折叠时的图标
        /// </summary>
        [JsonProperty(PropertyName = "iconClose")]
        public string IconClose { get; set; }

        /// <summary>
        /// 得到该节点所有孩子节点，直接下级，若要得到所有下属层级节点，需要自己写递归得到
        /// </summary>
        [JsonProperty(PropertyName = "children")]
        public List<ZTreeNode> Children { get; set; }
    }
}
