using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CQUT.JJ.MusicPlayer.Client.Converters
{
    public class VolumePercentToToolTip : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "音量：" + (System.Convert.ToInt32(((string)value).Replace("%",string.Empty)) != 0 ? value : "静音");
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
