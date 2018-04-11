using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CQUT.JJ.MusicPlayer.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IUserService”。
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserContract Login(string userName, string password);
    }
}
