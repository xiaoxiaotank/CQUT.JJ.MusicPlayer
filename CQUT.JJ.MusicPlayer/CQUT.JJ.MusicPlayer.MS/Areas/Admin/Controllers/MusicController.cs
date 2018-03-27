using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Music;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Uitls.Helpers;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Controllers
{
    [Area("Admin")]   
    public class MusicController : Controller
    {
        private const string Log_Source = "音乐管理";

        private readonly IMusicAppService _musicAppService;
        private readonly ISingerAppService _singerAppService;
        private readonly IAlbumAppService _albumAppService;

        public MusicController(IMusicAppService musicAppService,
            ISingerAppService singerAppService, IAlbumAppService albumAppService)
        {
            _musicAppService = musicAppService;
            _singerAppService = singerAppService;
            _albumAppService = albumAppService;
        }

        [HttpGet]
        public IActionResult Unpublished()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问音乐未发布管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        public IActionResult Published()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试访问音乐已发布管理页面"
                    , userName
                    , LogType.Info
                    , Log_Source));
            return View();
        }

        [HttpGet]
        public IActionResult GetUnpublishedMusics()
        {
            var model = _musicAppService.GetUnpublishedMusics()?
                .OrderByDescending(s1 => s1.LastModificationTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((m,i) => new MusicViewModel()
                {
                    SId = ++i,
                    Id = m.Id,
                    SingerId = m.SingerId,
                    AlbumId = m.AlbumId,
                    Name = m.Name,
                    SingerName = m.SingerName,
                    AlbumName = m.AlbumName,
                    Duration = m.Duration.GetMinutesAndSeconds(),
                    FileUrl = m.FileUrl,
                    CreationTime = m.CreationTime.ToStandardDateOfChina(),
                    LastModificationTime = m.LastModificationTime?.ToStandardDateOfChina()
                });
            return Json(model);
        }

        [HttpGet]
        public IActionResult GetPublishedMusics()
        {
            var model = _musicAppService.GetPublishedMusics()?
                .OrderByDescending(s1 => s1.PublishmentTime)
                .ThenByDescending(s2 => s2.CreationTime)
                .Select((m, i) => new MusicViewModel()
                {
                    SId = ++i,
                    Id = m.Id,
                    SingerId = m.SingerId,
                    AlbumId = m.AlbumId,
                    Name = m.Name,
                    SingerName = m.SingerName,
                    AlbumName = m.AlbumName,
                    Duration = m.Duration.GetMinutesAndSeconds(),
                    FileUrl = m.FileUrl,
                    CreationTime = m.CreationTime.ToStandardDateOfChina(),
                    PublishmentTime = m.PublishmentTime?.ToStandardDateOfChina()
                });
            return Json(model);
        }

        #region 创建

        [HttpGet]
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试音乐创建操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var model = new CreateMusicViewModel()
            {
                Singers = _singerAppService.GetPublishedSingersHasAlbums()?
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }),
                Albums = _albumAppService.GetPublishedAlbums()?
                .Select(s => new SelectListItemEntity()
                {
                    Item = new SelectListItem()
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                    },
                    ParentId = s.SingerId.ToString()
                }),
            };
            if (model.Singers.Any())
                model.Singers.First().Selected = true;
            if (model.Albums.Any())
                model.Albums.First().Item.Selected = true;
            return PartialView("_Create", model);
        }

        [HttpPost]
        public IActionResult Create([FromServices]IHostingEnvironment env, CreateMusicViewModel model)
        {            
            if (int.TryParse(model.SingerId, out int singerId)
                && int.TryParse(model.AlbumId,out int albumId))
            {
                model.File = Request.Form.Files[0];

                var music = new MusicModel()
                {
                    Name = model.Name,
                    SingerId = singerId,
                    AlbumId = albumId,
                    FileUrl = SaveMusicFile(model.File,singerId,albumId)
                };
                music = _musicAppService.Create(music);
                var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
                LogHelper.Log(new LogItemEntity($"{userName} 成功完成音乐创建操作"
                        , userName
                        , LogType.Succeess
                        , Log_Source));
                return Json(new JsonResultEntity()
                {
                    Message = "添加音乐成功！",
                    JsonObject = Json(new MusicViewModel()
                    {
                        Id = music.Id,
                        SingerId = music.SingerId,
                        AlbumId = music.AlbumId,
                        Name = music.Name,
                        SingerName = music.SingerName,
                        AlbumName = music.AlbumName,
                        FileUrl = music.FileUrl,
                        Duration = music.Duration.GetMinutesAndSeconds(),
                        CreationTime = music.CreationTime.ToStandardDateOfChina(),
                    })
                });
            }
            else
                throw new JMBasicException("歌唱家不存在");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>文件保存相对路径</returns>
        [NonAction]
        private string SaveMusicFile(IFormFile file,int singerId,int albumId)
        {
            var singer = _singerAppService.GetSingerById(singerId);
            var album = _albumAppService.GetAlbumById(albumId);
            var fileName = FileHelper.GetFullFileNameOfMusic(file.ContentDisposition, singer, album);
            if(!EntityFramework.Persistences.GlobalHelper.IsEffectiveMusicFile(fileName))
                throw new JMBasicException("文件无效");
            if (file.SaveTo(fileName))
                return fileName;
            throw new JMBasicException("文件上传失败");
        }

        #endregion

        #region 更新基本信息

        [HttpGet]
        public IActionResult UpdateBasic(int id, UpdateBasicMusicViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试音乐更新基本信息操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var music = _musicAppService.GetMusicById(id);
            model = new UpdateBasicMusicViewModel()
            {
                Id = music.Id,
                Name = music.Name,
                Singers = _singerAppService.GetPublishedSingersHasAlbums()?
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                    Selected = music.SingerId == s.Id
                }),
                Albums = _albumAppService.GetPublishedAlbums()?
                .Select(s => new SelectListItemEntity()
                {
                    Item = new SelectListItem()
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                        Selected = music.AlbumId == s.Id
                    },
                    ParentId = s.SingerId.ToString()
                }),
            };
            return PartialView("_UpdateBasic", model);
        }

        [HttpPost]
        public IActionResult UpdateBasic(UpdateBasicMusicViewModel model)
        {
            if (int.TryParse(model.SingerId, out int singerId)
                && int.TryParse(model.AlbumId,out int albumId))
            {
                var music = new MusicModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    SingerId = singerId,
                    AlbumId = albumId
                };
                music = _musicAppService.UpdateBasic(music);
                var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
                LogHelper.Log(new LogItemEntity($"{userName} 成功完成音乐更新基本信息操作"
                        , userName
                        , LogType.Succeess
                        , Log_Source));
                return Json(new JsonResultEntity()
                {
                    Message = "更新基本信息成功！",
                    JsonObject = Json(new MusicViewModel()
                    {
                        Id = music.Id,
                        SingerId = music.SingerId,
                        AlbumId = music.AlbumId,
                        Name = music.Name,
                        SingerName = music.SingerName,
                        AlbumName = music.AlbumName,
                        LastModificationTime = music.LastModificationTime?.ToStandardDateOfChina()
                    })
                });
            }
            throw new JMBasicException("歌唱家或专辑不存在");
        }

        #endregion

        #region 删除

        [HttpGet]
        public IActionResult Delete(int id, DeleteMusicViewModel model)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试音乐删除操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            var music = _musicAppService.GetMusicById(id);
            model = new DeleteMusicViewModel()
            {
                Id = music.Id,
                Name = music.Name
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteMusicViewModel model)
        {
            _musicAppService.Delete(model.Id);
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成音乐删除操作"
                    , userName
                    , LogType.Succeess
                    , Log_Source));
            return Json(new JsonResultEntity()
            {
                Message = "删除成功",
                JsonObject = Json(new MusicViewModel() { Id = model.Id })
            });
        }

        #endregion

        #region 发布和下架

        [HttpPost]
        public IActionResult Publish(int id)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试音乐发布操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _musicAppService.Publish(id);
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成音乐发布操作"
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
        public IActionResult Unpublish(int id)
        {
            var userName = HttpContext.Session.GetCurrentUser()?.UserName ?? Utils.Helpers.GlobalHelper.Unlogin_User_Name;
            LogHelper.Log(new LogItemEntity($"{userName} 尝试音乐下架操作"
                    , userName
                    , LogType.Info
                    , Log_Source));
            _musicAppService.Unpublish(id);
            LogHelper.Log(new LogItemEntity($"{userName} 成功完成音乐下架操作"
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