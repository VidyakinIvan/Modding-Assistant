using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Implementations;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Dialogs;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using System.IO;

namespace Modding_Assistant.Core
{
    public static class HostBuilderFactory
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddServices()
                    .AddDatabase(context.Configuration)
                    .AddViewModels()
                    .AddViews();
            });

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationInitializer, ApplicationInitializer>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IExcelExportService, ExcelExportService>();
            services.AddSingleton<IOpenDialogService, OpenDialogService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IMainWindowService, MainWindowService>();
            services.AddSingleton<ILocalizationService>(provider =>
                new LocalizationService(Resources.Strings.Strings.ResourceManager));
            return services;
        }
        
        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "ModdingAssistant");

            try
            {
                Directory.CreateDirectory(appFolder);
            }
            catch
            {
                appFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                Directory.CreateDirectory(appFolder);
            }

            var connectionString = configuration.GetConnectionString("DefaultConnectionPath");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }

            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(appFolder, connectionString)
            };

            services.AddDbContext<ModContext>(options =>
                options.UseSqlite(connectionStringBuilder.ConnectionString));

            services.AddScoped<IDatabaseService, DatabaseService>();
            return services;
        }

        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MoveModsViewModel>();
            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<MoveModsDialog>();
            return services;
        }
    }
}
