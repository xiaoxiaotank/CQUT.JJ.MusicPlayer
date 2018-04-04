using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Role;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Filters;
using CQUT.JJ.MusicPlayer.MS.Uitls.Extensions;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleAppService _roleAppService;

        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role)]
        public JsonResult GetRoles()
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                var model = _roleAppService.GetAllRoles()
                    .Select((r, i) => new RoleViewModel()
                    {
                        SId = i + 1,
                        Id = r.Id,
                        Name = r.Name,
                        IsDefault = r.IsDefault,
                        CreationTime = r.CreationTime.ToStandardDateOfChina(),
                        LastModificationTime = r.LastModificationTime?.ToStandardDateOfChina()
                    });

                return Json(model);
            }
            throw new JMBasicException("访问出错");
        }

        [HttpGet]
        public IActionResult GetRolePermissions(int id)
        {
            var permissions = _roleAppService.GetAllPermissionsByRoleId(id).MapToPermissionTree();
            return Json(permissions);
        }

        #region 创建
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Create)]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Create)]
        public IActionResult Create(CreateRoleViewModel model)
        {
            var role = new RoleModel
            {
                Name = model.Name,
                IsDefault = model.IsDefault
            };
            role = _roleAppService.CreateRole(role, model.PermissionCodes);
            return Json(new JsonResultEntity()
            {
                Message = "添加角色成功",
                JsonObject = Json(new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsDefault = role.IsDefault,
                    CreationTime = role.CreationTime.ToStandardDateOfChina(),
                    LastModificationTime = role.LastModificationTime?.ToStandardDateOfChina(),
                })
            });           
        }
        #endregion

        #region 更新
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Update)]
        public IActionResult Update(int id, UpdateRoleViewModel model)
        {
            var role = _roleAppService.GetRoleById(id);
            model = new UpdateRoleViewModel
            {
                Id = role.Id,
                IsDefault = role.IsDefault,
                Name = role.Name
            };
            return PartialView("_Update", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Update)]
        public IActionResult Update(UpdateRoleViewModel model)
        {
            var role = new RoleModel()
            {
                Id = model.Id,
                Name = model.Name,
                IsDefault = model.IsDefault
            };
            role = _roleAppService.UpdateRole(role);
            return Json(new JsonResultEntity()
            {
                Message = "修改角色基本信息成功！",
                JsonObject = Json(new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsDefault = role.IsDefault,
                    LastModificationTime = role.LastModificationTime?.ToStandardDateOfChina(),
                })
            });
        }
        #endregion

        #region 设置默认

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_SetDefault)]
        public IActionResult ToggleSetDefault(int id)
        {
            _roleAppService.ToggleSetDefault(id);
            return Json(new JsonResultEntity() { Message = "切换默认角色成功！" });
        }

        #endregion


        #region 删除
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Delete)]
        public IActionResult Delete(int id, DeleteRoleViewModel model)
        {
            var role = _roleAppService.GetRoleById(id);
            model = new DeleteRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Delete)]
        public IActionResult Delete(DeleteRoleViewModel model)
        {
            _roleAppService.DeleteRoleById(model.Id);
            return Json(new JsonResultEntity()
            {
                Message = "删除角色成功！",
                JsonObject = Json(new { id = model.Id })
            });
        }
        #endregion


        #region 授权

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Authorize)]
        public IActionResult Authorize(int id, AuthorizeRoleViewModel model)
        {
            model = new AuthorizeRoleViewModel() { Id = id };
            return View("_Authorize", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Role_Authorize)]
        public IActionResult Authorize(AuthorizeRoleViewModel model)
        {
            _roleAppService.SetPermissions(model.Id, model.PermissionCodes);
            return Json(new JsonResultEntity()
            {
                Message = "设置权限成功！",
                JsonObject = Json(new RoleViewModel()
                {
                    Id = model.Id,
                    LastModificationTime = DateTime.Now.ToStandardDateOfChina()
                })
            });
        }

        #endregion
    }
}