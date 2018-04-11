using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models.DataContracts.Common
{
    [Serializable]
    [DataContract]
    public class UserContract
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        [DataMember]
        public string ProfilePhotoPath { get; set; }
    }
}
