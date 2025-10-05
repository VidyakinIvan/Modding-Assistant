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

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        public string this[string key] => GetString(key);

        /// <inheritdoc/>
        public IEnumerable<CultureInfo> SupportedCultures { get; } = supportedCultures ??
        [
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU")
        ];

        /// <inheritdoc/>
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (!SupportedCultures.Contains(value))
                {
                    _logger?.LogWarning("Unsupported culture: {Culture}", value.Name);
                    return;
                }

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

        /// <summary>
        /// Gets localized string for the specified key
        /// </summary>
        /// <returns>Localized string or fallback value</returns>
        private string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger?.LogWarning("Attempt to get localization string with empty key");

                return "[empty_key]";
            }

            var cacheKey = (key, _currentCulture);

            if (_cache.TryGetValue(cacheKey, out var cachedValue))
                return cachedValue;

            string? result = _resourceManager.GetString(key, _currentCulture);
            if (result == null)
            {
                _logger?.LogWarning("Localization key '{Key}' not found for culture '{Culture}'", key, _currentCulture.Name);
                return $"[{key}]";
            }

            _cache.TryAdd(cacheKey, result);
            return result;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
