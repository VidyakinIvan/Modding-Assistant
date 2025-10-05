using System.Windows;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for managing the main application window
    /// </summary>
    public interface IMainWindowService
    {
        /// <summary>
        /// Sets the main application window instance
        /// </summary>
        void SetMainWindow(Window window);

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
