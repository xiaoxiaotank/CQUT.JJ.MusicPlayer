using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic
{
    /// <summary>
    /// SearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();

            MusicSearchInfoChangedUtil.QMSearchChangedEvent += MusicSearchInfoChangedUtil_QMSearchChangedEvent;

            Waiting.Visibility = Visibility.Visible;
            GdSong.Visibility = Visibility.Collapsed;
        }

        private void MusicSearchInfoChangedUtil_QMSearchChangedEvent(object sender, QMSearchChangedArgs e)
        {
            Waiting.Visibility = Visibility.Collapsed;
            GdSong.Visibility = Visibility.Visible;
        }
    }
}
