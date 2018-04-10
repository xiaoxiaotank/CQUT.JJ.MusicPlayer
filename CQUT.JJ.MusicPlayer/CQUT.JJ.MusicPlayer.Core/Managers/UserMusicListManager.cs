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

        private void ValidateForCreate(UserMusicList model)
        {
            if (JMDbContext.User.SingleOrDefault(u => u.Id == model.UserId && !u.IsAdmin && !u.IsDeleted) == null)
                ThrowException("用户不存在!");
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("歌单名不能为空");
            if (model.Name.Length > 20)
                ThrowException("歌单名最多只能输入20个文字");
        }
    }
}
