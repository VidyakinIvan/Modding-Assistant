using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.Core
{
    /// <summary>
    /// Main application level exceptions handler
    /// </summary>
    public static class StartupErrorHandler
    {
        public static void HandleStartupException(Exception ex, IHost? host)
        {
            string message = $"Critical application startup error: {ex.Message ?? "Unknown error"}";
            string caption = "Startup Error";

            var localizationService = host?.Services.GetService<ILocalizationService>();
            if (localizationService != null)
            {
                message = localizationService.GetString("StartupError") ?? message;
                caption = localizationService.GetString("StartupErrorHeader") ?? caption;
            }
            var notificationService = host?.Services.GetService<INotificationService>();
            if (notificationService != null)
                notificationService.ShowError(message, caption);
            else
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
