using CQUT.JJ.MusicPlayer.Client.Converters;
using CQUT.JJ.MusicPlayer.Client.Utils;
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

        private static MusicPlayerMenuViewModel _musicPlayerMenuViewModel = new MusicPlayerMenuViewModel();

        public MusicPlayerMenu()
        {
            _mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            _mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            MusicPlayStateChangedUtil.MusicPlayStateChangedEvent += MusicPlayStateChangedUtil_MusicPlayStateChangedEvent;
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(1);

            InitializeComponent();

            SetBindingAboutMediaPlayer();
        }

       

        #region Events


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitVolumeState();
            InitMusicInfo();
        }  

        private void MusicPlayStateChangedUtil_MusicPlayStateChangedEvent(object sender, MusicPlayStateChangedArgs e)
        {
            if (e.IsToPlay && (_mediaPlayer.Source == null || !_mediaPlayer.Source.Equals(e.MusicInfo.Uri)))
            {
                PlayNewMusic(e.MusicInfo.Uri);
                _musicPlayerMenuViewModel.MusicName = e.MusicInfo.Name;
                _musicPlayerMenuViewModel.SingerName = e.MusicInfo.SingerName;
                _isPlaying = false;
            }
            ChangePlayState();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MusicProgressSlider.Value = _mediaPlayer.Position.TotalSeconds;
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Start();
            MusicProgressSlider.Maximum = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            SliderVolume.Maximum = _mediaPlayer.Volume;           
        }

        private void MediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            StopTimer();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            StopTimer();
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
            ChangeToILove();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaPlayer.Source == null)
                PlayNewMusic(new Uri(@"好想再爱你.mp3", UriKind.Relative));
            ChangePlayState();
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

        private void ChangeToILove()
        {
            TbILove.Text = "\ue603";
            BtnILove.Foreground = new SolidColorBrush(Color.FromRgb(255, 106, 106));
            BtnILove.FontSize += 2;
            BtnILove.ToolTip = "取消喜欢";
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
            if (_isPlaying)
            {
                _mediaPlayer.Pause();
                _isPlaying = false;
                TbPlay.Text = "\ue60f";
            }
            else
            {
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
