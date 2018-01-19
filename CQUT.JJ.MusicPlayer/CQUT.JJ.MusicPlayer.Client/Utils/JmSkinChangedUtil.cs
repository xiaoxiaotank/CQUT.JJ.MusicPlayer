using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class JmSkinChangedUtil
    {
        public static event EventHandler<SkinChangedArgs> SkinChangedEvent;

        public static SkinChangedArgs DefaultImageSkinArgs = new SkinChangedArgs(
            new Uri("pack://application:,,,/CQUT.JJ.MusicPlayer.Client;component/Asserts/ThemeSkins/DefaultWindowBackground.jpg", UriKind.Absolute)
            .ImageUriToImageBrush()
            ,true);

        public static void Invoke(Brush background,bool isImageBrush = false)
        {
            var e = new SkinChangedArgs(background,isImageBrush);
            SkinChangedEvent(null,e);
        }
    }

    public class SkinChangedArgs:EventArgs
    {
        public Brush Background { get; set; }

        public bool IsImageBrush { get; set; }

        public SkinChangedArgs(Brush background,bool isImageBrush)
        {
            Background = background;
            IsImageBrush = isImageBrush;
        }
    }
}
