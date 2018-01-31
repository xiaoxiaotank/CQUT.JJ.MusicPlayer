using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private static TaskScheduler _syncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// 即将要播放的TextBlock对象
        /// </summary>
        private static TextBlock _nextPlayingTbObject = null;

        /// <summary>
        /// 正在播放的TextBlock对象
        /// </summary>
        private static TextBlock _currentPlayingTbObject = null;

        public SearchPage()
        {
            InitializeComponent();
            MusicSearchInfoChangedUtil.QMSearchChangedEvent += MusicSearchInfoChangedUtil_QMSearchChangedEvent;
            MusicPlayStateChangedUtil.QMusicPlayStateChangedEvent += QMusicPlayStateChanged;
        }

    

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Waiting.Visibility = Visibility.Visible;
            TbError.Visibility = Visibility.Collapsed;
            GdSong.Visibility = Visibility.Collapsed;
            NonNavPageDisplayedUtil.Invoke();

            MusicSearchInfoChangedUtil.InvokeFromQMRequest(1);
        }

        private void MusicSearchInfoChangedUtil_QMSearchChangedEvent(object sender, MusicSearchInfoChangedArgs e)
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
                            TimeDuration = m.TimeDuration,
                            SourcePath = m.SourcePath
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
            if(sender is JmTransparentButton btn && btn.Content is TextBlock tb)
            {
                var id = btn.Tag;
                if (id != null)
                {
                    var musicViewModel = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(id));
                    if (musicViewModel != null)
                    {
                        _nextPlayingTbObject = tb;

                        Task.Factory.StartNew(() =>
                        {
                            var htmlWeb = new HtmlWeb() { BrowserTimeout = TimeSpan.FromSeconds(10) };
                            var doc = htmlWeb.Load(musicViewModel.SourcePath);
                            var photoUrl = doc.DocumentNode.SelectNodes("//img[@class='data__photo']")?.FirstOrDefault()?.Attributes["src"].Value?.ToHttpUrl();

                            var musicInfo = new QMPlayInfoModel()
                            {
                                Id = musicViewModel.Id,
                                Name = musicViewModel.Name,
                                SingerName = musicViewModel.SingerName,
                                TimeDuration = musicViewModel.TimeDuration,
                                Uri = new Uri($"http://ws.stream.qqmusic.qq.com/C100{id}.m4a?fromtag=38", UriKind.Absolute),
                                PhotoUri = photoUrl == null ? null : new Uri(photoUrl)
                            };

                            MusicPlayStateChangedUtil.InvokeFromQM(musicInfo, true);
                        });
                        
                    }
                }
            }
        }

        private void QMusicPlayStateChanged(object sender, QMusicPlayStateChangedArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                ChangeMusicPlayBtnState(_nextPlayingTbObject);
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }

        private void PageNumberBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button pageNumberBtn)
            {
                var clickPageNumber = Convert.ToInt32(pageNumberBtn.Content);
                if (_currentPageNumber.Equals(clickPageNumber)) return;

                ChangePageNumber(clickPageNumber);
            }
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePageNumber(_currentPageNumber - 1);
        }


        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePageNumber(_currentPageNumber + 1);
        }

        #region Helpers

        private void InitPageNumber(int totalPageNumber,int currentPageNumber)
        {
            if (totalPageNumber < currentPageNumber || currentPageNumber <= 0) return;
            SpPageNumber.Children.Clear();
            const int MaxPageNumber = 6;

            
            if(currentPageNumber != 1)
            {
                var previousBtn = new JmTransparentButton() { Content = "<" };
                previousBtn.Click += PreviousBtn_Click;
                SpPageNumber.Children.Add(previousBtn);
            }
                

            #region 页码显示规则
            if (totalPageNumber <= MaxPageNumber)
            {
                Enumerable.Range(1, totalPageNumber).ToList()
                    .ForEach(i =>
                    {
                        var btn = new JmTransparentButton() { Content = i };
                        btn.Click += PageNumberBtn_Click;
                        SpPageNumber.Children.Add(btn);
                    });
            }
            else if (currentPageNumber < MaxPageNumber - 1)
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
            else if (currentPageNumber > MaxPageNumber - 1 && totalPageNumber - currentPageNumber > MaxPageNumber - 3)
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

                Enumerable.Range(totalPageNumber - MaxPageNumber + 1, MaxPageNumber).ToList()
                    .ForEach(i =>
                    {
                        var btn = new JmTransparentButton() { Content = i };
                        btn.Click += PageNumberBtn_Click;
                        SpPageNumber.Children.Add(btn);
                    });
            } 
            #endregion

            if(currentPageNumber != totalPageNumber)
            {
                var nextBtn = new JmTransparentButton() { Content = ">" };
                nextBtn.Click += NextBtn_Click;
                SpPageNumber.Children.Add(nextBtn);
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

        private void ChangeMusicPlayBtnState(TextBlock tb)
        {
            if (tb.Text.Equals("\ue774"))
                tb.Text = "\ue69d";
            else
                tb.Text = "\ue774";
            if(_currentPlayingTbObject != null && !_currentPlayingTbObject.Equals(tb))
                _currentPlayingTbObject.Text = "\ue774";
            _currentPlayingTbObject = tb;
        }

        private void ChangePageNumber(int targetPageNumber)
        {
            if (targetPageNumber < 1) return;
            Waiting.Visibility = Visibility.Visible;
            GdSong.Visibility = Visibility.Collapsed;
            TbError.Visibility = Visibility.Collapsed;
            SpPageNumber.IsEnabled = false;

            _currentPageNumber = targetPageNumber;
            MusicSearchInfoChangedUtil.InvokeFromQMRequest(_currentPageNumber);
        }

        #endregion
    }
}
