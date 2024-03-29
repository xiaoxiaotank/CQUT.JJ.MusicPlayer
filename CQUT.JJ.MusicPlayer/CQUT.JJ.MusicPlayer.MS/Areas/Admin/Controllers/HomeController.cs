﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Home;
using CQUT.JJ.MusicPlayer.MS.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ISingerAppService _singerAppService;
        private readonly IAlbumAppService _albumAppService;
        private readonly IMusicAppService _musicAppService;
        private readonly IUserAppService _userAppService;

        public HomeController(ISingerAppService singerAppService, IAlbumAppService albumAppService, 
            IMusicAppService musicAppService, IUserAppService userAppService)
        {
            _singerAppService = singerAppService;
            _albumAppService = albumAppService;
            _musicAppService = musicAppService;
            _userAppService = userAppService;
        }

        [HttpGet]
        [MvcAuthorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPublishedInfoPerDay(int dayNumber)
        {
            var singerDic = _singerAppService.GetPublishedSingerCountPerDay(dayNumber);
            var albumDic = _albumAppService.GetPublishedAlbumCountPerDay(dayNumber);
            var musicDic = _musicAppService.GetPublishedMusicCountPerDay(dayNumber);

            var today = DateTime.Now.Date;
            for (int i = 0; i < dayNumber; i++)
            {
                var date = DateTime.SpecifyKind(today.AddDays(-i), DateTimeKind.Unspecified);
                
                if (!singerDic.ContainsKey(date))
                    singerDic[date] = 0;
                if (!albumDic.ContainsKey(date))
                    albumDic[date] = 0;
                if (!musicDic.ContainsKey(date))
                    musicDic[date] = 0;
            }

            var models = new List<DayCountViewModel>()
            {
                new DayCountViewModel(){ Name = "歌唱家",Value = singerDic.OrderBy(a => a.Key).ToDictionary(a => a.Key,b => b.Value)},
                new DayCountViewModel(){ Name = "专辑",Value = albumDic},
                new DayCountViewModel(){ Name = "音乐",Value = musicDic}
            };
            return Json(models);
        }

        [HttpGet]
        public IActionResult GetUserCount()
        {
            var model = new UserCountViewModel()
            {
                MemberCount = _userAppService.GetMemberCount(),
                TodayRegisterMemberCount = _userAppService.GetTodayRegisterMemberCount(),
                EmployeeCount = _userAppService.GetEmployeeCount(),
                TodayCreateEmployeeCount = _userAppService.GetTodyCreateEmployeeCount()
            };
            return Json(model);
        }
    }
}