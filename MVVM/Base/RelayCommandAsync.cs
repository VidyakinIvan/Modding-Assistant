using System.Windows.Input;

namespace Modding_Assistant.MVVM.Base
{
    public class RelayCommandAsync(Func<object?, Task> execute, Func<object?, bool>? canExecute = null) : ICommand
    {
        private readonly Func<object?, Task> _execute = execute;
        private readonly Func<object?, bool>? _canExecute = canExecute;

        public RelayCommandAsync(Func<Task> execute)
            : this(_ => execute(), null) { }

        public RelayCommandAsync(Func<Task> execute, Func<bool> canExecute)
            : this(_ => execute(), _ => canExecute()) { }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object? parameter)
        {
            await _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
