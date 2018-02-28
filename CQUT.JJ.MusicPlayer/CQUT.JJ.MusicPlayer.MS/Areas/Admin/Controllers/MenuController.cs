using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Data.Entities;
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
                TargetUrl = model.TargetUrl ?? string.Empty,
                RequiredAuthorizeCode = model.RequiredAuthorizeCode ?? string.Empty,                
            };
            _menuAppService.CreateMenuItem(menuItem);
            return Json("添加成功！");
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
            return Json("删除成功！");
        }
        #endregion

        //#region 更新
        //[HttpGet]
        //public IActionResult UpdateColumn(int id, UpdateColumnViewModel model)
        //{
        //    var column = _menuAppService.GetColumnById(id);
        //    if (column != null)
        //    {
        //        model = new UpdateColumnViewModel
        //        {
        //            Id = column.Id,
        //            Name = column.Name,
        //            Url = column.Url
        //        };
        //        var permissions = PermissionProvider._permissions;
        //        permissions.ForEach(p => model.Permissions.Add(new SelectListItem() { Text = p.DisplayName, Value = p.Code }));
        //        var selectedCode = model.Permissions.SingleOrDefault(p => p.Value.Equals(column.RequiredAuthorizeCode));
        //        if (selectedCode != null)
        //            selectedCode.Selected = true;
        //        return PartialView("_Update", model);
        //    }
        //    else
        //        throw new UserFriendlyException("栏目不存在！", HttpStatusCode.BadRequest);
        //}

        //[HttpPost]
        //public IActionResult UpdateColumn(int id, string name, string url, string requiredAuthorizeCode)
        //{
        //    var column = new Navigation
        //    {
        //        Id = id,
        //        Name = name,
        //        Url = url ?? string.Empty,
        //        RequiredAuthorizeCode = requiredAuthorizeCode ?? string.Empty
        //    };
        //    _menuAppService.UpdateColumn(column);
        //    return Json("更新成功！");
        //}
        //#endregion

        //#region 迁移
        //[HttpGet]
        //public IActionResult MigrateColumn(int id, int? parentId, MigrateColumnViewModel model)
        //{
        //    var column = _menuAppService.GetColumnById(id);
        //    if (column != null)
        //    {
        //        var parentColumn = _menuAppService.GetColumnById(parentId);
        //        model = new MigrateColumnViewModel()
        //        {
        //            Id = column.Id,
        //            ParentId = column.ParentId == parentId ? -1 : parentId ?? 0,
        //            Name = column.Name,
        //            ParentName = parentColumn?.Name,
        //        };
        //        return PartialView("_Migrate", model);
        //    }
        //    else
        //        throw new UserFriendlyException("栏目不存在！", HttpStatusCode.BadRequest);
        //}

        //[HttpPost]
        //public IActionResult MigrateColumn(int id, int? parentId)
        //{
        //    _menuAppService.MigrateColumn(id, parentId);
        //    return Json("迁移成功！");
        //}
        //#endregion

        [HttpGet]
        public IActionResult RefreshMenu(string keywords)
        {
            return ViewComponent("Menus", new {  keywords });
        }
    }
}