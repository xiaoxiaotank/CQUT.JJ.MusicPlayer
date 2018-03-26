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
    public class MusicAppService : IMusicAppService
    {
        private readonly JMDbContext _ctx;
        private readonly MusicManager _musicManager;

        public MusicAppService(JMDbContext ctx, MusicManager musicManager)
        {
            _ctx = ctx;
            _musicManager = musicManager;
        }

        public MusicModel Create(MusicModel model)
        {
            var music = _musicManager.Create(model);
            return new MusicModel()
            {
                Id = music.Id,
                SingerId = music.SingerId,
                AlbumId = music.AlbumId,
                Name = music.Name,
                SingerName = music.Singer.Name,
                AlbumName = music.Album.Name,
                Duration = music.Duration,
                FileUrl = music.FileUrl,
                CreationTime = music.CreationTime
            };
        }

        public void Delete(int id)
        {
            _musicManager.Delete(id);
        }

        public IEnumerable<MusicModel> GetUnpublishedMusics()
        {
            var musicList = _ctx.Music.Where(m => !m.IsPublished && !m.IsDeleted);
            return musicList
                .Select(s => new MusicModel()
                {
                    Id = s.Id,
                    SingerId = s.SingerId,
                    AlbumId = s.AlbumId,
                    Name = s.Name,
                    SingerName = s.Singer.Name,
                    AlbumName = s.Album.Name,
                    Duration = s.Duration,
                    FileUrl = s.FileUrl,
                    CreationTime = s.CreationTime,
                    LastModificationTime = s.LastModificationTime
                });
        }


        public IEnumerable<MusicModel> GetPublishedMusics()
        {
            var musicList = _ctx.Music.Where(m => m.IsPublished && !m.IsDeleted);
            return musicList
                .Select(s => new MusicModel()
                {
                    Id = s.Id,
                    SingerId = s.SingerId,
                    AlbumId = s.AlbumId,
                    Name = s.Name,
                    SingerName = s.Singer.Name,
                    AlbumName = s.Album.Name,
                    Duration = s.Duration,
                    FileUrl = s.FileUrl,
                    CreationTime = s.CreationTime,
                    PublishmentTime = s.PublishmentTime,
                });
        }

        public MusicModel GetMusicById(int id)
        {
            var music = _musicManager.Find(id);
            return music == null ? null
                : new MusicModel()
                {
                    Id = music.Id,
                    SingerId = music.SingerId,
                    AlbumId = music.AlbumId,
                    Name = music.Name,
                    SingerName = music.Singer.Name,
                    AlbumName = music.Album.Name,
                    Duration = music.Duration,
                    FileUrl = music.FileUrl,
                    CreationTime = music.CreationTime,
                    LastModificationTime = music.LastModificationTime,
                    PublishmentTime = music.PublishmentTime,
                };
        }

        public IEnumerable<MusicModel> GetMusicsByAlbumId(int id)
        {
            return _ctx.Music.Where(m => m.AlbumId == id && !m.IsDeleted)?
                .Select(music => new MusicModel()
                {
                    Id = music.Id,
                    SingerId = music.SingerId,
                    AlbumId = music.AlbumId,
                    Name = music.Name,
                    SingerName = music.Singer.Name,
                    AlbumName = music.Album.Name,
                    Duration = music.Duration,
                    FileUrl = music.FileUrl,
                    CreationTime = music.CreationTime,
                    LastModificationTime = music.LastModificationTime,
                    PublishmentTime = music.PublishmentTime,
                });
        }

        public IEnumerable<MusicModel> GetMusicsBySingerId(int id)
        {
            return _ctx.Music.Where(m => m.SingerId == id && !m.IsDeleted)?
                .Select(music => new MusicModel()
                {
                    Id = music.Id,
                    SingerId = music.SingerId,
                    AlbumId = music.AlbumId,
                    Name = music.Name,
                    SingerName = music.Singer.Name,
                    AlbumName = music.Album.Name,
                    Duration = music.Duration,
                    FileUrl = music.FileUrl,
                    CreationTime = music.CreationTime,
                    LastModificationTime = music.LastModificationTime,
                    PublishmentTime = music.PublishmentTime,
                });
        }

        public MusicModel UpdateBasic(MusicModel model)
        {
            var music = _musicManager.UpdateBasic(model);
            return music == null ? null
                : new MusicModel()
                {
                    Id = music.Id,
                    SingerId = music.SingerId,
                    AlbumId = music.AlbumId,
                    Name = music.Name,
                    SingerName = music.Singer.Name,
                    AlbumName = music.Album.Name,
                    Duration = music.Duration,
                    FileUrl = music.FileUrl,
                    CreationTime = music.CreationTime,
                    LastModificationTime = music.LastModificationTime,                    
                };
        }

        public MusicModel Publish(int id)
        {
            var music = _musicManager.Publish(id);
            return new MusicModel()
            {
                Id = music.Id,
                SingerId = music.SingerId,
                AlbumId = music.AlbumId,
                Name = music.Name,
                SingerName = music.Singer.Name,
                AlbumName = music.Album.Name,
                CreationTime = music.CreationTime,
                LastModificationTime = music.LastModificationTime,
                PublishmentTime = music.PublishmentTime
            };
        }

        public MusicModel Unpublish(int id)
        {
            var music = _musicManager.Unpublish(id);
            return new MusicModel()
            {
                Id = music.Id,
                SingerId = music.SingerId,
                AlbumId = music.AlbumId,
                Name = music.Name,
                SingerName = music.Singer.Name,
                AlbumName = music.Album.Name,
                CreationTime = music.CreationTime,
                LastModificationTime = music.LastModificationTime,
            };
        }

    }
}
