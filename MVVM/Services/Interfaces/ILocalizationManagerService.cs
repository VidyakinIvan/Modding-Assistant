using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface ILocalizationManagerService
    {
        string CurrentLanguage { get; }
        void SwitchToNextLanguage();
        event PropertyChangedEventHandler? LanguageChanged;
    }
}
