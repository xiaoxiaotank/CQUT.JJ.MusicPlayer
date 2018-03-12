using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee;
using CQUT.JJ.MusicPlayer.MS.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IUserAppService _userAppService;

        public EmployeeController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            var employees = _userAppService.GetAllAdmins()
                .Select((m, i) => new EmployeeViewModel()
                {
                    Id = m.Id,
                    SId = i + 1,
                    UserName = m.UserName,
                    NickName = m.NickName,
                    CreationTime = GetFormattingTime(m.CreationTime)
                });
            return Json(employees);
        }

        #region 创建

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeViewModel model)
        {
            var user = new UserModel
            {
                UserName = model.UserName,
                NickName = model.NickName,
                IsAdmin = true
            };
            user = _userAppService.Register(user);

            return Json(new JsonResultEntity()
            {
                Message = "创建员工成功！",
                JsonObject = Json(new EmployeeViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    CreationTime = GetFormattingTime(user.CreationTime)
                })
            });
        }

        #endregion

        #region 更新

        [HttpGet]
        public IActionResult Update(int id, UpdateEmployeeViewModel model)
        {
            var user = _userAppService.GetAdminById(id);
            model = new UpdateEmployeeViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
            };
            return PartialView("_Update", model);
        }

        [HttpPost]
        public IActionResult Update(UpdateEmployeeViewModel model)
        {
            var user = new UserModel()
            {
                Id = model.Id,
                NickName = model.NickName,
            };
            user = _userAppService.UpdateBasicInfo(user);
            return Json(new JsonResultEntity()
            {
                Message = "编辑信息成功！",
                JsonObject = Json(new EmployeeViewModel()
                {
                    Id = user.Id,
                    NickName = user.NickName,
                })
            });
        }

        #endregion

        #region 删除

        [HttpGet]
        public IActionResult Delete(int id, DeleteEmployeeViewModel model)
        {
            var user = _userAppService.GetAdminById(id);
            model = new DeleteEmployeeViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteEmployeeViewModel model)
        {
            _userAppService.DeleteUserById(model.Id);
            return Json(new JsonResultEntity()
            {
                Message = "删除员工帐号成功！",
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