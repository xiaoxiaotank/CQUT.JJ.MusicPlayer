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
            return View();
        }

        [HttpGet]
        public IActionResult Published()
        {
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
                    CreatorId = s.CreatorId,
                    MenderId = s.MenderId,
                    UnpublisherId = s.UnpublisherId,
                    Name = s.Name,
                    CreatorName = s.CreatorName,
                    MenderName = s.MenderName,
                    UnpublisherName = s.UnpublisherName,
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
                    PublisherId = s.PublisherId,
                    Name = s.Name,
                    SingerName = s.SingerName,
                    PublisherName = s.PublisherName,
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
                    SingerId = singerId,
                    CreatorId = HttpContext.Session.GetCurrentUserId()
                };
                album = _albumAppService.Create(album);
                return Json(new JsonResultEntity()
                {
                    Message = "添加专辑成功！",
                    JsonObject = Json(new AlbumViewModel()
                    {
                        Id = album.Id,
                        SingerId = album.SingerId,
                        CreatorId = album.CreatorId,
                        Name = album.Name,
                        SingerName = album.SingerName,
                        CreatorName = album.CreatorName,
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
                    SingerId = singerId,
                    MenderId = HttpContext.Session.GetCurrentUserId()
                };
                album = _albumAppService.UpdateBasic(album);
                return Json(new JsonResultEntity()
                {
                    Message = "更新基本信息成功！",
                    JsonObject = Json(new AlbumViewModel()
                    {
                        Id = album.Id,
                        SingerId = album.SingerId,
                        MenderId = album.MenderId,
                        Name = album.Name,
                        SingerName = album.SingerName,
                        MenderName = album.MenderName,
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
            _albumAppService.Publish(id, HttpContext.Session.GetCurrentUserId());
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
            _albumAppService.Unpublish(id, HttpContext.Session.GetCurrentUserId());
            return Json(new JsonResultEntity()
            {
                Message = "下架成功",
                JsonObject = Json(new { id })
            });
        }

        #endregion

    
    }
}