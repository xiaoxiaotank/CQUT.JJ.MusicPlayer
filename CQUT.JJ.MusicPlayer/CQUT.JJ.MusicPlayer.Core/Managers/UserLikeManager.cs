using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class UserLikeManager : BaseManager<UserLike>
    {
        private readonly UserManager _userManager;
        private readonly MusicManager _musicManager;

        public UserLikeManager(JMDbContext ctx, UserManager userManager
            ,MusicManager musicManager):base(ctx)
        {
            _userManager = userManager;
            _musicManager = musicManager;
        }

        /// <summary>
        /// 添加或删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        public void Toggle(int userId,int objId,MusicRequestType type)
        {
            UserLike userLike = null;
            switch (type)
            {
                case MusicRequestType.Song:
                    userLike = JMDbContext.UserLike.SingleOrDefault(u => u.UserId == userId && u.MusicId == objId);                   
                    break;
                default:
                    return;
            }
            if (userLike == null)
                Create(userId, objId, type);
            else
                Remove(userLike);
        }

        /// <summary>
        /// 添加UserLike
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        public void Create(int userId,int objId,MusicRequestType type)
        {
            var user = _userManager.Find(userId);
            if (user.IsAdmin)
                ThrowException("用户不存在!");
            UserLike userLike = null;
            switch (type)
            {
                case MusicRequestType.Song:
                    var music = _musicManager.Find(objId);
                    if (!music.IsPublished)
                        ThrowException("歌曲不存在！");
                    if (JMDbContext.UserLike.Any(u => u.UserId == userId && u.MusicId == objId))
                        ThrowException("您的喜好歌曲中已存在该歌曲，无需重复添加");
                    userLike = new UserLike()
                    {
                        UserId = userId,
                        MusicId = objId,
                    };
                    break;
                default:
                    return;
            }
            Create(userLike);
            Save();
        }

        public void Delete(int userId, int objId, MusicRequestType type)
        {
            UserLike userLike = null;
            switch (type)
            {
                case MusicRequestType.Song:                  
                    userLike = JMDbContext.UserLike.SingleOrDefault(u => u.UserId == userId && u.MusicId == objId);
                    break;
                default:
                    return;
            }
            if(userLike != null)
                Remove(userLike);
        }

        public UserLike Find(int userId,int objId,MusicRequestType type)
        {
            UserLike userLike = null;
            switch (type)
            {
                case MusicRequestType.Song:
                    userLike = JMDbContext.UserLike.SingleOrDefault(u => u.UserId == userId && u.MusicId == objId);
                    break;
            }
            return userLike;
        }
    }
}
