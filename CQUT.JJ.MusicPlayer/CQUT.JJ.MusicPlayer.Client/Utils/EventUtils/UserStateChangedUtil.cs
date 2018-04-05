using CQUT.JJ.MusicPlayer.Models.JM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class UserStateChangedUtil
    {        
        /// <summary>
        /// 用户状态改变事件（登录、注销）
        /// </summary>
        public static EventHandler<EventArgs> UserStateChangedEvent;     

        public static void Invoke()
        {
            UserStateChangedEvent?.Invoke(null,new EventArgs());
        }
    }
}
