using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
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
    public class PageResult
    {
        [DataMember]
        public IEnumerable<object> Results { get; set; }

        public MusicRequestType ResultType { get; set; }

        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public int PageNumber { get; set; }
    }
}
