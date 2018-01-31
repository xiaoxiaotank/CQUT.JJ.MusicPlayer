using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class MusicPageRefreshUtil
    {
        public static event EventHandler<EventArgs> MusicPageRefreshEvent;

        public static void Invoke()
        {
            MusicPageRefreshEvent(null, new EventArgs());
        }
    }
}
