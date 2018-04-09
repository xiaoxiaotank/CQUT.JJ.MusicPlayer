using CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
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
            InitializeComponent();
        }

        private void FSong_Loaded(object sender, RoutedEventArgs e)
        {
            if(App.User != null)
            {
                var musicInfoOfPageModel = new MusicSearchPageResult()
                {
                    PageCount = 1,
                    PageNumber = 1,
                    ResultType = MusicRequestType.Song,
                    Results = _musicService.GetLoveMusicsByUserId(App.User.Id)
                };
                MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(musicInfoOfPageModel, 1);
                SongCount.Text = (musicInfoOfPageModel?.Results.Count() ?? 0).ToString();
            }
            else
            {
                FSong.Content = "请登录";
            }
        }
    }
}
