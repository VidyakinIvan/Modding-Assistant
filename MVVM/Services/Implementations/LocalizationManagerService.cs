using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.ComponentModel;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Class for managing application localization, including retrieving the current language, 
    /// switching between available languages, and notifying when the language changes
    /// </summary>
    public class LocalizationManagerService : ILocalizationManagerService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<LocalizationManagerService> _logger;

        /// <inheritdoc/>
        public string CurrentLanguage => 
            _localizationService.CurrentCulture.TwoLetterISOLanguageName;

        public LocalizationManagerService(ILocalizationService localizationService, 
            ILogger<LocalizationManagerService> logger)
        {
            _localizationService = localizationService;
            _logger = logger;
            _localizationService.PropertyChanged += OnLocalizationChanged;
        }

        /// <inheritdoc/>
        public void SwitchToNextLanguage()
        {
            _logger.LogInformation("Switching to next language");

            var cultures = _localizationService.SupportedCultures.ToList();
            var current = _localizationService.CurrentCulture;
            int currentIndex = cultures.IndexOf(current);
            int nextIndex = (currentIndex + 1) % cultures.Count;

            _localizationService.CurrentCulture = cultures[nextIndex];

            _logger.LogInformation("Switched language to {Language}", 
                _localizationService.CurrentCulture.DisplayName);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? LanguageChanged;

        private void OnLocalizationChanged(object? sender, PropertyChangedEventArgs e)
        {
            LanguageChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
        }
    }
}
