using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Uitls.Helpers
{
    public static class TimeHelper
    {
        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToStandardDateOfChina(this DateTime date)
        {
            return ((DateTime)date).ToString("yyyy年MM月dd日");
        }

        /// <summary>
        /// 获取分钟和秒的部分
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetMinutesAndSeconds(this TimeSpan time)
        {
            return $"{time.Minutes}:{time.Seconds}";
        }

        /// <summary>
        /// yyyy年MM月dd日 HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStandardDateTimeOfChina(this DateTime dateTime)
        {
            return ((DateTime)dateTime).ToString("yyyy年MM月dd日 HH : mm : ss");
        }
    }
}
