using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CQUT.JJ.MusicPlayer.Controls.Converters
{
    public class RadiusToMarginConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;

            if (((string)parameter).Equals("H"))
                return new Thickness(-val, 0, -val, 0);
            else if (((string)parameter).Equals("V"))
                return new Thickness(0, val, 0, val);

            return new Thickness();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Thickness)value;

            if (((string)parameter).Equals("H"))
                return -val.Left;
            else if (((string)parameter).Equals("V"))
                return val.Top;

            return 0d;
        }
    }
}
