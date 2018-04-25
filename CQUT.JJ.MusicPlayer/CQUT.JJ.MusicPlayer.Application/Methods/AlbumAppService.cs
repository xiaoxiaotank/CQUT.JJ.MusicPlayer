using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Methods
{
    public class AlbumAppService : IAlbumAppService
    {
        private readonly JMDbContext _ctx;
        private readonly AlbumManager _albumManager;

        public AlbumAppService(JMDbContext ctx, AlbumManager albumManager)
        {
            _ctx = ctx;
            _albumManager = albumManager;
        }

        public AlbumModel Create(AlbumModel model)
        {
            var album = _albumManager.Create(model);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.SingerId,
                CreatorId = album.Creator.Id,
                CreatorName = album.Creator.NickName,
                Name = album.Name,
                SingerName = album.Singer.Name,               
                CreationTime = album.CreationTime
            };
        }

        public void Delete(int id)
        {
            _albumManager.Delete(id);
        }

        public AlbumModel GetAlbumById(int id)
        {
            var album = _albumManager.Find(id);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.Singer.Id,
                Name = album.Name,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime
            };
        }

        public Dictionary<DateTime, int> GetPublishedAlbumCountPerDay(int dayNumber)
        {
            var today = DateTime.Now.Date;
            var dates = new List<DateTime>();
            for (int i = 0; i < dayNumber; i++)
            {
                dates.Add(today.AddDays(-i));
            }

            var data = _ctx.Album.Where(a => a.IsPublished
                    && !a.IsDeleted
                    && a.PublishmentTime != null
                    && dates.Contains(((DateTime)a.PublishmentTime).Date))
                .GroupBy(a => ((DateTime)a.PublishmentTime).Date)
                .Select(a => new KeyValuePair<DateTime, int>(a.Key, a.Count()));

            var result = data.ToDictionary(a => a.Key,b => b.Value);
            return result;
        }

        public IEnumerable<AlbumModel> GetPublishedAlbums()
        {
            return _ctx.Album.Where(a => a.IsPublished && !a.IsDeleted)
               .Select(r => new AlbumModel()
               {
                   Id = r.Id,
                   SingerId = r.SingerId,
                   PublisherId = (int)r.Publisher.Id,                  
                   Name = r.Name,
                   SingerName = r.Singer.Name,
                   PublisherName = r.Publisher.UserName,
                   CreationTime = r.CreationTime,
                   PublishmentTime = r.PublishmentTime
               });
        }

        public IEnumerable<AlbumModel> GetUnpublishedAlbums()
        {
            return _ctx.Album.Where(a => !a.IsPublished && !a.IsDeleted)
              .Select(r => new AlbumModel()
              {
                  Id = r.Id,
                  SingerId = r.Singer.Id,
                  CreatorId = r.CreatorId,
                  MenderId = (int)r.MenderId,
                  UnpublisherId = (int)r.UnpublisherId,
                  Name = r.Name,
                  SingerName = r.Singer.Name,
                  CreatorName = r.Creator.UserName,
                  MenderName = r.Mender.UserName,
                  UnpublisherName = r.Unpublisher.UserName,
                  CreationTime = r.CreationTime,
                  LastModificationTime = r.LastModificationTime
              });
        }

        public AlbumModel Publish(int id, int userId)
        {
            var album = _albumManager.Publish(id,userId);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.SingerId,
                PublisherId = (int)album.PublisherId,
                Name = album.Name,
                PublisherName = album.Publisher.UserName,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime,
                PublishmentTime = album.PublishmentTime
            };
        }

        public AlbumModel Unpublish(int id,int userId)
        {
            var album = _albumManager.Unpublish(id, userId);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.SingerId,
                UnpublisherId = (int)album.UnpublisherId,
                UnpublisherName = album.Unpublisher.UserName,
                Name = album.Name,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime,
            };
        }

        public AlbumModel UpdateBasic(AlbumModel model)
        {
            var album = _albumManager.UpdateBasic(model);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.SingerId,
                MenderId = album.Mender.Id,
                Name = album.Name,
                MenderName = album.Mender.UserName,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime,
            };
        }
    }
}
