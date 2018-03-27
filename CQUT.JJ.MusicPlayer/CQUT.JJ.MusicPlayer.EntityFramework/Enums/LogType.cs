using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Enums
{
    public enum LogType
    {
        [Description("信息")]
        Info,
        [Description("成功")]
        Succeess,
        [Description("警告")]
        Warning,
        [Description("失败")]
        Fail,
    }
}
