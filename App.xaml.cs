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

        private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(2);
        private static readonly TimeSpan ShutdownTimeout = TimeSpan.FromSeconds(5);

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
                _logger?.LogCritical(ex, "Application startup failed");

                StartupErrorHandler.HandleStartupException(ex, _host);
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

                _logger.LogInformation("Application initialization started, opening main window");

                var applicationInitializer = _host.Services.GetRequiredService<IApplicationInitializer>();
                await applicationInitializer.InitializeAsync(cancellationToken);

            }
            catch (OperationCanceledException)
            {
                _logger?.LogWarning("Application initialization was cancelled");
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
                if (_host != null)
                {
                    using var cts = new CancellationTokenSource(ShutdownTimeout);
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
                _host?.Dispose();
                base.OnExit(e);
            }
        }
    }
}
