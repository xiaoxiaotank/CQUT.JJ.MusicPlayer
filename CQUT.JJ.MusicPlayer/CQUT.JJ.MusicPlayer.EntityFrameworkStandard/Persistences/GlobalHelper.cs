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
            var stopDict = LoadStopDict(GlobalConstants.MusicStopDictionaryPath);
            var segmentList = (segmenter.CutForSearch(key))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
            var segments = segmentList.Except(stopDict);
            return segments.ToArray();
        }

        /// <summary>
        /// 加载停用词
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static IEnumerable<string> LoadStopDict(string path)
        {
            return File.ReadAllLines(path,Encoding.Default);
        }
    }
}
