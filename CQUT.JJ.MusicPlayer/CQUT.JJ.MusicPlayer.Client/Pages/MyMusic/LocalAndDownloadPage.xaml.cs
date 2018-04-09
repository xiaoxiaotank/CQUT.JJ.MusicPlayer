using CQUT.JJ.MusicPlayer.Client.Utils.Configs;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.MyMusic
{
    /// <summary>
    /// LocalAndDownloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class LocalAndDownloadPage : Page
    {
        private static List<MusicInfoViewModel> _musicListViewModel = null;

        public LocalAndDownloadPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var downloadPath = await SettingUtil.GetDowloadPathAsync();
            _musicListViewModel = Directory.GetFiles(downloadPath)
                .Where(m => GlobalConstants.Music_File_Suffix.Contains(System.IO.Path.GetExtension(m)))
                .Select(m => new MusicInfoViewModel()
                {
                    MusicName = System.IO.Path.GetFileNameWithoutExtension(m),
                    FileUrl = m
                })
                .ToList();
            MusicList.ItemsSource = _musicListViewModel;
            LocalCount.Text = (_musicListViewModel?.Count ?? 0).ToString();
        }
    }
}
