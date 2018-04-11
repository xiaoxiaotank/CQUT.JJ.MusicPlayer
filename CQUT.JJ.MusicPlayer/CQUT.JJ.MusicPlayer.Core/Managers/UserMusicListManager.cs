using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class UserMusicListManager : BaseManager<UserMusicList>
    {
        public UserMusicListManager(JMDbContext ctx) : base(ctx)
        {
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
