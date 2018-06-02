using CQUT.JJ.MusicPlayer.Client.UserControls;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.UserMusicList
{
    /// <summary>
    /// UserMusicList.xaml 的交互逻辑
    /// </summary>
    public partial class UserMusicList : Page
    {
        private readonly IMusicService _musicService;

        private int _userMusicListId;

        public UserMusicList(int userMusicListId)
        {
            _userMusicListId = userMusicListId;

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
                        Results = _musicService.GetMusicsByMusicListId(_userMusicListId).Reverse()
                    };
                    SongCount.Text = (musicInfoOfPageModel?.Results.Count() ?? 0).ToString();
                    MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(musicInfoOfPageModel, 1);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var userMusicList = UserMusicNavigtion.UserMusicListService.GetUserMusicListById(_userMusicListId);
            TbUserMusicListName.Content = userMusicList?.Name;
        }
    }
}
