﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Models
{
    public class BaseQMusicInfoModel
    {
        public string Name { get; set; }

        public string Singer { get; set; }

        public string TimeDuration { get; set; }

        public Uri PhotoUri { get; set; }
    }
}
