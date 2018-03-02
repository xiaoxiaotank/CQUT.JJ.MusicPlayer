using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Entities
{
    public class JsonResultEntity
    {
        /// <summary>
        /// 发送的消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 标识是否成功
        /// 默认值为true
        /// </summary>
        public bool isSuccessed { get; set; } = true;

    }
}
