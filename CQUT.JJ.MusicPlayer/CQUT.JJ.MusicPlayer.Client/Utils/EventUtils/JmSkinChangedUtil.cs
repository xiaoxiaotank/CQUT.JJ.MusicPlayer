using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class JmSkinChangedUtil
    {
        public static readonly string SkinConfigFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/SkinConfig.bat");

        public static event EventHandler<SkinChangedArgs> SkinChangedEvent;

        public static string DefaultImageSkinPath = "pack://application:,,,/CQUT.JJ.MusicPlayer.Client;component/Asserts/Skins/ThemeSkins/DefaultWindowBackground.jpg";

        public static SkinChangedArgs DefaultImageSkinArgs;

        static JmSkinChangedUtil()
        {
            DefaultImageSkinArgs = new SkinChangedArgs(
                new Uri(DefaultImageSkinPath, UriKind.Absolute)
                .ToImageBrush()
                , true);
        }

        public static void Invoke(SkinModel skin)
        {
            var e = new SkinChangedArgs(skin.Background, skin.IsImageBrush);
            File.WriteAllLines(JmSkinChangedUtil.SkinConfigFilePath, new[] { skin.IsImageBrush.ToString(), skin.ImagePathOrColor });
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
