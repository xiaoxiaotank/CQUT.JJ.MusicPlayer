using CQUT.JJ.MusicPlayer.Controls.Enums.JMMessageBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Controls.Controls
{
    /// <summary>
    /// JMMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class JMMessageBox : JmWindow
    {

        #region Filed
        private const string Icon_Path = "../Asserts/Images/JMMessageBox/";
        private const string Info_Icon_Path = Icon_Path + "Info.png";
        private const string Error_Icon_Path = Icon_Path + "Error.png";
        private const string Success_Icon_Path = Icon_Path + "Success.png";
        private const string Warning_Icon_Path = Icon_Path + "Warning.png";

        private JMMessageBoxViewModel _jmMessageBoxViewModel = new JMMessageBoxViewModel();

        /// <summary>
        /// 消息框的返回值
        /// </summary>
        public JMMessageBoxResultType ResultType { get; set; }


        #endregion

        #region Constructor
        private JMMessageBox()
        {
            InitializeComponent();
            this.DataContext = _jmMessageBoxViewModel;

            _jmMessageBoxViewModel.OkButtonVisibility = Visibility.Collapsed;
            _jmMessageBoxViewModel.CancelButtonVisibility = Visibility.Collapsed;
            _jmMessageBoxViewModel.YesButtonVisibility = Visibility.Collapsed;
            _jmMessageBoxViewModel.NoButtonVisibility = Visibility.Collapsed;

            ResultType = JMMessageBoxResultType.None;

        }
        #endregion

        #region Events

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ResultType = JMMessageBoxResultType.OK;
            this.Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultType = JMMessageBoxResultType.Yes;
            this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            ResultType = JMMessageBoxResultType.No;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResultType = JMMessageBoxResultType.Cancel;
            this.Close();
        }

        #endregion


        public static JMMessageBoxResultType Show(string title, string messageText, JMMessageBoxButtonType messageButtonType, JMMessageBoxIconType messageIconType)
        {
            var msgBox = new JMMessageBox();
            msgBox.Owner = Application.Current.MainWindow;
            msgBox.Topmost = true;
            msgBox._jmMessageBoxViewModel.Title = title;
            msgBox._jmMessageBoxViewModel.MessageText = messageText;
            switch (messageIconType)
            {
                case JMMessageBoxIconType.Info:
                    msgBox._jmMessageBoxViewModel.IconPath = Info_Icon_Path;
                    break;
                case JMMessageBoxIconType.Error:
                    msgBox._jmMessageBoxViewModel.IconPath = Error_Icon_Path;
                    break;
                case JMMessageBoxIconType.Warning:
                    msgBox._jmMessageBoxViewModel.IconPath = Warning_Icon_Path;
                    break;
                case JMMessageBoxIconType.Success:
                    msgBox._jmMessageBoxViewModel.IconPath = Success_Icon_Path;
                    break;
            }
            switch (messageButtonType)
            {
                case JMMessageBoxButtonType.OK:
                    msgBox._jmMessageBoxViewModel.OkButtonVisibility = Visibility.Visible;
                    break;
                case JMMessageBoxButtonType.OKCancel:
                    msgBox._jmMessageBoxViewModel.OkButtonVisibility = Visibility.Visible;
                    msgBox._jmMessageBoxViewModel.CancelButtonVisibility = Visibility.Visible;
                    break;
                case JMMessageBoxButtonType.YesNo:
                    msgBox._jmMessageBoxViewModel.YesButtonVisibility = Visibility.Visible;
                    msgBox._jmMessageBoxViewModel.NoButtonVisibility = Visibility.Visible;
                    break;
                case JMMessageBoxButtonType.YesNoCancel:
                    msgBox._jmMessageBoxViewModel.YesButtonVisibility = Visibility.Visible;
                    msgBox._jmMessageBoxViewModel.NoButtonVisibility = Visibility.Visible;
                    msgBox._jmMessageBoxViewModel.CancelButtonVisibility = Visibility.Visible;
                    break;
                default:
                    msgBox._jmMessageBoxViewModel.OkButtonVisibility = Visibility.Visible;
                    break;
            }

            msgBox.ShowDialog();
            return msgBox.ResultType;
        }
    }

    public class JMMessageBoxViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _messageText;
        private string _iconPath;
        private Visibility _okButtonVisibility;
        private Visibility _cancelButtonVisibility;
        private Visibility _yesButtonVisibility;
        private Visibility _noButtonVisibility;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }
        /// <summary>
        /// 显示的内容
        /// </summary>
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageText)));
            }
        }
        /// <summary>
        /// 显示的图片
        /// </summary>
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconPath)));
            }
        }
        /// <summary>
        /// 控制显示 OK 按钮
        /// </summary>
        public Visibility OkButtonVisibility
        {
            get => _okButtonVisibility;
            set
            {
                _okButtonVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkButtonVisibility)));
            }
        }
        /// <summary>
        /// 控制显示 Cacncel 按钮
        /// </summary>
        public Visibility CancelButtonVisibility
        {
            get => _cancelButtonVisibility;
            set
            {
                _cancelButtonVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CancelButtonVisibility)));
            }
        }
        /// <summary>
        /// 控制显示 Yes 按钮
        /// </summary>
        public Visibility YesButtonVisibility
        {
            get => _yesButtonVisibility ;
            set
            {
                _yesButtonVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YesButtonVisibility)));
            }
        }
        /// <summary>
        /// 控制显示 No 按钮
        /// </summary>
        public Visibility NoButtonVisibility
        {
            get => _noButtonVisibility;
            set
            {
                _noButtonVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoButtonVisibility)));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
