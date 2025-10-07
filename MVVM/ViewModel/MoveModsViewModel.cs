using Modding_Assistant.MVVM.Base;

namespace Modding_Assistant.MVVM.ViewModel
{
    public class MoveModsViewModel : ObservableObject
    {
        private int? _modNumber;
        private bool? _dialogResult;
        public int? ModNumber
        {
            get => _modNumber;
            set
            {
                SetProperty(ref _modNumber, value);
                OKCommand.RaiseCanExecuteChanged();
            }
        }

        public bool? DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        public RelayCommand OKCommand { get; }
        public RelayCommand CloseCommand { get; }

        public MoveModsViewModel()
        {
            OKCommand = new RelayCommand(
                _ => DialogResult = true,
                _ => ModNumber.HasValue && ModNumber > 0
            );

            CloseCommand = new RelayCommand(_ => DialogResult = false);
        }
    }
}
