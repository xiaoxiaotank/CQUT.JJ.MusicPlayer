using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CQUT.JJ.MusicPlayer.Client.Converters
{
    public class DoubleParametersSubtractionConverter : IMultiValueConverter
    {      
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var firArg = System.Convert.ToDouble(values[0]);
            var secondArg = System.Convert.ToDouble(values[1]);
            return Math.Abs(firArg - secondArg);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
