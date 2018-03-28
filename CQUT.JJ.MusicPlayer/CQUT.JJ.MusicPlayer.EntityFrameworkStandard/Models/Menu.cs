using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string TargetUrl { get; set; }
        public int ParentId { get; set; }
        public string RequiredAuthorizeCode { get; set; }
        public short Priority { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
