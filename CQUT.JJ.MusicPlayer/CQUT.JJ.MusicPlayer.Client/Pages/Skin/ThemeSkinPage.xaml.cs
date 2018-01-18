using CQUT.JJ.MusicPlayer.Client.ControlAttachProperties;
using CQUT.JJ.MusicPlayer.Client.Pages.Skin.ThemeSkin;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.Skin
{
    /// <summary>
    /// ThemeSkinPage.xaml 的交互逻辑
    /// </summary>
    public partial class ThemeSkinPage : Page
    {
        private static readonly string _pageOfColumn = "ThemeSkin";
        public ThemeSkinPage()
        {
            InitializeComponent();
        }

        private void TcSkin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPage = TcSkin.SelectedItem as TabItem;
            var frame = selectedPage.Content as Frame;
            var pageName = TabItemControlAttachProperty.GetPageNameWithoutExtension(selectedPage);
            var dataType = TabItemControlAttachProperty.GetDataType(selectedPage);
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                switch(pageName)
                {
                    case "PrimaryPage":
                        frame.Navigate(new PrimaryPage(dataType));
                        break;

                }
            }
            else
                frame.Source = new Uri($@"{_pageOfColumn}/{pageName}.xaml",UriKind.Relative);
        }
    }
}
