using CQUT.JJ.MusicPlayer.Client.Pages.Common;
using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic.MusicHall
{
    /// <summary>
    /// SingerPage.xaml 的交互逻辑
    /// </summary>
    public partial class SingerPage : Page
    {
        private readonly ISingerService _singerService;

        public SingerPage()
        {
            _singerService = new SingerService();

            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var singers = await Task.Factory.StartNew(() =>
            {
                return _singerService.GetHotSingersOfCount(10);
            });

            WpSinger.Children.Clear();
            foreach (var singer in singers)
            {
                var ellipse = new Ellipse()
                {
                    Fill = new ImageBrush((ConstantsUtil.APP_Directory + ConstantsUtil.DefaultSingerHeaderPath).ToImageSource(UriKind.Absolute)),
                    Margin = new Thickness(40, 10, 40, 40)
                };

                var singerBtn = new JmTransparentButton()
                {
                    Content = singer.Name,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 0, 0, 10),
                    Tag = singer.Id
                };

                singerBtn.MouseEnter += SingerBtn_MouseEnter;
                singerBtn.MouseLeave += SingerBtn_MouseLeave;
                singerBtn.Click += SingerBtn_Click;

                var grid = new Grid()
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#30FFFFFF")),
                    Width = 180,
                    Height = 150,
                    Margin = new Thickness(0, 0, 15, 15)
                };

                grid.Children.Add(ellipse);
                grid.Children.Add(singerBtn);

                WpSinger.Children.Add(grid);
            }
        }

        private void SingerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn)
            {
                var id = Convert.ToInt32(btn.Tag);
                var singerPage = new SingerInfoPage(id);
                ControlUtil.FMusicPageNavigateTo(singerPage, false);
            }
        }

        private void SingerBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            if(sender is Button btn)
            {
                btn.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void SingerBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Foreground = new SolidColorBrush(Colors.SkyBlue);
            }
        }
    }
}
