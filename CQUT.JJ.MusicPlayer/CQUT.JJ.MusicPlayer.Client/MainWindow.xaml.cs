﻿using CQUT.JJ.MusicPlayer.Client.Utils;
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
            TopFloorBackground = new SolidColorBrush(Colors.Black)
        };

        /// <summary>
        /// 背景是否为图片
        /// </summary>
        private bool _isBackgroundOfImage = false;

        private const double PurityOpacity = 0.9;

        public MainWindow()
        {
            //页面切换
            MusicPageChangedUtil.PageChangedEvent += MusicPageChanged;
            //皮肤切换
            JmSkinChangedUtil.SkinChangedEvent += JmSkinChanged;
            //皮肤透明度调节
            JmSkinOpacityChangedUtil.SkinOpacityChangedEvent += JmSkinOpacityChanged;

            InitializeComponent();
            InitializeSkin();
        }



        private void InitializeSkin()
        {
            if(!File.Exists(JmSkinChangedUtil.SkinConfigFilePath))
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
                var isImageBrush =Convert.ToBoolean(skinInfo[0]);
                Brush background = null;
                if (isImageBrush)
                {
                    background = new Uri(skinInfo[1], UriKind.Absolute).ToImageBrush();
                    _isBackgroundOfImage = true;
                }
                else
                    background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(skinInfo[1]));
                JmSkinChangedUtil.Invoke(new SkinModel(background,skinInfo[1],isImageBrush));
            }
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
                TopBarBackground
                    = LeftBarBackground
                    = BottomBarBackground
                    = ContentBackground
                    = _mainWinViewModel.TopFloorBackground;
                TopBarBackgroundOpacity = _mainWinViewModel.TopBarBackgroundOpacity;
                LeftBarBackgroundOpacity = _mainWinViewModel.LeftBarBackgroundOpacity;
                BottomBarBackgroundOpacity = _mainWinViewModel.BottomBarBackgroundOpacity;
                ContentBackgroundOpacity = _mainWinViewModel.ContentBackgroundOpacity;
                _isBackgroundOfImage = true;
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
                    = PurityOpacity;
                Background = new SolidColorBrush(Colors.Transparent);
                _isBackgroundOfImage = false;
            }
        }

        private void MusicPageChanged(object sender, PageChangedEventArgs e) => FMusicPage.Source = e.PageSource;

        private void JmSkinOpacityChanged(object sender, SkinOpacityChangedArgs e)
        {
            if (_isBackgroundOfImage)
            {
                BackgroundOpacity = SetOpacityBySlider(_mainWinViewModel.BackgroundOpacity, e.Opacity);
                TopBarHeadBackgroundOpacity = SetOpacityBySlider(_mainWinViewModel.TopBarBackgroundOpacity, e.Opacity);
                LeftBarBackgroundOpacity = SetOpacityBySlider(_mainWinViewModel.LeftBarBackgroundOpacity, e.Opacity);
                BottomBarBackgroundOpacity = SetOpacityBySlider(_mainWinViewModel.BottomBarBackgroundOpacity, e.Opacity);
                ContentBackgroundOpacity = SetOpacityBySlider(_mainWinViewModel.ContentBackgroundOpacity, e.Opacity);
            }
            else
            {
                BackgroundOpacity = SetOpacityBySlider(PurityOpacity, e.Opacity);
                TopBarHeadBackgroundOpacity = SetOpacityBySlider(PurityOpacity, e.Opacity);
                LeftBarBackgroundOpacity = SetOpacityBySlider(PurityOpacity, e.Opacity);
                BottomBarBackgroundOpacity = SetOpacityBySlider(PurityOpacity, e.Opacity);
                ContentBackgroundOpacity = SetOpacityBySlider(PurityOpacity, e.Opacity);
            }
        }

        private double SetOpacityBySlider(double startOpacity, double opacityOffset)
        {
            var opacity = startOpacity - opacityOffset;
            if (opacity < 0)
                opacity = 0;
            else if (opacity > 1)
                opacity = 1;
            return opacity;
        }
    }
    
}
