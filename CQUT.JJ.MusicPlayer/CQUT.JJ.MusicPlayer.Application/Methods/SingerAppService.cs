using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Methods
{
    public class SingerAppService : ISingerAppService
    {
        private readonly JMDbContext _ctx;
        private readonly SingerManager _singerManager;

        public SingerAppService(JMDbContext ctx, SingerManager singerManager)
        {
            _ctx = ctx;
            _singerManager = singerManager;
        }

        public SingerModel Create(SingerModel model)
        {
            var singer =  _singerManager.Create(model);
            return new SingerModel()
            {
                Id = singer.Id,
                CreatorId = singer.CreatorId,
                CreatorName = singer.Creator.NickName,
                Name = singer.Name,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality,
                CreationTime = singer.CreationTime,
            }
;        }

        public void Delete(int id)
        {
            _singerManager.Delete(id);
        }

        public SingerModel Publish(int id, int userId)
        {
            var singer = _singerManager.Publish(id,userId);
            return new SingerModel()
            {
                Id = singer.Id,
                PublisherId = (int)singer.PublisherId,
                Name = singer.Name,
                PublisherName = singer.Publisher.UserName,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality,
                CreationTime = singer.CreationTime,
                PublishmentTime = singer.PublishmentTime,
                LastModificationTime = singer.LastModificationTime
            };
        }

        public SingerModel Unpublish(int id, int userId)
        {
            var singer = _singerManager.Unpublish(id,userId);
            return new SingerModel()
            {
                Id = singer.Id,
                UnpublisherId = singer.UnpublisherId,
                UnpublisherName = singer.Unpublisher.UserName,
                Name = singer.Name,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality,
                CreationTime = singer.CreationTime,
                LastModificationTime = singer.LastModificationTime
            };
        }

        public SingerModel UpdateBasic(SingerModel model)
        {
            var singer = _singerManager.UpdateBasic(model);
            return new SingerModel()
            {
                Id = singer.Id,
                MenderId = singer.Mender.Id,
                Name = singer.Name,
                MenderName = singer.Mender.UserName,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality,
                CreationTime = singer.CreationTime,
                LastModificationTime = singer.LastModificationTime
            };
        }

        public IEnumerable<SingerModel> GetUnpublishedSingers()
        {
            return _ctx.Singer.Where(s => !s.IsPublished && !s.IsDeleted)
                .Select(r => new SingerModel()
                {
                    Id = r.Id,
                    CreatorId = r.CreatorId,
                    MenderId = (int)r.MenderId,
                    UnpublisherId = (int)r.UnpublisherId,
                    Name = r.Name,
                    CreatorName = r.Creator.UserName,
                    MenderName = r.Mender.UserName,
                    UnpublisherName = r.Unpublisher.UserName,
                    ForeignName = r.ForeignName,
                    Nationality = r.Nationality,
                    CreationTime = r.CreationTime,
                    LastModificationTime = r.LastModificationTime
                });
        }

        public IEnumerable<SingerModel> GetPublishedSingers()
        {
            return _ctx.Singer.Where(s => s.IsPublished && !s.IsDeleted)
                .Select(r => new SingerModel()
                {
                    Id = r.Id,
                    PublisherId = (int)r.PublisherId,
                    PublisherName = r.Publisher.UserName,
                    Name = r.Name,
                    ForeignName = r.ForeignName,
                    Nationality = r.Nationality,
                    CreationTime = r.CreationTime,
                    PublishmentTime = r.PublishmentTime
                });
        }


        public IEnumerable<SingerModel> GetPublishedSingersHasAlbums()
        {
            return _ctx.Singer.Where(s => s.IsPublished && !s.IsDeleted)?
                .Join(_ctx.Album.Where(m => m.IsPublished && !m.IsDeleted)
                    ,s => s.Id,m => m.SingerId,(s,m) =>
                    new SingerModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        ForeignName = s.ForeignName,
                        Nationality = s.Nationality,
                        CreationTime = s.CreationTime,
                        PublishmentTime = s.PublishmentTime
                    })
                .Distinct();
        }

        public SingerModel GetSingerById(int id)
        {
            var singer = _singerManager.Find(id);
            return new SingerModel()
            {
                Id = singer.Id,
                Name = singer.Name,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality,
                CreationTime = singer.CreationTime,
                LastModificationTime = singer.LastModificationTime
            };
        }

    }
}
