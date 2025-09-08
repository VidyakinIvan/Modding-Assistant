using Modding_Assistant.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Modding_Assistant.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private RelayCommand? loadCommand;
        private RelayCommand? minimizeCommand;
        private RelayCommand? moveWindowCommand;
        private RelayCommand? exitCommand;

        public RelayCommand? LoadCommand
        {
            get
            {
                return loadCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        double left = Properties.Settings.Default.MainWindowLeft;
                        double top = Properties.Settings.Default.MainWindowTop;
                        w.Left = !double.IsNaN(left) ? left : (SystemParameters.WorkArea.Width - w.Width) / 2 + SystemParameters.WorkArea.Left;
                        w.Top = !double.IsNaN(top) ? top : (SystemParameters.WorkArea.Height - w.Height) / 2 + SystemParameters.WorkArea.Top;
                    }
                });
            }
        }
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
        public RelayCommand? MoveWindowCommand
        {
            get
            {
                return moveWindowCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        w.DragMove();
                    }
                });
            }
        }
        public RelayCommand? ExitCommand
        {
            get
            {
                return exitCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        Properties.Settings.Default.MainWindowLeft = w.Left;
                        Properties.Settings.Default.MainWindowTop = w.Top;
                        w.Hide();
                    }
                    Properties.Settings.Default.Save();
                    Application.Current.Shutdown();
                });
            }
        }
    }
}
