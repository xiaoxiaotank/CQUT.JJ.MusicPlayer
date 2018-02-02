﻿using CQUT.JJ.MusicPlayer.Client.Converters;
using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.Enums;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Client.ViewModels;
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
        /// <summary>
        /// 定时更新歌曲进度
        /// </summary>
        private static readonly DispatcherTimer _timer = new DispatcherTimer();

        /// <summary>
        /// 默认照片Uri
        /// </summary>
        private static readonly Uri _defaultPhotoUri = new Uri("/Asserts/Images/DefaultMusicHeader.png", UriKind.Relative);

        /// <summary>
        /// 是否静音
        /// </summary>
        private static bool _isMute = false;

        /// <summary>
        /// 非静音时音量
        /// </summary>
        private static double _notMuteVolume = 0;

        /// <summary>
        /// 播放器
        /// </summary>
        private static MediaPlayer _mediaPlayer = new MediaPlayer();

        /// <summary>
        /// 是否正在播放
        /// </summary>
        private static bool _isPlaying = false;

        /// <summary>
        /// 当前播放歌曲打开是否失败
        /// </summary>
        private static bool _isCurrentPlayingMusicOpenFailed = false;

        /// <summary>
        /// 音乐源类型
        /// </summary>
        private static MusicSourceType _musicSourceType = MusicSourceType.JM;

        private static MusicPlayerMenuViewModel _musicPlayerMenuViewModel = new MusicPlayerMenuViewModel() { PhotoUri = _defaultPhotoUri };

        private static TaskScheduler _syncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        public MusicPlayerMenu()
        {
            _mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            _mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            MusicPlayStateChangedUtil.QMusicPlayStateChangedEvent += QMusicPlayStateChanged;
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(0.1);

            InitializeComponent();

            SetBindingAboutMediaPlayer();
        }

        #region Events


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitVolumeState();
            InitMusicInfo();
        }  

        private void QMusicPlayStateChanged(object sender, QMusicPlayStateChangedArgs e)
        {           
            Task.Factory.StartNew(() =>
            {
                //进行播放并且是属于（当前无播放源、不是同一首歌、播放失败）之一的情况 
                if (e.IsToPlay && e.IsNeedRefresh 
                && (_mediaPlayer.Source == null || !_mediaPlayer.Source.Equals(e.MusicInfo.Uri) || _isCurrentPlayingMusicOpenFailed))
                {
                    _isPlaying = false;
                    PlayNewMusic(e.MusicInfo.Uri);
                    _musicPlayerMenuViewModel.MusicName = e.MusicInfo.Name;
                    _musicPlayerMenuViewModel.SingerName = e.MusicInfo.SingerName;
                    _musicPlayerMenuViewModel.PhotoUri = e.MusicInfo.PhotoUri ?? _defaultPhotoUri;                   
                    _musicSourceType = MusicSourceType.QM;                  
                }
                else
                    ChangePlayState();
            }, CancellationToken.None, TaskCreationOptions.None, _syncTaskScheduler);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MusicProgressSlider.Value = _mediaPlayer.Position.TotalSeconds;
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            MusicPlayStateChangedUtil.InvokeFromQM(null, true, false);
            MusicProgressSlider.Maximum = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            SliderVolume.Maximum = _mediaPlayer.Volume;
            _isCurrentPlayingMusicOpenFailed = false;
        }

        private void MediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            _isCurrentPlayingMusicOpenFailed = true;
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            StopTimer();
            _mediaPlayer.Position = TimeSpan.FromSeconds(0);
            ChangePlayState();
            MusicPlaySwitchedUtil.InvokeFromQM(MusicPlayMode.List,true);
        }

        private void BtnVolume_Click(object sender, RoutedEventArgs e)
        {
            PopVolume.IsOpen = true;
        }

        private void BtnPopVolume_Click(object sender, RoutedEventArgs e)
        {
            if (!_isMute)
                ChangeVolumeToMute();
            else
            {
                ChangeVolumeToNotMute();
                SliderVolume.Value = _notMuteVolume;
            }
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderVolume.Value == 0)
            {
                ChangeVolumeToMute();
                return;
            }
            else if (_isMute)
                ChangeVolumeToNotMute();
            _notMuteVolume = SliderVolume.Value;
        }

        private void BtnILove_Click(object sender, RoutedEventArgs e)
        {
            ChangeILoveState();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_musicSourceType.Equals(MusicSourceType.JM))
            {
                if(_mediaPlayer.Source == null)
                {
                    PlayNewMusic(new Uri(@"好想再爱你.mp3", UriKind.Relative));
                    _musicPlayerMenuViewModel.MusicName = "好想再爱你";
                    _musicPlayerMenuViewModel.SingerName = "潘广益";
                    _musicPlayerMenuViewModel.PhotoUri = _defaultPhotoUri;
                    _musicSourceType = MusicSourceType.JM;
                }               
                ChangePlayState();
            }
            else if (_musicSourceType == MusicSourceType.QM)           
                MusicPlayStateChangedUtil.InvokeFromQM(null, !_isPlaying,false);               
        }

        private void BtnPreviousMusic_Click(object sender, RoutedEventArgs e)
        {
            MusicPlaySwitchedUtil.InvokeFromQM(MusicPlayMode.List, false);
        }

        private void BtnNextMusic_Click(object sender, RoutedEventArgs e)
        {
            MusicPlaySwitchedUtil.InvokeFromQM(MusicPlayMode.List,true);
        }


        #endregion

        #region 辅助方法
        private void InitMusicInfo()
        {
            DataContext = _musicPlayerMenuViewModel;
            _musicPlayerMenuViewModel.MusicName = "听我想听的歌";
            _musicPlayerMenuViewModel.SingerName = "JM音乐";
        }

        private void SetBindingAboutMediaPlayer()
        {
            var musicProgressBinding = new Binding()
            {
                Source = _mediaPlayer,
                Path = new PropertyPath("Position"),
                Converter = new TimeSpanToSecondsConverter(),
                Mode = BindingMode.TwoWay
            };
            MusicProgressSlider.SetBinding(Slider.ValueProperty, musicProgressBinding);

            var musicVolumeBingding = new Binding()
            {
                Source = _mediaPlayer,
                Path = new PropertyPath("Volume"),
                Mode = BindingMode.TwoWay
            };
            SliderVolume.SetBinding(Slider.ValueProperty, musicVolumeBingding);
        }

        /// <summary>
        /// 初始化音量状态
        /// </summary>
        private void InitVolumeState()
        {
            _isMute = SliderVolume.Value == 0d;
            if (!_isMute)
                _notMuteVolume = SliderVolume.Value;
            else
                _notMuteVolume = SliderVolume.Maximum;
        }

        private void PlayNewMusic(Uri uri)
        {
            if (_isCurrentPlayingMusicOpenFailed)
                _mediaPlayer.Close();
            _mediaPlayer.Open(uri);
        }

        /// <summary>
        /// 将音量改为静音
        /// </summary>
        private void ChangeVolumeToMute()
        {
            TbVolume.Text = "\ue609";
            SliderVolume.Value = 0;
            _mediaPlayer.IsMuted = true;
            _isMute = true;
        }

        /// <summary>
        /// 将音量改为非静音
        /// </summary>
        private void ChangeVolumeToNotMute()
        {
            TbVolume.Text = "\ue60b";
            _mediaPlayer.IsMuted = false;
            _isMute = false;
        }

        private void ChangeILoveState()
        {
            if(_mediaPlayer.HasAudio 
                && _musicSourceType == MusicSourceType.JM)
            {
                if(TbILove.Text.Equals("\ue60e"))
                {
                    TbILove.Text = "\ue603";
                    BtnILove.Foreground = new SolidColorBrush(Color.FromRgb(255, 106, 106));
                    BtnILove.FontSize += 2;
                    BtnILove.ToolTip = "取消喜欢";
                }
                else
                {
                    TbILove.Text = "\ue60e";
                    BtnILove.Foreground = new SolidColorBrush(Colors.Silver);
                    BtnILove.FontSize -= 2;
                    BtnILove.ToolTip = "我喜欢";
                }
            }
        }

        private void ChangeToCancelILove()
        {
            TbILove.Text = "\ue60e";
            BtnILove.Foreground = new SolidColorBrush(Colors.SkyBlue);
            BtnILove.FontSize -= 2;
            BtnILove.ToolTip = "我喜欢";
        }

        private void ChangePlayState()
        {                       
            if (_isCurrentPlayingMusicOpenFailed || _isPlaying)
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

        private void StopTimer()
        {
            Timer_Tick(_timer, null);
            _timer.Stop();
        }



        #endregion

 
    }
}
