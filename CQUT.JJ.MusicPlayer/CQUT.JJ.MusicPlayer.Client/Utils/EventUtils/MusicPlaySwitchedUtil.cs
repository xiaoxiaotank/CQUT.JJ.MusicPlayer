using CQUT.JJ.MusicPlayer.Client.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public class MusicPlaySwitchedUtil
    {
        public static EventHandler<MusicPlaySwitchedEventArgs> QMusicPlaySwitchedEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="musicPlayMode"></param>
        /// <param name="isDescending">true 降序 false 升序</param>
        public static void InvokeFromQM(MusicPlayMode musicPlayMode, bool isDescending)
        {
            var e = new MusicPlaySwitchedEventArgs(musicPlayMode, isDescending);
            QMusicPlaySwitchedEvent?.Invoke(null, e);
        }
    }

    public class MusicPlaySwitchedEventArgs : EventArgs
    {
        public MusicPlayMode MusicPlayMode { get; set; }

        public bool IsDescending { get; set; }

        public MusicPlaySwitchedEventArgs(MusicPlayMode musicPlayMode,bool isDescending)
        {
            MusicPlayMode = musicPlayMode;
            IsDescending = isDescending;
        }
    }
}
