using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Modding_Assistant.MVVM.View.Converters
{
    public class RowIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[1];
            if (values[0] is DataGrid dataGrid && item != null)
            {
                int index = dataGrid.Items.IndexOf(item);
                if (index >= 0)
                    return (index + 1).ToString();
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
