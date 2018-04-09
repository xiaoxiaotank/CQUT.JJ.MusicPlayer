using CQUT.JJ.MusicPlayer.Controls.Enums.JmBubbleMessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CQUT.JJ.MusicPlayer.Controls.Controls
{
    /// <summary>
    /// JmBubbleMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class JmBubbleMessageBox : Window
    {
        private static readonly Brush _errorBackground = new SolidColorBrush(Colors.Red);

        private static readonly Brush _infoBackground = new SolidColorBrush(Colors.AliceBlue);

        private static readonly Brush _successBackground = new SolidColorBrush(Colors.Green);

        private static readonly Brush _warningBackground = new SolidColorBrush(Colors.YellowGreen);

        private static readonly TaskScheduler _syncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();


        private JmBubbleMessageBox(string msg)
        {
            InitializeComponent();

            TbMsg.Text = msg;
        }

        public static async Task Show(string messageText, JmBubbleMessageBoxType messageType,TimeSpan? stayTime = null)
        {
            if (stayTime == null)
                stayTime = TimeSpan.FromSeconds(3);
            var msgBox = new JmBubbleMessageBox(messageText)
            {
                Owner = Application.Current.MainWindow,
                Topmost = true,
            };
            switch (messageType)
            {
                case JmBubbleMessageBoxType.Info:
                    msgBox.Background = _infoBackground;
                    break;
                case JmBubbleMessageBoxType.Error:
                    msgBox.Background = _errorBackground;
                    break;
                case JmBubbleMessageBoxType.Warning:
                    msgBox.Background = _warningBackground;
                    break;
                case JmBubbleMessageBoxType.Success:
                    msgBox.Background = _successBackground;
                    break;
            }

            msgBox.Show();
            await Task.Delay((TimeSpan)stayTime);
            msgBox.Close();
        }
    }
}
