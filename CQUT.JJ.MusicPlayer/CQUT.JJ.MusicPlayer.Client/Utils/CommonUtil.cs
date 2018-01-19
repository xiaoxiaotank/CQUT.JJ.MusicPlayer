using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class CommonUtil
    {
        /// <summary>
        /// 将ImageUri转为ImageBrush
        /// </summary>
        /// <param name="imageUri"></param>
        /// <returns></returns>
        public static ImageBrush ToImageBrush(this Uri imageUri)
        {
            return new ImageBrush()
            {
                ImageSource = new BitmapImage(imageUri),
                Stretch = Stretch.UniformToFill
            };
        }
    }
}
