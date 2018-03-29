using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
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
        public static event EventHandler<BaseMusicSearchInfoRequestArgs> QMRequestEvent;

        public static event EventHandler<MusicSearchInfoRequestArgs> JMRequestEvent;

        public static event EventHandler<MusicSearchInfoChangedArgs> QMSearchChangedEvent;

        public static event EventHandler<MusicSearchInfoChangedArgs> JMSearchChangedEvent;

        public static void InvokeFromQMRequest(int targetPageNumber = 1) 
            => QMRequestEvent?.Invoke(null, new BaseMusicSearchInfoRequestArgs(targetPageNumber));

        public static void InvokeFromJMRequest(MusicSearchType type,int targetPageNumber = 1, int size = 20)
            => JMRequestEvent?.Invoke(null, new MusicSearchInfoRequestArgs(type,targetPageNumber,size));

        public static void InvokeFromQMSearchChanged(MusicsOfPageModel musicInfoOfPageModels, int targetPageNumber, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchInfoChangedArgs(musicInfoOfPageModels,targetPageNumber, isSuccessed, errorInfo);
            QMSearchChangedEvent?.Invoke(null, e);
        }

        public static void InvokeFromJMSearchChanged(MusicsOfPageModel musicInfoOfPageModels,int targetPageNumber, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchInfoChangedArgs(musicInfoOfPageModels,targetPageNumber,isSuccessed, errorInfo);
            JMSearchChangedEvent?.Invoke(null, e);
        }
    }

    public class MusicSearchInfoChangedArgs : EventArgs
    {
        public MusicsOfPageModel MusicInfoOfPageModels { get; set; }

        public int TargetPageNumber { get; set; }

        public bool IsSuccessed { get; set; }

        public string ErrorInfo { get; set; }
        public MusicSearchInfoChangedArgs(MusicsOfPageModel musicInfoOfPageModels,int targetPageNumber, bool isSuccessed = true, string erroInfo = null)
        {
            MusicInfoOfPageModels = musicInfoOfPageModels;
            TargetPageNumber = targetPageNumber;
            IsSuccessed = isSuccessed;
            ErrorInfo = erroInfo;
        }
    }

    public class BaseMusicSearchInfoRequestArgs : EventArgs
    {
        public int TargetPageNumber { get; set; }

        public BaseMusicSearchInfoRequestArgs(int targetPageNumber)
        {
            TargetPageNumber = targetPageNumber;
        }
    }

    public class MusicSearchInfoRequestArgs : BaseMusicSearchInfoRequestArgs
    {
        public MusicSearchType Type { get; set; }

        public int Size { get; set; }

        public MusicSearchInfoRequestArgs(MusicSearchType type,int targetPageNumber,int size = 20) : base(targetPageNumber)
        {
            Type = type;
            Size = size;
        }
    }
}
