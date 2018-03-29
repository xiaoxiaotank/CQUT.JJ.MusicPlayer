using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class BaseQMusicPlayInfoModel
    {
        public string Name { get; set; }

        public string SingerName { get; set; }

        public string TimeDuration { get; set; }

        public Uri PhotoUri { get; set; }
    }
}
