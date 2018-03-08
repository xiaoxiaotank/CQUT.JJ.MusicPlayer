using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class Role
    {
        public Role()
        {
            Permission = new HashSet<Permission>();
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }

        public ICollection<Permission> Permission { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
