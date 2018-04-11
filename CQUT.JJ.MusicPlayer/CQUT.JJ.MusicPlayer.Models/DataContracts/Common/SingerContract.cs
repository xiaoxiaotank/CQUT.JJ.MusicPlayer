using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models.DataContracts.Common
{
    [DataContract]
    [Serializable]
    public class SingerContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ForeignName { get; set; }

        [DataMember]
        public string Nationality { get; set; }
    }
}
