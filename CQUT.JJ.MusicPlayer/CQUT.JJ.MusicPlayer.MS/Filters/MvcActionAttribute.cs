using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Filters
{
    public class MvcActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Session.GetCurrentUser();
            var area = context.RouteData.Values["area"]?.ToString();
            if (user?.IsAdmin == true && area != null)
            {
                var userName = user.UserName;
                var controller = context.RouteData.Values["controller"].ToString();
                if (controller == "LocalLog")
                    return;
                var action = context.RouteData.Values["action"].ToString();
                var httpMethod = (context.GetActionAttributesByContext(typeof(HttpMethodAttribute)).FirstOrDefault() as HttpMethodAttribute)
                    .HttpMethods.FirstOrDefault();

                Task.Factory.StartNew(() => LogHelper.Log(new LogItemEntity($"{userName} 访问{context.ActionDescriptor.DisplayName}，方式:{httpMethod}"
                            , userName
                            , LogType.Info
                            , $"{area}.{controller}.{action}")));
            }
            base.OnActionExecuting(context);

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var user = context.HttpContext.Session.GetCurrentUser();
            var area = context.RouteData.Values["area"]?.ToString();
            if (user?.IsAdmin == true && area != null)
            {
                var userName = user.UserName;
                var controller = context.RouteData.Values["controller"].ToString();
                if (controller == "LocalLog")
                    return;
                var action = context.RouteData.Values["action"].ToString();
                var httpMethod = (context.GetActionAttributesByContext(typeof(HttpMethodAttribute)).FirstOrDefault() as HttpMethodAttribute)
                    .HttpMethods.FirstOrDefault(); ;

                Task.Factory.StartNew(() => LogHelper.Log(new LogItemEntity($"{userName} 完成{context.ActionDescriptor.DisplayName}，方式:{httpMethod}"
                            , userName
                            , LogType.Info
                            , $"{area}.{controller}.{action}")));
            }

            base.OnActionExecuted(context);

        }
    }
}
