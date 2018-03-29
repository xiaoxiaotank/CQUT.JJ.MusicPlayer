using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models.JM.Common
{
    public class MusicModel
    {
        public int Id { get; set; }

        public int SingerId { get; set; }

        public int AlbumId { get; set; }

        public string Name { get; set; }

        public string SingerName { get; set; }

        public string AlbumName { get; set; }

        public TimeSpan Duration { get; set; }

        public string FileUrl { get; set; }

        public Uri FileUri { get; set; }

        public AlbumModel Album { get; set; }
    }
}
