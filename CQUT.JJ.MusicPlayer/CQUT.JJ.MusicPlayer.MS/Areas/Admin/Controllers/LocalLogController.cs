using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Extensions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.LocalLog;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocalLogController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = LogHelper.GetLogs()
                .OrderByDescending(l => l.DateTime)
                .Select((l, i) => new LogViewModel()
                {
                    SId = ++i,
                    Message = l.Message,
                    Type = l.Type.GetEnumDescription(),
                    UserName = l.UserName,
                    Source = l.Source,
                    DateTime = l.DateTime.ToStandardDateTimeOfChina()
                });
            return Json(logs);
        }
    }
}