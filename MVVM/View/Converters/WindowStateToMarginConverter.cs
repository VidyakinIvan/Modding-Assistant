using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Modding_Assistant.MVVM.View.Converters
{
    /// <summary>
    /// Converts <see cref="WindowState"/> to <see cref="Thickness"/> margins to compensate for window chrome differences
    /// </summary>
    /// <remarks>
    /// This converter is specifically designed to handle the visual differences between window states.
    /// When a window is maximized, it typically loses its window chrome (border, title bar shadows, etc.),
    /// which can cause content to appear too close to the screen edges.
    /// </remarks>
    public class WindowStateToMarginConverter : IValueConverter
    {
        public Thickness MaximizedMargin { get; set; } = new Thickness(0, 6, 6, 0);
        public Thickness NormalMargin { get; set; } = new Thickness(0);

        /// <summary>
        /// Converts a <see cref="WindowState"/> value to a <see cref="Thickness"/> margin
        /// </summary>
        /// <returns>
        /// <see cref="Thickness"/> with margins appropriate for the window state
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WindowState state)
            {
                return state == WindowState.Maximized ? MaximizedMargin : NormalMargin;
            }
            return new Thickness(0);
        }

        /// <summary>
        /// This method is not implemented as window state to margin conversion is one-way only
        /// </summary>
        /// <returns>
        /// This method always throws <see cref="NotImplementedException"/>
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
