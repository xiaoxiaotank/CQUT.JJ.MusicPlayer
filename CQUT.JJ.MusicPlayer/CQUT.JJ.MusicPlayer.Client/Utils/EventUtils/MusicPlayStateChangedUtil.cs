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

        public static void InvokeFromQM(QMPlayInfoModel musicInfo, bool isToPlay,bool isNeedRefresh = true)
        {
            var e = new QMusicPlayStateChangedArgs(musicInfo, isToPlay,isNeedRefresh);
            QMusicPlayStateChangedEvent(null, e);
        }
    }

    public class QMusicPlayStateChangedArgs : EventArgs
    {
        public QMPlayInfoModel MusicInfo { get; set; }
        public bool IsToPlay { get; set; }

        public bool IsNeedRefresh { get; set; }
        public QMusicPlayStateChangedArgs(QMPlayInfoModel musicInfo, bool isToPlay, bool isNeedRefresh = true)
        {
            MusicInfo = musicInfo;
            IsToPlay = isToPlay;
            IsNeedRefresh = isNeedRefresh;
        }
    }
}
