using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.Core.Application
{
    /// <summary>
    /// Main application level exceptions handler
    /// </summary>
    public static class StartupErrorHandler
    {
        /// <summary>
        /// Handling method with <see cref="ILocalizationService"/> and <see cref="INotificationService"/> using
        /// </summary>
        public static void HandleStartupException(Exception ex, ILocalizationService? localizationService = null,
            INotificationService? notificationService = null)
        {
            string message = $"Critical application startup error: {ex.Message ?? "Unknown error"}";
            string caption = "Startup Error";

            if (localizationService != null)
            {
                message = localizationService.GetString("StartupError") ?? message;
                caption = localizationService.GetString("StartupErrorHeader") ?? caption;
            }

            if (notificationService != null)
                notificationService.ShowError(message, caption);
            else
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
