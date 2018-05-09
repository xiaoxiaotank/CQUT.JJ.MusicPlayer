using System;
using System.Collections.Generic;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class SingerAttach
    {
        public int Id { get; set; }
        public int SingerId { get; set; }
        public int FansNumber { get; set; }

        public Singer Singer { get; set; }
    }
}
