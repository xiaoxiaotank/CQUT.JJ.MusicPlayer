using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAppService _userAppService;

        public AccountController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        #region 注册

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var user = new UserModel()
            {
                UserName = model.UserName,
                NickName = model.NickName,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };
            user = _userAppService.Register(user);
            return Json(new JsonResultEntity()
            {
                JsonObject = Json("/Home/Index")
            });
        } 

        #endregion
    }
}