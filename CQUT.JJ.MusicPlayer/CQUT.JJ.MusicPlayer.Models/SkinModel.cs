using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Models
{
    /// <summary>
    /// 皮肤方面数据模型
    /// </summary>
    public class SkinModel
    {
        public Brush Background { get; set; }

        public bool IsImageBrush { get; set; }

        public string ImagePathOrColor { get; set; }

        public SkinModel(Brush background, string imagePathOrColor, bool isImageBrush = false)
        {
            Background = background;
            ImagePathOrColor = imagePathOrColor;
            IsImageBrush = isImageBrush;
        }
    }
}
