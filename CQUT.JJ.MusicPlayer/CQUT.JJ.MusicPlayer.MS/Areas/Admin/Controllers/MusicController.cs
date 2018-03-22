using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Music;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MusicController : Controller
    {
        private readonly IMusicAppService _musicAppService;

        public MusicController(IMusicAppService musicAppService)
        {
            _musicAppService = musicAppService;
        }

        [HttpGet]
        public IActionResult Unpublished()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUnpublishedMusics()
        {
            var model = _musicAppService.GetUnpublishedMusics()
                .Select(m => new MusicViewModel()
                {
                    Id = m.Id,
                    SingerId = m.SingerId,
                    AlbumId = m.AlbumId,
                    Name = m.Name,
                    SingerName = m.SingerName,
                    AlbumName = m.AlbumName,
                    Duration = m.Duration,
                    FileUrl = m.FileUrl,
                    CreationTime = m.CreationTime.ToStandardDateOfChina()
                });
            return Json(model);
        }
    }
}