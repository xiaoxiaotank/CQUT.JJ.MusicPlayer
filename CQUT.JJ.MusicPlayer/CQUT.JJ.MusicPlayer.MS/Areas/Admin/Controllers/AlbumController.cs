using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Album;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
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
        public IActionResult Create(CreateAlbumViewModel model)
        {
            if (int.TryParse(model.SingerId, out int singerId))
            {
                var album = new AlbumModel()
                {
                    Name = model.Name,
                    SingerId = singerId
                };
                album = _albumAppService.Create(album);
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
        public IActionResult UpdateBasic(UpdateBasicAlbumViewModel model)
        {
            if (int.TryParse(model.SingerId, out int singerId))
            {
                var album = new AlbumModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    SingerId = singerId
                };
                album = _albumAppService.UpdateBasic(album);
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
        public IActionResult Publish(int id)
        {
            _albumAppService.Publish(id);
            return Json(new JsonResultEntity()
            {
                Message = "发布成功",
                JsonObject = Json(new { id })
            });
        }

        [HttpPost]
        public IActionResult Unpublish(int id)
        {
            _albumAppService.Unpublish(id);
            return Json(new JsonResultEntity()
            {
                Message = "下架成功",
                JsonObject = Json(new { id })
            });
        }

        #endregion
    }
}