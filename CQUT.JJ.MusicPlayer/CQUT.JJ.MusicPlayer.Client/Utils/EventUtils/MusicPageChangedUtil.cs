using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class MusicPageChangedUtil
    {
        private static readonly string _pageExtension = ".xaml";

        public static readonly string PageBaseUrl = "pack://application:,,,/CQUT.JJ.MusicPlayer.Client;component/Pages";

        public static event EventHandler<PageChangedEventArgs> PageChangedEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageName">Pages下页面路径，含扩展名</param>
        /// <param name="isRefresh"></param>
        public static void Invoke(string pageName,bool isRefresh = false)
        {
            if (pageName.Contains(_pageExtension))
            {
                var pageUri = new Uri($"{PageBaseUrl}/{pageName}", UriKind.Absolute);
                var e = new PageChangedEventArgs(pageUri,isRefresh);
                PageChangedEvent(null, e);
            }
        }
    }

    public class PageChangedEventArgs : EventArgs
    {
        public Uri PageSource { get; set; }

        public bool IsRefresh { get; set; }

        public PageChangedEventArgs(Uri pageSource,bool isRefresh)
        {
            PageSource = pageSource;
            IsRefresh = isRefresh;
        }
    }
}
