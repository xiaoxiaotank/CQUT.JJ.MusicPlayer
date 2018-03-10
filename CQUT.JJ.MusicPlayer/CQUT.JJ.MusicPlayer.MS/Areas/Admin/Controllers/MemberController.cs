using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Member;
using CQUT.JJ.MusicPlayer.MS.Entities;
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
                    id = m.Id,
                    sId = i + 1,
                    userName = m.UserName,
                    nickName = m.NickName,
                    creationTime = m.CreationTime.ToString("yyyy年MM月dd日")
                });
            return Json(members);
        }


        #region 删除

        [HttpGet]
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
        public IActionResult Delete(DeleteMemberViewModel model)
        {
            _userAppService.DeleteUserById(model.Id);
            return Json(new JsonResultEntity()
            {
                message = "删除会员帐号成功！",
                jsonObject = new JsonResult(new { id = model.Id })
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