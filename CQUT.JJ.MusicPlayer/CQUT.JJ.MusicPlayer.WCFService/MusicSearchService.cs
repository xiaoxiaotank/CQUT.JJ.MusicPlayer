using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

/// <summary>
/// 调用方需要引用EFCore及其SqlServer包 并且需要将Remotion.Linq更新到最新版
/// </summary>
namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“MusicSearchService”。
    public class MusicSearchService : IMusicSearchService
    {
        private JMDbContext _ctx;
        public MusicSearchService()
        {
            _ctx = new JMDbContext();
        }

        public IEnumerable<MusicInfo> Search(MusicSearchType type, string key, int page, int size = 20)
        {
            IEnumerable<MusicInfo> result = null;
            switch (type)
            {
                case MusicSearchType.Song:
                    result = SearchSong(key, page, size);
                    break;
                case MusicSearchType.Album:
                    break;
                case MusicSearchType.PlayList:
                    break;
                case MusicSearchType.MV:
                    break;
                case MusicSearchType.Lyric:
                    break;
                case MusicSearchType.Singer:
                    break;
                case MusicSearchType.User:
                    break;
            }
            return result;
        }

        #region 私有方法

        /// <summary>
        /// 搜索歌曲
        /// </summary>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private IEnumerable<MusicInfo> SearchSong(string key, int page, int size)
        {
            return _ctx.Music.Where(m => !m.IsDeleted && m.IsPublished)
                .Select(m => new MusicInfo()
                {
                    Id = m.Id,
                    Name = m.Name
                });
        }


        #endregion
    }
}
