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
        /// <returns></returns>
        SingerModel Publish(int id);

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SingerModel Unpublish(int id);

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
        /// 通过id获取歌唱家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SingerModel GetSingerById(int id);
    }
}
