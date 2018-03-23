using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Models
{
    public class MusicModel
    {
        public int Id { get; set; }
        public int SingerId { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string SingerName { get; set; }
        public string AlbumName { get; set; }
        public TimeSpan Duration { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public DateTime? PublishmentTime { get; set; }
    }
}
