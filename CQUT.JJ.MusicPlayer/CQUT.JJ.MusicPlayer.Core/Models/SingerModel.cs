using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Models
{
    public class SingerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ForeignName { get; set; }
        public string Nationality { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime? PublishmentTime { get; set; }
    }
}
