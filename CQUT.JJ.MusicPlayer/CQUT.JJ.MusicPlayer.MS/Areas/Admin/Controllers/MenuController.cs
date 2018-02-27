using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
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

        //[HttpGet]
        //public IActionResult GetMenu()
        //{
        //    //if (Request. IsAjaxRequest())
        //    //{
        //    //var menus = _menuAppService.GetMenus().MapToMenuTree();
        //    //return Json(menus);
        //    //}
        //    //else
        //    //{
        //    //    throw new UserFriendlyException("访问错误!", HttpStatusCode.BadRequest);
        //    //}
        //}

        //#region 创建
        //[HttpGet]
        //public IActionResult CreateColumn(int? parentId)
        //{
        //    var column = _menuAppService.GetMenuItemById(parentId);
        //    //var permissions = PermissionProvider._permissions;

        //    var model = new CreateColumnViewModel()
        //    {
        //        ParentId = column?.Id ?? 0,
        //        ParentName = column?.Name,
        //    };
        //    permissions.ForEach(p => model.Permissions.Add(new SelectListItem() { Text = p.DisplayName, Value = p.Code }));

        //    return PartialView("_Create", model);
        //}

        //[HttpPost]
        //public IActionResult CreateColumn(int? parentId, string name, string url = "", string requiredAuthorizeCode = "")
        //{
        //    var column = new Navigation
        //    {
        //        ParentId = parentId ?? 0,
        //        Name = name,
        //        Url = url ?? string.Empty,
        //        RequiredAuthorizeCode = requiredAuthorizeCode ?? string.Empty
        //    };
        //    _menuAppService.CreateColumn(column);
        //    return Json("添加成功！");
        //}
        //#endregion

        //#region 删除
        //[HttpGet]
        //public IActionResult DeleteColumn(int id, DeleteColumnViewModel model)
        //{
        //    var column = _menuAppService.GetColumnById(id);
        //    if (column != null)
        //    {
        //        model = new DeleteColumnViewModel
        //        {
        //            Id = column.Id,
        //            Name = column.Name
        //        };
        //        return PartialView("_Delete", model);
        //    }
        //    else
        //        throw new UserFriendlyException("栏目不存在！", HttpStatusCode.BadRequest);
        //}

        //[HttpPost]
        //public IActionResult DeleteColumn(int id)
        //{
        //    _menuAppService.DeleteColumn(id);
        //    return Json("删除成功！");
        //}
        //#endregion

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