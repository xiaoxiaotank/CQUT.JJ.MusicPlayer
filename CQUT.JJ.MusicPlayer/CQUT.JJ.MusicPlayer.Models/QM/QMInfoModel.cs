using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class QMInfoModel : BaseQMusicInfoModel
    {
        public string Id { get; set; }      

        public QMAlbumInfoModel AlbumInfo { get; set; }

        public string SourcePath { get; set; }
    }
}
