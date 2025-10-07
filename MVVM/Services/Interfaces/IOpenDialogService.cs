using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service interface for displaying various system dialogs and custom application dialogs
    /// </summary>
    /// <remarks>
    /// This service provides a unified interface for showing file system dialogs (open, save, folder picker)
    /// and custom application dialogs. All methods are synchronous as they represent modal UI operations
    /// that block the calling thread until user interaction is complete.
    /// </remarks>
    public interface IOpenDialogService
    {
        /// <summary>
        /// Shows a folder picker dialog to select a directory
        /// </summary>
        /// <returns>
        /// The full path of the selected folder, or null if the dialog was cancelled
        /// </returns>
        string? ShowPickFolderDialog(string title);

        /// <summary>
        /// Shows a save file dialog to specify a file path for saving
        /// </summary>
        /// <returns>
        /// The full path of the selected file, or null if the dialog was cancelled
        /// </returns>
        string? ShowSaveFileDialog(string defaultFileName, string filter);

        /// <summary>
        /// Shows an open file dialog to select an existing file
        /// </summary>
        /// <returns>
        /// The full path of the selected file, or null if the dialog was cancelled
        /// </returns>
        string? ShowOpenFileDialog(string title, string filter);

        /// <summary>
        /// Shows the custom Move Mods dialog for selecting mod positions
        /// </summary>
        /// <returns>
        /// The selected mod number if confirmed, or null if the dialog was cancelled
        /// </returns>
        /// <remarks>
        /// This dialog is specific to the application's mod management functionality
        /// and allows users to specify mod movement operations.
        /// </remarks>
        int? ShowMoveModsDialog(Window? owner);
    }
}
