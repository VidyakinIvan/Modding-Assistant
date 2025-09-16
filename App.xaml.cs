using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Implementations;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.View.Dialogs;
using Modding_Assistant.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Modding_Assistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<MoveModsDialog>();
                    services.AddTransient<MoveModsViewModel>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<IMoveModsDialogService, MoveModsDialogService>();

                    services.AddSingleton<IMainWindowService>(provider =>
                    {
                        var mainWindow = provider.GetRequiredService<MainWindow>();
                        return new MainWindowService(mainWindow);
                    });
                    services.AddDbContext<ModContext>(options => 
                        options.UseSqlite("Data Source=modding_assistant.db"));
                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
            base.OnExit(e);
        }
    }

}
