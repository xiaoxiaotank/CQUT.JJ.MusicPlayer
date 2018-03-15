using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Member;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly IUserAppService _userAppService;

        public MemberController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMembers()
        {
            var members = _userAppService.GetAllMembers()
                .Select((m,i) => new MemberViewModel()
                {
                    Id = m.Id,
                    SId = i + 1,
                    UserName = m.UserName,
                    NickName = m.NickName,
                    CreationTime = m.CreationTime.ToString("yyyy年MM月dd日")
                });
            return Json(members);
        }


        #region 删除

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Member_Delete)]
        public IActionResult Delete(int id, DeleteMemberViewModel model)
        {
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
            return Json(new JsonResultEntity()
            {
                Message = "删除会员帐号成功！",
                JsonObject = new JsonResult(new { id = model.Id })
            });
        }

        #endregion

        [NonAction]
        private string GetFormattingTime(DateTime time)
        {
            return time.ToString("yyyy年MM月dd日");
        }
    }
}