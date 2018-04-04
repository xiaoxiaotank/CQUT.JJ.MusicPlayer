using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
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
        bool IsUserLike(int userId,int objId, MusicRequestType type);
    }
}
