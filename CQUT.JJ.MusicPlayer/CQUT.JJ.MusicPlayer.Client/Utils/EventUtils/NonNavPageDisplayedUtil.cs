using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    /// <summary>
    /// 例如搜索等页面不需要用户导航栏中的TabItem,引发该事件
    /// </summary>
    public class NonNavPageDisplayedUtil
    {
        public static EventHandler<EventArgs> NonNavPageDisplayedEvent;

        public static void Invoke() => NonNavPageDisplayedEvent?.Invoke(null, new EventArgs());
    }
}
