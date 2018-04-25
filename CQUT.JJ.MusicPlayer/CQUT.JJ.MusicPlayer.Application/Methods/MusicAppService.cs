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
                CreatorId = music.CreatorId,
                CreatorName = music.Creator.UserName,
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
                    CreatorId = s.CreatorId,
                    MenderId = s.MenderId,
                    UnpublisherId = s.UnpublisherId,
                    Name = s.Name,
                    SingerName = s.Singer.Name,
                    AlbumName = s.Album.Name,
                    CreatorName = s.Creator.UserName,
                    MenderName = s.Mender.UserName,
                    UnpublisherName = s.Unpublisher.UserName,
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
                    PublisherId = (int)s.PublisherId,
                    PublisherName = s.Publisher.UserName,
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
                    MenderId = music.Mender.Id,
                    Name = music.Name,
                    SingerName = music.Singer.Name,
                    AlbumName = music.Album.Name,
                    MenderName = music.Mender.UserName,
                    Duration = music.Duration,
                    FileUrl = music.FileUrl,
                    CreationTime = music.CreationTime,
                    LastModificationTime = music.LastModificationTime,                    
                };
        }

        public MusicModel Publish(int id, int userId)
        {
            var music = _musicManager.Publish(id,userId);
            return new MusicModel()
            {
                Id = music.Id,
                SingerId = music.SingerId,
                AlbumId = music.AlbumId,
                PublisherId = (int)music.PublisherId,
                Name = music.Name,
                PublisherName = music.Publisher.UserName,
                SingerName = music.Singer.Name,
                AlbumName = music.Album.Name,
                CreationTime = music.CreationTime,
                LastModificationTime = music.LastModificationTime,
                PublishmentTime = music.PublishmentTime
            };
        }

        public MusicModel Unpublish(int id, int userId)
        {
            var music = _musicManager.Unpublish(id,userId);
            return new MusicModel()
            {
                Id = music.Id,
                SingerId = music.SingerId,
                AlbumId = music.AlbumId,
                UnpublisherId = music.UnpublisherId,
                UnpublisherName = music.Unpublisher.UserName,
                Name = music.Name,
                SingerName = music.Singer.Name,
                AlbumName = music.Album.Name,
                CreationTime = music.CreationTime,
                LastModificationTime = music.LastModificationTime,
            };
        }

        public Dictionary<DateTime, int> GetPublishedMusicCountPerDay(int dayNumber)
        {
            var today = DateTime.Now.Date;
            var dates = new List<DateTime>();
            for (int i = 0; i < dayNumber; i++)
            {
                dates.Add(today.AddDays(-i));
            }

            var data = _ctx.Music.Where(a => a.IsPublished
                    && !a.IsDeleted
                    && a.PublishmentTime != null
                    && dates.Contains(((DateTime)a.PublishmentTime).Date))
                .GroupBy(a => ((DateTime)a.PublishmentTime).Date)
                .Select(a => new KeyValuePair<DateTime, int>(a.Key, a.Count()));

            var result = data.ToDictionary(a => a.Key, b => b.Value);
            return result;
        }
    }
}
