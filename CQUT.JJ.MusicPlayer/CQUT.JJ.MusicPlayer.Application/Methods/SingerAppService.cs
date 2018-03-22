﻿using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
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

        public SingerModel Publish(int id)
        {
            var singer = _singerManager.Pubulish(id);
            return new SingerModel()
            {
                Id = singer.Id,
                Name = singer.Name,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality,
                CreationTime = singer.CreationTime,
                PublishmentTime = singer.PublishmentTime,
                LastModificationTime = singer.LastModificationTime
            };
        }

        public SingerModel Unpublish(int id)
        {
            var singer = _singerManager.Unpublish(id);
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

        public SingerModel UpdateBasic(SingerModel model)
        {
            var singer = _singerManager.UpdateBasic(model);
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

        public IEnumerable<SingerModel> GetUnpublishedSingers()
        {
            return _ctx.Singer.Where(s => !s.IsPublished && !s.IsDeleted)
                .Select(r => new SingerModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    ForeignName = r.ForeignName,
                    Nationality = r.Nationality,
                    CreationTime = r.CreationTime,
                    LastModificationTime = r.LastModificationTime
                });
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
