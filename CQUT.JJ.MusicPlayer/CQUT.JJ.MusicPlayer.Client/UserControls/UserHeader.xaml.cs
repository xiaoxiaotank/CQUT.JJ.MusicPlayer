using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.ViewModels.UserHeader;
using CQUT.JJ.MusicPlayer.Client.Windows;
using CQUT.JJ.MusicPlayer.WCFService;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// UserHeader.xaml 的交互逻辑
    /// </summary>
    public partial class UserHeader : UserControl
    {
        private const string _defaultProfilePhotoPath = "/Asserts/Images/DefaultUserHeader.png";

        private UserHeaderViewModel _userHeaderViewModel = null;

        public UserHeader()
        {
            InitializeComponent();

            //必须在赋值DataContext对视图模型进行初始化，以完成对象绑定，以后重新new实例无效，因为其引用改变了
            _userHeaderViewModel = new UserHeaderViewModel()
            {
                NickName = "请登录",
                ProfilePhotoPath = _defaultProfilePhotoPath.ToImageSource(),
            };
            DataContext = _userHeaderViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (App.User == null)
            {
                var loginWin = new LoginWindow();
                try
                {
                    if (loginWin.ShowDialog() == false)
                    {
                        if (App.User != null)
                        {
                            _userHeaderViewModel.Id = App.User.Id;
                            _userHeaderViewModel.NickName = App.User.NickName;
                            _userHeaderViewModel.ProfilePhotoPath = (string.IsNullOrWhiteSpace(App.User.ProfilePhotoPath) || !File.Exists(App.User.ProfilePhotoPath) ?
                                _defaultProfilePhotoPath : App.User.ProfilePhotoPath)
                                .ToImageSource();
                            BtnLevelTip.Visibility = Visibility.Visible;
                        }
                        else
                            BtnLevelTip.Visibility = Visibility.Hidden;
                    }
                }
                catch
                {

                }
            }
            else
            {

            }
        }

        private void BtnNickName_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
