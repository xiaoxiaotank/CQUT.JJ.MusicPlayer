using CQUT.JJ.MusicPlayer.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Utils.Helpers
{
    public static class GlobalHelper
    {
        public const string Unlogin_User_Name = "未登录用户";

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

        public static int GetCurrentUserId(this ISession session)
        {
            return GetCurrentUser(session).Id;
        }

        public static void SaveCurrentUser(this ISession session, UserModel user)
        {
            var jsonValue = JsonConvert.SerializeObject(user);
            session.SetString("User", jsonValue);
        }

        /// <summary>
        /// 获取Controller特性
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static object[] GetControllerAttributesByContext(this ActionContext context, Type attributeType,bool inherit = true)
        {
            var attributes = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(attributeType, inherit);
            return attributes;
        }

        /// <summary>
        /// 获取Action特性
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static object[] GetActionAttributesByContext(this ActionContext context,Type attributeType,bool inherit = true)
        {
            var attributes = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(attributeType, inherit);
            return attributes;
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

        /// <summary>
        /// 获取国家键值对列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string,string>> GetNationalities()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("中国","中国"),
                new KeyValuePair<string, string>("美国","美国"),
                new KeyValuePair<string, string>("英国","英国"),
                new KeyValuePair<string, string>("日本","日本"),
                new KeyValuePair<string, string>("澳大利亚","澳大利亚"),
                new KeyValuePair<string, string>("韩国","韩国"),
                new KeyValuePair<string, string>("挪威","挪威"),
            };
        }
    }

   
}
