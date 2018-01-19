using CQUT.JJ.MusicPlayer.Client.Utils;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.Skin.ThemeSkin
{
    /// <summary>
    /// PurityPage.xaml 的交互逻辑
    /// </summary>
    public partial class PurityPage : Page
    {
        private const int ColorBlockCountPerLine = 10;

        public PurityPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Color color;
            var blockMargin = new Thickness(0, 0, 7, 7);
            var blockWidth = (WPanel.ActualWidth - blockMargin.Right * ColorBlockCountPerLine) / ColorBlockCountPerLine;
            var blockHeight = 35d;
            foreach(var colorProperty in typeof(Colors).GetProperties())
            {
                if (colorProperty.PropertyType.Equals(typeof(Color)))
                {
                    color = (Color)colorProperty.GetValue(null);
                    var background = new SolidColorBrush(color);
                    var colorBlock = new Rectangle()
                    {
                        Width = blockWidth,
                        Height = blockHeight,
                        Margin = blockMargin,
                        Fill = background,
                        SnapsToDevicePixels = true
                    };
                    colorBlock.MouseLeftButtonUp += SetMusicPlayerSkin;
                    WPanel.Children.Add(colorBlock);
                }
            }
        }

        private void SetMusicPlayerSkin(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle o)
                JmSkinChangedUtil.Invoke(o.Fill);
        }
    }
}
