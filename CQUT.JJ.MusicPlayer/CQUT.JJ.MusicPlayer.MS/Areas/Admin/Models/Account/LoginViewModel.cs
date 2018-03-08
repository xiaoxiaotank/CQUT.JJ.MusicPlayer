using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Account
{
    public class LoginViewModel
    {
        [Display(Name = "用户名"), Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        [Display(Name = "密码"), Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        public bool IsRememberMe { get; set; }
    }
}
