using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Member;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Filters;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private const string Log_Source = "会员管理";

        private readonly IUserAppService _userAppService;

        public MemberController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        [MvcAuthorize]
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问会员管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        [MvcAuthorize]
        public JsonResult GetMembers()
        {
            var members = _userAppService.GetAllMembers()
                .Select((m,i) => new MemberViewModel()
                {
                    Id = m.Id,
                    SId = i + 1,
                    UserName = m.UserName,
                    NickName = m.NickName,
                    CreationTime = m.CreationTime.ToStandardDateOfChina()
                });
            return Json(members);
        }


        #region 删除

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Member_Delete)]
        public IActionResult Delete(int id, DeleteMemberViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试会员删除操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var user = _userAppService.GetMemberById(id);
            model = new DeleteMemberViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Member_Delete)]
        public IActionResult Delete(DeleteMemberViewModel model)
        {
            _userAppService.DeleteUserById(model.Id);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成会员删除操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "删除会员帐号成功！",
                JsonObject = new JsonResult(new { id = model.Id })
            });
        }

        #endregion

    }
}