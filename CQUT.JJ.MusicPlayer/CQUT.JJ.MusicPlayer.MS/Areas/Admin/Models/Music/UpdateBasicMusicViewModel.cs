using CQUT.JJ.MusicPlayer.MS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Music
{
    public class UpdateBasicMusicViewModel
    {
        public int Id { get; set; }

        [DisplayName("音乐名")]
        public string Name { get; set; }

        [DisplayName("歌唱家")]
        public string SingerId { get; set; }

        [DisplayName("专辑")]
        public string AlbumId { get; set; }

        public IEnumerable<SelectListItem> Singers { get; set; }

        public IEnumerable<SelectListItemEntity> Albums { get; set; }
    }
}
