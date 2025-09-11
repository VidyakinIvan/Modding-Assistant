using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services
{
    public interface ISettingsService
    {
        double MainWindowLeft { get; set; }
        double MainWindowTop { get; set; }
        bool MainWindowFullScreen { get; set; }
        void Save();
    }
}
