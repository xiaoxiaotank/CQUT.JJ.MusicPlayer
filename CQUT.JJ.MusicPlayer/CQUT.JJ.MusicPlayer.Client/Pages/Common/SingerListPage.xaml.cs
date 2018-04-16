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
    /// SingerListPage.xaml 的交互逻辑
    /// </summary>
    public partial class SingerListPage : Page
    {
        private readonly ISearchService _searchService;
        private readonly string _singerName;

        public SingerListPage(string singerName)
        {
            _searchService = new SearchService();

            _singerName = singerName;
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var singers = await Task.Factory.StartNew(() =>
            {
                return _searchService.Search(MusicRequestType.Singer, _singerName, 1);
            }) as SingerSearchPageResult;
            Panel.Children.Clear();
            singers.Results.ToList()
            .ForEach(s => 
            {
                var ellipse = new Ellipse()
                {
                    Width = 100,
                    Height = 100,
                    Fill = new ImageBrush((ConstantsUtil.APP_Directory + ConstantsUtil.DefaultProfilePhotoPath).ToImageSource(UriKind.Absolute))
                };

                var tb = new TextBlock()
                {
                    Text = s.Name
                };

                var btn = new JmTransparentButton()
                {
                    Margin = new Thickness(0,10,10,10),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = tb,
                    Tag = s.Id
                };
                btn.Click += SingerNameBtn_Click;

                var sp = new StackPanel()
                {
                    Margin = new Thickness(20, 10, 20, 50),
                };

                sp.Children.Add(ellipse);
                sp.Children.Add(btn);

                Panel.Children.Add(sp);
            });


        }

        private void SingerNameBtn_Click(object sender, RoutedEventArgs e)
        {
            var singerId = Convert.ToInt32((sender as Button).Tag);
            var singerInfoPage = new SingerInfoPage(singerId);
            (App.Current.MainWindow.FindName("FMusicPage") as Frame).Navigate(singerInfoPage);
        }
    }
}
