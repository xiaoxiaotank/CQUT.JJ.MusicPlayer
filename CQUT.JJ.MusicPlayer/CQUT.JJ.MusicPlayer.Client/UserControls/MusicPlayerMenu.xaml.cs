using CQUT.JJ.MusicPlayer.Client.Converters;
using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.Enums;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
using CQUT.JJ.MusicPlayer.Client.ViewModels.MusicPlayerMenu;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Controls.Enums.JMMessageBox;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.WCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// MusicPlayerMenu.xaml 的交互逻辑
    /// </summary>
    public partial class MusicPlayerMenu : UserControl
    {
        #region 变量

        #region 字段

        public static readonly IMusicService MusicService;
        /// <summary>
        /// 定时更新歌曲进度
        /// </summary>
        private static readonly DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.1) };
        /// <summary>
        /// 默认照片Uri
        /// </summary>
        private static readonly Uri _defaultPhotoUri = new Uri("/Asserts/Images/DefaultMusicHeader.png", UriKind.Relative);
        /// <summary>
        /// 默认歌曲路径
        /// </summary>
        private static readonly Uri _defaultMusicUri = new Uri("Asserts/Musics/好想再爱你.mp3", UriKind.Relative);
        /// <summary>
        /// 非静音时音量，用来记录静音前的音量，以便于恢复
        /// </summary>
        private static double _notMuteVolume = 0;
        /// <summary>
        /// 播放器
        /// </summary>
        private static MediaPlayer _mediaPlayer = new MediaPlayer() { Volume = 1 };
        /// <summary>
        /// 是否正在播放
        /// </summary>
        private static bool _isPlaying = false;
        /// <summary>
        /// 当前播放歌曲打开是否失败
        /// </summary>
        private static bool _isOpenFailedOfCurrentPlayingMusic = false;
        /// <summary>
        /// 音乐源类型
        /// </summary>
        private static MusicSourceType _musicSourceType = MusicSourceType.JM;
        /// <summary>
        /// 播放菜单ViewModel
        /// </summary>
        private static MusicPlayerMenuViewModel _musicPlayerMenuViewModel = new MusicPlayerMenuViewModel() { PhotoUri = _defaultPhotoUri };
        /// <summary>
        /// 音乐播放列表ViewModel
        /// </summary>
        public static MusicPlayListViewModel MusicPlayListViewModel = new MusicPlayListViewModel();

        private static TaskScheduler _syncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        #endregion

        private static MusicPlayerMenu _musicPlayerMenu = null;

        public static MusicPlayerMenu MusicPlayerMenuUserControl
        {
            get
            {
                if (_musicPlayerMenu == null)
                    _musicPlayerMenu = new MusicPlayerMenu();
                return _musicPlayerMenu;
            }
        }

        #endregion


        #region 构造函数

        static MusicPlayerMenu()
        {
            MusicService = new MusicService();
        }

        private MusicPlayerMenu()
        {

            _mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            _mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;

            MusicPlayStateChangedUtil.QMusicPlayStateChangedEvent += QMusicPlayStateChanged;
            MusicPlayStateChangedUtil.JMusicPlayStateChangedEvent += JMusicPlayStateChanged;

            UserStateChangedUtil.UserStateChangedEvent += UserStateChanged;

            _timer.Tick += Timer_Tick;

            InitializeComponent();

            SetBindingOfMediaPlayer();
            SetBindingOfMusicList();
        }

        #endregion


        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitVolumeState();
            InitMusicMenuInfo();
        }

        /// <summary>
        /// 音乐播放状态发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JMusicPlayStateChanged(object sender, MusicPlayStateChangedArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                //进行播放并且是属于（当前无播放源、不是同一首歌、播放失败）之一的情况 
                if (e.IsToPlay && e.IsNeedRefresh
                    && (_mediaPlayer.Source == null || !_mediaPlayer.Source.Equals(e.MusicInfo.FileUri) || _isOpenFailedOfCurrentPlayingMusic))
                {
                    _isPlaying = false;
                    PlayNewMusic(e.MusicInfo.FileUri);
                    _musicPlayerMenuViewModel.Id = e.MusicInfo.Id;
                    _musicPlayerMenuViewModel.MusicName = e.MusicInfo.Name;
                    _musicPlayerMenuViewModel.SingerName = e.MusicInfo.SingerName;
                    /////////////////////////TODO 封面图
                    _musicPlayerMenuViewModel.PhotoUri =  _defaultPhotoUri;
                    _musicPlayerMenuViewModel.FileUri = e.MusicInfo.FileUri;
                    _musicSourceType = MusicSourceType.JM;
                    InitILikeState();

                    if (MusicPlayListViewModel?.MusicPlayList != null)
                    {
                        var music = MusicPlayListViewModel.MusicPlayList.SingleOrDefault(m => m.Id.Equals(e.MusicInfo.Id));
                        if (music == null)
                        {
                            var newMusic = new MusicOfPlayListViewModel()
                            {
                                Id = e.MusicInfo.Id.ToString(),
                                Name = e.MusicInfo.Name,
                                SingerName = e.MusicInfo.SingerName,
                                TimeDuration = e.MusicInfo.Duration.GetMinuteAndSecondPart()
                            };
                            MusicPlayListViewModel.MusicPlayList.Add(newMusic);
                            LvMusicPlayList.SelectedItem = newMusic;
                        }
                        else
                            LvMusicPlayList.SelectedItem = music;
                    }
                }
                else
                    ChangePlayState();
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }


        /// <summary>
        /// QM播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QMusicPlayStateChanged(object sender, QMusicPlayStateChangedArgs e)
        {           
            Task.Factory.StartNew(() =>
            {
                //进行播放并且是属于（当前无播放源、不是同一首歌、播放失败）之一的情况 
                if (e.IsToPlay && e.IsNeedRefresh 
                && (_mediaPlayer.Source == null || !_mediaPlayer.Source.Equals(e.MusicInfo.Uri) || _isOpenFailedOfCurrentPlayingMusic))
                {
                    _isPlaying = false;
                    PlayNewMusic(e.MusicInfo.Uri);
                    _musicPlayerMenuViewModel.Id = 0;
                    _musicPlayerMenuViewModel.MusicName = e.MusicInfo.Name;
                    _musicPlayerMenuViewModel.SingerName = e.MusicInfo.SingerName;
                    _musicPlayerMenuViewModel.PhotoUri = e.MusicInfo.PhotoUri ?? _defaultPhotoUri;                   
                    _musicSourceType = MusicSourceType.QM;
                    InitILikeState();

                    if (MusicPlayListViewModel?.MusicPlayList != null)
                    {
                        var music = MusicPlayListViewModel.MusicPlayList.SingleOrDefault(m => m.Id.Equals(e.MusicInfo.Id));
                        if (music == null)
                        {
                            var newMusic = new MusicOfPlayListViewModel()
                            {
                                Id = e.MusicInfo.Id,
                                Name = e.MusicInfo.Name,
                                SingerName = e.MusicInfo.SingerName,
                                TimeDuration = e.MusicInfo.TimeDuration
                            };
                            MusicPlayListViewModel.MusicPlayList.Add(newMusic);
                            LvMusicPlayList.SelectedItem = newMusic;
                        }
                        else
                            LvMusicPlayList.SelectedItem = music;
                    }
                }
                else
                    ChangePlayState();
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }

        /// <summary>
        /// 用户登录事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserStateChanged(object sender, EventArgs e)
        {
            InitILikeState();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            MusicProgressSlider.Value = _mediaPlayer.Position.TotalSeconds;
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (_musicSourceType == MusicSourceType.JM)
                MusicPlayStateChangedUtil.InvokeFromJM(null, true, false);
            else
                MusicPlayStateChangedUtil.InvokeFromQM(null, true, false);
            MusicProgressSlider.Maximum = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            SliderVolume.Maximum = _mediaPlayer.Volume;
            _isOpenFailedOfCurrentPlayingMusic = false;
        }

        /// <summary>
        /// 歌曲播放失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            _isOpenFailedOfCurrentPlayingMusic = true;
            //为了同步播放按钮的状态
            if(_musicSourceType == MusicSourceType.JM)
                MusicPlayStateChangedUtil.InvokeFromJM(null, false, false);
            else
                MusicPlayStateChangedUtil.InvokeFromQM(null, false, false);
            JMMessageBox.Show("歌曲失效", "因歌曲文件失效导致播放失败，请欣赏其他歌曲", JMMessageBoxButtonType.OK, JMMessageBoxIconType.Error);
        }

        /// <summary>
        /// 歌曲播放结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            StopTimer();
            _mediaPlayer.Position = TimeSpan.FromSeconds(0);
            ChangePlayState();
            MusicPlaySwitchedUtil.Invoke(GetMusicPlayMode(), true);
        }

        /// <summary>
        /// 打开音量菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVolume_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PopVolume.IsOpen = !PopVolume.IsOpen;
        }

        /// <summary>
        /// 点击Pop中音量按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPopVolume_Click(object sender, RoutedEventArgs e)
        {
            if (!_mediaPlayer.IsMuted)
                ChangeVolumeToMute();
            else
            {
                ChangeVolumeToNotMute();
                SliderVolume.Value = _notMuteVolume;
            }
        }

        /// <summary>
        /// 音量改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderVolume.Value == 0)
            {
                ChangeVolumeToMute();
                return;
            }
            else if (_mediaPlayer.IsMuted)
                ChangeVolumeToNotMute();

            _notMuteVolume = SliderVolume.Value;
        }

        /// <summary>
        /// 我喜欢图标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnILike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mediaPlayer.HasAudio)
                {
                    ToggleUserLike(_musicPlayerMenuViewModel.Id, MusicRequestType.Song);                   
                }
                else
                    JMMessageBox.Show("添加我喜欢出错", "不存在播放音乐", JMMessageBoxButtonType.OK, JMMessageBoxIconType.Error);
            }
            catch(Exception ex)
            {
                JMMessageBox.Show("添加我喜欢出错",ex.Message, JMMessageBoxButtonType.OK, JMMessageBoxIconType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="musicId"></param>
        /// <param name="type"></param>
        /// <param name="isToAdd">false 删除  true添加  null toggle</param>
        /// <returns></returns>
        public bool ToggleUserLike(int musicId,MusicRequestType type,bool? isToAdd = null)
        {
            if (_musicSourceType == MusicSourceType.JM)
            {
                var user = App.User;
                if (user != null)
                {
                    switch (isToAdd)
                    {
                        case true:
                            if (!MusicService.IsUserLike(user.Id, musicId, type))
                            {
                                MusicService.ToggleUserLike(user.Id, musicId, type);
                            }
                            else
                                return true;
                            break;
                        case false:
                            if(MusicService.IsUserLike(user.Id, musicId, type))
                            {
                                MusicService.ToggleUserLike(user.Id, musicId, type);
                            }
                            else
                                return true;
                            break;
                        case null:
                            MusicService.ToggleUserLike(user.Id, musicId, type);
                            break;
                    }
                    if(_musicPlayerMenuViewModel.Id == musicId)
                        ChangeILikeState();
                    return true;
                }
                else
                    JMMessageBox.Show("添加我喜欢出错", "请先登录", JMMessageBoxButtonType.OK, JMMessageBoxIconType.Error);
            }
            else
                JMMessageBox.Show("添加我喜欢出错", "该音乐类型不支持", JMMessageBoxButtonType.OK, JMMessageBoxIconType.Error);

            return false;
        }

        /// <summary>
        /// 播放按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_musicSourceType.Equals(MusicSourceType.JM))
            {
                if (_mediaPlayer.Source == null)
                {
                    PlayNewMusic(_defaultMusicUri);
                    _musicPlayerMenuViewModel.MusicName = "好想再爱你";
                    _musicPlayerMenuViewModel.SingerName = "潘广益";
                    _musicPlayerMenuViewModel.PhotoUri = _defaultPhotoUri;
                    _musicSourceType = MusicSourceType.JM;
                }
                else
                    MusicPlayStateChangedUtil.InvokeFromJM(null, !_isPlaying, false);
            }
            else if (_musicSourceType == MusicSourceType.QM)           
                MusicPlayStateChangedUtil.InvokeFromQM(null, !_isPlaying,false);               
        }

        /// <summary>
        /// 上一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviousMusic_Click(object sender, RoutedEventArgs e)
        {
            SwitchMusic(false);
        }

        /// <summary>
        /// 下一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextMusic_Click(object sender, RoutedEventArgs e)
        {
            SwitchMusic(true);
        }

        /// <summary>
        /// 打开播放模式菜单左击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMusicPlayMode_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PopMusicPlayMode.IsOpen = !PopMusicPlayMode.IsOpen;
        }

        /// <summary>
        /// 在选择播放模式按钮时左击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMusicPlayMode_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is JmButton btn)
            {
                if(btn.Icon is TextBlock icon && BtnMusicPlayMode.Content is TextBlock btnIcon)
                {
                    btnIcon.Text = icon.Text;
                }
                BtnMusicPlayMode.Tag = btn.Tag;
                BtnMusicPlayMode.ToolTip = btn.Content;
                PopMusicPlayMode.IsOpen = false;
            }
        }

        /// <summary>
        /// 播单按钮左击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMusicPlayList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PopMusicPlayList.IsOpen = !PopMusicPlayList.IsOpen;
        }

        private async void BtnDownloadMusic_Click(object sender, RoutedEventArgs e)
        {
            if(_mediaPlayer.HasAudio && _musicSourceType == MusicSourceType.JM)
            {
                var uri = _musicPlayerMenuViewModel.FileUri;
                var musicName = _musicPlayerMenuViewModel.MusicName + System.IO.Path.GetExtension(uri.OriginalString);
                var singerName = _musicPlayerMenuViewModel.SingerName;
                await FileUtil.DownLoadMusicsAsync(uri, musicName, singerName);
                JMMessageBox.Show("歌曲下载", $"{musicName}下载成功", JMMessageBoxButtonType.OK, JMMessageBoxIconType.Success);
            }
            else
                JMMessageBox.Show("下载音乐文件出错", "不支持下载", JMMessageBoxButtonType.OK, JMMessageBoxIconType.Error);
        }

        /// <summary>
        /// 歌曲列表中的播放按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnListPlay_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion

        #region 辅助方法

        #region 初始化和绑定

        /// <summary>
        /// 设置播放器的绑定元素
        /// </summary>
        private void SetBindingOfMediaPlayer()
        {
            //播放位置
            var musicProgressBinding = new Binding()
            {
                Source = _mediaPlayer,
                Path = new PropertyPath("Position"),
                Converter = new TimeSpanToSecondsConverter(),
                Mode = BindingMode.TwoWay
            };
            MusicProgressSlider.SetBinding(Slider.ValueProperty, musicProgressBinding);

            //音量
            var musicVolumeBingding = new Binding()
            {
                Source = _mediaPlayer,
                Path = new PropertyPath("Volume"),
                Mode = BindingMode.TwoWay
            };
            SliderVolume.SetBinding(Slider.ValueProperty, musicVolumeBingding);
        }

        /// <summary>
        /// 设置播放列表绑定
        /// </summary>
        private void SetBindingOfMusicList()
        {
            LvMusicPlayList.ItemsSource = MusicPlayListViewModel.MusicPlayList;
            var musicTotalCountOfmusicListBinding = new Binding()
            {
                Source = MusicPlayListViewModel,
                Path = new PropertyPath(nameof(MusicPlayListViewModel.MusicTotalCount)),
                Mode = BindingMode.OneWay
            };
            TbMusicTotalCount.SetBinding(TextBlock.TextProperty, musicTotalCountOfmusicListBinding);
            TbMusicTotalCountOfMenu.SetBinding(TextBlock.TextProperty, musicTotalCountOfmusicListBinding);
        }

        /// <summary>
        /// 初始化音量状态
        /// </summary>
        private void InitVolumeState()
        {
            _mediaPlayer.IsMuted = SliderVolume.Value == 0d;
            if (!_mediaPlayer.IsMuted)
                _notMuteVolume = SliderVolume.Value;
            else
                _notMuteVolume = SliderVolume.Maximum;
        }

        /// <summary>
        /// 初始化音乐菜单信息
        /// </summary>
        private void InitMusicMenuInfo()
        {
            DataContext = _musicPlayerMenuViewModel;
            _musicPlayerMenuViewModel.MusicName = "听我想听的歌";
            _musicPlayerMenuViewModel.SingerName = "JM音乐";
        } 

        #endregion

        /// <summary>
        /// 播放新音乐
        /// </summary>
        /// <param name="uri"></param>
        private void PlayNewMusic(Uri uri)
        {
            if (_isOpenFailedOfCurrentPlayingMusic)
                _mediaPlayer.Close();
            _mediaPlayer.Open(uri);
        }

        #region 改变状态

        /// <summary>
        /// 将音量置为静音
        /// </summary>
        private void ChangeVolumeToMute()
        {
            TbVolume.Text = "\ue609";
            SliderVolume.Value = 0;
            _mediaPlayer.IsMuted = true;
        }

        /// <summary>
        /// 将音量改为非静音
        /// </summary>
        private void ChangeVolumeToNotMute()
        {
            TbVolume.Text = "\ue60b";
            _mediaPlayer.IsMuted = false;
        }

        /// <summary>
        /// 改变“喜欢”图标的状态
        /// </summary>
        private void ChangeILikeState()
        {
            if (TbILike.Text.Equals("\ue60e"))
            {
                TbILike.Text = "\ue603";
                BtnILove.Foreground = new SolidColorBrush(Color.FromRgb(255, 106, 106));
                BtnILove.FontSize += 2;
                BtnILove.ToolTip = "取消喜欢";
            }
            else
            {
                TbILike.Text = "\ue60e";
                BtnILove.Foreground = new SolidColorBrush(Colors.Silver);
                BtnILove.FontSize -= 2;
                BtnILove.ToolTip = "我喜欢";
            }
        }

        /// <summary>
        /// 初始化我喜欢图标
        /// </summary>
        private void InitILikeState()
        {
            if( App.User != null
                && MusicService.IsUserLike(App.User.Id, _musicPlayerMenuViewModel.Id, MusicRequestType.Song))
            {
                if (TbILike.Text.Equals("\ue60e"))
                {
                    TbILike.Text = "\ue603";
                    BtnILove.Foreground = new SolidColorBrush(Color.FromRgb(255, 106, 106));
                    BtnILove.FontSize += 2;
                    BtnILove.ToolTip = "取消喜欢";
                }
            }           
            else if (TbILike.Text.Equals("\ue603"))
            {
                TbILike.Text = "\ue60e";
                BtnILove.Foreground = new SolidColorBrush(Colors.Silver);
                BtnILove.FontSize -= 2;
                BtnILove.ToolTip = "我喜欢";
            }
        }

        /// <summary>
        /// 改变播放状态
        /// </summary>
        private void ChangePlayState()
        {
            if (_isOpenFailedOfCurrentPlayingMusic || _isPlaying)
            {
                _timer.Stop();
                _mediaPlayer.Pause();
                _isPlaying = false;
                TbPlay.Text = "\ue60f";
            }
            else
            {
                _timer.Start();
                _mediaPlayer.Play();
                _isPlaying = true;
                TbPlay.Text = "\ue606";
            }

        } 


        #endregion

        /// <summary>
        /// 停止计时器
        /// </summary>
        private void StopTimer()
        {
            Timer_Tick(_timer, null);
            _timer.Stop();
        }

        /// <summary>
        /// 获取播放模式
        /// </summary>
        /// <returns></returns>
        private MusicPlayMode GetMusicPlayMode()
        {
            return BtnMusicPlayMode.Tag as MusicPlayMode? ?? MusicPlayMode.Random;
        }

        /// <summary>
        /// 切歌
        /// </summary>
        /// <param name="isDown">是否向下切换</param>
        private void SwitchMusic(bool isDown)
        {
            var musicPlayMode = GetMusicPlayMode();
            if (musicPlayMode.Equals(MusicPlayMode.Single))
                musicPlayMode = MusicPlayMode.List;
            MusicPlaySwitchedUtil.Invoke(musicPlayMode, isDown);
        }



        #endregion

  
    }
}
