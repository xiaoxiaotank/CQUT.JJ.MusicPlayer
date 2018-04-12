using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“IUserMusicListService”。
    public class UserMusicListService : IUserMusicListService
    {
        private readonly JMDbContext _ctx;
        private readonly UserManager _userManager;
        private readonly MusicManager _musicManager;
        private readonly UserMusicListManager _userMusicListManager;

        public UserMusicListService()
        {
            _ctx = new JMDbContext();
            _userManager = new UserManager(_ctx);
            _musicManager = new MusicManager(_ctx, _userManager);
            _userMusicListManager = new UserMusicListManager(_ctx, _userManager,_musicManager);
        }

        public IEnumerable<UserMusicListContract> GetUserMusicListByUserId(int userId)
        {
            var result = _ctx.UserMusicList.Where(u => u.UserId == userId && !u.IsDeleted)?
                .Select(u => new UserMusicListContract()
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    Name = u.Name,
                    CreationTime = u.CreationTime
                });
            return result;
        }

        public UserMusicListContract Create(UserMusicListContract userMusicList)
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

        public void Update(int id, string name)
        {
            _userMusicListManager.Update(id, name);
        }

        public void Delete(int id)
        {
            _userMusicListManager.Delete(id);
        }

        public void AddToUserMusicList(int userId, int objId, int musicListId, MusicRequestType type)
        {
            _userMusicListManager.AddToUserMusicList(userId, objId, musicListId, type);
        }

        public UserMusicListContract GetUserMusicListById(int id)
        {
            var userMusicList = _userMusicListManager.Find(id);
            if (userMusicList == null)
                return null;
            return new UserMusicListContract()
            {
                Id = userMusicList.Id,
                UserId = userMusicList.UserId,
                Name = userMusicList.Name
            };
        }
    }
}
