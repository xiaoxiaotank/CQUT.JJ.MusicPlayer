using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface IAlbumAppService
    {
        /// <summary>
        /// 创建专辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        AlbumModel Create(AlbumModel model);

        /// <summary>
        /// 更新基本信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        AlbumModel UpdateBasic(AlbumModel model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AlbumModel Publish(int id,int userId);

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        AlbumModel Unpublish(int id,int userId);

        /// <summary>
        /// 获取未发布专辑信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<AlbumModel> GetUnpublishedAlbums();

        /// <summary>
        /// 获取已发布专辑信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<AlbumModel> GetPublishedAlbums();

        /// <summary>
        /// 通过id获取歌唱家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AlbumModel GetAlbumById(int id);
    }
}
