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
        public static EventHandler<MusicPlaySwitchedEventArgs> MusicPlaySwitchedEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="musicPlayMode"></param>
        /// <param name="isDown">true 向下 false 向上</param>
        public static void Invoke(MusicPlayMode musicPlayMode, bool isDown)
        {
            var e = new MusicPlaySwitchedEventArgs(musicPlayMode, isDown);
            MusicPlaySwitchedEvent?.Invoke(null, e);
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
