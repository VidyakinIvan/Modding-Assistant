using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class LocalizationService : INotifyPropertyChanged, ILocalizationService
    {
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCulture;

        public event PropertyChangedEventHandler? PropertyChanged;
        public IEnumerable<CultureInfo> SupportedCultures { get; }
        public LocalizationService(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _currentCulture = CultureInfo.CurrentUICulture;
            SupportedCultures = [ new CultureInfo("en-US"), new CultureInfo("ru-RU") ];
        }
        
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    OnPropertyChanged(nameof(CurrentCulture));
                }
            }
        }
        public string GetString(string key)
        {
            return _resourceManager.GetString(key, _currentCulture) ?? $"[{key}]";
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
