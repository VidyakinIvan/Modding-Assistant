using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.Core
{
    /// <summary>
    /// Class for starting <see cref="IHost"/> container, <see cref="ILocalizationService"/> and <see cref="IDatabaseService"/>
    /// </summary>
    public class ApplicationInitializer() : IApplicationInitializer
    {
        public async Task InitializeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            await InitializeDatabaseAsync(serviceProvider, cancellationToken);
            var localizationService = serviceProvider.GetService<ILocalizationService>();
            if (localizationService != null)
            {
                Application.Current.Resources["LocalizationService"] = localizationService;
            }
            ShowMainWindow(serviceProvider);

        }
        /// <summary>
        /// Async Task for <see cref="IDatabaseService"/> initialization
        /// </summary>
        private async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            using var scope = serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token
            );
            await databaseService.InitializeAsync(linkedCts.Token);
        }

        /// <summary>
        /// Opens main application window
        /// </summary>
        private void ShowMainWindow(IServiceProvider serviceProvider)
        {
            if (serviceProvider != null)
            {
                var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                var mainWindowService = serviceProvider.GetRequiredService<IMainWindowService>();
                mainWindowService.SetMainWindow(mainWindow);
                mainWindow.Show();
            }
        }
    }
}