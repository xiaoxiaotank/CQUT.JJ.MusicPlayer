using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class User
    {
        public User()
        {
            Permission = new HashSet<Permission>();
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }

        public ICollection<Permission> Permission { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
