using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface ILocalizationService
    {
        CultureInfo CurrentCulture { get; set; }
        IEnumerable<CultureInfo> SupportedCultures { get; }
        string GetString(string key);
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
