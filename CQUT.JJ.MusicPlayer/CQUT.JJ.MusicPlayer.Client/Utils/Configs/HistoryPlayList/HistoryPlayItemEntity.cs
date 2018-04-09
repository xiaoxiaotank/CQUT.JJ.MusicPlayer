using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.Configs.HistoryPlayList
{
    public class HistoryPlayItemEntity
    {
        public int MusicId { get; set; }

        public int SingerId { get; set; }

        public int AlbumId { get; set; }

        public string MusicName { get; set; }

        public string SingerName { get; set; }

        public string AlbumName { get; set; }

        public Uri MusicFileUri { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
