using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    [ServiceContract]
    public interface IUserMusicListService
    {
        /// <summary>
        /// 通过userId获取用户歌单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<UserMusicListContract> GetUserMusicListByUserId(int userId);

        /// <summary>
        /// 通过id获取用户歌单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserMusicListContract GetUserMusicListById(int id);

        /// <summary>
        /// 创建用户歌单
        /// </summary>
        /// <param name="userMusicList"></param>
        /// <returns></returns>
        [OperationContract]
        UserMusicListContract Create(UserMusicListContract userMusicList);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        [OperationContract]
        void Update(int id, string name);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void Delete(int id);

        [OperationContract]
        void AddToUserMusicList(int userId, int objId, int musicListId, MusicRequestType type);
    }
}
