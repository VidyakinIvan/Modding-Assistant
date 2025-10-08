using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class LocalizationManagerService : ILocalizationManagerService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<LocalizationManagerService> _logger;

        public string CurrentLanguage => 
            _localizationService.CurrentCulture.TwoLetterISOLanguageName;

        public event PropertyChangedEventHandler? LanguageChanged;

        public LocalizationManagerService(ILocalizationService localizationService, 
            ILogger<LocalizationManagerService> logger)
        {
            _localizationService = localizationService;
            _logger = logger;
            _localizationService.PropertyChanged += OnLocalizationChanged;
        }

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

        private void OnLocalizationChanged(object? sender, PropertyChangedEventArgs e)
        {
            LanguageChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
        }
    }
}
