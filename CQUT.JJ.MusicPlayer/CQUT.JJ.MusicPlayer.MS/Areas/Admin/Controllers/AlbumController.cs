using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Album;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Filters;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumController : Controller
    {
        private const string Log_Source = "专辑管理";

        private readonly IAlbumAppService _albumAppService;
        private readonly ISingerAppService _singerAppService;

        public AlbumController(IAlbumAppService albumAppService, ISingerAppService singerAppService)
        {
            _albumAppService = albumAppService;
            _singerAppService = singerAppService;
        }

        [HttpGet]
        public IActionResult Unpublished()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问专辑未发布管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        public IActionResult Published()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问专辑已发布管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        public IActionResult GetUnpublishedAlbums()
        {
            var models = _albumAppService.GetUnpublishedAlbums()?
                .OrderByDescending(s1 => s1.LastModificationTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((s, i) => new AlbumViewModel()
                {
                    SId = ++i,
                    Id = s.Id,
                    SingerId = s.SingerId,
                    Name = s.Name,
                    SingerName = s.SingerName,
                    CreationTime = s.CreationTime.ToStandardDateOfChina(),
                    LastModificationTime = s.LastModificationTime?.ToStandardDateOfChina()
                });
            return Json(models);
        }

        [HttpGet]
        public IActionResult GetPublishedAlbums()
        {
            var models = _albumAppService.GetPublishedAlbums()?
                .OrderByDescending(s1 => s1.PublishmentTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((s, i) => new AlbumViewModel()
                {
                    SId = ++i,
                    Id = s.Id,
                    SingerId = s.SingerId,
                    Name = s.Name,
                    SingerName = s.SingerName,
                    CreationTime = s.CreationTime.ToStandardDateOfChina(),
                    PublishmentTime = s.PublishmentTime?.ToStandardDateOfChina()
                });
            return Json(models);
        }

        #region 创建
        
        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Create)]
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试专辑创建操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var model = new CreateAlbumViewModel()
            {
                Singers = _singerAppService.GetPublishedSingers()?
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
            };
            if (model.Singers.Any())
                model.Singers.First().Selected = true;
            return PartialView("_Create", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Create)]
        public IActionResult Create(CreateAlbumViewModel model)
        {
            if (int.TryParse(model.SingerId, out int singerId))
            {
                var album = new AlbumModel()
                {
                    Name = model.Name.Trim(),
                    SingerId = singerId
                };
                album = _albumAppService.Create(album);
                var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
                LogHelper.Log(new LogItemEntity($"{userName} 成功完成专辑创建操作"
                        , userName
                        , LogType.Succeess
                        , Log_Source));
                return Json(new JsonResultEntity()
                {
                    Message = "添加专辑成功！",
                    JsonObject = Json(new AlbumViewModel()
                    {
                        Id = album.Id,
                        SingerId = album.SingerId,
                        Name = album.Name,
                        SingerName = album.SingerName,
                        CreationTime = album.CreationTime.ToStandardDateOfChina(),
                    })
                });
            }
            else
                throw new JMBasicException("歌唱家不存在");
            
        }

        #endregion

        #region 更新基本信息

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Update)]
        public IActionResult UpdateBasic(int id, UpdateBasicAlbumViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试专辑更新基本信息操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var album = _albumAppService.GetAlbumById(id);
            model = new UpdateBasicAlbumViewModel()
            {
                Id = album.Id,
                Name = album.Name,
                Singers = _singerAppService.GetPublishedSingers()?
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                    Selected = s.Id == album.SingerId
                })
            };  
            return PartialView("_UpdateBasic", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Update)]
        public IActionResult UpdateBasic(UpdateBasicAlbumViewModel model)
        {
            if (int.TryParse(model.SingerId, out int singerId))
            {
                var album = new AlbumModel()
                {
                    Id = model.Id,
                    Name = model.Name.Trim(),
                    SingerId = singerId
                };
                album = _albumAppService.UpdateBasic(album);
                var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
                LogHelper.Log(new LogItemEntity($"{userName} 成功完成专辑更新基本信息操作"
                        , userName
                        , LogType.Succeess
                        , Log_Source));
                return Json(new JsonResultEntity()
                {
                    Message = "更新基本信息成功！",
                    JsonObject = Json(new AlbumViewModel()
                    {
                        Id = album.Id,
                        SingerId = album.SingerId,
                        Name = album.Name,
                        SingerName = album.SingerName,
                        LastModificationTime = album.LastModificationTime?.ToStandardDateOfChina()
                    })
                });
            }
            throw new JMBasicException("歌唱家不存在");
        }

        #endregion

        #region 删除

        [HttpGet]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Delete)]
        public IActionResult Delete(int id, DeleteAlbumViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试专辑删除操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var singer = _albumAppService.GetAlbumById(id);
            model = new DeleteAlbumViewModel()
            {
                Id = singer.Id,
                Name = singer.Name
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Delete)]
        public IActionResult Delete(DeleteAlbumViewModel model)
        {
            _albumAppService.Delete(model.Id);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成专辑删除操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "删除成功",
                JsonObject = Json(new AlbumViewModel() { Id = model.Id })
            });
        }

        #endregion

        #region 发布和下架

        [HttpPost]
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Publish)]
        public IActionResult Publish(int id)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试专辑发布操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _albumAppService.Publish(id);
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成专辑发布操作"
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
        [MvcAuthorize(PermissionCode = PermissionCodes.Album_Unpublish)]
        public IActionResult Unpublish(int id)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试专辑下架操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _albumAppService.Unpublish(id);
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成专辑下架操作"
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