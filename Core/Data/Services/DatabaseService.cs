using Microsoft.Extensions.Logging;
using Modding_Assistant.Core.Data.Interfaces;
using Modding_Assistant.Core.Data.Models;
namespace Modding_Assistant.Core.Data.Services
{
    public class DatabaseService(ModContext context, ILogger<DatabaseService> logger) : IDatabaseService
    {
        private readonly ModContext _context = context;
        private readonly ILogger<DatabaseService> _logger = logger;

        /// <inheritdoc/>
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting database initialization...");

                await _context.Database.EnsureCreatedAsync(cancellationToken);

                _logger.LogInformation("Database initializing completed successfully.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Database initialization was cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database initializing failed.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken)
        {
            try
            {
                var isHealthy = await _context.Database.CanConnectAsync(cancellationToken);

                _logger.LogDebug("Database health check: {HealthStatus}",
                    isHealthy ? "Healthy" : "Unhealthy");

                return isHealthy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed.");
                return false;
            }
        }
    }
}
