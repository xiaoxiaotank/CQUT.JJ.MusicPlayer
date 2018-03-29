using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class TimeUtil
    {
        public static string GetMinuteAndSecondPart(this TimeSpan time)
        {
            return time.ToString(@"mm\:ss");
        }
    }
}
