using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Filters;
using CQUT.JJ.MusicPlayer.MS.Uitls.Extensions;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;

        public EmployeeController(IUserAppService userAppService, IRoleAppService roleAppService)
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;
        }

        [HttpGet]
        [MvcAuthorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [MvcAuthorize]
        public JsonResult GetEmployees()
        {
            var employees = _userAppService.GetAllAdmins()
                .Select((m, i) => new EmployeeViewModel()
                {
                    Id = m.Id,
                    SId = i + 1,
                    UserName = m.UserName,
                    NickName = m.NickName,
                    CreationTime = m.CreationTime.ToStandardDateOfChina(),
                    LastModificationTime = m.LastModificationTime?.ToStandardDateOfChina(),
                    RoleNames = _roleAppService.GetAllRolesByUserId(m.Id)?.Select(r => r.Name)
                }).ToList();
            employees.ForEach(e =>
            {
                if (IsSuperManager(e.Id))
                {
                    e.IsSuperManager = true;
                    return;
                }
            });
            return Json(employees);
        }

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee)]
        public ActionResult GetPermissions(int id)
        {
            var permissionCodesOfRoles = new List<string>();
            var roleIds = _roleAppService.GetAllRolesByUserId(id).Select(r => r.Id).ToList();
            roleIds.ForEach(i => permissionCodesOfRoles.AddRange(_roleAppService.GetAllPermissionsByRoleId(i)));
            var permissions = _userAppService.GetAllPermissionsByUserId(id).MapToPermissionTree(permissionCodesOfRoles.Distinct());
            return Json(permissions);
        }

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee)]
        public JsonResult GetUserRolesByUserId(int id)
        {
            var userRoleIds = _roleAppService.GetAllRolesByUserId(id)
                .Select(r => r.Id);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var model = _roleAppService.GetAllRoles()
                    .Select((r, i) => new UserRoleViewModel()
                    {
                        SId = i + 1,
                        Id = r.Id,
                        RoleName = r.Name,
                        IsDefault = r.IsDefault,
                        HasOwned = userRoleIds.Contains(r.Id)
                    });

                return Json(model);
            }
            throw new JMBasicException("访问出错");
        }


        #region 创建

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee_Create)]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee_Create)]
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
                    CreationTime = user.CreationTime.ToStandardDateOfChina(),
                    RoleNames = _roleAppService.GetAllRolesByUserId(user.Id).Select(r => r.Name)
                })
            });
        }

        #endregion

        #region 更新

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee_Update)]
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
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee_Update)]
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
                    LastModificationTime = user.LastModificationTime?.ToStandardDateOfChina()
                })
            });
        }

        #endregion

        #region 删除

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee_Delete)]
        public IActionResult Delete(int id, DeleteEmployeeViewModel model)
        {
            if (_userAppService.IsSuperManager(id, out UserModel user))
                return Json(new JsonResultEntity() { IsSuccessed = false, Message = "超级管理账户不能删除!" });

            model = new DeleteEmployeeViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Employee_Delete)]
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

        #region 授权

        [HttpGet]
        public IActionResult Authorize(int id, AuthorizeEmployeeViewModel model)
        {
            model = new AuthorizeEmployeeViewModel() { Id = id };
            return View("_Authorize", model);
        }

        [HttpPost]
        public IActionResult Authorize(AuthorizeEmployeeViewModel model)
        {
            _userAppService.SetPermissions(model.Id, model.PermissionCodes);
            return Json(new JsonResultEntity() { Message = "设置权限成功！" });
        }

        #endregion

        #region 设置角色
       
        [HttpGet]
        public IActionResult SetRoles(int id, SetRolesToEmployeeViewModel model)
        {
            var user = _userAppService.GetAdminById(id);
            model = new SetRolesToEmployeeViewModel()
            {
                Id = id,
                UserName = user.UserName
            };
            return View("_SetRoles", model);
        }

        [HttpPost]
        public IActionResult SetRoles(SetRolesToEmployeeViewModel model)
        {
            _userAppService.SetRolesByUserId(model.Id, model.RoleIds);
            var roleNames = model.RoleIds?.Select(id => _roleAppService.GetRoleById(id).Name);
            return Json(new JsonResultEntity()
            {
                Message = "设置角色成功！",
                JsonObject = Json(roleNames)
            });
        }

        #endregion

        [NonAction]
        private bool IsSuperManager(int id)
        {
            return _userAppService.IsSuperManager(id, out UserModel user);
        }
    }
}