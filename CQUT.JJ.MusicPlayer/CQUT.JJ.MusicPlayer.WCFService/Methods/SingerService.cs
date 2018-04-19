using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“SingerService”。
    public class SingerService : ISingerService
    {
        private readonly JMDbContext _ctx;
        private readonly UserManager _userManager;
        private readonly AlbumManager _albumManager;
        private readonly MusicManager _musicManager;
        private readonly SingerManager _singerManager;

        public SingerService()
        {
            _ctx = new JMDbContext();
            _userManager = new UserManager(_ctx);
            _musicManager = new MusicManager(_ctx, _userManager);
            _albumManager = new AlbumManager(_ctx, _userManager, _musicManager);
            _singerManager = new SingerManager(_ctx, _userManager, _albumManager, _musicManager);
        }

        public IEnumerable<SingerContract> GetHotSingersOfCount(int singerCount)
        {
            var singers = _ctx.Singer.Where(s => s.IsPublished && !s.IsDeleted)?
                .Take(singerCount)?
                .Select(s => new SingerContract()
                {
                    Id = s.Id,
                    Name = s.Name,
                });
            return singers;
        }

        public SingerContract GetSingerById(int id)
        {
            var singer = _ctx.Singer.SingleOrDefault(s => s.Id == id && s.IsPublished && !s.IsDeleted);
            return new SingerContract()
            {
                Id = singer.Id,
                Name = singer.Name,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality
            };
        }
    }
}
