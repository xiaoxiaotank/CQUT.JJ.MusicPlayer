using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CQUT.JJ.MusicPlayer.Client.Converters
{
    /// <summary>
    /// 去除相同颜色叠加效果 
    /// 即上下两层颜色相同，但是透明度不同，想要通过颜色叠加获得一定透明度的情况
    /// </summary>
    public class RemoveIdenticalColorSuperpositionEffectOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var origOpacity = System.Convert.ToDouble(value);
            return 2 * origOpacity - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
