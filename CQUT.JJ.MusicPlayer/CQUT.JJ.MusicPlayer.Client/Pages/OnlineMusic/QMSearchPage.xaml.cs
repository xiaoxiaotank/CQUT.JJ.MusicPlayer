using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic
{
    /// <summary>
    /// SearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearchPage : Page
    {
        /// <summary>
        /// 歌曲列表视图模型
        /// </summary>
        private static List<QMInfoViewModel> _musicListViewModel = null;

        public SearchPage()
        {
            InitializeComponent();
            MusicSearchInfoChangedUtil.QMSearchChangedEvent += MusicSearchInfoChangedUtil_QMSearchChangedEvent;  
        }

        private void MusicSearchInfoChangedUtil_QMSearchChangedEvent(object sender, QMSearchChangedArgs e)
        {
            _musicListViewModel = new List<QMInfoViewModel>();
            e.QMSongInfoModels.ToList().ForEach(m =>
            {
                _musicListViewModel.Add(new QMInfoViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    SingerName = m.Singer,
                    AlbumName = m.AlbumInfo.Name,
                    TimeDuration = m.TimeDuration
                });
            });
            MusicList.ItemsSource = _musicListViewModel;

            Waiting.Visibility = Visibility.Collapsed;
            GdSong.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Waiting.Visibility = Visibility.Visible;
            GdSong.Visibility = Visibility.Collapsed;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as FrameworkElement).Tag;
            if (id != null)
            {
                var musicViewModel = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(id));
                if(musicViewModel != null)
                {
                    var musicInfo = new MusicPlayInfoModel()
                    {
                        Id = musicViewModel.Id,
                        Name = musicViewModel.Name,
                        SingerName = musicViewModel.SingerName,                      
                        TimeDuration = musicViewModel.TimeDuration,
                        Uri = new Uri($"http://ws.stream.qqmusic.qq.com/C100{id}.m4a?fromtag=38", UriKind.Absolute),
                    };
                    MusicPlayStateChangedUtil.Invoke(musicInfo, true);
                }
            }
                
        }
    }
}
