using CQUT.JJ.MusicPlayer.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Utils.Helpers
{
    public static class GlobalHelper
    {
        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static UserModel GetCurrentUser(this ISession session)
        {
            var jsonValue = session.GetString("User");
            var user = jsonValue != null ? JsonConvert.DeserializeObject<UserModel>(jsonValue) : default(UserModel);
            return user;
        }

        public static void SaveCurrentUser(this ISession session, UserModel user)
        {
            var jsonValue = JsonConvert.SerializeObject(user);
            session.SetString("User", jsonValue);
        }

        /// <summary>
        /// 判断是否为ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return (header != null && header == "XMLHttpRequest") ? true : false;
        }
    }

   
}
