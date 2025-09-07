using Modding_Assistant.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private RelayCommand? minimizeCommand;
        private RelayCommand? exitCommand;

        public RelayCommand? MinimizeCommand
        {
            get
            {
                return minimizeCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                });
            }
        }

        public RelayCommand? ExitCommand
        {
            get
            {
                return exitCommand ??= new RelayCommand(obj =>
                {
                    Application.Current.Shutdown();
                });
            }
        }
    }
}
