using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.Skin.ThemeSkin
{
    /// <summary>
    /// PrimaryPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrimaryPage : Page
    {
        private static readonly string _themeSkinBaseUrl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Asserts/Skins");

        private readonly ThemeSkinType _themeSkinType;

        private double _skinWidth = 0;

        private static readonly double _skinHeight = 78;

        private static readonly double _skinCountPerLine = 5;

        private static readonly Thickness _skinMargin = new Thickness(0, 0, 10, 10);

        private string _skinPath = string.Empty;

        public PrimaryPage(ThemeSkinType themeSkinType)
        {
            InitializeComponent();

            _themeSkinType = themeSkinType;
            SpCustom.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _skinWidth = (WPanel.ActualWidth - (_skinMargin.Right * _skinCountPerLine)) / +_skinCountPerLine;

            switch (_themeSkinType)
            {
                case ThemeSkinType.Recommend:
                    LoadRecommendThemeSkins();
                    break;
                case ThemeSkinType.Custom:
                    LoadCustomThemeSkins();
                    break;
            }
        }


        private void Cover_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var cover = sender as Rectangle;
            SkinChanged(cover.Tag, e);
        }

        private void SkinChanged(object sender, MouseButtonEventArgs e)
        {
            var block = sender as Rectangle;
            JmSkinChangedUtil.Invoke(new SkinModel(block.Fill, block.Tag.ToString(), true));
        }

        private void BtnSelectLocalImgBg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "图像文件(*jpg)|*jpg",
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                var imagePath = dialog.FileName;
                var imageName = dialog.SafeFileName;

                var savedPath = FileUtil.SaveFileToLocal(imagePath,_skinPath, imageName);
                if (string.IsNullOrWhiteSpace(savedPath))
                    MessageBox.Show("上传失败!");
                else
                {
                    var control = ShowImageSkin(new Uri(savedPath, UriKind.Absolute));
                    SkinChanged(control, null);
                }
            }
        }


        #region Helpers
        private void LoadRecommendThemeSkins()
        {
            _skinPath = $"{_themeSkinBaseUrl}/ThemeSkins";
            LoadThemeSkins(_skinPath);
        }

        private void LoadCustomThemeSkins()
        {
            SpCustom.Visibility = Visibility.Visible;
            _skinPath = $"{_themeSkinBaseUrl}/CustomSkins";
            LoadThemeSkins(_skinPath);
        }

        private void LoadThemeSkins(string skinPath)
        {
            var fileFilters = @"*.jpg";
            if (Directory.Exists(skinPath))
            {
                foreach (var imagePath in Directory.GetFiles(skinPath, fileFilters, SearchOption.AllDirectories))
                {
                    var imageUri = new Uri(imagePath, UriKind.Absolute);
                    ShowImageSkin(imageUri);
                }
            }
        }

        /// <summary>
        /// 把背景添加到窗口中进行展示
        /// </summary>
        /// <param name="imageUri"></param>
        /// <returns>添加的背景样图</returns>
        private object ShowImageSkin(Uri imageUri)
        {
            var imageBlock = new Rectangle()
            {
                Fill = imageUri.ToImageBrush(),
                Tag = imageUri.AbsolutePath,
                SnapsToDevicePixels = true
            };
            var cover = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Black),
                Opacity = 0,
                Tag = imageBlock,
                Style = FindResource("CoverStyle") as Style
            };
            var grid = new Grid()
            {
                Width = _skinWidth,
                Height = _skinHeight,
                Margin = _skinMargin,
                SnapsToDevicePixels = true
            };

            grid.Children.Add(imageBlock);
            grid.Children.Add(cover);

            imageBlock.MouseLeftButtonUp += SkinChanged;
            cover.MouseLeftButtonUp += Cover_MouseLeftButtonUp;

            WPanel.Children.Add(grid);
            return imageBlock;
        }

        #endregion
    }
}
