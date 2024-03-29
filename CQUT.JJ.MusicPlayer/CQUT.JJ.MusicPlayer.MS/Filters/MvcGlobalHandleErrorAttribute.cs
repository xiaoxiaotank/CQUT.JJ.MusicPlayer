﻿using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Filters
{
    public class MvcGlobalHandleErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var originalException = context.Exception;
            
            JMBasicException ex = null;
            //判断该异常是否兼容JMBasicException
            if (originalException is JMBasicException)
                ex = originalException as JMBasicException;
            else
                ex = new JMBasicException(originalException.Message);

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            var userName = context.HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            Task.Factory.StartNew(() => LogHelper.Log(new LogItemEntity($"{userName} {ex.Message}"
                    , userName
                    , LogType.Fail
                    , $"{controller}.{action}")));

            //is ajax request
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(new JsonResultEntity
                {
                    Message = ex.Message,
                    IsSuccessed = false
                });
            }
            else
            {
                //将异常信息保存到session中
                if (context.HttpContext.Session != null)
                    context.HttpContext.Session.SetString("ErrorMessage", ex.Message);
                var returnUrl = "/Home/Error";
                context.Result = new RedirectResult(returnUrl);
            }

            context.ExceptionHandled = true;
        }
    }
}
