using CQUT.JJ.MusicPlayer.MS.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Music
{
    public class CreateMusicViewModel
    {
        [DisplayName("歌唱家")]
        public string SingerId { get; set; }

        [DisplayName("专辑")]
        public string AlbumId { get; set; }

        [DisplayName("音乐名")]
        public string Name { get; set; }

        [DisplayName("文件")]
        [FileExtensions(Extensions = ".mp3")]
        public IFormFile File { get; set; }

        public IEnumerable<SelectListItem> Singers { get; set; }

        public IEnumerable<SelectListItemEntity> Albums { get; set; }
    }
}
