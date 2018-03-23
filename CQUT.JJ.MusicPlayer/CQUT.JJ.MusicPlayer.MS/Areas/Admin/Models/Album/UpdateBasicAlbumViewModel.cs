using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Album
{
    public class UpdateBasicAlbumViewModel
    {
        public int Id { get; set; }

        [DisplayName("专辑名")]
        public string Name { get; set; }

        [DisplayName("歌手")]
        public string SingerId { get; set; }

        public IEnumerable<SelectListItem> Singers { get; set; }
    }
}
