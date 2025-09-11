using Modding_Assistant.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services
{
    public class SettingsService : ISettingsService
    {
        public double MainWindowLeft
        {
            get => Settings.Default.MainWindowLeft;
            set => Settings.Default.MainWindowLeft = value;
        }

        public double MainWindowTop
        {
            get => Settings.Default.MainWindowTop;
            set => Settings.Default.MainWindowTop = value;
        }

        public bool MainWindowFullScreen
        {
            get => Settings.Default.MainWindowFullScreen;
            set => Settings.Default.MainWindowFullScreen = value;
        }

        public void Save() => Settings.Default.Save();
    }
}
