using System.Globalization;
using System.ComponentModel;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface ILocalizationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the current culture for localization
        /// </summary>
        CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Gets supported cultures
        /// </summary>
        IEnumerable<CultureInfo> SupportedCultures { get; }

        /// <summary>
        /// Gets localized string using indexer syntax by key
        /// </summary>
        string this[string key] { get; }
    }
}
