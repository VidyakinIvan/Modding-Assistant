using Microsoft.Extensions.DependencyInjection;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using Modding_Assistant.MVVM.View.Dialogs;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class MoveModsDialogService(IServiceProvider serviceProvider) : IMoveModsDialogService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public int? ShowNumberDialog()
        {
            var moveDialog = _serviceProvider.GetRequiredService<MoveModsDialog>();
            var viewModel = _serviceProvider.GetRequiredService<MoveModsViewModel>();
            moveDialog.Owner = _serviceProvider.GetRequiredService<MainWindow>();
            moveDialog.DataContext = viewModel;
            if (moveDialog.ShowDialog() == true)
                return moveDialog.ModNumber;
            else
                return null;
        }
    }
}
