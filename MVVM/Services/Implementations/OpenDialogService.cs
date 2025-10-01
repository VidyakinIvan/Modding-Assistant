using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Dialogs;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class OpenDialogService(IServiceProvider serviceProvider) : IOpenDialogService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task<string?> ShowPickFolderDialogAsync(string title)
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var pickFolderDialog = new OpenFolderDialog
                {
                    Title = title
                };
                return pickFolderDialog.ShowDialog() == true ? pickFolderDialog.FolderName : null;
            });
        }
        public async Task<string?> ShowSaveFileDialogAsync(string defaultFileName, string filter)
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = defaultFileName,
                    Filter = filter
                };
                return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
            });
        }

        public async Task<string?> ShowOpenFileDialogAsync(string title, string filter)
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = title,
                    Filter = filter
                };
                return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
            });
        }

        public int? ShowMoveModsDialog()
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
        public async Task ShowMessageAsync(string title, string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public async Task ShowErrorAsync(string title, string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
