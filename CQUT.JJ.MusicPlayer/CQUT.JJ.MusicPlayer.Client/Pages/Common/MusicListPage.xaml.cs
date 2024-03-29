﻿using CQUT.JJ.MusicPlayer.Client.Pages.Common;
using CQUT.JJ.MusicPlayer.Client.UserControls;
using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.Enums;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Client.ViewModels.MusicPlayerMenu;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Controls.Enums.JmBubbleMessageBox;
using CQUT.JJ.MusicPlayer.Controls.Enums.JMMessageBox;
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
using System.Windows.Controls.Primitives;
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
    public partial class MusicListPage : Page
    {
        private static List<MusicViewModel> _musicListViewModel = null;

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

        private static bool _isToPlay = false;

        private readonly ISearchService _searchService;

        private List<JmMenuItem> _userMusicList = new List<JmMenuItem>();

        private bool _isInited = false;

        public MusicListPage()
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
            //右键菜单点击开始播放时事件
            ContextMenuStartPlayMusicUtil.ContextMenuStartPlayMusicEvent += ContextMenuStartPlayMusicEvent;

        }


        #region 事件Handler

        private async void JMSearchChangedEvent(object sender, MusicSearchInfoChangedArgs e)
        {
            if (!IsVisible) return;
            if (e.IsSuccessed)
            {
                _musicListViewModel = new List<MusicViewModel>();
                await Task.Factory.StartNew(() =>
                {
                    
                    //无音乐
                    if (e.PageResult?.ResultType == null)
                    {
                        TipNonMusicInfo();
                        return;
                    }
                    switch (e.PageResult.ResultType)
                    {
                        case MusicRequestType.Song:
                            var songs = (MusicSearchPageResult)e.PageResult;
                            //无音乐
                            if (songs.Results?.Any() != true)
                            {
                                TipNonMusicInfo();
                                return;
                            }
                            songs.Results?.ToList().ForEach(r =>
                            {
                                var model = new MusicViewModel
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
                                };
                                _musicListViewModel.Add(model);
                            });
                            break;
                        default:
                            return;
                    }
                });

                await Dispatcher.InvokeAsync(() =>
                {
                    MusicList.ItemsSource = _musicListViewModel;
                    //确保只绑定一次
                    MusicList.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
                    MusicList.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

                    InitPageNumber(e.PageResult.PageCount, e.PageResult.PageNumber);

                    TbError.Visibility = Visibility.Collapsed;
                    GdSong.Visibility = Visibility.Visible;
                });               
            }
            else
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    TbError.Text = e.ErrorInfo;
                    GdSong.Visibility = Visibility.Collapsed;
                    TbError.Visibility = Visibility.Visible;
                });
                
            }
            await Dispatcher.InvokeAsync(() =>
            {
                TbInfo.Visibility
                = Waiting.Visibility
                = Visibility.Collapsed;
                SpPageNumber.IsEnabled = true;
                if (_musicListViewModel != null && _musicListViewModel.Any())
                    MusicList.ScrollIntoView(_musicListViewModel[0]);
            });
        }

        private void ContextMenuStartPlayMusicEvent(object sender, EventArgs e)
        {
            StartPlayMusic();
        }


        /// <summary>
        /// 展示无音乐资源时的提示
        /// </summary>
        private void TipNonMusicInfo()
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (IsVisible)
                {
                    TbError.Visibility
                        = GdSong.Visibility
                        = Waiting.Visibility
                        = Visibility.Collapsed;
                    TbInfo.Visibility = Visibility.Visible;
                }
            });
        }

        
        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (MusicList.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated && !_isInited)
            {
                var currentPlayingMusic = _musicListViewModel.SingleOrDefault(m => m.Id == JMApp.CurrentPlayingMusicsInfo?.CurrentPlayingMusic?.Id);
                if (MusicList.ItemContainerGenerator.ContainerFromItem(currentPlayingMusic) is JmListViewItem lvi
                    && lvi.GetChildObjectByName<Button>("BtnPlay")?.Content is TextBlock tb)
                {
                    ChangeMusicPlayBtnState(tb, _isToPlay);
                    ChangeMusicActivatedState(currentPlayingMusic);
                    _currentPlayingTbObject = new KeyValuePair<int, TextBlock>(currentPlayingMusic.Id, tb);
                    JMApp.CurrentPlayingMusicsInfo.IsCurrentPlayingPage = true;
                    JMApp.CurrentPlayingMusicsInfo.PlayingListMusics = _musicListViewModel.Select(m => new MusicModel()
                    {
                        Id = m.Id,
                        Name = m.MusicName,
                        SingerName = m.SingerName,
                        AlbumName = m.AlbumName,
                        Duration = m.Duration,
                        FileUrl = m.FileUrl
                    });
                    _isInited = true;
                }
                else
                {
                    if(JMApp.CurrentPlayingMusicsInfo != null)
                        JMApp.CurrentPlayingMusicsInfo.IsCurrentPlayingPage = false;
                }
            }
        }

        private void JMusicPlayStateChanged(object sender, MusicPlayStateChangedArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                _isToPlay = e.IsToPlay;
                ChangeMusicPlayBtnState(_nextPlayingTbObject.Value, e.IsToPlay);
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }


        #endregion



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Waiting.Visibility = Visibility.Visible;
            TbError.Visibility = Visibility.Collapsed;
            GdSong.Visibility = Visibility.Collapsed;
            _isInited = false;
            PageLoadedUtil.InvokeFromMusicListPageLoaded();
        }


        #region 页码Helper
        /// <summary>
        /// 初始化页码
        /// </summary>
        /// <param name="totalPageNumber"></param>
        /// <param name="currentPageNumber"></param>
        private void InitPageNumber(int totalPageNumber, int currentPageNumber)
        {
            SpPageNumber.Visibility = Visibility.Collapsed;
            SpPageNumber.Children.Clear();
            if (totalPageNumber < currentPageNumber || currentPageNumber <= 0 || totalPageNumber == 1) return;

            SpPageNumber.Visibility = Visibility.Visible;
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
            MusicSearchInfoChangedUtil.InvokeFromJMRequest(MusicRequestType.Song,_currentPageNumber);
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
                if (_musicListViewModel != null && IsVisible)
                {
                    var currentPlayingObjIndex = _musicListViewModel.FindIndex(m => m.Id.Equals(_currentPlayingTbObject.Key));
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
                                do
                                {
                                    nextPlayingObjIndex = new Random().Next(0, _musicListViewModel.Count);
                                } while (nextPlayingObjIndex == currentPlayingObjIndex && _musicListViewModel.Count > 1);

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
                            JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic = new MusicModel()
                            {
                                Id = nextPlayingObj.Id,
                                SingerId = nextPlayingObj.SingerId,
                                AlbumId = nextPlayingObj.AlbumId,
                                Name = nextPlayingObj.MusicName,
                                SingerName = nextPlayingObj.SingerName,
                                AlbumName = nextPlayingObj.AlbumName,
                                FileUri = new Uri(nextPlayingObj.FileUrl, UriKind.Relative),
                                Duration = nextPlayingObj.Duration
                            };
                            MusicList.ScrollIntoView(nextPlayingObj);
                        }
                    }
                }
                //不是当前页
                else
                {
                    var isCurrentPage = false;
                    var frame = App.Current.MainWindow.FindName("FMusicPage") as Frame;
                    if(frame.Content is Page visiblePage)
                    {
                        var frames = visiblePage.GetAllChildObject<Frame>();
                        var uri = new Uri("Pages/Common/MusicListPage.xaml", UriKind.Relative);
                        isCurrentPage = frames.Any(f => f.Source.Equals(uri));
                    }

                    if (!(JMApp.CurrentPlayingMusicsInfo.IsCurrentPlayingPage && isCurrentPage) && JMApp.CurrentPlayingMusicsInfo != null)
                    {
                        var currentPlayingMusicList = JMApp.CurrentPlayingMusicsInfo.PlayingListMusics.ToList();
                        var currentPlayingObjIndex = currentPlayingMusicList.FindIndex(m => m.Id.Equals(JMApp.CurrentPlayingMusicsInfo?.CurrentPlayingMusic?.Id));
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
                                    do
                                    {
                                        nextPlayingObjIndex = new Random().Next(0, currentPlayingMusicList.Count);
                                    } while (nextPlayingObjIndex == currentPlayingObjIndex && currentPlayingMusicList.Count > 1);
                                    break;
                                case MusicPlayMode.Single:
                                    break;
                            }

                            var nextPlayingObj = currentPlayingMusicList[nextPlayingObjIndex];
                            ChangeMusicPlayState(new MusicViewModel()
                            {
                                Id = nextPlayingObj.Id,
                                MusicName = nextPlayingObj.Name,
                                SingerName = nextPlayingObj.SingerName,
                                Duration = nextPlayingObj.Duration,
                                DurationDescription = nextPlayingObj.Duration.GetMinuteAndSecondPart(),
                                FileUrl = nextPlayingObj.FileUrl ?? nextPlayingObj.FileUri.OriginalString
                            }, null);
                            JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic = nextPlayingObj;
                        }
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
                var id = Convert.ToInt32(btn.Tag);
                PlayMusic(id, tb);
            }
        }

        /// <summary>
        /// 播放全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlayAll_Click(object sender, RoutedEventArgs e)
        {
            StartPlayMusic();
        }




        /// <summary>
        /// 左击歌手名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSinger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(sender is TextBlock tb)
            {
                var singerId = Convert.ToInt32(tb.Tag);
                var singerInfoPage = new SingerInfoPage(singerId);

                var parentFrame = this.ParentFrame();
                NavigationService.GetNavigationService(parentFrame).Navigate(singerInfoPage);
            }
        }

        /// <summary>
        /// 左击专辑名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbAlbum_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb)
            {
                var albumId = Convert.ToInt32(tb.Tag);
                var ablumPage = new AlbumInfoPage(albumId);

                var parentFrame = this.ParentFrame();
                NavigationService.GetNavigationService(parentFrame).Navigate(ablumPage);
            }
        }


        /// <summary>
        /// 下载按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (sender is JmTransparentButton btn && btn.Content is TextBlock tb)
            {
                var id = btn.Tag;
                if (id != null)
                {
                    var musicViewModel = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(id));
                    var uri = new Uri(musicViewModel.FileUrl,UriKind.Relative);
                    var musicName = musicViewModel.MusicName + System.IO.Path.GetExtension(musicViewModel.FileUrl);
                    var singerName = musicViewModel.SingerName;
                    await FileUtil.DownLoadMusicsAsync(uri, musicName, singerName);              
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
        private void ChangeMusicPlayState(MusicViewModel model, TextBlock tb)
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
        private void ChangeMusicActivatedState(MusicViewModel model)
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
            if (JMApp.CurrentPlayingMusicsInfo?.CurrentPlayingMusic?.Id != null && _musicListViewModel != null)
            {
                var currentItem = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic.Id));
                if (currentItem != null)
                    currentItem.IsActivated = false;
            }
        }

     
        private void BtnDowload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBatchOperation_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 开始播放音乐
        /// </summary>
        private void StartPlayMusic()
        {
            var nextMusic = _musicListViewModel?.FirstOrDefault();
            if (nextMusic != null)
            {
                if (MusicList.ItemContainerGenerator.ContainerFromItem(nextMusic) is JmListViewItem lvi
                    && lvi.GetChildObjectByName<Button>("BtnPlay")?.Content is TextBlock tb)
                {                    
                    PlayMusic(nextMusic.Id, tb);
                }
            }
        }

        private void PlayMusic(int id,TextBlock tb)
        {
            var musicViewModel = _musicListViewModel.SingleOrDefault(m => m.Id.Equals(id));
            if(MusicList.ItemContainerGenerator.ContainerFromItem(musicViewModel) is JmListViewItem viewItem)
            {
                viewItem.IsSelected = true;
            }
            ChangeMusicActivatedState(musicViewModel);
            ChangeMusicPlayState(musicViewModel, tb);

            JMApp.CurrentPlayingMusicsInfo = new CurrentPlayingMusicsInfo()
            {
                PlayingListMusics = _musicListViewModel.Select(m => new MusicModel()
                {
                    Id = m.Id,
                    Name = m.MusicName,
                    SingerName = m.SingerName,
                    AlbumName = m.AlbumName,
                    Duration = m.Duration,
                    FileUrl = m.FileUrl
                }),
                IsCurrentPlayingPage = true,
            };

            if (JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic?.Id.Equals(id) != true)
            {
                JMApp.CurrentPlayingMusicsInfo.CurrentPlayingMusic = new MusicModel()
                {
                    Id = musicViewModel.Id,
                    SingerId = musicViewModel.SingerId,
                    AlbumId = musicViewModel.AlbumId,
                    Name = musicViewModel.MusicName,
                    SingerName = musicViewModel.SingerName,
                    AlbumName = musicViewModel.AlbumName,
                    FileUri = new Uri(musicViewModel.FileUrl, UriKind.Relative),
                    Duration = musicViewModel.Duration
                };
            }
        }

        private void CtxPlayNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CtxPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuPlayQueue_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = GetMusicViewModeByMenuObject(sender);
            if (viewModel != null)
            {
                if (MusicPlayerMenu.MusicPlayListViewModel?.MusicPlayList?.Any(m => m.Id == viewModel.Id.ToString()) == true)
                    return;

                MusicPlayerMenu.MusicPlayListViewModel.MusicPlayList.Add(new MusicOfPlayListViewModel()
                {
                    Id = viewModel.Id.ToString(),
                    Name = viewModel.MusicName,
                    SingerName = viewModel.SingerName,
                    TimeDuration = viewModel.DurationDescription
                });
            }
            
        }

        private MusicViewModel GetMusicViewModeByMenuObject(object sender)
        {
            if (sender is JmMenuItem menuItem
                && menuItem.TemplatedParent is ContentPresenter content
                && content.Content is MusicViewModel viewModel)
                return viewModel;
            return null;
        }

        private void MenuILike_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = GetMusicViewModeByMenuObject(sender);
            if (viewModel != null)
            {
                MusicPlayerMenu.MusicPlayerMenuUserControl.ToggleUserLike(viewModel.Id, MusicRequestType.Song,true);
            }
        }

        private void MenuTestListeningList_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = GetMusicViewModeByMenuObject(sender);
            if(viewModel != null && !JMApp.ListeningTestList.Any(m => m.Id == viewModel.Id))
            {
                JMApp.ListeningTestList.Add(new MusicModel()
                {
                    Id = viewModel.Id,
                    SingerId = viewModel.SingerId,
                    AlbumId = viewModel.AlbumId,
                    Name = viewModel.AlbumName,
                    SingerName = viewModel.SingerName,
                    AlbumName = viewModel.AlbumName,
                    Duration = viewModel.Duration,
                    FileUrl = viewModel.FileUrl
                });
            }
        }

        private void BtnAddTo_Click(object sender, RoutedEventArgs e)
        {
            if(sender is JmTransparentButton btn && btn.Parent is StackPanel sp)
            {
                var popup = sp.FindName("PopAddTo") as Popup;
               
                if (sp.FindName("MenuAddTo") is Menu menu)
                {
                    _userMusicList?.ForEach(item => menu.Items.Remove(item));
                    if (App.User != null 
                        && popup.TemplatedParent is ContentPresenter content
                        && content.Content is MusicViewModel viewModel)
                    {
                        #region 初始化菜单项

                        var userMusicList = UserMusicNavigtion.UserMusicListService.GetUserMusicListByUserId(App.User.Id).Reverse().ToList();
                        foreach (var item in userMusicList)
                        {
                            if (UserMusicNavigtion.UserMusicListService.IsExistOfUserMusicList(item.Id, viewModel.Id, MusicRequestType.Song))
                                continue;

                            var result = new JmMenuItem()
                            {
                                Header = item.Name,
                                Tag = new { item.Id, item.UserId, MusicId = viewModel.Id },
                            };
                            result.Click += UserMusicList_Click;
                            menu.Items.Add(result);
                            _userMusicList.Add(result);
                        }
                      
                        #endregion

                        if (MusicList.ItemContainerGenerator.ContainerFromItem(viewModel) is JmListViewItem viewItem)
                        {
                            viewItem.IsSelected = true;
                        }
                        if (MusicPlayerMenu.MusicService.IsUserLike(App.User.Id, viewModel.Id, MusicRequestType.Song))
                        {
                            (popup.FindName("MenuILike") as JmMenuItem).IsEnabled = false;
                        }
                        if(JMApp.ListeningTestList.Any(m => m.Id == viewModel.Id))
                        {
                            (popup.FindName("MenuTestListeningList") as JmMenuItem).IsEnabled = false;
                        }
                    }
                } 
               
                popup.IsOpen = !popup.IsOpen;
            }
        }

        private void UserMusicList_Click(object sender, RoutedEventArgs e)
        {
            if(sender is JmMenuItem menuItem)
            {
                var musicListId = menuItem.Tag.GetPropertyValue<int>("Id");
                var userId = menuItem.Tag.GetPropertyValue<int>("UserId");
                var musicId = menuItem.Tag.GetPropertyValue<int>("MusicId");

                try
                {
                    UserMusicNavigtion.UserMusicListService.AddToUserMusicList(userId, musicId, musicListId, MusicRequestType.Song);
                    JmBubbleMessageBox.Show($"添加到{menuItem.Header}成功", JmBubbleMessageBoxType.Success);
                }
                catch(Exception ex)
                {
                    JmBubbleMessageBox.Show(ex.Message, JmBubbleMessageBoxType.Error);
                }
            }
        }

        private void MusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is JmListView view)
            {
                foreach (var item in e.RemovedItems)
                {
                    if (view.ItemContainerGenerator.ContainerFromItem(item) is JmListViewItem lastViewItem)
                    {
                        var sps = lastViewItem.GetAllChildObject<StackPanel>();
                        var sp = sps.SingleOrDefault(s => s.Name == "SpOptions");
                        if (sp != null)
                        {
                            sp.Visibility = Visibility.Hidden;
                            var column = lastViewItem.FindVisualChild<GridViewRowPresenter>().Columns[0];
                            
                        }                                
                    }
                }
                if (view.ItemContainerGenerator.ContainerFromItem(view.SelectedItem) is JmListViewItem viewItem)
                {
                    var sps = viewItem.GetAllChildObject<StackPanel>();
                    var sp = sps.SingleOrDefault(s => s.Name == "SpOptions");
                    if (sp != null)
                        sp.Visibility = Visibility.Visible;                    
                }
            }
            
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if(sender is Panel panel 
                && panel.TemplatedParent is ContentPresenter content
                && content.Parent is GridViewRowPresenter gridViewRow
                && gridViewRow.TemplatedParent is JmListViewItem viewItem)
            {
                var sp = viewItem.GetAllChildObject<StackPanel>().SingleOrDefault(s => s.Name == "SpOptions"); ;
                sp.Visibility = Visibility.Visible;
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Panel panel
                && panel.TemplatedParent is ContentPresenter content
                && content.Parent is GridViewRowPresenter gridViewRow
                && gridViewRow.TemplatedParent is JmListViewItem viewItem)
            {
                if (MusicList.SelectedItem == viewItem.Content) return;
                var sp = viewItem.GetAllChildObject<StackPanel>().SingleOrDefault(s => s.Name == "SpOptions"); ;
                sp.Visibility = Visibility.Hidden;
            }
        }

        private void PopAddTo_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if(sender is Popup popup)
            {
                if (e.Source is Menu menu)
                    return;

                popup.IsOpen = false;
            }
        }

        private void BtnAddTo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is JmTransparentButton btn && btn.Parent is StackPanel sp)
            {
                var popup = sp.FindName("PopAddTo") as Popup;
                popup.IsOpen = false;
            }
        }

        private void CtxDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
