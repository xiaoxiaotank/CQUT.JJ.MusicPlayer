using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Menu
{
    public class CreateMenuItemViewModel
    {
        public int Id { set; get; }

        [Required]
        public string Header { get; set; }

        public int ParentId { set; get; }

        public string ParentHeader { set; get; }
       
        public string TargetUrl { get; set; }

        public string RequiredAuthorizeCode { set; get; }
    }
}
