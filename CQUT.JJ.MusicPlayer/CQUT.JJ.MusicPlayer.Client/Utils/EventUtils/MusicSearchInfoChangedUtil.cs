using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class MusicSearchInfoChangedUtil
    {
        public static event EventHandler<MusicSearchChangedArgs> QMSearchChangedEvent;

        public static event EventHandler<MusicSearchChangedArgs> JMSearchChangedEvent;

        public static void InvokeFromQM(MusicInfoOfPageModel musicInfoOfPageModels,bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchChangedArgs(musicInfoOfPageModels, isSuccessed, errorInfo);
            QMSearchChangedEvent(null, e);
        }

        public static void InvokeFromJM(MusicInfoOfPageModel musicInfoOfPageModels, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchChangedArgs(musicInfoOfPageModels,isSuccessed, errorInfo);
            JMSearchChangedEvent(null, e);
        }
    }

    public class MusicSearchChangedArgs : EventArgs
    {
        public MusicInfoOfPageModel MusicInfoOfPageModels { get; set; }

        public bool IsSuccessed { get; set; }

        public string ErrorInfo { get; set; }
        public MusicSearchChangedArgs(MusicInfoOfPageModel musicInfoOfPageModels, bool isSuccessed = true, string erroInfo = null)
        {
            MusicInfoOfPageModels = musicInfoOfPageModels;
            IsSuccessed = isSuccessed;
            ErrorInfo = erroInfo;
        }
    }
}
