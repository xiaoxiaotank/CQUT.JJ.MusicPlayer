using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Menu
{
    public class UpdateMenuItemViewModel
    {
        public int Id { set; get; }

        public string Header { get; set; }
        public string TargetUrl { get; set; }
        public string RequiredAuthorizeCode { set; get; }
        public short Priority { get; set; }

    }
}
