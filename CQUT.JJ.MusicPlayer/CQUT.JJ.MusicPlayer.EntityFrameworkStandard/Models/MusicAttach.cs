using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class MusicAttach
    {
        public int Id { get; set; }
        public int MusicId { get; set; }
        public string CoverUrl { get; set; }
        public int Passion { get; set; }

        public Music Music { get; set; }
    }
}
