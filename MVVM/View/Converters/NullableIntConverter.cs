using System.Globalization;
using System.Windows.Data;

namespace Modding_Assistant.MVVM.View.Converters
{
    /// <summary>
    /// Converts between nullable <see cref="int"/> and <see cref="string"/> types for data binding in WPF
    /// </summary>
    public class NullableIntConverter: IValueConverter
    {
        /// <summary>
        /// Converts a nullable <see cref="int"/> value to a <see cref="string"/> representation
        /// </summary>
        /// <returns>
        /// String representation of the integer value, or null if the input value is null
        /// </returns>
        /// <remarks>
        /// This conversion is straightforward and uses the default ToString() conversion.
        /// Null values are preserved as null in the conversion.
        /// </remarks>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        /// <summary>
        /// Converts a <see cref="string"/> value back to a nullable <see cref="int"/>
        /// </summary>
        /// <returns>
        /// Parsed integer value if conversion succeeds, null if input is empty, whitespace, or invalid
        /// </returns>
        /// <remarks>
        /// The conversion attempts to parse the string using integer parsing rules.
        /// Empty strings, whitespace, and non-numeric values are converted to null.
        /// The conversion uses the specified culture for number formatting.
        /// </remarks>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str) && int.TryParse(str, culture, out int result))
            {
                return result;
            }
            return null;
        }
    }
}
