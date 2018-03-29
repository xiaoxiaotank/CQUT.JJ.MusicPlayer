using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models.DataContracts
{
    [Serializable]
    [DataContract]
    public class MusicInfo
    {
        /// <summary>
        /// 音乐id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 歌唱家id
        /// </summary>
        [DataMember]
        public int SingerId { get; set; }

        /// <summary>
        /// 专辑id
        /// </summary>
        [DataMember]
        public int AlbumId { get; set; }

        /// <summary>
        /// 音乐名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 歌唱家名字
        /// </summary>
        [DataMember]
        public string SingerName { get; set; }

        /// <summary>
        /// 专辑名
        /// </summary>
        [DataMember]
        public string AlbumName { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [DataMember]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string FileUrl { get; set; }
    }
}
