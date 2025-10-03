using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.Core;
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

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown;

                using var startupCts = new CancellationTokenSource(TimeSpan.FromMinutes(2));

                await InitializeAsync(e, startupCts.Token);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Application startup failed");

                StartupErrorHandler.HandleStartupException(ex, _host);
                Shutdown(1);
            }
        }

        /// <summary>
        /// Async Task for application initialization
        /// </summary>
        private async Task InitializeAsync(StartupEventArgs e, CancellationToken cancellationToken)
        {
            try
            {
                _host = HostBuilderFactory.CreateHostBuilder(e.Args).Build();
                _logger = _host.Services.GetRequiredService<ILogger<App>>();

                _logger.LogInformation("Application initialization started, opening main window");

                var applicationInitializer = _host.Services.GetRequiredService<IApplicationInitializer>();
                await applicationInitializer.InitializeAsync(_host, cancellationToken);

                ShutdownMode = ShutdownMode.OnLastWindowClose;

            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger?.LogError(ex, "Error during application initialization");
                throw;
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                if (_host != null)
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await _host.StopAsync(cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogWarning("Application shutdown was cancelled");
            }
            catch (AggregateException ex) when (ex.InnerException != null)
            {
                _logger?.LogError(ex.InnerException, "Error during application shutdown");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during application shutdown");
            }
            finally
            {
                base.OnExit(e);
                _host?.Dispose();
            }
        }
    }
}
