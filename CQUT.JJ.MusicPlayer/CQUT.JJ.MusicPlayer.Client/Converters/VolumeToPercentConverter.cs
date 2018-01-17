using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CQUT.JJ.MusicPlayer.Client.Converters
{
    public class VolumeToPercentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentVolume = System.Convert.ToInt32(values[0]);
            var maxVolume = System.Convert.ToInt32(values[1]);
            return $"{currentVolume * 100 / maxVolume}%";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
