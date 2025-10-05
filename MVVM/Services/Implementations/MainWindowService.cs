using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Class for managing the main application window
    /// </summary>
    public class MainWindowService(ILogger<MainWindowService> logger) : IMainWindowService
    {
        private Window? _window;
        private readonly ILogger<MainWindowService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public void SetMainWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            if (_window is not null)
                throw new InvalidOperationException("Main window is already set");

            _window = window;
            _logger.LogInformation("Main window set: {WindowType}", window.GetType().Name);
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            if (_window is not null)
                _window.WindowState = WindowState.Minimized;
        }

        /// <inheritdoc/>
        public void Maximize()
        {
            if (_window is not null)
                _window.WindowState = WindowState.Maximized;
        }

        /// <inheritdoc/>
        public void Restore()
        {
            if (_window is not null)
                _window.WindowState = WindowState.Normal;
        }

        /// <inheritdoc/>
        public void DragMove()
        {
            try
            {
                if (_window is not null &&
                    _window.WindowState != WindowState.Maximized &&
                    _window.ResizeMode != ResizeMode.NoResize)
                {
                    _window.DragMove();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to drag move window");
            }
        }

        /// <inheritdoc/>
        public void Close()
        {
            _window?.Close();
        }

        /// <inheritdoc/>
        public void Hide()
        {
            _window?.Hide();
        }

        /// <inheritdoc/>
        public double Left
        {
            get => _window?.Left ?? 0;
            set
            {
                if (_window is not null)
                    _window.Left = value;
            }
        }

        /// <inheritdoc/>
        public double Top
        {
            get => _window?.Top ?? 0;
            set
            {
                if (_window is not null)
                    _window.Top = value;
            }
        }

        /// <inheritdoc/>
        public double Width
        {
            get => _window?.Width ?? 0;
            set
            {
                if (_window is not null)
                    _window.Width = value >= _window.MinWidth ? value : _window.MinWidth;
            }
        }

        /// <inheritdoc/>
        public double Height
        {
            get => _window?.Height ?? 0;
            set
            {
                if (_window is not null)
                    _window.Height = value >= _window.MinHeight ? value : _window.MinHeight;
            }
        }

        /// <inheritdoc/>
        public WindowState WindowState
        {
            get => _window?.WindowState ?? WindowState.Normal;
            set
            {
                if (_window is not null)
                    _window.WindowState = value;
            }
        }
    }
}
