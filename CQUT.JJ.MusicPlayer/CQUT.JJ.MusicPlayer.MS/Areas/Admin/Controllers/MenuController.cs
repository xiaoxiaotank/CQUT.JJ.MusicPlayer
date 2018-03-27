using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Menu;
using CQUT.JJ.MusicPlayer.MS.Uitls.Extensions;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Filters;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private const string Log_Source = "菜单管理";

        private readonly IMenuAppService _menuAppService;
        public MenuController(IMenuAppService sidebarAppService)
        {
            _menuAppService = sidebarAppService;
        }

        [HttpGet]
        [MvcAuthorize]
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问菜单管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        [MvcAuthorize]
        public IActionResult GetMenuByParentId(string parentId)
        {
            if (Request.IsAjaxRequest())
            {
                var menus = new List<ZTreeNode>();
                if (parentId == null)
                    menus = _menuAppService.GetMenus().MapMenuToZTree();
                else if (int.TryParse(parentId, out int id))
                {
                    menus = _menuAppService.GetChildMenuItemsById(id).MapMenuToZTree();                  
                }
                return Json(menus);
            }
            else
            {
                throw new JMBasicException("访问错误!", HttpStatusCode.BadRequest);
            }
        }

        #region 创建
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Create)]
        public IActionResult CreateMenuItem(int? parentId,CreateMenuItemViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试菜单创建操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            if (parentId == null)
            {
                model.ParentId = 0;
                model.ParentHeader = string.Empty;
            }
            else
            {
                var parent = _menuAppService.GetMenuItemById((int)parentId);

                model.ParentId = parent.Id;
                model.ParentHeader = parent.Header;
            }

            return PartialView("_Create", model);
        }

        /// <summary>
        /// 初次创建不涉及优先级
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Create)]
        public IActionResult CreateMenuItem(CreateMenuItemViewModel model)
        {
            var menuItem = new MenuItemModel
            {
                ParentId = model.ParentId,
                Header = model.Header,
                TargetUrl = GetValidUrl(model.TargetUrl),
                RequiredAuthorizeCode = model.RequiredAuthorizeCode ?? string.Empty,                
            };
            _menuAppService.CreateMenuItem(menuItem);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成菜单创建操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity() { Message = "添加成功！" });
        }
        #endregion

        #region 重命名

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Rename)]
        public IActionResult RenameMenuItem(int id, string header)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试执行菜单重命名操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _menuAppService.RenameMenuItem(id, header);
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成菜单重命名操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity() { Message = "重命名成功！" });
        } 

        #endregion

        #region 删除
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Delete)]
        public IActionResult DeleteMenuItem(int id, DeleteMenuItemViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试菜单删除操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var menuItem = _menuAppService.GetMenuItemById(id);
            if (menuItem != null)
            {
                model = new DeleteMenuItemViewModel
                {
                    Id = menuItem.Id,
                    Header = menuItem.Header
                };
                return PartialView("_Delete", model);
            }
            else
                throw new JMBasicException("栏目不存在！", HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Delete)]
        public IActionResult DeleteMenuItem(DeleteMenuItemViewModel model)
        {
            _menuAppService.DeleteMenuItem(model.Id);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成菜单删除操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity() { Message = "删除成功！" });
        }
        #endregion

        #region 更新
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Update)]
        public IActionResult UpdateMenuItem(int id, UpdateMenuItemViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试菜单更新基本信息操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var menuItem = _menuAppService.GetMenuItemById(id);
            if (menuItem != null)
            {
                model = new UpdateMenuItemViewModel
                {
                    Id = menuItem.Id,
                    TargetUrl = menuItem.TargetUrl,
                    Priority = menuItem.Priority,
                    Header = menuItem.Header,
                };
                
                return PartialView("_Update", model);
            }
            else
                throw new JMBasicException("菜单不存在！", HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Update)]
        public IActionResult UpdateMenuItem(UpdateMenuItemViewModel model)
        {
            var menuItem = new MenuItemModel
            {
                Id = model.Id,
                RequiredAuthorizeCode = model.RequiredAuthorizeCode ?? string.Empty,
                Priority = model.Priority,
            };
            menuItem.TargetUrl = GetValidUrl(model.TargetUrl);
            _menuAppService.UpdateMenuItem(menuItem);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成菜单更新基本信息操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity() { Message = "更新成功！" });
        }
    
        #endregion

        #region 迁移
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Migrate)]
        public IActionResult MigrateMenuItem(int id, int? parentId, MigrateMenuItemViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试菜单迁移操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            if (parentId == null)
                parentId = 0;
            var menuItem = _menuAppService.GetMenuItemById(id);
            if (menuItem != null)
            {                
                var parentMenuItem = parentId == 0 ? null : _menuAppService.GetMenuItemById((int)parentId);
                if (menuItem.ParentId == parentId)
                {
                    return Json(new JsonResultEntity()
                    {
                        Message = $"{parentMenuItem?.Header ?? "根"}菜单下已包含子菜单{menuItem.Header},无需迁移!",
                        IsSuccessed = false
                    });
                }

                model = new MigrateMenuItemViewModel()
                {
                    Id = menuItem.Id,
                    ParentId = (int)parentId,
                    Header = menuItem.Header,
                    ParentHeader = parentMenuItem?.Header,
                };
                return PartialView("_Migrate", model);
            }
            else
                throw new JMBasicException("菜单不存在！", HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.MenuManage_Migrate)]
        public IActionResult MigrateMenuItem(MigrateMenuItemViewModel model)
        {
            _menuAppService.MigrateMenuItem(model.Id, model.ParentId);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成菜单迁移操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity() { Message = "迁移成功！" });
        }
        #endregion

        /// <summary>
        /// 刷新菜单
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpGet]
        [MvcAuthorize]
        public IActionResult RefreshMenu(string keywords)
        {
            return ViewComponent("Menu", new { keywords });
        }

        [NonAction]
        private string GetValidUrl(string targetUrl)
        {
            if (string.IsNullOrWhiteSpace(targetUrl))
                return string.Empty;

            var area = ControllerContext.RouteData.Values["area"];
            if (targetUrl[0] != '/')
                targetUrl = targetUrl.Insert(0, "/");
            if (!targetUrl.StartsWith($@"/{ area }"))
                targetUrl = targetUrl.Insert(0, $@"/{ area }");
            return targetUrl;
        }
    }
}