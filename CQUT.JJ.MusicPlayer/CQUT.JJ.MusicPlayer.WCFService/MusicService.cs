using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“MusicService”。
    public class MusicService : IMusicService
    {
        private readonly JMDbContext _ctx;
        private readonly UserLikeManager _userLikeManager;
        private readonly UserManager _userManager;
        private readonly MusicManager _musicManager;

        public MusicService()
        {
            _ctx = new JMDbContext();
            _userManager = new UserManager(_ctx);
            _musicManager = new MusicManager(_ctx, _userManager);
            _userLikeManager = new UserLikeManager(_ctx,_userManager,_musicManager);
        }

        public void ToggleUserLike(int userId, int objId, MusicRequestType type)
        {
            _userLikeManager.Toggle(userId, objId, type);
        }

        public bool IsUserLike(int userId, int objId, MusicRequestType type)
        {
            return _userLikeManager.Find(userId, objId, type) != null;
        }
    }
}
