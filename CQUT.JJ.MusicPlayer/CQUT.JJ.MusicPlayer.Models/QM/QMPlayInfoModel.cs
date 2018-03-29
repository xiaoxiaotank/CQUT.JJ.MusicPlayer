using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class QMPlayInfoModel : BaseQMusicPlayInfoModel
    {
        public string Id { get; set; }

        public Uri Uri { get; set; }  
      
    }
}
