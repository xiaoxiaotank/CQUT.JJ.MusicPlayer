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
        public SingerManager(JMDbContext ctx) : base(ctx)
        {
        }

        public Singer Create(SingerModel model)
        {
            ValidForCreate(model);

            var singer = new Singer()
            {
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
            ValidForUpdateBasic(model,out Singer singer);

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

            Save();
        }

        public Singer Publish(int id)
        {
            var singer = JMDbContext.Singer.SingleOrDefault(s => s.Id == id && !s.IsDeleted);
            if (singer == null)
                ThrowException("歌唱家不存在！");
            if (singer.IsPublished)
                ThrowException("歌唱家已发布，无需再次操作！");

            singer.IsPublished = true;
            singer.PublishmentTime 
                = singer.LastModificationTime
                = DateTime.Now;

            Save();
            return singer;
        }

        public Singer Unpublish(int id)
        {
            var singer = Find(id);
            if (!singer.IsPublished)
                ThrowException("歌唱家属于未发布状态，无需再次操作！");

            singer.IsPublished = false;
            singer.LastModificationTime = DateTime.Now;
            singer.PublishmentTime = null;

            //下架专辑
            JMDbContext.Album
                .Where(a => a.SingerId == singer.Id && a.IsPublished && !a.IsDeleted)
                .ToList()
                .ForEach(a =>
                {
                    a.IsPublished = false;
                    a.LastModificationTime = DateTime.Now;
                    a.PublishmentTime = null;
                });

            //下架音乐
            JMDbContext.Music
                .Where(m => m.SingerId == singer.Id && m.IsPublished && !m.IsDeleted)
                .ToList()
                .ForEach(m =>
                {
                    m.IsPublished = false;
                    m.LastModificationTime = DateTime.Now;
                    m.PublishmentTime = null;
                });


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
