using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class PageLoadedUtil
    {
        /// <summary>
        /// 音乐播放列表页面加载完毕事件
        /// </summary>
        public static event EventHandler<EventArgs> MusicListPageLoadedEvent;

        public static void InvokeFromMusicListPageLoaded()
        {
            MusicListPageLoadedEvent?.Invoke(null, new EventArgs());
        }
    }
}
