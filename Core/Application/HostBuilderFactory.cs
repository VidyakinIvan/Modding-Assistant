using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modding_Assistant.Core.Data.Interfaces;
using Modding_Assistant.Core.Data.Models;
using Modding_Assistant.Core.Data.Services;
using Modding_Assistant.MVVM.Services.Implementations;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Dialogs;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using System.IO;

namespace Modding_Assistant.Core.Application
{
    /// <summary>
    /// Static class for DI host building
    /// </summary>
    public static class HostBuilderFactory
    {
        private const string AppSettingsFile = "appsettings.json";
        private const string DbPathKey = "DefaultDbPath";
        private const string AppFolderName = "ModdingAssistant";
        private const string FallbackDataFolder = "Data";

        /// <summary>
        /// Factory method for <see cref="IHostBuilder"/>
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(AppSettingsFile, optional: false);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddServices()
                    .AddDatabase(context.Configuration)
                    .AddViewModels()
                    .AddViews();
            });
        
        /// <summary>
        /// Initializes MVVM layer services
        /// </summary>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationInitializer, ApplicationInitializer>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IExcelExportService, ExcelExportService>();
            services.AddTransient<IOpenDialogService, OpenDialogService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IMainWindowService, MainWindowService>();
            services.AddSingleton<ILocalizationService>(provider =>
                new LocalizationService(Resources.Strings.Strings.ResourceManager));
            return services;
        }

        /// <summary>
        /// Initializes database and it's service
        /// </summary>
        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, AppFolderName);

            try
            {
                Directory.CreateDirectory(appFolder);
            }
            catch
            {
                appFolder = Path.Combine(Directory.GetCurrentDirectory(), FallbackDataFolder);
                Directory.CreateDirectory(appFolder);
            }

            var connectionString = configuration.GetConnectionString(DbPathKey);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{DbPathKey}' is not configured.");
            }

            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(appFolder, connectionString)
            };

            services.AddDbContext<ModContext>(options =>
                options.UseSqlite(connectionStringBuilder.ConnectionString));

            services.AddSingleton<IDatabaseService, DatabaseService>();
            return services;
        }

        /// <summary>
        /// Initialize ViewModels
        /// </summary>
        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MoveModsViewModel>();
            services.AddTransient<Func<MoveModsViewModel>>(sp => () => sp.GetRequiredService<MoveModsViewModel>());
            return services;
        }

        /// <summary>
        /// Initializes windows and dialogs
        /// </summary>
        private static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<MoveModsDialog>();
            services.AddTransient<Func<MoveModsDialog>>(sp => () => sp.GetRequiredService<MoveModsDialog>());
            return services;
        }
    }
}
