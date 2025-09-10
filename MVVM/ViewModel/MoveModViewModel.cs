using Modding_Assistant.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.ViewModel
{
    internal class MoveModViewModel : ObservableObject
    {
        private int? modNumber;
        public int? ModNumber
        {
            get => modNumber;
            set { modNumber = value; OnPropertyChanged(); }
        }

        private RelayCommand? okCommand;
        private RelayCommand? closeCommand;
        private RelayCommand? moveWindowCommand;
        public RelayCommand MoveWindowCommand
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
        public RelayCommand? OKCommand
        {
            get
            {
                return okCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        w.DialogResult = true;
                        w.Hide();
                    }
                });
            }
        }
        public RelayCommand? CloseCommand
        {
            get
            {
                return closeCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        w.Hide();
                    }
                });
            }
        }
    }
}
