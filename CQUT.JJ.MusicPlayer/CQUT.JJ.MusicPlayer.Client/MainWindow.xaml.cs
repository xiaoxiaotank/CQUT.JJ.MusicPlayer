using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Client.Windows;
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
        private static MainWindowViewModel _mainWinViewModel = new MainWindowViewModel()
        {
            TopBarBackgroundOpacity = 0.7,
            LeftBarBackgroundOpacity = 0.4,
            BottomBarBackgroundOpacity = 0.6,
            ContentBackgroundOpacity = 0.5,
            BackgroundOpacity = 0.9,
            Background = new SolidColorBrush(Colors.Black)
        };

        public MainWindow()
        {
            InitializeComponent();

            //页面切换
            MusicPageChangedUtil.PageChangedEvent += MusicPageChanged;
            //皮肤切换
            JmSkinChangedUtil.SkinChangedEvent += JmSkinChanged;
        }
       

        private void JmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWinViewModel;

            JmSkinChangedUtil.Invoke(
                JmSkinChangedUtil.DefaultImageSkinArgs.Background
                , JmSkinChangedUtil.DefaultImageSkinArgs.IsImageBrush);
        }

        private void BtnSkin_Click(object sender,RoutedEventArgs e)
        {
            var skinWinWidth = Width * 0.7;
            var skinWinHeight = Height * 0.7;
            var skinWin = new SkinManagerWindow(skinWinWidth, skinWinHeight)
            {
                Owner = this
            };
            skinWin.ShowDialog();
        }

        private void JmSkinChanged(object sender, SkinChangedArgs e)
        {
            if(e.IsImageBrush)
            {
                Background = e.Background;
                TopBarBackground
                    = LeftBarBackground
                    = BottomBarBackground
                    = ContentBackground
                    = _mainWinViewModel.Background;               
            }
            else
            {
                TopBarBackground
                    = LeftBarBackground
                    = BottomBarBackground
                    = ContentBackground
                    = e.Background;
                TopBarBackgroundOpacity
                    = LeftBarBackgroundOpacity
                    = BottomBarBackgroundOpacity
                    = ContentBackgroundOpacity
                    = 0.9;
                Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void MusicPageChanged(object sender, PageChangedEventArgs e) => FMusicPage.Source = e.PageSource;
    }
    
}
