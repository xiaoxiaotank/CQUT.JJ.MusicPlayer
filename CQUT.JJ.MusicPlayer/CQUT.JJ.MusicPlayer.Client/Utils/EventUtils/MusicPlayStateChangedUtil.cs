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
        public static event EventHandler<MusicPlayStateChangedArgs> MusicPlayStateChangedEvent;

        public static void Invoke(MusicPlayInfoModel musicInfo, bool isToPlay)
        {
            var e = new MusicPlayStateChangedArgs(musicInfo, isToPlay);
            MusicPlayStateChangedEvent(null, e);
        }
    }

    public class MusicPlayStateChangedArgs : EventArgs
    {
        public MusicPlayInfoModel MusicInfo { get; set; }
        public bool IsToPlay { get; set; }
        public MusicPlayStateChangedArgs(MusicPlayInfoModel musicInfo, bool isToPlay)
        {
            MusicInfo = musicInfo;
            IsToPlay = isToPlay;
        }
    }
}
