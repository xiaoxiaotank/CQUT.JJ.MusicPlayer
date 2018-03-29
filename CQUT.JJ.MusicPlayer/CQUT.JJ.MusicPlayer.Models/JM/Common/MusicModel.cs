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

        public AlbumModel Album { get; set; }

        public string FileUrl { get; set; }
    }
}
