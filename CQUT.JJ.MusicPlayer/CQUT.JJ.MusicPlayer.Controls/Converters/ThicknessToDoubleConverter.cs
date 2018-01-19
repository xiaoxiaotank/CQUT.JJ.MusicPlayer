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
    public class ThicknessToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Thickness thickness)
            {
                return new List<double>()
                {
                    thickness.Left,
                    thickness.Top,
                    thickness.Right,
                    thickness.Bottom
                }.Max();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double val)
            {
                return new Thickness(val);
            }
            return new Thickness();
        }
    }
}
