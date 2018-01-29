using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class QMSongInfoModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Singer { get; set; }

        public QMAlbumInfoModel AlbumInfo { get; set; }
    }
}
