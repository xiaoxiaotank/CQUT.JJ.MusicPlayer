using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class Permission
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public int Code { get; set; }
        public DateTime CreationTime { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
