using CQUT.JJ.MusicPlayer.Client.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public class MusicPlayEndedUtil
    {
        public static EventHandler<MusicPlayEndedEventArgs> QMusicPlayEndedEvent;

        public static void InvokeFromQM(MusicPlayMode musicPlayMode)
        {
            var e = new MusicPlayEndedEventArgs(musicPlayMode);
            QMusicPlayEndedEvent?.Invoke(null, e);
        }
    }

    public class MusicPlayEndedEventArgs : EventArgs
    {
        public MusicPlayMode MusicPlayMode { get; set; }

        public MusicPlayEndedEventArgs(MusicPlayMode musicPlayMode)
        {
            MusicPlayMode = musicPlayMode;
        }
    }
}
