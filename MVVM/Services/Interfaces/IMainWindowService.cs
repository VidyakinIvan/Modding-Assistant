using System.Windows;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for managing the main application window
    /// </summary>
    public interface IMainWindowService
    {
        event EventHandler<WindowState>? WindowStateChanged;

        /// <summary>
        /// Gets the main application window instance (if exists)
        /// </summary>
        /// <returns>
        /// Returns <see cref="Window"/> if main window exists, null-reference otherwise
        /// </returns>
        Window? GetMainWindow();

        /// <summary>
        /// Sets the main application window instance
        /// </summary>
        void SetMainWindow(Window window);

        /// <summary>
        /// Initializes the application window and prepares it for display.
        /// </summary>
        /// <remarks>
        /// This method sets up the necessary configurations required for the window to function
        /// properly.  It should be called before attempting to display or interact with the window.
        /// </remarks>
        void InitializeWindow();

        /// <summary>
        /// Sets window state to minimized
        /// </summary>
        void Minimize();

        /// <summary>
        /// Maximizes the window to fill the screen
        /// </summary>
        void Maximize();

        /// <summary>
        /// Restores the window to normal state from minimized or maximized
        /// </summary>
        void Restore();

        /// <summary>
        /// Enables window dragging when called during mouse events
        /// </summary>
        void DragMove();

        /// <summary>
        /// Saves the current window settings, such as size, position, and state, to persistent storage.
        /// </summary>
        void SaveWindowSettings();

        /// <summary>
        /// Closes the application window
        /// </summary>
        void Close();

        /// <summary>
        /// Hides the window from view without closing
        /// </summary>
        void Hide();

        /// <summary>
        /// Gets or sets the window's left position relative to the screen
        /// </summary>
        double Left { get; set; }

        /// <summary>
        /// Gets or sets the window's top position relative to the screen
        /// </summary>
        double Top { get; set; }

        /// <summary>
        /// Gets or sets the window's width
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the window's height
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the window's current state (Normal, Minimized, Maximized)
        /// </summary>
        WindowState WindowState { get; set; }
    }
}
