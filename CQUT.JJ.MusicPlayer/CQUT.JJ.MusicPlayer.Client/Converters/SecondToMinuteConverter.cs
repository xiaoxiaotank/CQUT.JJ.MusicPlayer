using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CQUT.JJ.MusicPlayer.Client.Converters
{
    public class SecondToMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = TimeSpan.FromSeconds((double)value);
            return $"{time.Hours * 60 + time.Minutes:00}:{time.Seconds:00}"; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
