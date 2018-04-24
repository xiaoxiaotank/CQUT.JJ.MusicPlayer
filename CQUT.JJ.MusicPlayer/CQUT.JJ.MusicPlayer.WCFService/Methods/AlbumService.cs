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
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“AlbumService”。
    public class AlbumService : IAlbumService
    {
        private readonly JMDbContext _ctx;
        private readonly UserManager _userManager;
        private readonly AlbumManager _albumManager;
        private readonly MusicManager _musicManager;

        public AlbumService()
        {
            _ctx = new JMDbContext();
            _userManager = new UserManager(_ctx);
            _musicManager = new MusicManager(_ctx, _userManager);
            _albumManager = new AlbumManager(_ctx, _userManager, _musicManager);
        }

        public AlbumContract GetAlbumById(int id)
        {
            var album = _ctx.Album.Include(a => a.Singer).SingleOrDefault(s => s.Id == id && s.IsPublished && !s.IsDeleted);
            return new AlbumContract()
            {
                Id = album.Id,
                SingerId = album.Singer.Id,                
                Name = album.Name,
                SingerName = album.Singer.Name
            };
        }
    }
}
