using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Application.Methods;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Singer;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Filters;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SingerController : Controller
    {
        private const string Log_Source = "歌唱家管理";

        private readonly ISingerAppService _singerAppService;

        public SingerController(ISingerAppService singerAppService)
        {
            _singerAppService = singerAppService;
        }

        [HttpGet]
        public IActionResult Unpublished()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问歌唱家未发布管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        public IActionResult Published()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问歌唱家已发布管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        public IActionResult GetUnpublishedSingers()
        {
            var models = _singerAppService.GetUnpublishedSingers()?
                .OrderByDescending(s1 => s1.LastModificationTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((s, i) => new SingerViewModel()
                {
                    SId = ++i,
                    Id = s.Id,
                    CreatorId = s.CreatorId,
                    MenderId = s.MenderId,
                    UnpublisherId = s.UnpublisherId,
                    Name = s.Name,
                    CreatorName = s.CreatorName,
                    MenderName = s.MenderName,
                    UnpublisherName = s.UnpublisherName,
                    ForeignName = s.ForeignName,
                    Nationality = s.Nationality,
                    CreationTime = s.CreationTime.ToStandardDateOfChina(),
                    LastModificationTime = s.LastModificationTime?.ToStandardDateOfChina()
                });
            return Json(models);
        }

        [HttpGet]
        public IActionResult GetPublishedSingers()
        {
            var models = _singerAppService.GetPublishedSingers()?
                .OrderByDescending(s1 => s1.PublishmentTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((s, i) => new SingerViewModel()
                {
                    SId = ++i,
                    Id = s.Id,
                    PublisherId = s.PublisherId,
                    Name = s.Name,
                    PublisherName = s.PublisherName,
                    ForeignName = s.ForeignName,
                    Nationality = s.Nationality,
                    CreationTime = s.CreationTime.ToStandardDateOfChina(),
                    PublishmentTime = s.PublishmentTime?.ToStandardDateOfChina()
                });
            return Json(models);
        }

        #region 创建

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Create)]
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试歌唱家创建操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return PartialView("_Create",new CreateSingerViewModel());
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Create)]
        public IActionResult Create(CreateSingerViewModel model)
        {
            var singer = new SingerModel()
            {
                Name = model.Name.Trim(),
                ForeignName = model.ForeignName,
                Nationality = model.Nationality,
                CreatorId = HttpContext.Session.GetCurrentUserId()
            };
            singer = _singerAppService.Create(singer);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成歌唱家创建操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "添加歌唱家成功！",
                JsonObject = Json(new SingerViewModel()
                {
                    Id = singer.Id,
                    CreatorId = singer.CreatorId,
                    Name = singer.Name,
                    CreatorName = singer.CreatorName,
                    ForeignName = singer.ForeignName,
                    Nationality = singer.Nationality,
                    CreationTime = singer.CreationTime.ToStandardDateOfChina(),
                })
            });
        }

        #endregion

        #region 更新基本信息

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Update)]
        public IActionResult UpdateBasic(int id,UpdateBasicSingerViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试歌唱家更新基本信息操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var singer = _singerAppService.GetSingerById(id);
            model = new UpdateBasicSingerViewModel()
            {
                Id = singer.Id,
                Name = singer.Name,
                ForeignName = singer.ForeignName,
                Nationality = singer.Nationality
            };
            return PartialView("_UpdateBasic", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Update)]
        public IActionResult UpdateBasic(UpdateBasicSingerViewModel model)
        {
            var singer = new SingerModel()
            {
                Id = model.Id,
                ForeignName = model.ForeignName,
                Nationality = model.Nationality,
                MenderId = HttpContext.Session.GetCurrentUserId(),
            };
            singer = _singerAppService.UpdateBasic(singer);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成歌唱家更新基本信息操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "更新基本信息成功！",
                JsonObject = Json(new SingerViewModel()
                {
                    Id = singer.Id,
                    MenderId = singer.MenderId,
                    MenderName = singer.MenderName,
                    ForeignName = singer.ForeignName,
                    Nationality = singer.Nationality,
                    LastModificationTime = singer.LastModificationTime?.ToStandardDateOfChina()
                })
            });
        }

        #endregion

        #region 删除

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Delete)]
        public IActionResult Delete(int id, DeleteSingerViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试歌唱家删除操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var singer = _singerAppService.GetSingerById(id);
            model = new DeleteSingerViewModel()
            {
                Id = singer.Id,
                Name = singer.Name
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Delete)]
        public IActionResult Delete(DeleteSingerViewModel model)
        {
            _singerAppService.Delete(model.Id);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成歌唱家删除操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "删除成功",
                JsonObject = Json(new SingerViewModel() { Id = model.Id })
            });
        }

        #endregion

        #region 发布和下架

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Publish)]
        public IActionResult Publish(int id)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试歌唱家发布操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _singerAppService.Publish(id, HttpContext.Session.GetCurrentUserId());
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成歌唱家发布操作"
                   , userName
                   , LogType.Succeess
                   , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "发布成功",
                JsonObject = Json(new { id })
            });
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Singer_Unpublish)]
        public IActionResult Unpublish(int id)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试歌唱家下架操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _singerAppService.Unpublish(id, HttpContext.Session.GetCurrentUserId());
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成歌歌唱家下架操作"
                   , userName
                   , LogType.Succeess
                   , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "下架成功",
                JsonObject = Json(new { id })
            });
        }

        #endregion
    }
}