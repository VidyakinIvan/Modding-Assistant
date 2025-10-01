using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Implementations;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Dialogs;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;

namespace Modding_Assistant.Core
{
    public static class DIConfigurator
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IExcelExportService, ExcelExportService>();
            services.AddSingleton<IOpenDialogService, OpenDialogService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IMainWindowService, MainWindowService>();
            services.AddSingleton<ILocalizationService>(provider =>
                new LocalizationService(Resources.Strings.Strings.ResourceManager));
            return services;
        }
        
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }
            services.AddDbContext<ModContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<IDatabaseService, DatabaseService>();
            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
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
