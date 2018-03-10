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

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly IMenuAppService _menuAppService;
        public MenuController(IMenuAppService sidebarAppService)
        {
            _menuAppService = sidebarAppService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
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
        public IActionResult CreateMenuItem(int? parentId,CreateMenuItemViewModel model)
        {
            if(parentId == null)
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
            return Json(new JsonResultEntity() { message = "添加成功！" });
        }
        #endregion

        #region 重命名

        [HttpPost]
        public IActionResult RenameMenuItem(int id, string header)
        {
            _menuAppService.RenameMenuItem(id, header);
            return Json(new JsonResultEntity() { message = "重命名成功！" });
        } 

        #endregion

        #region 删除
        [HttpGet]
        public IActionResult DeleteMenuItem(int id, DeleteMenuItemViewModel model)
        {
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
        public IActionResult DeleteMenuItem(DeleteMenuItemViewModel model)
        {
            _menuAppService.DeleteMenuItem(model.Id);
            return Json(new JsonResultEntity() { message = "删除成功！" });
        }
        #endregion

        #region 更新
        [HttpGet]
        public IActionResult UpdateMenuItem(int id, UpdateMenuItemViewModel model)
        {
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
            return Json(new JsonResultEntity() { message = "更新成功！" });
        }
    
        #endregion

        #region 迁移
        [HttpGet]
        public IActionResult MigrateMenuItem(int id, int? parentId, MigrateMenuItemViewModel model)
        {
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
                        message = $"{parentMenuItem?.Header ?? "根"}菜单下已包含子菜单{menuItem.Header},无需迁移!",
                        isSuccessed = false
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
        public IActionResult MigrateMenuItem(MigrateMenuItemViewModel model)
        {
            _menuAppService.MigrateMenuItem(model.Id, model.ParentId);
            return Json(new JsonResultEntity() { message = "迁移成功！" });
        }
        #endregion

        /// <summary>
        /// 刷新菜单
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RefreshMenu(string keywords)
        {
            return ViewComponent("Menu", new { keywords });
        }


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