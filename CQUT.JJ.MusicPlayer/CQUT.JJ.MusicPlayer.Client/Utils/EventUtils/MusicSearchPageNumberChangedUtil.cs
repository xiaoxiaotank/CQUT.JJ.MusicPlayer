using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class MusicSearchPageNumberChangedUtil
    {
        public static event EventHandler<MusicSearchPageNumberChangedArgs> QMSearchPageNumberChangedEvent;

        public static event EventHandler<MusicSearchPageNumberChangedArgs> JMSearchPageNumberChangedEvent;

        public static void InvokeFromQM(int pageNumber)
        {
            var e = new MusicSearchPageNumberChangedArgs(pageNumber);
            QMSearchPageNumberChangedEvent(null, e);
        }

        public static void InvokeFromJM(int pageNumber)
        {
            var e = new MusicSearchPageNumberChangedArgs(pageNumber);
            JMSearchPageNumberChangedEvent(null, e);
        }
    }

    public class MusicSearchPageNumberChangedArgs : EventArgs
    {
        public int PageNumber { get; set; }
        public MusicSearchPageNumberChangedArgs(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }
}
