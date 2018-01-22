using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Client.Windows;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
        private const double PurityOpacity = 0.9;
        private const double DefaultTopBarBackgroundOpacity = 0.7;
        private const double DefaultLeftBarBackgroundOpacity = 0.4;
        private const double DefaultBottomBarBackgroundOpacity = 0.6;
        private const double DefaultContentBackgroundOpacity = 0.5;
        private const double DefaultBackgroundOpacity = 0.9;

        /// <summary>
        /// 背景是否为图片
        /// </summary>
        private bool _isBackgroundOfImage = false;

        private static MainWindowViewModel _mainWinViewModel = new MainWindowViewModel()
        {
            TopBarBackgroundOpacity = DefaultTopBarBackgroundOpacity,
            LeftBarBackgroundOpacity = DefaultLeftBarBackgroundOpacity,
            BottomBarBackgroundOpacity = DefaultBottomBarBackgroundOpacity,
            ContentBackgroundOpacity = DefaultContentBackgroundOpacity,
            BackgroundOpacity = DefaultBackgroundOpacity,
            TopFloorBackground = new SolidColorBrush(Colors.Black)
        };

        public MainWindow()
        {
            //页面切换
            MusicPageChangedUtil.PageChangedEvent += MusicPageChanged;
            //皮肤切换
            JmSkinChangedUtil.SkinChangedEvent += JmSkinChanged;
            //皮肤透明度调节
            JmSkinOpacityChangedUtil.SkinOpacityChangedEvent += JmSkinOpacityChanged;

            InitializeComponent();
            InitializeTaskBarIcon();
            InitializeSkin();
        }

        private void JmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWinViewModel;
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
                _mainWinViewModel.BackgroundOpacity = DefaultBackgroundOpacity;
                _mainWinViewModel.TopFloorBackground = new SolidColorBrush(Colors.Black);
                _mainWinViewModel.TopBarBackgroundOpacity = DefaultTopBarBackgroundOpacity;
                _mainWinViewModel.LeftBarBackgroundOpacity = DefaultLeftBarBackgroundOpacity;
                _mainWinViewModel.BottomBarBackgroundOpacity = DefaultBottomBarBackgroundOpacity;
                _mainWinViewModel.ContentBackgroundOpacity = DefaultContentBackgroundOpacity;
                _isBackgroundOfImage = true;
            }
            else
            {
                _mainWinViewModel.TopFloorBackground = e.Background;
                if (_isBackgroundOfImage)
                {
                    _mainWinViewModel.TopBarBackgroundOpacity
                   = _mainWinViewModel.LeftBarBackgroundOpacity
                   = _mainWinViewModel.BottomBarBackgroundOpacity
                   = _mainWinViewModel.ContentBackgroundOpacity
                   = PurityOpacity;
                   Background = new SolidColorBrush(Colors.Transparent);
                   _isBackgroundOfImage = false;
                }                
            }
        }

        private void MusicPageChanged(object sender, PageChangedEventArgs e) => FMusicPage.Source = e.PageSource;

        private void JmSkinOpacityChanged(object sender, SkinOpacityChangedArgs e)
        {
            if (_isBackgroundOfImage)
            {
                _mainWinViewModel.BackgroundOpacity = SetOpacityBySlider(DefaultBackgroundOpacity, e.Opacity);
                _mainWinViewModel.TopBarBackgroundOpacity = SetOpacityBySlider(DefaultTopBarBackgroundOpacity, e.Opacity);
                _mainWinViewModel.LeftBarBackgroundOpacity = SetOpacityBySlider(DefaultLeftBarBackgroundOpacity, e.Opacity);
                _mainWinViewModel.BottomBarBackgroundOpacity = SetOpacityBySlider(DefaultBottomBarBackgroundOpacity, e.Opacity);
                _mainWinViewModel.ContentBackgroundOpacity = SetOpacityBySlider(DefaultContentBackgroundOpacity, e.Opacity);
            }
            else
            {
                _mainWinViewModel.BackgroundOpacity 
                    = _mainWinViewModel.TopBarBackgroundOpacity
                    = _mainWinViewModel.LeftBarBackgroundOpacity 
                    = _mainWinViewModel.BottomBarBackgroundOpacity 
                    = _mainWinViewModel.ContentBackgroundOpacity 
                    = SetOpacityBySlider(PurityOpacity, e.Opacity);
            }
        }


        private void OnNotifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                Topmost = true;
            }
            else if(e.Button == MouseButtons.Right)
            {
                var nofityIconMenu = (System.Windows.Controls.ContextMenu)FindResource("NofityIconMenu");
                nofityIconMenu.IsOpen = true;
            }
        }

        private void JmWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            e.Cancel = true;
        }

        private void JmWindow_Close(object sender, RoutedEventArgs e)
        {
            Close();
            System.Environment.Exit(0);
        }


        #region Helpers
        private double SetOpacityBySlider(double startOpacity, double opacityOffset)
        {
            var opacity = startOpacity - opacityOffset;
            if (opacity < 0)
                opacity = 0;
            else if (opacity > 1)
                opacity = 1;
            return opacity;
        }


        private void InitializeTaskBarIcon()
        {
            NotifyIcon.Text = NotifyIcon.BalloonTipText = "JM音乐，听我想听的歌！";
            NotifyIcon.Icon = new System.Drawing.Icon("JM.ico");
            //ShowInTaskbar = false;
            NotifyIcon.MouseClick += OnNotifyIcon_Click;
            NotifyIcon.ShowBalloonTip(1000);
        }

        private void InitializeSkin()
        {
            if (!File.Exists(JmSkinChangedUtil.SkinConfigFilePath))
            {
                File.Create(JmSkinChangedUtil.SkinConfigFilePath).Close();
                var skinModel = new SkinModel(
                    JmSkinChangedUtil.DefaultImageSkinArgs.Background
                    , JmSkinChangedUtil.DefaultImageSkinPath
                    , JmSkinChangedUtil.DefaultImageSkinArgs.IsImageBrush);
                JmSkinChangedUtil.Invoke(skinModel);
            }
            else
            {
                var skinInfo = File.ReadAllLines(JmSkinChangedUtil.SkinConfigFilePath);
                var isImageBrush = Convert.ToBoolean(skinInfo[0]);
                Brush background = null;
                if (isImageBrush)
                {
                    background = new Uri(skinInfo[1], UriKind.Absolute).ToImageBrush();
                    _isBackgroundOfImage = true;
                }
                else
                    background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(skinInfo[1]));
                JmSkinChangedUtil.Invoke(new SkinModel(background, skinInfo[1], isImageBrush));
            }
        }
        #endregion

       
  
    }

}
