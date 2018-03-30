using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using CQUT.JJ.MusicPlayer.EntityFramework.Algorithms;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Core.Managers;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“UserService”。
    public class UserService : IUserService
    {
        private JMDbContext _ctx;
        private UserManager _userManager;
        public UserService()
        {
            _ctx = new JMDbContext();
            _userManager = new UserManager(_ctx);
        }

        public UserInfo Login(string userName, string password)
        {
            password = password.EncryptByMD5();
            var user = _ctx.User.SingleOrDefault(m => m.UserName.Equals(userName, StringComparison.Ordinal) && !m.IsDeleted && !m.IsAdmin);
            if (user == null)
                _userManager.ThrowException("用户不存在");
            else if (!user.Password.Equals(password))
                _userManager.ThrowException("用户名或密码错误");

            return new UserInfo()
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
            };
        }
    }
}
