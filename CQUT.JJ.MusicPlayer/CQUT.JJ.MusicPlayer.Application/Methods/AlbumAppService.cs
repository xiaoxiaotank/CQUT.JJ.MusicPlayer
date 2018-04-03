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
                CreatorId = album.CreatorId,
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
                SingerId = album.SingerId,
                Name = album.Name,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime
            };
        }

        public IEnumerable<AlbumModel> GetPublishedAlbums()
        {
            return _ctx.Album.Where(a => a.IsPublished && !a.IsDeleted)
               .Select(r => new AlbumModel()
               {
                   Id = r.Id,
                   SingerId = r.SingerId,
                   PublisherId = (int)r.PublisherId,
                   PublisherName = r.Publisher.UserName,
                   Name = r.Name,
                   SingerName = r.Singer.Name,
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
                  SingerId = r.SingerId,
                  CreatorId = r.CreatorId,
                  MenderId = (int)r.MenderId,
                  UnpublisherId = (int)r.UnpublisherId,
                  Name = r.Name,
                  CreatorName = r.Creator.UserName,
                  MenderName = r.Mender.UserName,
                  UnpublisherName = r.Unpublisher.UserName,
                  SingerName = r.Singer.Name,
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
