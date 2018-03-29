using CQUT.JJ.MusicPlayer.Models;
using CQUT.JJ.MusicPlayer.Models.JM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CQUT.JJ.MusicPlayer.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
       
    }

    public static class JMApp
    {
        public static CurrentPlayingMusicsInfo CurrentPlayingMusicsInfo = null;
    }

    public class CurrentPlayingMusicsInfo
    {
        /// <summary>
        /// 当前播放歌曲所存在的列表
        /// </summary>
        public IEnumerable<QMInfoModel> CurrentQMPlayingMusics { get; set; }

        public IEnumerable<MusicModel> CurrentPlayingMusics { get; set; }

        public string CurrentQMPlayingMusicId { get; set; }

        public int CurrentPlayingMusicId { get; set; }

        /// <summary>
        /// 正在播放的歌曲是否在当前页面
        /// </summary>
        public bool IsCurrentPlayingPage { get; set; } = false;
    }
}
