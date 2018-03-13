using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Account;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IUserAppService _userAppService;

        public AccountController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        #region 登录
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel()
            {
                IsRememberMe = HttpContext.User.Identity.IsAuthenticated
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = new UserModel();
            try
            {
                user = _userAppService.LoginOfAdmin(model.UserName, model.Password);
            }
            catch (JMBasicException ex)
            {
                HttpContext.Session.SetString("ErrorMessage", ex.Message);
                return View();
            }
            if (user != null)
            {
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
                return RedirectToAction("Index", "Home");
            }
            return View();
        } 
        #endregion

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login");
        }
    }
}