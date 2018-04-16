using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
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
    public class SingerSearchPageResult : PageResult
    {
        [DataMember]
        public new IEnumerable<SingerContract> Results { get; set; }
    }
}
