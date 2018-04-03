using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class Album
    {
        public Album()
        {
            Music = new HashSet<Music>();
            UserLike = new HashSet<UserLike>();
        }

        public int Id { get; set; }
        public int SingerId { get; set; }
        public int CreatorId { get; set; }
        public int? MenderId { get; set; }
        public int? PublisherId { get; set; }
        public int? UnpublisherId { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishmentTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }

        public User Creator { get; set; }
        public User Mender { get; set; }
        public User Publisher { get; set; }
        public Singer Singer { get; set; }
        public User Unpublisher { get; set; }
        public ICollection<Music> Music { get; set; }
        public ICollection<UserLike> UserLike { get; set; }
    }
}
