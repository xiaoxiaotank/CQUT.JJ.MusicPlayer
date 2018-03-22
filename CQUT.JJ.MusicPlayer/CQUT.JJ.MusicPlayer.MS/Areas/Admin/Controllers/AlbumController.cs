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
        public IActionResult GetPublishedSingers()
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
    }
}