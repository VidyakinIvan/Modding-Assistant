using Modding_Assistant.Core;

namespace Modding_Assistant.MVVM.ViewModel
{
    public class MoveModsViewModel : ObservableObject
    {
        private int? modNumber;
        public int? ModNumber
        {
            get => modNumber;
            set
            {
                modNumber = value;
                OnPropertyChanged();
                OKCommand.RaiseCanExecuteChanged();
            }
        }
        public event Action? RequestOk;
        public event Action? RequestClose;

        public RelayCommand OKCommand { get; }
        public RelayCommand CloseCommand { get; }

        public MoveModsViewModel()
        {
            OKCommand = new RelayCommand(
                _ => RequestOk?.Invoke(),
                _ => ModNumber.HasValue && ModNumber > 0
            );
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }
    }
}
