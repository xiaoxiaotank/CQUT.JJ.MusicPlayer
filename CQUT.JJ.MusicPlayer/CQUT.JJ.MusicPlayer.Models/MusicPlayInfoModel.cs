using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class MusicPlayInfoModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SingerName { get; set; }

        public string TimeDuration { get; set; }

        public Uri Uri { get; set; }
    }
}
