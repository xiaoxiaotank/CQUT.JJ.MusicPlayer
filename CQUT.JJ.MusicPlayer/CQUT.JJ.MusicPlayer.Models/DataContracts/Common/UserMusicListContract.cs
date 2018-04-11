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
    public class UserMusicListContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreationTime { get; set; }
    }
}
