using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.Core;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using System.Windows;

namespace Modding_Assistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host = null!;
        private ILogger<App>? _logger;

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationServices()
                        .AddDatabase(context.Configuration)
                        .AddViewModels()
                        .AddViews();
                });

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                _host = CreateHostBuilder(e.Args).Build();
                _logger = _host.Services.GetRequiredService<ILogger<App>>();
                await _host.StartAsync();
                await InitializeDatabaseAsync();
                Current.Resources["LocalizationService"] = _host.Services.GetRequiredService<ILocalizationService>();
                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                mainWindow.Show();  
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                HandleStartupException(ex);
            }
        }
        private void HandleStartupException(Exception ex)
        {
            _logger?.LogCritical(ex, "Application startup failed");
            string message = "Critical application startup error.";
            string caption = "Startup Error";
            if (_host != null)
            {
                try
                {
                    var localizationService = _host.Services.GetRequiredService<ILocalizationService>();
                    message = localizationService.GetString("StartupError") ?? message;
                    caption = localizationService.GetString("StartupErrorHeader") ?? caption;
                }
                catch (Exception serviceEx)
                {
                    _logger?.LogWarning(serviceEx, "Failed to get localization service");
                }
            }
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
        private async Task InitializeDatabaseAsync()
        {
            using var scope = _host.Services.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await databaseService.InitializeAsync(cts.Token);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                if (_host != null)
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await _host.StopAsync(cts.Token);
                    _host.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during application shutdown");
            }
            finally
            {
                base.OnExit(e);
            }
        }
    }
}
