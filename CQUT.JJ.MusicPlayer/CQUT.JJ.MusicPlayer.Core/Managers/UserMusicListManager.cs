using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class UserMusicListManager : BaseManager<UserMusicList>
    {
        private readonly UserManager _userManager;

        private readonly MusicManager _musicManager;

        public UserMusicListManager(JMDbContext ctx,UserManager userManager,MusicManager musicManager) : base(ctx)
        {
            _userManager = userManager;
            _musicManager = musicManager;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public override UserMusicList Create(UserMusicList model)
        {
            ValidateForCreate(model);

            model.CreationTime = DateTime.Now;
            model.IsDeleted = false;

            return base.Create(model);
        }

        public UserMusicList Update(int id, string name)
        {
            ValidateForUpdate(id,name,out UserMusicList userMusicList);

            userMusicList.Name = name;
            Save();
            return userMusicList;
        }

        public void Delete(int id)
        {
            var userMusicList = Find(id);
            if(userMusicList != null)
            {
                userMusicList.IsDeleted = true;
                userMusicList.DeletionTime = DateTime.Now;
                Save();
            }
        }

        public override UserMusicList Find(int id)
        {
            var userMusicList = base.Find(id);
            if (userMusicList != null && !userMusicList.IsDeleted)
                return userMusicList;
            return null;
        }

        public void AddToUserMusicList(int userId,int objId,int userMusicListId,MusicRequestType type)
        {
            var user = _userManager.Find(userId);
            if (user == null || user.IsAdmin)
                ThrowException("用户不存在");

            UserMusicList list = JMDbContext.UserMusicList.SingleOrDefault(u => u.Id == userMusicListId && u.UserId == userId && !u.IsDeleted);
            if (list == null)
                ThrowException("未找到该用户的歌单");

            switch (type)
            {
                case MusicRequestType.Song:
                    var music = _musicManager.Find(objId);
                    if (music == null || !music.IsPublished)
                        ThrowException("歌曲不存在");
                    var listMusic = new UserMusicListMusic()
                    {
                        MusicId = music.Id,
                        MusicListId = list.Id
                    };
                    JMDbContext.UserMusicListMusic.Add(listMusic);
                    break;
                default:
                    return;
            }
            Save();
        }

        private void ValidateForCreate(UserMusicList model)
        {
            if (JMDbContext.User.SingleOrDefault(u => u.Id == model.UserId && !u.IsAdmin && !u.IsDeleted) == null)
                ThrowException("用户不存在!");
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("歌单名不能为空");
            if (model.Name.Length > 20)
                ThrowException("歌单名最多只能输入20个文字");
            if (JMDbContext.UserMusicList.Any(u => u.Name.Equals(model.Name) && !u.IsDeleted))
                ThrowException("您已创建相同名字的歌单");
        }

        private void ValidateForUpdate(int id, string name,out UserMusicList userMusicList)
        {
            userMusicList = JMDbContext.UserMusicList.SingleOrDefault(u => u.Id == id && !u.IsDeleted);
            if (userMusicList == null)
                ThrowException("未找到该歌单");
            if (string.IsNullOrWhiteSpace(name))
                ThrowException("歌单名不能为空");
            if (name.Length > 20)
                ThrowException("歌单名最多只能输入20个文字");
            if (JMDbContext.UserMusicList.Any(u => u.Name.Equals(name) && u.Id != u.Id && !u.IsDeleted))
                ThrowException("您已创建相同名字的歌单");
        }

    }
}
