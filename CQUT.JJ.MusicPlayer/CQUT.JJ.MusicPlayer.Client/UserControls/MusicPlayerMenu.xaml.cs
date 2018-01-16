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

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// MusicPlayerMenu.xaml 的交互逻辑
    /// </summary>
    public partial class MusicPlayerMenu : UserControl
    {
        public MusicPlayerMenu()
        {
            InitializeComponent();
        }

        private void BtnVolume_Click(object sender, RoutedEventArgs e)
        {
            PopVolume.IsOpen = true;
        }

        private void BtnPopVolume_Click(object sender, RoutedEventArgs e)
        {
            BtnPopVolume.IsOddClickNumber = !BtnPopVolume.IsOddClickNumber;
            if (BtnPopVolume.IsOddClickNumber)
            {
                TbVolume.Text = "\ue609";
                BtnPopVolume.ToolTip = "音量:静音";
            }
            else
            {
                TbVolume.Text = "\ue60b";
                BtnPopVolume.ToolTip = "音量:";
            }
        }
    }
}
