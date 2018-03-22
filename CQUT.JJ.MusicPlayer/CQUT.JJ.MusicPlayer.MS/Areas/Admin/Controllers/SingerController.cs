﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Application.Methods;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Singer;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SingerController : Controller
    {
        private readonly ISingerAppService _singerAppService;

        public SingerController(ISingerAppService singerAppService)
        {
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
        public IActionResult GetUnpublishedSingers()
        {
            var models = _singerAppService.GetUnpublishedSingers()?
                .OrderByDescending(s1 => s1.LastModificationTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((s, i) => new SingerViewModel()
                {
                    SId = ++i,
                    Id = s.Id,
                    Name = s.Name,
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
                    Name = s.Name,
                    ForeignName = s.ForeignName,
                    Nationality = s.Nationality,
                    CreationTime = s.CreationTime.ToStandardDateOfChina(),
                    PublishmentTime = s.PublishmentTime?.ToStandardDateOfChina()
                });
            return Json(models);
        }

        #region 创建

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create",new CreateSingerViewModel());
        }

        [HttpPost]
        public IActionResult Create(CreateSingerViewModel model)
        {
            var singer = new SingerModel()
            {
                Name = model.Name,
                ForeignName = model.ForeignName,
                Nationality = model.Nationality
            };
            singer = _singerAppService.Create(singer);
            return Json(new JsonResultEntity()
            {
                Message = "添加歌唱家成功！",
                JsonObject = Json(new SingerViewModel()
                {
                    Id = singer.Id,
                    Name = singer.Name,
                    ForeignName = singer.ForeignName,
                    Nationality = singer.Nationality,
                    CreationTime = singer.CreationTime.ToStandardDateOfChina(),
                })
            });
        }

        #endregion

        #region 更新基本信息

        [HttpGet]
        public IActionResult UpdateBasic(int id,UpdateBasicSingerViewModel model)
        {
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
        public IActionResult UpdateBasic(UpdateBasicSingerViewModel model)
        {
            var singer = new SingerModel()
            {
                Id = model.Id,
                ForeignName = model.ForeignName,
                Nationality = model.Nationality,
            };
            singer = _singerAppService.UpdateBasic(singer);
            return Json(new JsonResultEntity()
            {
                Message = "更新基本信息成功！",
                JsonObject = Json(new SingerViewModel()
                {
                    Id = singer.Id,
                    ForeignName = singer.ForeignName,
                    Nationality = singer.Nationality,
                    LastModificationTime = singer.LastModificationTime?.ToStandardDateOfChina()
                })
            });
        }

        #endregion

        #region 删除

        [HttpGet]
        public IActionResult Delete(int id, DeleteSingerViewModel model)
        {
            var singer = _singerAppService.GetSingerById(id);
            model = new DeleteSingerViewModel()
            {
                Id = singer.Id,
                Name = singer.Name
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteSingerViewModel model)
        {
            _singerAppService.Delete(model.Id);
            return Json(new JsonResultEntity()
            {
                Message = "删除成功",
                JsonObject = Json(new SingerViewModel() { Id = model.Id })
            });
        }

        #endregion

        #region 发布和下架

        [HttpPost]
        public IActionResult Publish(int id)
        {
            _singerAppService.Publish(id);
            return Json(new JsonResultEntity()
            {
                Message = "发布成功",
                JsonObject = Json(new { id })
            });
        }

        [HttpPost]
        public IActionResult Unpublish(int id)
        {
            _singerAppService.Unpublish(id);
            return Json(new JsonResultEntity()
            {
                Message = "下架成功",
                JsonObject = Json(new { id })
            });
        }

        #endregion
    }
}