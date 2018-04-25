using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface ISingerAppService
    {
        /// <summary>
        /// 创建歌唱家
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        SingerModel Create(SingerModel model);

        /// <summary>
        /// 更新基本信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        SingerModel UpdateBasic(SingerModel model);

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
        SingerModel Publish(int id,int userId);

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        SingerModel Unpublish(int id,int userId);

        /// <summary>
        /// 获取未发布歌唱家信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<SingerModel> GetUnpublishedSingers();

        /// <summary>
        /// 获取已发布歌唱家信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<SingerModel> GetPublishedSingers();

        /// <summary>
        /// 获取已发布的且发布了专辑的歌唱家信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<SingerModel> GetPublishedSingersHasAlbums();

        /// <summary>
        /// 通过id获取歌唱家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SingerModel GetSingerById(int id);

        /// <summary>
        /// 根据天数获取前每一天发布的歌唱家数
        /// key:日期，value:数量
        /// </summary>
        /// <param name="dayNumber"></param>
        /// <returns></returns>
        Dictionary<DateTime,int> GetPublishedSingerCountPerDay(int dayNumber);
    }
}
