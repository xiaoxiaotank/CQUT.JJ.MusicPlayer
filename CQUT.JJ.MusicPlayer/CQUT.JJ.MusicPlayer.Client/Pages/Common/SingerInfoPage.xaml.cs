﻿using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.ViewModels.Common;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using CQUT.JJ.MusicPlayer.WCFService;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.Common
{
    /// <summary>
    /// SingerInfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class SingerInfoPage : Page
    {
        private readonly ISingerService _singerService;

        private readonly int _singerId;

        private SingerViewModel _singerViewModel;

        public SingerInfoPage(int singerId)
        {
            _singerService = new SingerService();

            _singerId = singerId;
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var singer = await Task.Factory.StartNew(() =>
            {
                return _singerService.GetSingerById(_singerId);
            });
            
            if(singer != null)
            {
                _singerViewModel = new SingerViewModel()
                {
                    Id = singer.Id,
                    Name = singer.Name,
                    Nationality = singer.Nationality,
                    ForeignerName = singer.ForeignName,
                    ProfilePhotoPath = ConstantsUtil.DefaultProfilePhotoPath
                };
                DataContext = _singerViewModel;
                InitSingerInfoLabels();
            }
            
        }

        private void InitSingerInfoLabels()
        {
            if (!string.IsNullOrWhiteSpace(_singerViewModel.ForeignerName))
                WpForeignerName.Visibility = Visibility.Visible;
            if (!string.IsNullOrWhiteSpace(_singerViewModel.Nationality))
                WpNationality.Visibility = Visibility.Visible;
            if (!string.IsNullOrWhiteSpace(_singerViewModel.MagnumOpusName))
                WpMagnumOpusName.Visibility = Visibility.Visible;
            if (DateTime.Parse(_singerViewModel.Birthday) != default(DateTime))
                WpBirthday.Visibility = Visibility.Visible;
        }
    }
}