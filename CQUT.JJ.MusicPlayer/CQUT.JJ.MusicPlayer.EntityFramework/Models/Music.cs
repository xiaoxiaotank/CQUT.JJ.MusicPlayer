using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class Music
    {
        public Music()
        {
            MusicAttach = new HashSet<MusicAttach>();
        }

        public int Id { get; set; }
        public int SingerId { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }

        public Album Album { get; set; }
        public Singer Singer { get; set; }
        public ICollection<MusicAttach> MusicAttach { get; set; }
    }
}
