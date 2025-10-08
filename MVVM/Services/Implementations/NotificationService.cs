using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Implementation of INotificationService using WPF MessageBox
    /// </summary>
    public class NotificationService : INotificationService
    {
        /// <inheritdoc/>
        public void ShowError(string caption, string message)
        {
            ShowMessageBox(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <inheritdoc/>
        public void ShowWarning(string caption, string message)
        {
            ShowMessageBox(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <inheritdoc/>
        public void ShowInformation(string caption, string message)
        {
            ShowMessageBox(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <inheritdoc/>
        public bool ShowConfirmation(string caption, string message)
        {
            return ShowConfirmationBox(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        /// <summary>
        /// Shows MessageBox
        /// </summary>
        private static void ShowMessageBox(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(message, caption, button, icon);
        }

        /// <summary>
        /// Shows confirmation MessageBox
        /// </summary>
        private static bool ShowConfirmationBox(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            var result = MessageBox.Show(message, caption, button, icon);

            return result == MessageBoxResult.Yes;
        }
    }
}
