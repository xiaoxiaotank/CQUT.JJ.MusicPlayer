using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences
{
    public static class GlobalHelper
    {
        public static bool IsEffectiveMusicFile(this string fileUrl)
        {
            return GlobalConstants.Music_File_Suffix.Contains(Path.GetExtension(fileUrl));
        }

        public static string[] ToSeparateByJieba(this string key)
        {
            var segmenter = new JiebaSegmenter();
            segmenter.LoadUserDict(GlobalConstants.MusicDictionaryPath);
            var segments = (segmenter.CutForSearch(key))
                .Where(s => !string.IsNullOrWhiteSpace(s));
            return segments.ToArray();
        }
    }
}
