using System.Globalization;
using System.Windows.Data;

namespace Modding_Assistant.MVVM.View.Converters
{
    /// <summary>
    /// Converts between <see cref="DateOnly"/> and <see cref="DateTime"/> types for data binding in WPF
    /// </summary>
    public class DateOnlyToDateTimeConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="DateOnly"/> value to a <see cref="DateTime"/> value
        /// </summary>
        /// <returns>
        /// A <see cref="DateTime"/> value with the same date as the input <see cref="DateOnly"/>
        /// and time set to <see cref="TimeOnly.MinValue"/>, or null if input is null
        /// </returns>
        /// <remarks>
        /// The conversion preserves the date portion while setting the time to 00:00:00 (midnight).
        /// </remarks>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            return null;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> value back to a <see cref="DateOnly"/> value
        /// </summary>
        /// <returns>
        /// A <see cref="DateOnly"/> value representing the date portion of the input <see cref="DateTime"/>,
        /// or null if input is null
        /// </returns>
        /// <remarks>
        /// The conversion extracts only the date portion from the <see cref="DateTime"/> value,
        /// effectively discarding the time component.
        /// </remarks>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
                return DateOnly.FromDateTime(dateTime);
            return null;
        }
    }
}
