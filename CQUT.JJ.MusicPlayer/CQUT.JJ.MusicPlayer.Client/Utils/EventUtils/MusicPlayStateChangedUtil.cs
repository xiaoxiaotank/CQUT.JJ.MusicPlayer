using CQUT.JJ.MusicPlayer.Models;
using CQUT.JJ.MusicPlayer.Models.JM.Common;
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

        public static event EventHandler<MusicPlayStateChangedArgs> JMusicPlayStateChangedEvent;

        public static void InvokeFromQM(QMPlayInfoModel musicInfo, bool isToPlay,bool isNeedRefresh = true)
        {
            var e = new QMusicPlayStateChangedArgs(musicInfo, isToPlay,isNeedRefresh);
            QMusicPlayStateChangedEvent(null, e);
        }

        public static void InvokeFromJM(MusicModel musicInfo,bool isToPlay,bool isNeedRefresh = true)
        {
            var e = new MusicPlayStateChangedArgs(musicInfo, isToPlay,isNeedRefresh);
            JMusicPlayStateChangedEvent(null, e);
        }
    }

    public class BaseMusicPlayStateChangedArgs : EventArgs
    {
        public bool IsToPlay { get; set; }

        public bool IsNeedRefresh { get; set; }
        public BaseMusicPlayStateChangedArgs(bool isToPlay, bool isNeedRefresh = true)
        {
            IsToPlay = isToPlay;
            IsNeedRefresh = isNeedRefresh;
        }
    }

    public class QMusicPlayStateChangedArgs : BaseMusicPlayStateChangedArgs
    {
        public QMPlayInfoModel MusicInfo { get; set; }
 
        public QMusicPlayStateChangedArgs(QMPlayInfoModel musicInfo, bool isToPlay, bool isNeedRefresh = true) : base(isToPlay,isNeedRefresh)
        {
            MusicInfo = musicInfo;
        }
    }

    public class MusicPlayStateChangedArgs : BaseMusicPlayStateChangedArgs
    {
        public MusicModel MusicInfo { get; set; }

        public MusicPlayStateChangedArgs(MusicModel musicInfo, bool isToPlay, bool isNeedRefresh = true) : base(isToPlay, isNeedRefresh)
        {
            MusicInfo = musicInfo;
        }
    }
}
