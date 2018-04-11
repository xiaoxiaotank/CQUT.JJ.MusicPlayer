﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<MusicContract> GetLoveMusicsByUserId(int userId)
        {
            var result = from u in _ctx.UserLike.Where(u => u.UserId == userId && u.MusicId != null)
                         join m in _ctx.Music.Include(m => m.Singer).Include(m => m.Album).Where(m => m.IsPublished && !m.IsDeleted)
                         on u.MusicId equals m.Id into mu
                         from music in mu.DefaultIfEmpty()
                         select new MusicContract()
                         {
                             Id = music.Id,
                             SingerId = music.Singer.Id,
                             AlbumId = music.AlbumId,
                             Name = music.Name,
                             SingerName = music.Singer.Name,
                             AlbumName = music.Album.Name,
                             FileUrl = music.FileUrl,
                             Duration = music.Duration
                         };

            return result;
        }
    }
}
