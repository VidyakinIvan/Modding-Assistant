using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ModContext _context;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(ModContext context, ILogger<DatabaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting database migration...");
                await _context.Database.MigrateAsync(cancellationToken);
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                if (!canConnect)
                    throw new InvalidOperationException("Database is not accessible after migration");
                _logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database migration failed.");
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
