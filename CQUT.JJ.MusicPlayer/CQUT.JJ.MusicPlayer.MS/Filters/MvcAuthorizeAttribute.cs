using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Filters
{
    public class MvcAuthorizeAttribute : IAuthorizationFilter
    {
        public string PermissionCode { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //bool isAllowAnonymous = context.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            //if (isAllowAnonymous)
            //    return;
            //var jsonValue = context.HttpContext.Session.GetCurrentUser();
            //var user = jsonValue != null ? JsonConvert.DeserializeObject<User>(jsonValue) : default(User);
            //string header = context.HttpContext.Request.Headers["X-Requested-With"];
            //if (user == null)
            //{
            //    //is ajax request
            //    if (header != null && header.Equals("XMLHttpRequest"))
            //    {

            //    }
            //    //if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            //    //    filterContext.Result = new RedirectResult("/Home/LoginAjax");
            //    //filterContext.Result = new RedirectResult("/Home/Login");
            //    //else
            //    context.Result = new RedirectResult("/Admin/Account/Login");
            //    return;
            //}
            //var permissionManager = context.HttpContext.RequestServices.GetService<IPermissionManager>();
            //if (string.IsNullOrWhiteSpace(PermissionCode))
            //    return;
            //if (!permissionManager.IsGranted(user.Id, PermissionCode))
            //{
            //    //if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            //    //    context.Result = new ContentResult() { Content = "您没有权限" };
            //    if (header != null && header.Equals("XMLHttpRequest"))
            //    {
            //        //context.Result = new JsonResult("您没有权限"); /*new ContentResult() { Content = "您没有权限" };*/
            //        context.Result = new ContentResult { Content = "<script>alert('您没有权限');</script>" };
            //    }
            //    else
            //    {
            //        context.HttpContext.Session.SetString("ErrorMessage", "您没有权限");
            //        context.Result = new RedirectResult("/Admin/Shared/Error");
            //    }
            //}
        }
    }
}
