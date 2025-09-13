using Modding_Assistant.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.ViewModel
{
    public class MoveModViewModel : ObservableObject
    {
        private int? modNumber;
        public int? ModNumber
        {
            get => modNumber;
            set { modNumber = value; OnPropertyChanged(); }
        }
        public event Action? RequestOk;
        public event Action? RequestClose;

        public RelayCommand OKCommand { get; }
        public RelayCommand CloseCommand { get; }

        public MoveModViewModel()
        {
            OKCommand = new RelayCommand(
                _ => RequestOk?.Invoke(),
                _ => ModNumber.HasValue && ModNumber > 0
            );
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }
    }
}
