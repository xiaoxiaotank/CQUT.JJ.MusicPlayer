using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Models.JM.Common;
using CQUT.JJ.MusicPlayer.WCFService;
using JiebaNet.Segmenter;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic.MusicHall
{
    /// <summary>
    /// HandpickedPage.xaml 的交互逻辑
    /// </summary>
    public partial class HandpickedPage : Page
    {
        private readonly IMusicService _musicService;

        public HandpickedPage()
        {
            _musicService = new MusicService();

            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            LoadNewSongs();
        }

        private async void LoadNewSongs()
        {
            WpNewSong.Children.Clear();
            var newSongs = await Task.Factory.StartNew(() =>
            {
                return _musicService.GetLastestMusics(10);
            });
            foreach (var song in newSongs)
            {
                var image = new Image()
                {
                    Width = 150,
                    Height = 150,
                    Stretch = Stretch.UniformToFill,
                    Source = ConstantsUtil.DefaultMusicHeaderPath.ToImageSource(),
                    Margin = new Thickness(0,10,10,10),
                };

                var ceiling = new Rectangle()
                {
                    Name = "Ceiling",
                    Width = image.Width,
                    Height = image.Height,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#30000000")),
                    Visibility = Visibility.Hidden
                };

                var imageGrid = new Grid() { Tag = song.Id };
                imageGrid.Children.Add(image);
                imageGrid.Children.Add(ceiling);


                imageGrid.MouseEnter += ImageGrid_MouseEnter;
                imageGrid.MouseLeave += ImageGrid_MouseLeave;
                imageGrid.MouseUp += Song_Click;

                var tbSong = new TextBlock()
                {
                    Text = song.Name,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 14
                };

                var btn = new JmTransparentButton()
                {
                    Content = tbSong,
                    Tag = song.Id
                };

                btn.Click += Song_Click;
                
                var tbSinger = new TextBlock()
                {
                    Text = song.SingerName,
                    Foreground = new SolidColorBrush(Colors.Silver),
                    Margin = new Thickness(0, 10, 0, 0)
                };
                var sp = new StackPanel()
                {
                    Width = 150,
                    Margin = new Thickness(0, 0, 15, 0)
                };
                sp.Children.Add(imageGrid);
                sp.Children.Add(btn);
                sp.Children.Add(tbSinger);

                WpNewSong.Children.Add(sp);
            }
        }



        private void ImageGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;
            var ceiling = grid.Children[1] as Rectangle;
            ceiling.Visibility = Visibility.Visible;
        }

        private void ImageGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;
            var ceiling = grid.Children[1] as Rectangle;
            ceiling.Visibility = Visibility.Hidden;
        }

        private async void Song_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as FrameworkElement).Tag);
            var song = await Task.Factory.StartNew(() =>
            {
                return _musicService.GetMusicById(id);
            });

            var model = new MusicModel()
            {
                Id = song.Id,
                SingerId = song.SingerId,
                AlbumId = song.AlbumId,
                Name = song.Name,
                SingerName = song.SingerName,
                AlbumName = song.AlbumName,
                FileUri = new Uri(song.FileUrl, UriKind.Relative),
                Duration = song.Duration
            };

            JMApp.CurrentPlayingMusicsInfo = new CurrentPlayingMusicsInfo()
            {
                PlayingListMusics = new List<MusicModel>()
                {
                    model
                },
                IsCurrentPlayingPage = true,
            };

            if (JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic?.Id.Equals(id) != true)
            {
                JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic = model;
            }

            await Task.Factory.StartNew(() => MusicPlayStateChangedUtil.InvokeFromJM(model, true));
        }
    }
}
