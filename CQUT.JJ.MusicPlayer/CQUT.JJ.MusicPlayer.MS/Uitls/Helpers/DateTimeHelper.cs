using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Uitls.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// yyyy
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToStandardDateOfChina(this DateTime date)
        {
            return ((DateTime)date).ToString("yyyy年MM月dd日");
        }
    }
}
