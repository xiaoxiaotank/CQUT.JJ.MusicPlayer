using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels.Common;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.Common
{
    /// <summary>
    /// AlbumInfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlbumInfoPage : Page
    {
        private readonly IAlbumService _albumService;
        private readonly IMusicService _musicService;
        private readonly int _albumId;

        private AlbumViewModel _albumViewModel;

        public AlbumInfoPage(int albumId)
        {
            _albumId = albumId;
            _albumService = new AlbumService();
            _musicService = new MusicService();

            PageLoadedUtil.MusicListPageLoadedEvent += MusicListPageLoaded;

            InitializeComponent();
        }

        private async void MusicListPageLoaded(object sender, EventArgs e)
        {
            if (IsVisible)
            {
                var pagedResult = await Task.Factory.StartNew(() =>
                {
                    var musics = _musicService.GetMusicsByAlbumId(_albumId);
                    return new MusicSearchPageResult()
                    {
                        PageCount = 1,
                        PageNumber = 1,
                        ResultType = MusicRequestType.Song,
                        Results = musics
                    };
                });
                MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(pagedResult, 1);
                TbSongCount.Text = pagedResult.Results.Count().ToString();
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var album = await Task.Factory.StartNew(() =>
            {
                return _albumService.GetAlbumById(_albumId);
            });

            if (album != null)
            {
                _albumViewModel = new AlbumViewModel()
                {
                    Id = album.Id,
                    SingerId = album.SingerId,
                    Name = album.Name,
                    SingerName = album.SingerName,
                    HeaderPath = ConstantsUtil.DefaultAlbumHeaderPath
                };
                DataContext = _albumViewModel;
            }
        }
    }
}
