using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class StringFormatUtil
    {
        public static string ToHttpUrl(this string url)
        {
            if (url.StartsWith(@"http://")) return url;
            if (url.StartsWith(@"//")) return $"http:{url}";
            if (!url.Contains(@"//")) return $"http://{url}";
            return string.Empty;
        }
    }
}
