using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.Configs.HistoryPlayList;
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
        public static UserModel User = null;
    }

    public static class JMApp
    {
        public static CurrentPlayingMusicsInfo CurrentPlayingMusicsInfo = null;

        public static List<MusicModel> ListeningTestList = new List<MusicModel>();
    }

    public class CurrentPlayingMusicsInfo
    {
        private MusicModel _currentPlayingMusic;

        /// <summary>
        /// 当前播放歌曲所存在的列表
        /// </summary>
        public IEnumerable<QMInfoModel> CurrentQMPlayingMusics { get; set; }

        public IEnumerable<MusicModel> PlayingListMusics { get; set; }

        public string CurrentQMPlayingMusicId { get; set; }

        /// <summary>
        /// 正在播放的歌曲信息
        /// </summary>
        public MusicModel CurrentPlayingMusic
        {
            get => _currentPlayingMusic;
            set
            {
                _currentPlayingMusic = value;
                var historyItemEntity = new HistoryPlayItemEntity()
                {
                    MusicId = _currentPlayingMusic.Id,
                    SingerId = _currentPlayingMusic.SingerId,
                    AlbumId = _currentPlayingMusic.AlbumId,
                    MusicName = _currentPlayingMusic.Name,
                    SingerName = _currentPlayingMusic.SingerName,
                    AlbumName = _currentPlayingMusic.AlbumName,
                    MusicFileUri = _currentPlayingMusic.FileUri,
                    Duration = _currentPlayingMusic.Duration
                };

                HistoryPlayListUtil.SaveHistoryPlayItem(historyItemEntity);
            }
        }

        /// <summary>
        /// 正在播放的歌曲是否在当前页面
        /// </summary>
        public bool IsCurrentPlayingPage { get; set; } = false;
    }
}
