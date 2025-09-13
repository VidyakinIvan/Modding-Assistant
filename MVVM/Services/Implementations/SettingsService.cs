using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.Properties;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class SettingsService : ISettingsService
    {
        public double MainWindowLeft
        {
            get => Settings.Default.MainWindowLeft;
            set => Settings.Default.MainWindowLeft = value;
        }

        public double MainWindowTop
        {
            get => Settings.Default.MainWindowTop;
            set => Settings.Default.MainWindowTop = value;
        }

        public bool MainWindowFullScreen
        {
            get => Settings.Default.MainWindowFullScreen;
            set => Settings.Default.MainWindowFullScreen = value;
        }

        public void Save() => Settings.Default.Save();
    }
}
