using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class DatabaseService(ModContext context, ILogger<DatabaseService> logger) : IDatabaseService
    {
        private readonly ModContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly ILogger<DatabaseService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting database initializing...");
                using var creationCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                creationCts.CancelAfter(TimeSpan.FromMinutes(5));
                await _context.Database.EnsureCreatedAsync(creationCts.Token);
                Debug.WriteLine(_context.Database.CanConnect());
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                if (!canConnect)
                    throw new InvalidOperationException("Database is not accessible after initializing");
                _logger.LogInformation("Database initializing completed successfully.");
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Database initializing timed out");
                throw new TimeoutException("Database initializing operation timed out");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database initializing failed.");
                throw;
            }
        }

        public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Database.CanConnectAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed.");
                return false;
            }
        }

        public async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogWarning("Clearing the database...");
                await _context.Database.EnsureDeletedAsync(cancellationToken);
                await _context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Database cleared and re-initialized.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clear the database.");
                throw;
            }
        }
    }
}
