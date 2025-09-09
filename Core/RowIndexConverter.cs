using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Modding_Assistant
{
    public class RowIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var dataGrid = values[0] as DataGrid;
            var item = values[1];
            if (dataGrid != null && item != null)
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
