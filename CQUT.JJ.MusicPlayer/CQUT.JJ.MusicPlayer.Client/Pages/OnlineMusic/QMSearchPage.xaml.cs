﻿using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Models;
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

namespace CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic
{
    /// <summary>
    /// SearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearchPage : Page
    {
        /// <summary>
        /// 歌曲列表视图模型
        /// </summary>
        private static List<QMInfoViewModel> _musicListViewModel = null;

        private static int _currentPageNumber = 1;

        public SearchPage()
        {
            InitializeComponent();
            MusicSearchInfoChangedUtil.QMSearchChangedEvent += MusicSearchInfoChangedUtil_QMSearchChangedEvent;  
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Waiting.Visibility = Visibility.Visible;
            TbError.Visibility = Visibility.Collapsed;
            GdSong.Visibility = Visibility.Collapsed;
        }

        private void MusicSearchInfoChangedUtil_QMSearchChangedEvent(object sender, MusicSearchChangedArgs e)
        {
            if (e.IsSuccessed)
            {
                _musicListViewModel = new List<QMInfoViewModel>();
                e.MusicInfoOfPageModels.MusicInfoList.Select(m => m as QMInfoModel)?.ToList()
                    .ForEach(m =>
                    {
                        _musicListViewModel.Add(new QMInfoViewModel
                        {
                            Id = m.Id,
                            Name = m.Name,
                            SingerName = m.Singer,
                            AlbumName = m.AlbumInfo.Name,
                            TimeDuration = m.TimeDuration
                        });
                    });
                MusicList.ItemsSource = _musicListViewModel;
                InitPageNumber(e.MusicInfoOfPageModels.TotalPageNumber, e.MusicInfoOfPageModels.CurrentPageNumber);

                TbError.Visibility = Visibility.Collapsed;
                GdSong.Visibility = Visibility.Visible;
                
            }
            else
            {
                TbError.Text = e.ErrorInfo;
                GdSong.Visibility = Visibility.Collapsed;
                TbError.Visibility = Visibility.Visible;
            }
            Waiting.Visibility = Visibility.Collapsed;
            SpPageNumber.IsEnabled = true;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as FrameworkElement).Tag;
            if (id != null)
            {
                var musicViewModel = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(id));
                if(musicViewModel != null)
                {
                    var musicInfo = new MusicPlayInfoModel()
                    {
                        Id = musicViewModel.Id,
                        Name = musicViewModel.Name,
                        SingerName = musicViewModel.SingerName,                      
                        TimeDuration = musicViewModel.TimeDuration,
                        Uri = new Uri($"http://ws.stream.qqmusic.qq.com/C100{id}.m4a?fromtag=38", UriKind.Absolute),
                    };
                    MusicPlayStateChangedUtil.Invoke(musicInfo, true);                   
                }
            }
        }

        private void PageNumberBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button pageNumberBtn)
            {
                var clickPageNumber = Convert.ToInt32(pageNumberBtn.Content);
                if (_currentPageNumber.Equals(clickPageNumber)) return;
                
                Waiting.Visibility = Visibility.Visible;
                GdSong.Visibility = Visibility.Collapsed;
                TbError.Visibility = Visibility.Collapsed;
                SpPageNumber.IsEnabled = false;

                _currentPageNumber = clickPageNumber;
                MusicSearchPageNumberChangedUtil.InvokeFromQM(_currentPageNumber);
            }
        }

        #region Helpers

        private void InitPageNumber(int totalPageNumber,int currentPageNumber)
        {
            if (totalPageNumber < currentPageNumber || currentPageNumber <= 0) return;
            SpPageNumber.Children.Clear();
            const int MaxPageNumber = 6;

            if(totalPageNumber <= MaxPageNumber)
            {
                Enumerable.Range(1, totalPageNumber).ToList()
                    .ForEach(i => 
                    {
                        var btn = new JmTransparentButton() { Content = i };
                        btn.Click += PageNumberBtn_Click;
                        SpPageNumber.Children.Add(btn);
                    });
            }
            else if(currentPageNumber < MaxPageNumber - 1)
            {
                Enumerable.Range(1, MaxPageNumber).ToList()
                    .ForEach(i => 
                    {
                        var btn = new JmTransparentButton() { Content = i };
                        btn.Click += PageNumberBtn_Click;
                        SpPageNumber.Children.Add(btn);
                    });
                SpPageNumber.Children.Add(new TextBlock()
                {
                    Text = " … ",
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center
                });
                var lastBtn = new JmTransparentButton() { Content = totalPageNumber };
                lastBtn.Click += PageNumberBtn_Click;
                SpPageNumber.Children.Add(lastBtn);
            }
            else if(currentPageNumber > MaxPageNumber - 1 && totalPageNumber - currentPageNumber > MaxPageNumber - 1)
            {
                var fristBtn = new JmTransparentButton() { Content = 1 };
                fristBtn.Click += PageNumberBtn_Click;
                SpPageNumber.Children.Add(fristBtn);

                SpPageNumber.Children.Add(new TextBlock()
                {
                    Text = " … ",
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center
                });

                Enumerable.Range(currentPageNumber - 1, MaxPageNumber - 2).ToList()
                   .ForEach(i =>
                   {
                       var btn = new JmTransparentButton() { Content = i };
                       btn.Click += PageNumberBtn_Click;
                       SpPageNumber.Children.Add(btn);
                   });

                SpPageNumber.Children.Add(new TextBlock()
                {
                    Text = " … ",
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center
                });

                var lastBtn = new JmTransparentButton() { Content = totalPageNumber };
                lastBtn.Click += PageNumberBtn_Click;
                SpPageNumber.Children.Add(lastBtn);
            }
            else
            {
                var fristBtn = new JmTransparentButton() { Content = 1 };
                fristBtn.Click += PageNumberBtn_Click;
                SpPageNumber.Children.Add(fristBtn);

                SpPageNumber.Children.Add(new TextBlock()
                {
                    Text = " … ",
                    Foreground = new SolidColorBrush(Colors.White),
                    VerticalAlignment = VerticalAlignment.Center
                });

                Enumerable.Range(totalPageNumber - MaxPageNumber + 1, MaxPageNumber - 1).ToList()
                  .ForEach(i =>
                  {
                      var btn = new JmTransparentButton() { Content = i };
                      btn.Click += PageNumberBtn_Click;
                      SpPageNumber.Children.Add(btn);
                  });
            }

            foreach (var child in SpPageNumber.Children)
            {
                if (child is Button btn && btn.Content.Equals(currentPageNumber))
                {
                    btn.BorderBrush = btn.Foreground = new SolidColorBrush(Colors.SkyBlue);
                    break;
                }
            }
        }

        #endregion
    }
}
