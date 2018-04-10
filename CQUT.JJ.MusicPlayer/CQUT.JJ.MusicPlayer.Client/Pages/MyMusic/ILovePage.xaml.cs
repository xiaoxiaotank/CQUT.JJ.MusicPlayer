using CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.Windows;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Search;
using CQUT.JJ.MusicPlayer.WCFService;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.MyMusic
{
    /// <summary>
    /// ILovePage.xaml 的交互逻辑
    /// </summary>
    public partial class ILovePage : Page
    {
        private readonly IMusicService _musicService;

        public ILovePage()
        {
            _musicService = new MusicService();
            PageLoadedUtil.MusicListPageLoadedEvent += PageLoadedUtil_MusicListPageLoadedEvent;
            InitializeComponent();
        }

        private void PageLoadedUtil_MusicListPageLoadedEvent(object sender, EventArgs e)
        {
            if (IsVisible)
            {
                if (App.User != null)
                {
                    var musicInfoOfPageModel = new MusicSearchPageResult()
                    {
                        PageCount = 1,
                        PageNumber = 1,
                        ResultType = MusicRequestType.Song,
                        Results = _musicService.GetLoveMusicsByUserId(App.User.Id).Reverse()
                    };
                    MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(musicInfoOfPageModel, 1);
                    SongCount.Text = (musicInfoOfPageModel?.Results.Count() ?? 0).ToString();
                }
                else
                {
                    new LoginWindow().ShowDialog();
                }
            }
        }

        private void FSong_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
