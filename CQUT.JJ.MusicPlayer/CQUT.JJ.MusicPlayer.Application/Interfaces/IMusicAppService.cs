using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface IMusicAppService
    {
        /// <summary>
        /// 创建音乐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        MusicModel Create(MusicModel model);

        /// <summary>
        /// 更新音乐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        MusicModel UpdateBasic(MusicModel model);

        /// <summary>
        /// 删除音乐
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// 获取未发布音乐
        /// </summary>
        /// <returns></returns>
        IEnumerable<MusicModel> GetUnpublishedMusics();

        /// <summary>
        /// 获取已发布音乐
        /// </summary>
        /// <returns></returns>
        IEnumerable<MusicModel> GetPublishedMusics();

        /// <summary>
        /// 通过id获取音乐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MusicModel GetMusicById(int id);

        /// <summary>
        /// 通过歌唱家id获取音乐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<MusicModel> GetMusicsBySingerId(int id);

        /// <summary>
        /// 通过专辑Id获取音乐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<MusicModel> GetMusicsByAlbumId(int id);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        MusicModel Publish(int id,int userId);

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        MusicModel Unpublish(int id,int userId);

        /// <summary>
        /// 根据天数获取前每一天发布的音乐数
        /// </summary>
        /// <param name="dayNumber"></param>
        /// <returns></returns>
        Dictionary<DateTime, int> GetPublishedMusicCountPerDay(int dayNumber);
    }
}
