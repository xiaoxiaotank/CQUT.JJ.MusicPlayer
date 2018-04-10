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
        /// 通过userId获取用户音乐列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<UserMusicListInfo> GetUserMusicListByUserId(int userId);

        /// <summary>
        /// 创建用户歌单
        /// </summary>
        /// <param name="userMusicList"></param>
        /// <returns></returns>
        [OperationContract]
        UserMusicListInfo Create(UserMusicListInfo userMusicList);
    }
}
