using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Modding_Assistant.MVVM.Services.Interfaces;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class DialogService : IDialogService
    {
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
