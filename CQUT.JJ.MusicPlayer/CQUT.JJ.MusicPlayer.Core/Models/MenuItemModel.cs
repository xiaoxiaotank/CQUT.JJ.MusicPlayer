using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Models
{
    public class MenuItemModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string TargetUrl { get; set; }
        public int? ParentId { get; set; }
        public string RequiredAuthorizeCode { get; set; }
        public short Priority { get; set; }


    }
}
