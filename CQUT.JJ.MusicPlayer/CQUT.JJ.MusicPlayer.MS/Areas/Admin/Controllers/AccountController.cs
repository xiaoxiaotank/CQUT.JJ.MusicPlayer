using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Account;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private const string Log_Source = "员工登录";

        private readonly IUserAppService _userAppService;

        public AccountController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        #region 登录
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var model = new LoginViewModel()
            {
                IsRememberMe = HttpContext.User.Identity.IsAuthenticated
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                LogHelper.Log(new LogItemEntity($"{model.UserName} 请求登录操作"
                    , model.UserName
                    , LogType.Info
                    , Log_Source));
                var user = new UserModel();
                try
                {
                    user = _userAppService.LoginOfAdmin(model.UserName, model.Password);

                    if (user != null)
                    {
                        LogHelper.Log(new LogItemEntity($"{user.UserName} 登录成功"
                            , user.UserName
                            , LogType.Succeess
                            , Log_Source));
                        HttpContext.Session.SaveCurrentUser(user);

                        if (model.IsRememberMe)
                        {
                            var userClaim = new ClaimsPrincipal(new ClaimsIdentity(
                                new[] { new Claim(ClaimTypes.Name, model.UserName) },
                                CookieAuthenticationDefaults.AuthenticationScheme));
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userClaim, new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.Now.Add(TimeSpan.FromDays(7)),
                            });
                        }
                        else
                        {
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        }
                        return Json(new JsonResultEntity() { Message = $"{user.UserName}登录成功", JsonObject = new JsonResult($"/{ControllerContext.RouteData.Values["area"]}/Home/Index") });
                    }
                    throw new JMBasicException("用户名不存在");
                }
                catch (JMBasicException ex)
                {
                    LogHelper.Log(new LogItemEntity($"{user.UserName} {ex.Message}"
                        , user.UserName
                        , LogType.Fail
                        , Log_Source));
                    HttpContext.Session.SaveCurrentUser(user);
                    return Json(new JsonResultEntity() { IsSuccessed = false,Message = ex.Message });
                }
            }
            throw new JMBasicException("请求处理错误");
        } 
        #endregion

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            var user = GlobalHelper.GetCurrentUser(HttpContext.Session);
            HttpContext.Session.Remove("User");
            LogHelper.Log(new LogItemEntity($"{user.UserName} 注销成功"
                            , user.UserName
                            , LogType.Succeess
                            , Log_Source));
            HttpContext.Session.SaveCurrentUser(user);
            return RedirectToAction("Login");
        }
    }
}