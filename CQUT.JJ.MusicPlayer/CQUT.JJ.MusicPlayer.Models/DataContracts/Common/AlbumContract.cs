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
    public class AlbumContract
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int SingerId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SingerName { get; set; }
        [DataMember]
        public DateTime PublishedTime { get; set; }

        [DataMember]
        public int MusicCount { get; set; }

    }
}
