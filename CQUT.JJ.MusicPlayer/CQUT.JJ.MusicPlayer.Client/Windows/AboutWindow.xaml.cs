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
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : JmWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOk_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnOk.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A555"));
        }

        private void BtnOk_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnOk.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#31C27C"));
        }
    }
}
