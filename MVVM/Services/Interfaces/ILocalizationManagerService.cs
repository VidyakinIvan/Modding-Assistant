using System.ComponentModel;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for managing application localization, including retrieving the current language, 
    /// switching between available languages, and notifying when the language changes
    /// </summary>
    public interface ILocalizationManagerService
    {
        /// <summary>
        /// Gets the current language setting of the application.
        /// </summary>
        string CurrentLanguage { get; }

        /// <summary>
        /// Switches the application to the next available language in the predefined language sequence
        /// </summary>
        /// <remarks>
        /// This method cycles through the supported languages in a sequential order. When the
        /// last language in the sequence is reached, it wraps around to the first language
        /// </remarks>
        void SwitchToNextLanguage();

        /// <summary>
        /// Occurs when the application's language setting changes
        /// </summary>
        event PropertyChangedEventHandler? LanguageChanged;
    }
}
