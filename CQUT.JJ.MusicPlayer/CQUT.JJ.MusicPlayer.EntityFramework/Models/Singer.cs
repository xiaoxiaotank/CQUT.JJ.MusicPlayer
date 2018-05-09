using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class Singer
    {
        public Singer()
        {
            Album = new HashSet<Album>();
            Music = new HashSet<Music>();
            SingerAttach = new HashSet<SingerAttach>();
        }

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int? MenderId { get; set; }
        public int? PublisherId { get; set; }
        public int? UnpublisherId { get; set; }
        public string Name { get; set; }
        public string ForeignName { get; set; }
        public string Nationality { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishmentTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }

        public User Creator { get; set; }
        public User Mender { get; set; }
        public User Publisher { get; set; }
        public User Unpublisher { get; set; }
        public ICollection<Album> Album { get; set; }
        public ICollection<Music> Music { get; set; }
        public ICollection<SingerAttach> SingerAttach { get; set; }
    }
}
