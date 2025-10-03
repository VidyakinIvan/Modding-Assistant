using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Windows;
using System.Windows;

namespace Modding_Assistant.Core
{
    /// <summary>
    /// Class for starting <see cref="IHost"/> container, <see cref="ILocalizationService"/> and <see cref="IDatabaseService"/>
    /// </summary>
    public class ApplicationInitializer(IServiceProvider serviceProvider, ILogger<ApplicationInitializer> logger) 
        : IApplicationInitializer
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<ApplicationInitializer> _logger = logger;
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await InitializeDatabaseAsync(cancellationToken);
            var localizationService = _serviceProvider.GetService<ILocalizationService>();
            if (localizationService != null)
            {
                Application.Current.Resources["LocalizationService"] = localizationService;
            }
            ShowMainWindow();

        }
        /// <summary>
        /// Async Task for <see cref="IDatabaseService"/> initialization
        /// </summary>
        private async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
        {
            var databaseService = _serviceProvider.GetRequiredService<IDatabaseService>();

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token
            );
            await databaseService.InitializeAsync(linkedCts.Token);
        }

        /// <summary>
        /// Opens main application window
        /// </summary>
        private void ShowMainWindow()
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            var mainWindowService = _serviceProvider.GetRequiredService<IMainWindowService>();
            mainWindowService.SetMainWindow(mainWindow);
            mainWindow.Show();

            _logger.LogInformation("Main window opened");
        }
    }
}