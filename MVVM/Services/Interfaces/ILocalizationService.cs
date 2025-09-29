using System.Globalization;
using System.ComponentModel;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface ILocalizationService : INotifyPropertyChanged
    {
        CultureInfo CurrentCulture { get; set; }
        IEnumerable<CultureInfo> SupportedCultures { get; }
        string GetString(string key);
        string this[string key] { get; }
    }
}
