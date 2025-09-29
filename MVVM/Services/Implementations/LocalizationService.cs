using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class LocalizationService(ResourceManager resourceManager, 
        IEnumerable<CultureInfo>? supportedCultures = null, 
        ILogger<LocalizationService>? logger = null) : ILocalizationService
    {
        private const string IndexerPropertyName = "Item[]";

        private readonly ResourceManager _resourceManager = resourceManager 
            ?? throw new ArgumentNullException(nameof(resourceManager));
        private readonly ConcurrentDictionary<(string, CultureInfo), string> _cache = new();
        private readonly ILogger<LocalizationService>? _logger = logger;
        private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

        public string this[string key] => GetString(key);

        public event PropertyChangedEventHandler? PropertyChanged;

        public IEnumerable<CultureInfo> SupportedCultures { get; } = supportedCultures ??
        [
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU")
        ];
        
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _logger?.LogInformation("Changing culture from {OldCulture} to {NewCulture}",
                        _currentCulture.Name, value.Name);
                    _currentCulture = value;
                    _cache.Clear();
                    OnPropertyChanged(IndexerPropertyName);
                }
            }
        }

        public string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger?.LogWarning("Attempt to get localization string with empty key");
                return "[empty_key]";
            }
            string? result = _resourceManager.GetString(key, _currentCulture);
            if (result == null)
            {
                _logger?.LogWarning("Localization key '{Key}' not found for culture '{Culture}'", key, _currentCulture.Name);
                return $"[{key}]";
            }
            return result;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
