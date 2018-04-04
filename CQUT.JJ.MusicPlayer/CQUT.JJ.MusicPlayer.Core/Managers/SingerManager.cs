using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class SingerManager : BaseManager<Singer>
    {
        private readonly UserManager _userManager;
        private readonly AlbumManager _albumManager;
        private readonly MusicManager _musicManager;

        public SingerManager(JMDbContext ctx, UserManager userManager,AlbumManager albumManager,MusicManager musicManager) : base(ctx)
        {
            _userManager = userManager;
            _albumManager = albumManager;
            _musicManager = musicManager;
        }

        public Singer Create(SingerModel model)
        {
            _userManager.ValidAdminByUserId(model.CreatorId);
            ValidForCreate(model);

            var singer = new Singer()
            {
                CreatorId = model.CreatorId,
                Name = model.Name,
                ForeignName = model.ForeignName,
                Nationality = model.Nationality,
                CreationTime = DateTime.Now,
                IsPublished = false,
            };

            return Create(singer);
        }

        /// <summary>
        /// 更新基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Singer UpdateBasic(SingerModel model)
        {
            _userManager.ValidAdminByUserId((int)model.MenderId);
            ValidForUpdateBasic(model,out Singer singer);

            singer.MenderId = model.MenderId;
            singer.ForeignName = model.ForeignName;
            singer.Nationality = model.Nationality;
            singer.LastModificationTime = DateTime.Now;

            Save();
            return singer;
        }

        public void Delete(int id)
        {
            ValidForDelete(id, out Singer singer);

            singer.IsDeleted = true;
            singer.LastModificationTime
                = singer.DeletionTime
                = DateTime.Now;

            //删除专辑
            JMDbContext.Album
                .Where(a => a.SingerId == singer.Id && !a.IsDeleted)
                .ToList()
                .ForEach(a => _albumManager.Delete(a.Id));

            //删除音乐
            JMDbContext.Music
                .Where(m => m.SingerId == singer.Id && !m.IsDeleted)
                .ToList()
                .ForEach(m => _musicManager.Delete(m.Id));

            Save();
        }

        public Singer Publish(int id,int userId)
        {
            _userManager.ValidAdminByUserId(userId);
            var singer = JMDbContext.Singer.SingleOrDefault(s => s.Id == id && !s.IsDeleted);
            if (singer == null)
                ThrowException("歌唱家不存在！");
            if (singer.IsPublished)
                ThrowException("歌唱家已发布，无需再次操作！");

            singer.IsPublished = true;
            singer.PublisherId = userId;
            singer.PublishmentTime 
                = singer.LastModificationTime
                = DateTime.Now;

            Save();
            return singer;
        }

        public Singer Unpublish(int id,int userId)
        {
            _userManager.ValidAdminByUserId(userId);
            var singer = Find(id);
            if (!singer.IsPublished)
                ThrowException("歌唱家属于未发布状态，无需再次操作！");

            singer.UnpublisherId = userId;
            singer.IsPublished = false;
            singer.LastModificationTime = DateTime.Now;
            singer.PublishmentTime = null;

            //下架专辑
            JMDbContext.Album
                .Where(a => a.SingerId == singer.Id && a.IsPublished && !a.IsDeleted)
                .ToList()
                .ForEach(a => _albumManager.Unpublish(a.Id,userId));

            //下架音乐
            JMDbContext.Music
                .Where(m => m.SingerId == singer.Id && m.IsPublished && !m.IsDeleted)
                .ToList()
                .ForEach(m => _musicManager.Unpublish(m.Id,userId));


            Save();
            return singer;
        }

        public override Singer Find(int id)
        {
            var singer = base.Find(id);
            if (singer == null || singer.IsDeleted)
                ThrowException("歌唱家不存在");
            return singer;
        }

        private void ValidForCreate(SingerModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("姓名不能为空!");
            if (model.Name.Length > 32)
                ThrowException("姓名不能超过32个字符!");
            // ? 操作符  如果为null则返回false
            if (model.ForeignName?.Length > 32)
                ThrowException("外文名不能超过32个字符");
            if (string.IsNullOrWhiteSpace(model.Nationality))
                ThrowException("国籍不能为空");
            if (model.Nationality.Length > 32)
                ThrowException("国籍不能超过32个字符");

        }

        private void ValidForUpdateBasic(SingerModel model,out Singer singer)
        {
            ValidForDelete(model.Id, out singer);
            if (model.ForeignName?.Length > 32)
                ThrowException("外文名不能超过32个字符");
            if (string.IsNullOrWhiteSpace(model.Nationality))
                ThrowException("国籍不能为空");
            if (model.Nationality.Length > 32)
                ThrowException("国籍不能超过32个字符");
        }

        private void ValidForDelete(int id,out Singer singer)
        {
            singer = JMDbContext.Singer.SingleOrDefault(s => s.Id == id && !s.IsDeleted);
            if (singer == null)
                ThrowException("歌唱家不存在！");
            if (singer.IsPublished)
                ThrowException("歌唱家已发布，请撤销后进行该操作!");
        }

    }
}
