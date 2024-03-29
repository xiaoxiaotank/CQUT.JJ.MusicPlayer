﻿using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IMusicService”。
    [ServiceContract]
    public interface IMusicService
    {
        /// <summary>
        /// 添加喜欢
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="objId">添加项的id</param>
        /// <param name="type">音乐请求类型</param>
        [OperationContract]
        void ToggleUserLike(int userId,int objId,MusicRequestType type);

        /// <summary>
        /// 用户是否添加该项目为喜欢
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [OperationContract]
        bool IsUserLike(int userId,int objId, MusicRequestType type);

        /// <summary>
        /// 通过userId获取喜欢的音乐
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<MusicContract> GetLoveMusicsByUserId(int userId);

        /// <summary>
        /// 通过用户列表id获取音乐
        /// </summary>
        /// <param name="musicListId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<MusicContract> GetMusicsByMusicListId(int musicListId);

        /// <summary>
        /// 获取一定数目的最新歌曲
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<MusicContract> GetLastestMusics(int count);

        /// <summary>
        /// 通过id获取音乐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        MusicContract GetMusicById(int id);


        /// <summary>
        /// 通过歌唱家id获取音乐
        /// </summary>
        /// <param name="singerId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<MusicContract> GetMusicsBySingerId(int singerId);

        /// <summary>
        /// 通过专辑id获取音乐
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<MusicContract> GetMusicsByAlbumId(int albumId);

    }
}
