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
            throw new NotImplementedException();
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
                    CreationTime = s.CreationTime
                });
        }

        public MusicModel GetMusicById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MusicModel> GetMusicsByAlbumId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MusicModel> GetMusicsBySingerId(int id)
        {
            throw new NotImplementedException();
        }

        public MusicModel Update(MusicModel model)
        {
            throw new NotImplementedException();
        }
    }
}
