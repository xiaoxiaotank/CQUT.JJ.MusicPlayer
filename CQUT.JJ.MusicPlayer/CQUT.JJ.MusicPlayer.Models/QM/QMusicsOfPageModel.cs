using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class QMusicsOfPageModel
    {
        public List<BaseQMusicInfoModel> MusicInfoList { get; set; } = new List<BaseQMusicInfoModel>();

        public int TotalPageNumber { get; set; }

        public int CurrentPageNumber { get; set; }
    }
}
