using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.Properties;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Class for managing application and user settings
    /// </summary>
    public class SettingsService : ISettingsService
    {
        /// <inheritdoc/>
        public double MainWindowLeft
        {
            get => Settings.Default.MainWindowLeft;
            set => Settings.Default.MainWindowLeft = value;
        }

        /// <inheritdoc/>
        public double MainWindowTop
        {
            get => Settings.Default.MainWindowTop;
            set => Settings.Default.MainWindowTop = value;
        }

        /// <inheritdoc/>
        public bool MainWindowFullScreen
        {
            get => Settings.Default.MainWindowFullScreen;
            set => Settings.Default.MainWindowFullScreen = value;
        }

        /// <inheritdoc/>
        public string ModsFolder
        {
            get => Settings.Default.ModsFolder;
            set
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(value);
                Settings.Default.ModsFolder = value;
            }
        }

        /// <inheritdoc/>
        public void Save() => Settings.Default.Save();
    }
}
