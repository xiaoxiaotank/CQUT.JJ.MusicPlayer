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
                  Name = r.Name,
                  SingerName = r.Singer.Name,
                  CreationTime = r.CreationTime,
                  LastModificationTime = r.LastModificationTime
              });
        }

        public AlbumModel Publish(int id)
        {
            var album = _albumManager.Publish(id);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.SingerId,
                Name = album.Name,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime,
                PublishmentTime = album.PublishmentTime
            };
        }

        public AlbumModel Unpublish(int id)
        {
            var album = _albumManager.Unpublish(id);
            return new AlbumModel()
            {
                Id = album.Id,
                SingerId = album.SingerId,
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
                Name = album.Name,
                SingerName = album.Singer.Name,
                CreationTime = album.CreationTime,
                LastModificationTime = album.LastModificationTime,
            };
        }
    }
}
