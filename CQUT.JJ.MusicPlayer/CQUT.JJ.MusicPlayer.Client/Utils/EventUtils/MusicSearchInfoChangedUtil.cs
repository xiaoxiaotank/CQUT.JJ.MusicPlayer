using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
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

        public static event EventHandler<QMusicSearchInfoChangedArgs> QMSearchChangedEvent;

        public static event EventHandler<MusicSearchInfoChangedArgs> JMSearchChangedEvent;

        public static void InvokeFromQMRequest(int targetPageNumber = 1) 
            => QMRequestEvent?.Invoke(null, new BaseMusicSearchInfoRequestArgs(targetPageNumber));

        public static void InvokeFromJMRequest(MusicRequestType type,int targetPageNumber = 1, int size = 20)
            => JMRequestEvent?.Invoke(null, new MusicSearchInfoRequestArgs(type,targetPageNumber,size));

        public static void InvokeFromQMSearchChanged(QMusicsOfPageModel musicInfoOfPageModels, int targetPageNumber, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new QMusicSearchInfoChangedArgs(musicInfoOfPageModels,targetPageNumber, isSuccessed, errorInfo);
            QMSearchChangedEvent?.Invoke(null, e);
        }

        public static void InvokeFromJMSearchChanged(PageResult pageResult,int targetPageNumber, bool isSuccessed = true,string errorInfo = null)
        {
            var e = new MusicSearchInfoChangedArgs(pageResult,targetPageNumber,isSuccessed, errorInfo);
            JMSearchChangedEvent?.Invoke(null, e);
        }
    }

    public class QMusicSearchInfoChangedArgs : EventArgs
    {
        public QMusicsOfPageModel MusicInfoOfPageModels { get; set; }

        public int TargetPageNumber { get; set; }

        public bool IsSuccessed { get; set; }

        public string ErrorInfo { get; set; }
        public QMusicSearchInfoChangedArgs(QMusicsOfPageModel musicInfoOfPageModels,int targetPageNumber, bool isSuccessed = true, string erroInfo = null)
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
        public MusicRequestType Type { get; set; }

        public int Size { get; set; }

        public MusicSearchInfoRequestArgs(MusicRequestType type,int targetPageNumber,int size = 20) : base(targetPageNumber)
        {
            Type = type;
            Size = size;
        }
    }

    public class MusicSearchInfoChangedArgs : EventArgs
    {
        public PageResult PageResult { get; set; }

        public int TargetPageNumber { get; set; }

        public bool IsSuccessed { get; set; }

        public string ErrorInfo { get; set; }
        public MusicSearchInfoChangedArgs(PageResult pageResult, int targetPageNumber, bool isSuccessed = true, string errorInfo = null)
        {
            PageResult = pageResult;
            TargetPageNumber = targetPageNumber;
            IsSuccessed = isSuccessed;
            ErrorInfo = errorInfo;
        }
    }
}
