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
        public static event EventHandler<MusicSearchInfoRequestArgs> QMRequestEvent;

        public static event EventHandler<MusicSearchInfoRequestArgs> JMRequestEvent;

        public static event EventHandler<MusicSearchInfoChangedArgs> QMSearchChangedEvent;

        public static event EventHandler<MusicSearchInfoChangedArgs> JMSearchChangedEvent;

        public static void InvokeFromQMRequest(int targetPageNumber) => QMRequestEvent?.Invoke(null, new MusicSearchInfoRequestArgs(targetPageNumber));

        public static void InvokeFromJMRequest(int targetPageNumber) => JMRequestEvent?.Invoke(null, new MusicSearchInfoRequestArgs(targetPageNumber));

        public static void InvokeFromQMSearchChanged(MusicInfoOfPageModel musicInfoOfPageModels, int targetPageNumber, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchInfoChangedArgs(musicInfoOfPageModels,targetPageNumber, isSuccessed, errorInfo);
            QMSearchChangedEvent?.Invoke(null, e);
        }

        public static void InvokeFromJMSearchChanged(MusicInfoOfPageModel musicInfoOfPageModels,int targetPageNumber, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchInfoChangedArgs(musicInfoOfPageModels,targetPageNumber,isSuccessed, errorInfo);
            JMSearchChangedEvent?.Invoke(null, e);
        }
    }

    public class MusicSearchInfoChangedArgs : EventArgs
    {
        public MusicInfoOfPageModel MusicInfoOfPageModels { get; set; }

        public int TargetPageNumber { get; set; }

        public bool IsSuccessed { get; set; }

        public string ErrorInfo { get; set; }
        public MusicSearchInfoChangedArgs(MusicInfoOfPageModel musicInfoOfPageModels,int targetPageNumber, bool isSuccessed = true, string erroInfo = null)
        {
            MusicInfoOfPageModels = musicInfoOfPageModels;
            TargetPageNumber = targetPageNumber;
            IsSuccessed = isSuccessed;
            ErrorInfo = erroInfo;
        }
    }

    public class MusicSearchInfoRequestArgs : EventArgs
    {
        public int TargetPageNumber { get; set; }

        public MusicSearchInfoRequestArgs(int targetPageNumber)
        {
            TargetPageNumber = targetPageNumber;
        }
    }
}
