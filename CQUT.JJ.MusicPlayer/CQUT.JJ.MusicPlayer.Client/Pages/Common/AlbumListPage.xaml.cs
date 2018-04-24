using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.ViewModels.Common;
using CQUT.JJ.MusicPlayer.Controls.Controls;
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
    /// AlbumListPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlbumListPage : Page
    {
        private readonly ISearchService _searchService;
        private readonly string _albumName;

        public AlbumListPage(string albumName)
        {
            _searchService = new SearchService();
            _albumName = albumName;

            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var albums = await Task.Factory.StartNew(() =>
            {
                return _searchService.Search(MusicRequestType.Album, _albumName, 1);
            }) as AlbumSearchPageResult;

            Sp.Children.Clear();
            albums.Results.ToList()
            .ForEach(s =>
            {
                var header = new Image()
                {
                    Source = ConstantsUtil.DefaultAlbumHeaderPath.ToImageSource()
                };

                var albumNameBtn = new JmTransparentButton()
                {
                    Content = s.Name,
                    Tag = s.Id,
                };
                albumNameBtn.Click += AlbumNameBtn_Click;
                var albumNameWp = new WrapPanel();
                albumNameWp.Children.Add(albumNameBtn);

                var singerNameBtn = new JmTransparentButton()
                {
                    Content = s.SingerName,
                    Tag = s.SingerId,
                };
                singerNameBtn.Click += SingerNameBtn_Click;
                var singerNameWp = new WrapPanel();
                singerNameWp.Children.Add(singerNameBtn);

                var publishedDateTb = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Silver),
                    Text = s.PublishedTime.ToShortDateString()
                };

                var musicCountTb = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Silver),
                    Text = $"{s.MusicCount}首"
                };

                var colDef0 = new ColumnDefinition()
                {
                    Width = new GridLength(0,GridUnitType.Auto),
                };
                var colDef1 = new ColumnDefinition()
                {
                    Width = new GridLength(2, GridUnitType.Star)
                };
                var colDef2 = new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                var colDef3 = new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                var colDef4 = new ColumnDefinition()
                {
                    Width = new GridLength(60, GridUnitType.Pixel)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(colDef0);
                grid.ColumnDefinitions.Add(colDef1);
                grid.ColumnDefinitions.Add(colDef2);
                grid.ColumnDefinitions.Add(colDef3);
                grid.ColumnDefinitions.Add(colDef4);

                grid.Children.Add(header);
                grid.Children.Add(albumNameWp);
                grid.Children.Add(singerNameWp);
                grid.Children.Add(publishedDateTb);
                grid.Children.Add(musicCountTb);

                Grid.SetColumn(header, 0);
                Grid.SetColumn(albumNameWp, 1);
                Grid.SetColumn(singerNameWp, 2);
                Grid.SetColumn(publishedDateTb, 3);
                Grid.SetColumn(musicCountTb, 4);

                Sp.Children.Add(grid);
            });
        }

        private void AlbumNameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var id = Convert.ToInt32(btn.Tag);
                var albumPage = new AlbumInfoPage(id);
                ControlUtil.FMusicPageNavigateTo(albumPage);
            }
        }

        private void SingerNameBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn)
            {
                var id = Convert.ToInt32(btn.Tag);
                var singerPage = new SingerInfoPage(id);
                ControlUtil.FMusicPageNavigateTo(singerPage);
            }
        }
    }
}
