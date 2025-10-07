using Microsoft.Win32;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Dialogs;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Classs for displaying various system dialogs and custom application dialogs
    /// </summary>
    public class OpenDialogService(Func<MoveModsDialog> moveModsDialogFactory,
        Func<MoveModsViewModel> moveModsViewModelFactory) 
        : IOpenDialogService
    {
        private readonly Func<MoveModsDialog> _moveModsDialogFactory = moveModsDialogFactory;
        private readonly Func<MoveModsViewModel> _moveModsViewModelFactory = moveModsViewModelFactory;

        /// <inheritdoc/>
        public string? ShowPickFolderDialog(string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            var pickFolderDialog = new OpenFolderDialog
            {
                Title = title
            };
            return pickFolderDialog.ShowDialog() == true ? pickFolderDialog.FolderName : null;
        }

        /// <inheritdoc/>
        public string? ShowSaveFileDialog(string defaultFileName, string filter)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(defaultFileName);
            ArgumentException.ThrowIfNullOrWhiteSpace(filter);

            var saveFileDialog = new SaveFileDialog
            {
                FileName = defaultFileName,
                Filter = filter
            };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        /// <inheritdoc/>
        public string? ShowOpenFileDialog(string title, string filter)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(filter);

            var openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        /// <inheritdoc/>
        public int? ShowMoveModsDialog(Window? owner)
        {
            var moveModsDialog = _moveModsDialogFactory();
            var moveModsViewModel = _moveModsViewModelFactory();

            moveModsDialog.Owner = owner;
            moveModsDialog.DataContext = moveModsViewModel;

            bool? dialogResult = moveModsDialog.ShowDialog();

            return dialogResult == true ? moveModsViewModel.ModNumber : null;
        }
    }
}
