using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models.DataContracts.Search
{
    [Serializable]
    [DataContract]
    public class MusicSearchPageResult : PageResult
    {
        [DataMember]
        public new IEnumerable<MusicInfo> Results { get; set; }
    }
}
