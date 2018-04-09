using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.Configs;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Client.Pages.Setting
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {

        public SettingPage()
        {
            InitializeComponent();

            NonNavPageDisplayedUtil.Invoke();
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TxtDownloadPath.Text = await SettingUtil.GetDowloadPathAsync();
        }

        private void BtnChangeDownloadPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string dirPath = dialog.SelectedPath.Trim();

                if (Directory.Exists(dirPath))
                {
                    TxtDownloadPath.Text = dirPath;
                    SettingUtil.SetDownloadPath(dirPath);
                }
            }
        }

        private void BtnOpenDownloadDir_Click(object sender, RoutedEventArgs e)
        {
            var dirPath = TxtDownloadPath.Text.Trim();
            if (Directory.Exists(dirPath))
            {
                Process.Start(dirPath);
            }
        }

        private void TxtDownloadPath_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void TxtDownloadPath_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void TxtDownloadPath_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = true;
        }

    }
}
