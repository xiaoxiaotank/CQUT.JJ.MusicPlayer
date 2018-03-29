using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.Enums;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Search;
using CQUT.JJ.MusicPlayer.Models.JM.Common;
using CQUT.JJ.MusicPlayer.WCFService;
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
    /// JMSearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class JMSearchPage : Page
    {
        private static List<MusicInfoViewModel> _musicListViewModel = null;

        private static int _currentPageNumber = 1;

        private static TaskScheduler _syncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// 即将要播放的id ,TextBlock对象
        /// </summary>
        private static KeyValuePair<int, TextBlock> _nextPlayingTbObject;

        /// <summary>
        /// 正在播放的id,TextBlock对象
        /// </summary>
        private static KeyValuePair<int, TextBlock> _currentPlayingTbObject;

        private readonly ISearchService _searchService;

        public JMSearchPage()
        {
            InitializeComponent();

            _searchService = new SearchService();

            //页面信息发生变化
            MusicSearchInfoChangedUtil.JMSearchChangedEvent += JMSearchChangedEvent;
            //音乐播放状态更改
            if (_musicListViewModel == null)
                MusicPlayStateChangedUtil.JMusicPlayStateChangedEvent += JMusicPlayStateChanged;
            //音乐播放切换
            MusicPlaySwitchedUtil.MusicPlaySwitchedEvent += MusicSwitched;
        }

     


        #region 事件Handler

        private void JMSearchChangedEvent(object sender, MusicSearchInfoChangedArgs e)
        {
            if (e.IsSuccessed)
            {
                _musicListViewModel = new List<MusicInfoViewModel>();

                switch (e.PageResult.ResultType)
                {
                    case SearchType.Song:
                        var songs = (MusicSearchPageResult)e.PageResult;
                        songs.Results?.ToList().ForEach(r =>
                        {
                            _musicListViewModel.Add(new MusicInfoViewModel
                            {
                                Id = r.Id,
                                SingerId = r.SingerId,
                                AlbumId = r.AlbumId,
                                MusicName = r.Name,
                                SingerName = r.SingerName,
                                AlbumName = r.AlbumName,
                                Duration = r.Duration,
                                DurationDescription = r.Duration.GetMinuteAndSecondPart(),
                                FileUrl = r.FileUrl
                            });
                        });
                        break;
                }
                MusicList.ItemsSource = _musicListViewModel;
                InitPageNumber(e.PageResult.PageCount, e.PageResult.PageNumber);

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
            if(_musicListViewModel.Any())
                MusicList.ScrollIntoView(_musicListViewModel[0]);
            if (JMApp.CurrentPlayingMusicsInfo != null)
                JMApp.CurrentPlayingMusicsInfo.IsCurrentPlayingPage = false;
        }

        private void JMusicPlayStateChanged(object sender, MusicPlayStateChangedArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                ChangeMusicPlayBtnState(_nextPlayingTbObject.Value, e.IsToPlay);
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }


        #endregion



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Waiting.Visibility = Visibility.Visible;
            TbError.Visibility = Visibility.Collapsed;
            GdSong.Visibility = Visibility.Collapsed;
            NonNavPageDisplayedUtil.Invoke();

            MusicSearchInfoChangedUtil.InvokeFromJMRequest(SearchType.Song, 1);
        }



        #region 页码Helper
        /// <summary>
        /// 初始化页码
        /// </summary>
        /// <param name="totalPageNumber"></param>
        /// <param name="currentPageNumber"></param>
        private void InitPageNumber(int totalPageNumber, int currentPageNumber)
        {
            if (totalPageNumber < currentPageNumber || currentPageNumber <= 0) return;
            SpPageNumber.Children.Clear();
            const int MaxPageNumber = 6;


            if (currentPageNumber != 1)
            {
                var previousBtn = new JmTransparentButton() { Content = "<" };
                previousBtn.Click += PreviousPageBtn_Click;
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

            if (currentPageNumber != totalPageNumber)
            {
                var nextBtn = new JmTransparentButton() { Content = ">" };
                nextBtn.Click += NextPageBtn_Click;
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

        /// <summary>
        /// 更改页码
        /// </summary>
        /// <param name="targetPageNumber"></param>
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


        /// <summary>
        /// 页码点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageNumberBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button pageNumberBtn)
            {
                var clickPageNumber = Convert.ToInt32(pageNumberBtn.Content);
                if (_currentPageNumber.Equals(clickPageNumber)) return;

                ChangePageNumber(clickPageNumber);
            }
        }

        /// <summary>
        /// 上一页点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePageNumber(_currentPageNumber - 1);
        }

        /// <summary>
        /// 下一页点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePageNumber(_currentPageNumber + 1);
        }
        #endregion


        /// <summary>
        /// 音乐切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MusicSwitched(object sender, MusicPlaySwitchedEventArgs e)
        {
            if (JMApp.CurrentPlayingMusicsInfo != null)
            {
                //当前页
                if (JMApp.CurrentPlayingMusicsInfo.IsCurrentPlayingPage && _musicListViewModel != null)
                {
                    var currentPlayingObjIndex = _musicListViewModel.FindIndex(ml => ml == _musicListViewModel.SingleOrDefault(m => m.Id.Equals(_currentPlayingTbObject.Key)));
                    if (currentPlayingObjIndex >= 0)
                    {
                        int nextPlayingObjIndex = currentPlayingObjIndex;
                        switch (e.MusicPlayMode)
                        {
                            case MusicPlayMode.List:
                                nextPlayingObjIndex = e.IsDescending == true ? currentPlayingObjIndex + 1 : currentPlayingObjIndex - 1;
                                if (nextPlayingObjIndex >= _musicListViewModel.Count)
                                    nextPlayingObjIndex = 0;
                                else if (nextPlayingObjIndex < 0)
                                    nextPlayingObjIndex = _musicListViewModel.Count - 1;
                                break;
                            case MusicPlayMode.Order:
                                nextPlayingObjIndex = e.IsDescending == true ? currentPlayingObjIndex + 1 : currentPlayingObjIndex - 1;
                                if (nextPlayingObjIndex >= _musicListViewModel.Count || nextPlayingObjIndex < 0) return;
                                break;
                            case MusicPlayMode.Random:
                                nextPlayingObjIndex = new Random().Next(0, _musicListViewModel.Count);
                                break;
                            case MusicPlayMode.Single:
                                break;
                        }

                        var nextPlayingObj = _musicListViewModel[nextPlayingObjIndex];
                        if (MusicList.ItemContainerGenerator.ContainerFromItem(nextPlayingObj) is JmListViewItem lvi
                            && lvi.GetChildObjectByName<Button>("BtnPlay")?.Content is TextBlock tb)
                        {
                            ChangeMusicPlayState(nextPlayingObj, tb);
                            ChangeMusicActivatedState(nextPlayingObj);
                            JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusicId = nextPlayingObj.Id;
                            MusicList.ScrollIntoView(nextPlayingObj);
                        }
                    }
                }
                //不是当前页
                else if (!JMApp.CurrentPlayingMusicsInfo.IsCurrentPlayingPage && JMApp.CurrentPlayingMusicsInfo != null)
                {
                    var currentPlayingMusicList = JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusics.ToList();
                    var currentPlayingObjIndex = currentPlayingMusicList.FindIndex(m => m.Id.Equals(JMApp.CurrentPlayingMusicsInfo.CurrentQMPlayingMusicId));
                    if (currentPlayingObjIndex >= 0)
                    {
                        int nextPlayingObjIndex = currentPlayingObjIndex;
                        switch (e.MusicPlayMode)
                        {
                            case MusicPlayMode.List:
                                nextPlayingObjIndex = e.IsDescending == true ? currentPlayingObjIndex + 1 : currentPlayingObjIndex - 1;
                                if (nextPlayingObjIndex >= currentPlayingMusicList.Count)
                                    nextPlayingObjIndex = 0;
                                else if (nextPlayingObjIndex < 0)
                                    nextPlayingObjIndex = currentPlayingMusicList.Count - 1;
                                break;
                            case MusicPlayMode.Order:
                                nextPlayingObjIndex = e.IsDescending == true ? currentPlayingObjIndex + 1 : currentPlayingObjIndex - 1;
                                if (nextPlayingObjIndex >= currentPlayingMusicList.Count || nextPlayingObjIndex < 0) return;
                                break;
                            case MusicPlayMode.Random:
                                nextPlayingObjIndex = new Random().Next(0, currentPlayingMusicList.Count);
                                break;
                            case MusicPlayMode.Single:
                                break;
                        }

                        var nextPlayingObj = currentPlayingMusicList[nextPlayingObjIndex];
                        ChangeMusicPlayState(new MusicInfoViewModel()
                        {
                            Id = nextPlayingObj.Id,
                            MusicName = nextPlayingObj.Name,
                            SingerName = nextPlayingObj.SingerName,
                            Duration = nextPlayingObj.Duration,
                            DurationDescription = nextPlayingObj.Duration.GetMinuteAndSecondPart(),
                            FileUrl = nextPlayingObj.FileUrl
                        }, null);
                        JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusicId = nextPlayingObj.Id;
                    }
                }
            }
        }
        
        /// <summary>
        /// 播放按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (sender is JmTransparentButton btn && btn.Content is TextBlock tb)
            {
                var id = btn.Tag;
                if (id != null)
                {
                    var musicViewModel = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(id));
                    ChangeMusicActivatedState(musicViewModel);
                    ChangeMusicPlayState(musicViewModel, tb);

                    JMApp.CurrentPlayingMusicsInfo = new CurrentPlayingMusicsInfo()
                    {
                        CurrentPlayingMusics = _musicListViewModel.Select(m => new MusicModel()
                        {
                            Id = m.Id,
                            Name = m.MusicName,
                            SingerName = m.SingerName,
                            AlbumName = m.AlbumName,
                            Duration = m.Duration,
                            FileUrl = m.FileUrl
                        }),
                        IsCurrentPlayingPage = true,
                        CurrentPlayingMusicId = musicViewModel.Id
                    };
                }
            }
        }

        /// <summary>
        /// QM音乐播放状态改变（从播放菜单发起，激发该页面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QMusicPlayStateChanged(object sender, QMusicPlayStateChangedArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                ChangeMusicPlayBtnState(_nextPlayingTbObject.Value, e.IsToPlay);
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }

        /// <summary>
        /// 更改音乐播放状态
        /// </summary>
        /// <param name="musicViewModel"></param>
        /// <param name="tb"></param>
        private void ChangeMusicPlayState(MusicInfoViewModel model, TextBlock tb)
        {
            if (model != null)
            {
                _nextPlayingTbObject = new KeyValuePair<int, TextBlock>(model.Id, tb);
                var isToPlay = true;
                if (tb != null)
                    isToPlay = !tb.Text.Equals("\ue69d");

                Task.Factory.StartNew(() =>
                {
                    var musicInfo = new MusicModel()
                    {
                        Id = model.Id,
                        Name = model.MusicName,
                        SingerName = model.SingerName,
                        Duration = model.Duration,
                        FileUri = new Uri(model.FileUrl,UriKind.Relative)
                    };

                    MusicPlayStateChangedUtil.InvokeFromJM(musicInfo, isToPlay);
                });
            }
        }

        /// <summary>
        /// 更改音乐播放按钮状态
        /// </summary>
        /// <param name="tb"></param>
        private void ChangeMusicPlayBtnState(TextBlock tb, bool isToPlay)
        {
            if (tb != null)
            {
                if (isToPlay)
                    tb.Text = "\ue69d";
                else
                    tb.Text = "\ue774";
                if (_currentPlayingTbObject.Value != null && !_currentPlayingTbObject.Value.Equals(tb))
                    _currentPlayingTbObject.Value.Text = "\ue774";
                _currentPlayingTbObject = _nextPlayingTbObject;
            }
        }

        /// <summary>
        /// 更改激活状态
        /// </summary>
        /// <param name="lvi"></param>
        private void ChangeMusicActivatedState(MusicInfoViewModel model)
        {
            RemoveCurrentPlayingMusicActivatedState();
            if (model != null)
                model.IsActivated = true;
        }

        /// <summary>
        /// 移除当前播放音乐激活状态
        /// </summary>
        private void RemoveCurrentPlayingMusicActivatedState()
        {
            if (JMApp.CurrentPlayingMusicsInfo?.CurrentPlayingMusicId != null && _musicListViewModel != null)
            {
                var currentItem = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusicId));
                if (currentItem != null)
                    currentItem.IsActivated = false;
            }
        }

    }
}
