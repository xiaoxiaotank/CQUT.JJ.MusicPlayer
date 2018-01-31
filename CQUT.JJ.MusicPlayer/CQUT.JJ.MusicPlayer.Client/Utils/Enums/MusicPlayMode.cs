using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.Enums
{
    public enum MusicPlayMode
    {
        [Description("随机播放")]
        Random,
        [Description("顺序播放")]
        Order,
        [Description("单曲循环")]
        Single,
        [Description("列表循环")]
        List
    }
}
