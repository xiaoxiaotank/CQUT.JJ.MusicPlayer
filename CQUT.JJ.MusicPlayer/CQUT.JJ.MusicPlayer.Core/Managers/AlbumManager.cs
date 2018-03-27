using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class AlbumManager : BaseManager<Album>
    {
        public AlbumManager(JMDbContext ctx) : base(ctx)
        {
        }

        public Album Create(AlbumModel model)
        {
            ValidForCreate(model);

            var album = new Album()
            {
                SingerId = model.SingerId,
                Name = model.Name,
                IsPublished = false,
                IsDeleted = false,
                CreationTime = DateTime.Now,               
            };

            return Create(album);
        }

        public Album UpdateBasic(AlbumModel model)
        {
            ValidForUpdateBasic(model, out Album album);

            album.Name = model.Name;
            album.SingerId = model.SingerId;
            album.LastModificationTime = DateTime.Now;

            Save();
            return album;
        }

        public void Delete(int id)
        {
            ValidForDelete(id, out Album album);

            album.IsDeleted = true;
            album.LastModificationTime
                = album.DeletionTime
                = DateTime.Now;

            JMDbContext.Music
                .Where(m => m.AlbumId == album.Id && !m.IsDeleted)
                .ToList()
                .ForEach(m =>
                {
                    m.IsPublished = false;
                    m.IsDeleted = true;
                    m.DeletionTime 
                        = m.LastModificationTime 
                        = DateTime.Now;
                    m.PublishmentTime = null;
                });

            Save();
        }

        public Album Publish(int id)
        {
            var album = Find(id);
            if (album.IsPublished)
                ThrowException("专辑已发布，无需再次操作！");

            album.IsPublished = true;
            album.PublishmentTime
                = album.LastModificationTime
                = DateTime.Now;

            Save();
            return album;
        }

        public Album Unpublish(int id)
        {
            var album = Find(id);
            if (!album.IsPublished)
                ThrowException("专辑属于未发布状态，无需再次操作！");

            album.IsPublished = false;
            album.LastModificationTime = DateTime.Now;
            album.PublishmentTime = null;

            JMDbContext.Music
                .Where(m => m.AlbumId == album.Id && m.IsPublished && !m.IsDeleted)
                .ToList()
                .ForEach(m =>
                {
                    m.IsPublished = false;
                    m.LastModificationTime = DateTime.Now;
                    m.PublishmentTime = null;
                });

            Save();
            return album;
        }

        public override Album Find(int id)
        {
            var album = JMDbContext.Album.Include(a => a.Singer)
                .SingleOrDefault(s => s.Id == id && !s.IsDeleted);
            if (album == null)
                ThrowException("专辑不存在");
            return album;
        }

        private void ValidForCreate(AlbumModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("专辑名不能为空!");
            if (model.Name.Length > 32)
                ThrowException("专辑名不能超过32个字符!");
            var singer = JMDbContext.Singer.SingleOrDefault(s => s.Id == model.SingerId && !s.IsDeleted);
            if (singer == null)
                ThrowException("歌唱家不存在!");
            if (!singer.IsPublished)
                ThrowException("歌唱家为未发布状态，请发布后再添加该专辑");
        }

        private void ValidForUpdateBasic(AlbumModel model, out Album album)
        {
            ValidForDelete(model.Id, out album);
            ValidForCreate(model);
        }

        private void ValidForDelete(int id, out Album album)
        {
            album = Find(id);
            if (album.IsPublished)
                ThrowException("专辑已发布，请撤销后进行该操作!");
        }
    }
}
