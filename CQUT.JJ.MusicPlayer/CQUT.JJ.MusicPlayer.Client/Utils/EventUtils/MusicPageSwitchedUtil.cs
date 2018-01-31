using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class MusicPageSwitchedUtil
    {
        public static EventHandler<EventArgs> MusicPagePreviousSwitchedEvent;

        public static EventHandler<EventArgs> MusicPageNextSwitchedEvent;

        public static EventHandler<MusicPageSwitchedEventArgs> MusicPageEnablePreviousSwitchedEvent;

        public static EventHandler<MusicPageSwitchedEventArgs> MusicPageEnableNextSwitchedEvent;

        public static void InvokeOfPrevious()
        {
            MusicPagePreviousSwitchedEvent.Invoke(null, new EventArgs());
        }

        public static void InvokeOfNext()
        {
            MusicPageNextSwitchedEvent.Invoke(null, new EventArgs());
        }

        public static void InvokeOfCanPrevious(bool canSwitch)
        {
            MusicPageEnablePreviousSwitchedEvent(null, new MusicPageSwitchedEventArgs(canSwitch));
        }

        public static void InvokeOfCanNext(bool canSwitch)
        {
            MusicPageEnableNextSwitchedEvent(null, new MusicPageSwitchedEventArgs(canSwitch));
        }
    }

    public class MusicPageSwitchedEventArgs : EventArgs
    {     
        public bool CanSwitch { get; set; }

        public MusicPageSwitchedEventArgs(bool canSwitch)
        {
            CanSwitch = canSwitch;
        }
    }
}
