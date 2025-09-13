using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Modding_Assistant.MVVM.View.Converters
{
    public class NullableIntConverter: IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString();
        }
        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str) && int.TryParse(str, out int result))
            {
                return result;
            }
            return null;
        }
    }
}
