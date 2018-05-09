using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class User
    {
        public User()
        {
            AlbumCreator = new HashSet<Album>();
            AlbumMender = new HashSet<Album>();
            AlbumPublisher = new HashSet<Album>();
            AlbumUnpublisher = new HashSet<Album>();
            MusicCreator = new HashSet<Music>();
            MusicMender = new HashSet<Music>();
            MusicPublisher = new HashSet<Music>();
            MusicUnpublisher = new HashSet<Music>();
            Permission = new HashSet<Permission>();
            SingerCreator = new HashSet<Singer>();
            SingerMender = new HashSet<Singer>();
            SingerPublisher = new HashSet<Singer>();
            SingerUnpublisher = new HashSet<Singer>();
            UserLike = new HashSet<UserLike>();
            UserMusicList = new HashSet<UserMusicList>();
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

        public ICollection<Album> AlbumCreator { get; set; }
        public ICollection<Album> AlbumMender { get; set; }
        public ICollection<Album> AlbumPublisher { get; set; }
        public ICollection<Album> AlbumUnpublisher { get; set; }
        public ICollection<Music> MusicCreator { get; set; }
        public ICollection<Music> MusicMender { get; set; }
        public ICollection<Music> MusicPublisher { get; set; }
        public ICollection<Music> MusicUnpublisher { get; set; }
        public ICollection<Permission> Permission { get; set; }
        public ICollection<Singer> SingerCreator { get; set; }
        public ICollection<Singer> SingerMender { get; set; }
        public ICollection<Singer> SingerPublisher { get; set; }
        public ICollection<Singer> SingerUnpublisher { get; set; }
        public ICollection<UserLike> UserLike { get; set; }
        public ICollection<UserMusicList> UserMusicList { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
