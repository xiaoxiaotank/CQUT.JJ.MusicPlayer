using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class ContextMenuStartPlayMusicUtil
    {
        public static event EventHandler<EventArgs> ContextMenuStartPlayMusicEvent;

        public static void Invoke()
        {
            ContextMenuStartPlayMusicEvent?.Invoke(null, new EventArgs());
        }
    }
}
