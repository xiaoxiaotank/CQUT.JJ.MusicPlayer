﻿using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.Models;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Extensions.PageExtension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Search;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;

/// <summary>
/// 调用方需要引用EFCore及其SqlServer包 并且需要将Remotion.Linq更新到最新版
/// </summary>
namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“MusicSearchService”。
    public class SearchService : ISearchService
    {
        private JMDbContext _ctx;
        public SearchService()
        {
            _ctx = new JMDbContext();
        }

        public MusicContract GetMusicInfoById(int id)
        {
            var music = _ctx.Music.Include(m => m.Singer).Include(m => m.Album).SingleOrDefault(s => s.Id == id && s.IsPublished && !s.IsDeleted);
            if (music == null) return null;

            return new MusicContract()
            {
                Id = music.Id,
                SingerId = music.SingerId,
                AlbumId = music.AlbumId,
                Name = music.Name,
                SingerName = music.Singer?.Name,
                AlbumName = music.Album?.Name,
                Duration = music.Duration,
                FileUrl = music.FileUrl
            };
        }

        public PageResult Search(MusicRequestType type, string key, int page, int size = 20)
        {
            PageResult result = null;
            var keys = GlobalHelper.ToSeparateByJieba(key);
            switch (type)
            {
                case MusicRequestType.Song:
                    result = SearchSong(keys, page, size);
                    break;
                case MusicRequestType.Album:
                    result = SearchAlbum(keys, page, size);
                    break;
                case MusicRequestType.PlayList:
                    break;
                case MusicRequestType.MV:
                    break;
                case MusicRequestType.Lyric:
                    break;
                case MusicRequestType.Singer:
                    result = SearchSinger(keys, page, size);
                    break;
                case MusicRequestType.User:
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
        private PageResult SearchSong(string[] keys, int page, int size)
        {
            var pagedList = _ctx.Music.Include(m => m.Singer)
                .Where(m => keys.Any(k => m.Name.Contains(k) || m.Singer.Name.Contains(k)) && !m.IsDeleted && m.IsPublished)
                .OrderByDescending(m => m.PublishmentTime)
                .Select(m => new MusicContract()
                {
                    Id = m.Id,
                    SingerId = m.SingerId,
                    AlbumId = m.AlbumId,
                    Name = m.Name,
                    Duration = m.Duration,
                    FileUrl = m.FileUrl
                }).ToPagedList(page,size);
            var result = new MusicSearchPageResult()
            {
                PageNumber = pagedList.PageNumber,
                PageCount = pagedList.PageCount,
                Results = pagedList.Select(p => 
                {
                    p.SingerName = _ctx.Singer.SingleOrDefault(s => s.Id == p.SingerId && s.IsPublished && !s.IsDeleted)?.Name;
                    p.AlbumName = _ctx.Album.SingleOrDefault(s => s.Id == p.AlbumId && s.IsPublished && !s.IsDeleted)?.Name;
                    return p;
                }),
                ResultType = MusicRequestType.Song
            };
            return result;
        }

        /// <summary>
        /// 搜索歌唱家
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private PageResult SearchSinger(string[] keys, int page, int size)
        {
            var pagedList = _ctx.Singer
                .Where(m => keys.Any(k => m.Name.Contains(k) 
                        ||(m.ForeignName != null && m.ForeignName.ToLower().Contains(k.ToLower()))) 
                    && !m.IsDeleted 
                    && m.IsPublished)
                .OrderByDescending(m => m.PublishmentTime)
                .Select(m => new SingerContract()
                {
                    Id = m.Id,                    
                    Name = m.Name,                    
                }).ToPagedList(page, size);
            var result = new SingerSearchPageResult()
            {
                PageNumber = pagedList.PageNumber,
                PageCount = pagedList.PageCount,
                Results = pagedList,
                ResultType = MusicRequestType.Singer
            };
            return result;
        }

        /// <summary>
        /// 搜索专辑
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private PageResult SearchAlbum(string[] keys, int page, int size)
        {
            var pagedList = _ctx.Album.Include(a => a.Singer)
                .Where(a => keys.Any(k => a.Name.Contains(k))
                    && !a.IsDeleted
                    && a.IsPublished)
                .OrderByDescending(a => a.PublishmentTime)
                .Select(a => new AlbumContract()
                {
                    Id = a.Id,
                    SingerId = a.SingerId,
                    Name = a.Name,
                    SingerName = a.Singer.Name,
                    MusicCount = _ctx.Music.Count(m => m.AlbumId == a.Id && !m.IsDeleted && m.IsPublished),
                    PublishedTime = a.CreationTime
                }).ToPagedList(page, size);
            var result = new AlbumSearchPageResult()
            {
                PageNumber = pagedList.PageNumber,
                PageCount = pagedList.PageCount,
                Results = pagedList,
                ResultType = MusicRequestType.Album
            };
            return result;
        }



        #endregion
    }
}
