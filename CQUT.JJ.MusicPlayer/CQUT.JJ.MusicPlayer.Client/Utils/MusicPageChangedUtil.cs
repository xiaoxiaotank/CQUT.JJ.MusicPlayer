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

        public static readonly string PageSourceUrl = "pack://application:,,,/CQUT.JJ.MusicPlayer.Client;component/Pages/";

        public static event EventHandler<PageChangedEventArgs> PageChangedEvent;

        public static void Invoke(string pageName)
        {
            if (pageName.Contains(_pageExtension))
            {
                var pageUri = new Uri(PageSourceUrl + pageName, UriKind.Absolute);
                var e = new PageChangedEventArgs(pageUri);
                PageChangedEvent(null, e);
            }
        }
    }

    public class PageChangedEventArgs : EventArgs
    {
        public Uri PageSource { get; set; }

        public PageChangedEventArgs(Uri pageSource) => PageSource = pageSource;
    }
}
