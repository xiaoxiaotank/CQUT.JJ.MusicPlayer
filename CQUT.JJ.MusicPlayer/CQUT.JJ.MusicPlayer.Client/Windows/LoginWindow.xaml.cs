using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Controls.Enums.JmBubbleMessageBox;
using CQUT.JJ.MusicPlayer.Models.JM.Common;
using CQUT.JJ.MusicPlayer.WCFService;
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
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Client.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IUserService _userService;

        public LoginWindow()
        {
            InitializeComponent();

            _userService = new UserService();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            BtnLogin.IsEnabled = false;
            var userName = TbUserName.Text.Trim();
            var password = Pwd.Password.Trim();
            try
            {
                var user = await Task.Factory.StartNew(() =>
                {
                    return _userService.Login(userName, password);
                });
                if (user != null)
                {
                    App.User = new UserModel()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        NickName = user.NickName,
                        ProfilePhotoPath = user.ProfilePhotoPath
                    };
                    UserStateChangedUtil.Invoke();
                    Close();
                }
            }
            catch(Exception ex)
            {
                BtnLogin.IsEnabled = true;
                JmBubbleMessageBox.Show(ex.Message,JmBubbleMessageBoxType.Error);
            }     
        }
    }
}
