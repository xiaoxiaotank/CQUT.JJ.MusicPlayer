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

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// MusicPlayerMenu.xaml 的交互逻辑
    /// </summary>
    public partial class MusicPlayerMenu : UserControl
    {
        /// <summary>
        /// 是否静音
        /// </summary>
        private static bool _isMute = false;

        /// <summary>
        /// 非静音时音量
        /// </summary>
        private static double _notMuteVolume = 0;

        public MusicPlayerMenu()
        {
            InitializeComponent();
        }

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitVolumeState();
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
        #endregion

        #region 辅助方法
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


        /// <summary>
        /// 将音量改为静音
        /// </summary>
        private void ChangeVolumeToMute()
        {
            TbVolume.Text = "\ue609";
            SliderVolume.Value = 0;
            _isMute = true;
        }

        /// <summary>
        /// 将音量改为非静音
        /// </summary>
        private void ChangeVolumeToNotMute()
        {
            TbVolume.Text = "\ue60b";
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
        #endregion

    }
}
