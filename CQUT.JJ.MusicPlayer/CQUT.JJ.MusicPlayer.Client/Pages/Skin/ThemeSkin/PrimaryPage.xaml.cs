using CQUT.JJ.MusicPlayer.Client.Utils;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.Skin.ThemeSkin
{
    /// <summary>
    /// PrimaryPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrimaryPage : Page
    {
        private static readonly string _themeSkinBaseUrl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Asserts");

        private readonly ThemeSkinType _themeSkinType;

        private double _skinWidth = 0;

        private static readonly double _skinHeight = 78;

        private static readonly double _skinCountPerLine = 5;

        private static readonly Thickness _skinMargin = new Thickness(0, 0, 10, 10);

        public PrimaryPage(ThemeSkinType themeSkinType)
        {
            InitializeComponent();

            _themeSkinType = themeSkinType;
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
                    break;
            }
        }

        private void LoadRecommendThemeSkins()
        {
            string skinsPath = $"{_themeSkinBaseUrl}/ThemeSkins";
            var fileFilters = @"*.jpg";
            foreach (var imagePath in Directory.GetFiles(skinsPath,fileFilters,SearchOption.AllDirectories))
            {
                var imageUri = new Uri(imagePath, UriKind.Absolute);
                ShowImageSkin(imageUri);
            }
        }

        private void ShowImageSkin(Uri imageUri)
        {
            var imageBlock = new Rectangle()
            {
                Width = _skinWidth,
                Height = _skinHeight,
                Fill = imageUri.ImageUriToImageBrush(),
                SnapsToDevicePixels = true
            };
            imageBlock.MouseLeftButtonUp += SkinChanged;
            WPanel.Children.Add(imageBlock);
        }

        private void SkinChanged(object sender, MouseButtonEventArgs e)
        {
            var block = sender as Rectangle;
            JmSkinChangedUtil.Invoke(block.Fill, true);
        }
    }
}
