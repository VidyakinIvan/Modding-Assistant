using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.Core.Application;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;
        private ILogger<App>? _logger;

        private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(2);
        private static readonly TimeSpan ShutdownTimeout = TimeSpan.FromSeconds(30);

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using var startupCts = new CancellationTokenSource(StartupTimeout);

                await InitializeApplicationAsync(e.Args, startupCts.Token);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("Application startup failed");

                StartupErrorHandler.HandleStartupException(ex, _host?.Services.GetService<ILocalizationService>(),
                    _host?.Services.GetService<INotificationService>());

                Shutdown(1);
            }
        }

        /// <summary>
        /// Async Task for application initialization
        /// </summary>
        private async Task InitializeApplicationAsync(string[] args, CancellationToken cancellationToken)
        {
            try
            {
                _host = HostBuilderFactory.CreateHostBuilder(args).Build();
                _logger = _host.Services.GetRequiredService<ILogger<App>>();

                await _host.StartAsync(cancellationToken);

                _logger.LogInformation("Application initialization started");

                var applicationInitializer = _host.Services.GetRequiredService<IApplicationInitializer>();
                await applicationInitializer.InitializeAsync(cancellationToken);

            }
            catch (OperationCanceledException)
            {
                _logger?.LogError("Application initialization was cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during application initialization");
                throw;
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                if (_host is not null)
                {
                    var _modManagerService = _host.Services.GetRequiredService<IModManagerService>();

                    using var cts = new CancellationTokenSource(ShutdownTimeout);

                    _logger?.LogInformation("Saving changes before application shutdown");

                    await _modManagerService.SaveChangesAsync(cts.Token);
                    await _host.StopAsync(cts.Token);

                    _logger?.LogInformation("Application shutdown started");
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogWarning("Application shutdown was cancelled");
            }
            catch (AggregateException ex) when (ex.InnerException is not null)
            {
                _logger?.LogError(ex.InnerException, "Error during application shutdown");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during application shutdown");
            }
            finally
            {
                _host?.Dispose();
                base.OnExit(e);
            }
        }
    }
}
