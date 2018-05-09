using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class UserLike
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? MusicId { get; set; }
        public int? MusicListId { get; set; }
        public int? AlbumId { get; set; }

        public Album Album { get; set; }
        public Music Music { get; set; }
        public User User { get; set; }
    }
}
