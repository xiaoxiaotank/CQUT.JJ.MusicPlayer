using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic.Search
{
    /// <summary>
    /// MusicSearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class MusicSearchPage : Page
    {
        public MusicSearchPage()
        {
            PageLoadedUtil.MusicListPageLoadedEvent += PageLoadedUtil_MusicListPageLoadedEvent;
            InitializeComponent();
        }

        private void PageLoadedUtil_MusicListPageLoadedEvent(object sender, EventArgs e)
        {
            if(this.IsVisible)
                MusicSearchInfoChangedUtil.InvokeFromJMRequest(MusicRequestType.Song, 1);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
                    
        }

    }
}
