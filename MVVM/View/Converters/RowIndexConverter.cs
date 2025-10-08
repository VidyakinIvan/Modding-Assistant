using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Modding_Assistant.MVVM.View.Converters
{
    /// <summary>
    /// Converts a DataGrid item to its display row index (1-based) for showing row numbers
    /// </summary>
    /// <remarks>
    /// This multi-value converter is designed to display row numbers in DataGrid controls.
    /// It takes two input values: the DataGrid control and the current data item.
    /// </remarks>
    public class RowIndexConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts DataGrid and data item to a 1-based row index string
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length != 2)
                return string.Empty;

            var item = values[1];
            if (values[0] is DataGrid dataGrid && item != null)
            {
                int index = dataGrid.Items.IndexOf(item);
                if (index >= 0)
                    return (index + 1).ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// This method is not implemented as row index conversion is one-way only
        /// </summary>
        /// <returns>
        /// This method always throws <see cref="NotImplementedException"/>
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
