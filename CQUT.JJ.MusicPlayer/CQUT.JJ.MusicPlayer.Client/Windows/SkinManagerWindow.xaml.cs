using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
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
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Client.Windows
{
    /// <summary>
    /// SkinManagerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SkinManagerWindow : JmWindow
    {
        private static JmWidgetTabItem _selectedTabItem = null;

        private static readonly string _pageOfColumn = "Skin";

        public SkinManagerWindow(double width,double height)
        {
            InitializeComponent();

            Width = width;
            Height = height;
        }      

        private void JmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TiThemeSkin.IsSelected = true;
            _selectedTabItem = TiThemeSkin;
            ChangePage();
        }

        private void TabItem_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (sender is JmWidgetTabItem originalSource && !originalSource.Equals(_selectedTabItem))
            {
                _selectedTabItem.IsSelected = false;
                _selectedTabItem = originalSource;
                ChangePage();
            }
        }

        private void ChangePage()
        {
            if(_selectedTabItem != null)
                FSkinPage.Source = new Uri($@"{MusicPageChangedUtil.PageBaseUrl}/{_pageOfColumn}/{_selectedTabItem.PageName}");
        }

    }
}
