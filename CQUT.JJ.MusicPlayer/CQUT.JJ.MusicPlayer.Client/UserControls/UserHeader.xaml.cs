using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
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
        private UserHeaderViewModel _userHeaderViewModel = new UserHeaderViewModel();

        public UserHeader()
        {
            InitializeComponent();

            UserStateChangedUtil.UserStateChangedEvent += UserStateChanged;

            //必须在赋值DataContext对视图模型进行初始化，以完成对象绑定，以后重新new实例无效，因为其引用改变了
            InitViewModel();
            DataContext = _userHeaderViewModel;
        }


        private void UserStateChanged(object sender, EventArgs e)
        {
            if (App.User != null)
            {
                _userHeaderViewModel.Id = App.User.Id;
                _userHeaderViewModel.NickName = App.User.NickName;
                _userHeaderViewModel.ProfilePhotoPath = (string.IsNullOrWhiteSpace(App.User.ProfilePhotoPath) || !File.Exists(App.User.ProfilePhotoPath) ?
                    ConstantsUtil.DefaultProfilePhotoPath : App.User.ProfilePhotoPath)
                    .ToImageSource();
                BtnLevelTip.Visibility = Visibility.Visible;

                TbNickName.Text = App.User.NickName;
                TbMusicListCount.Text = UserMusicNavigtion.UserMusicListService.GetUserMusicListByUserId(App.User.Id)?.Count().ToString();
            }
            else
            {
                InitViewModel();
                BtnLevelTip.Visibility = Visibility.Hidden;
                PopUserInfo.IsOpen = false;
                TbNickName.Text = string.Empty;
                TbMusicListCount.Text = "0";
            }
                
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
                loginWin.ShowDialog();
            }
            else
            {

            }
        }

        private void BtnNickName_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private async void BtnProfile_MouseEnter(object sender, MouseEventArgs e)
        {
            if(App.User != null)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                if (BtnProfile.IsMouseOver)
                {
                    PopUserInfo.IsOpen = true;
                }
            }           
        }

        private async void BtnProfile_MouseLeave(object sender, MouseEventArgs e)
        {
            if(App.User != null)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.25));
                if (!BtnProfile.IsMouseOver && !PopUserInfo.IsMouseOver)
                {
                    PopUserInfo.IsOpen = false;
                }
            }
           
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            App.User = null;
            UserStateChangedUtil.Invoke();
        }

        private void InitViewModel()
        {
            _userHeaderViewModel.Id = 0;
            _userHeaderViewModel.NickName = "请登录";
            _userHeaderViewModel.ProfilePhotoPath = ConstantsUtil.DefaultProfilePhotoPath.ToImageSource();
        }
    }
}
