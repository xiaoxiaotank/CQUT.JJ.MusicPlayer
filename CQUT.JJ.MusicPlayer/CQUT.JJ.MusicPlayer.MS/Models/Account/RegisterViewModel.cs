using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Models.Account
{
    public class RegisterViewModel
    {
        [DisplayName("用户名")]
        public string UserName { get; set; }

        [DisplayName("昵称")]
        public string NickName { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("重复密码")]
        public string ConfirmPassword { get; set; }
    }
}
