using CQUT.JJ.MusicPlayer.Client.Utils.Configs.HistoryPlayList;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Search;
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
    /// PlayHistoryPage.xaml 的交互逻辑
    /// </summary>
    public partial class PlayHistoryPage : Page
    {
        public PlayHistoryPage()
        {
            PageLoadedUtil.MusicListPageLoadedEvent += PageLoadedUtil_MusicListPageLoadedEvent;
            InitializeComponent();
        }

        private async void PageLoadedUtil_MusicListPageLoadedEvent(object sender, EventArgs e)
        {
            if(this.IsVisible)
            {
                var musics = await GetHistoryMusics();
                TbSongCount.Text = (musics.Results?.Count() ?? 0).ToString();
                MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(musics, 1);
            }
                 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            
           
        }

        private async Task<MusicSearchPageResult> GetHistoryMusics()
        {
            var result = await Task.Factory.StartNew(() =>
            {
                return new MusicSearchPageResult()
                {
                    PageCount = 1,
                    PageNumber = 1,
                    ResultType = MusicRequestType.Song,
                    Results = HistoryPlayListUtil.GetHistoryPlayList()?.Select(e => new MusicInfo()
                    {
                        Id = e.MusicId,
                        SingerId = e.SingerId,
                        AlbumId = e.AlbumId,
                        Name = e.MusicName,
                        SingerName = e.SingerName,
                        AlbumName = e.AlbumName,
                        FileUrl = e.MusicFileUri.OriginalString,
                        Duration = e.Duration
                    }),
                    
                };
            });
            return result;
        }
    }
}
