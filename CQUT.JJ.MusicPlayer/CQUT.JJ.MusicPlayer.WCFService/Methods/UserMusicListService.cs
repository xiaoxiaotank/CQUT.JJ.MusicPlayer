using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“IUserMusicListService”。
    public class UserMusicListService : IUserMusicListService
    {
        private readonly JMDbContext _ctx;
        private readonly UserMusicListManager _userMusicListManager;

        public UserMusicListService()
        {
            _ctx = new JMDbContext();
            _userMusicListManager = new UserMusicListManager(_ctx);
        }

        public IEnumerable<UserMusicListInfo> GetUserMusicListByUserId(int userId)
        {
            var result = _ctx.UserMusicList.Where(u => u.UserId == userId && !u.IsDeleted)?
                .Select(u => new UserMusicListInfo()
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    Name = u.Name
                });
            return result;
        }

        public UserMusicListInfo Create(UserMusicListInfo userMusicList)
        {
            var model = new UserMusicList()
            {
                UserId = userMusicList.UserId,
                Name = userMusicList.Name
            };
            var result = _userMusicListManager.Create(model);
            userMusicList.Id = result.Id;
            return userMusicList;
        }

    }
}
