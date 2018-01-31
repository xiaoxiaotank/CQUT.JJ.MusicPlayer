using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{

    public static class MusicPlayStateChangedUtil
    {
        public static event EventHandler<QMusicPlayStateChangedArgs> QMusicPlayStateChangedEvent;

        public static void InvokeFromQM(QMPlayInfoModel musicInfo, bool isToPlay)
        {
            var e = new QMusicPlayStateChangedArgs(musicInfo, isToPlay);
            QMusicPlayStateChangedEvent(null, e);
        }
    }

    public class QMusicPlayStateChangedArgs : EventArgs
    {
        public QMPlayInfoModel MusicInfo { get; set; }
        public bool IsToPlay { get; set; }
        public QMusicPlayStateChangedArgs(QMPlayInfoModel musicInfo, bool isToPlay)
        {
            MusicInfo = musicInfo;
            IsToPlay = isToPlay;
        }
    }
}
