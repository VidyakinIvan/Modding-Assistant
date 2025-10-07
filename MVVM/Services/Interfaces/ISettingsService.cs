namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for managing application and user settings
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Service for managing application and user settings
        /// </summary>
        double MainWindowLeft { get; set; }

        /// <summary>
        /// Gets or sets the main window's top position  
        /// </summary>
        double MainWindowTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the main window is displayed in full-screen mode.
        /// </summary>
        bool MainWindowFullScreen { get; set; }

        /// <summary>
        /// Gets or sets the path to the mods folder.
        /// </summary>
        string ModsFolder { get; set; }

        /// <summary>
        /// Saves the current state of the object to a persistent storage medium.
        /// </summary>
        void Save();
    }
}
