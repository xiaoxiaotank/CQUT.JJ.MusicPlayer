using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class MusicInfoOfPageModel
    {
        public List<MusicInfoModel> MusicInfoList { get; set; } = new List<MusicInfoModel>();

        public int TotalPageNumber { get; set; }

        public int CurrentPageNumber { get; set; }
    }
}
