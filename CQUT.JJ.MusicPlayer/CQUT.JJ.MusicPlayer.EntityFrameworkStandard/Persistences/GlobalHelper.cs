﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences
{
    public static class GlobalHelper
    {
        public static bool IsEffectiveMusicFile(this string fileUrl)
        {
            return GlobalConstants.Music_File_Suffix.Contains(Path.GetExtension(fileUrl));
        }
    }
}