using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels.Common
{
    public class AlbumViewModel
    {

        public int Id { get; set; }

        public int SingerId { get; set; }

        public string Name { get; set; }

        public string SingerName { get; set; }

        public DateTime PublishedTime { get; set; }

        public int MusicCount { get; set; }

        public string HeaderPath { get; set; }
    }
}
