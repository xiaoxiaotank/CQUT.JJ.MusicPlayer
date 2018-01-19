using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class JmSkinOpacityChangedUtil
    {
        public static event EventHandler<SkinOpacityChangedArgs> SkinOpacityChangedEvent;

        public static void Invoke(double opacity)
        {
            var e = new SkinOpacityChangedArgs(opacity);
            SkinOpacityChangedEvent(null, e);
        }
    }

    public class SkinOpacityChangedArgs : EventArgs
    {
        public double Opacity { get; set; }
        public SkinOpacityChangedArgs(double opacity)
        {
            Opacity = opacity;
        }
    }
}
