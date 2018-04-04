using CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Filters
{
    public class MvcAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string PermissionCode { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Session.GetCurrentUser();

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();
            var area = context.RouteData.Values["area"]?.ToString();

            //用户没有登录
            if (user == null)
            {
                Task.Factory.StartNew(() => LogHelper.Log(new LogItemEntity($"{GlobalHelper.Unlogin_User_Name} 未被授权，已被阻止"
                        , GlobalHelper.Unlogin_User_Name
                        , LogType.Warning
                        , $"{area}.{controller}.{action}")));
                var returnUrl = "/Admin/Account/Login";

                context.Result = context.HttpContext.Request.IsAjaxRequest() ?
                      new ContentResult() { Content = $@"<script>location.href=""{returnUrl}""</script>" }
                    : (IActionResult)new RedirectResult(returnUrl);    
                return;
            }
            if (string.IsNullOrWhiteSpace(PermissionCode))
                return;

            var permissionManager = context.HttpContext.RequestServices.GetService(typeof(IPermissionManager)) as IPermissionManager;
            
            if (!permissionManager.IsGranted(user.Id, PermissionCode))
            {
                LogHelper.Log(new LogItemEntity($"{user.UserName} 尝试访问{controller}.{action},未被授权，已被阻止"
                       , GlobalHelper.Unlogin_User_Name
                       , LogType.Warning
                       , $"{controller}.{action}"));
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result = new JsonResult(new JsonResultEntity(){ IsSuccessed = false, Message = "您没有权限进行该操作！" }); 
                }
                else
                {
                    context.HttpContext.Session.SetString("ErrorMessage", "您没有权限进行该操作");
                    context.Result = new RedirectResult("/Admin/Shared/Error");
                }
            }
        }
    }
}
