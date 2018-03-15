using CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager;
using CQUT.JJ.MusicPlayer.MS.Entities;
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

            //用户没有登录
            if (user == null)
            {
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
