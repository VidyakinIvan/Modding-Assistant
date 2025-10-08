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

        public string CurrentLanguage => 
            _localizationService.CurrentCulture.TwoLetterISOLanguageName;

        public event PropertyChangedEventHandler? LanguageChanged;

        public LocalizationManagerService(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _localizationService.PropertyChanged += OnLocalizationChanged;
        }

        public void SwitchToNextLanguage()
        {
            var cultures = _localizationService.SupportedCultures.ToList();
            var current = _localizationService.CurrentCulture;
            int currentIndex = cultures.IndexOf(current);
            int nextIndex = (currentIndex + 1) % cultures.Count;

            _localizationService.CurrentCulture = cultures[nextIndex];
        }

        private void OnLocalizationChanged(object? sender, PropertyChangedEventArgs e)
        {
            LanguageChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
        }
    }
}
