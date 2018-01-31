using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public class NonNavPageDisplayedUtil
    {
        public static EventHandler<EventArgs> NonNavPageDisplayedEvent;

        public static void Invoke() => NonNavPageDisplayedEvent?.Invoke(null, new EventArgs());
    }
}
