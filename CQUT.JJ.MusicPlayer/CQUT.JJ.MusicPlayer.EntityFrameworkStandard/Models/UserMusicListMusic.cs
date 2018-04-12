using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class UserMusicListMusic
    {
        public int MusicListId { get; set; }
        public int MusicId { get; set; }

        public Music Music { get; set; }
        public UserMusicList MusicList { get; set; }
    }
}
