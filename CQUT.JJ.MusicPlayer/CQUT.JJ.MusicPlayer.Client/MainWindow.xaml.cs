using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : JmWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MusicPageChangedUtil.PageChangedEvent += MusicPageChanged;
        }

        private void MusicPageChanged(object sender, PageChangedEventArgs e)
        {
            FMusicPage.Source = e.PageSource;
        }

        private void JmWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }


    }
    
}
