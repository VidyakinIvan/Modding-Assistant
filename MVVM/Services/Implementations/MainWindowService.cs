using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Class for managing the main application window
    /// </summary>
    public class MainWindowService(ISettingsService settingsService,ILogger<MainWindowService> logger) : IMainWindowService
    {
        private Window? _window;
        private readonly ISettingsService _settingsService = settingsService;
        private readonly ILogger<MainWindowService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private bool _isInitialized = false;

        public event EventHandler<WindowState>? WindowStateChanged;

        /// <inheritdoc/>
        public Window? GetMainWindow() => _window;

        /// <inheritdoc/>
        public void SetMainWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            if (_window is not null)
            {
                _logger.LogWarning("Main window is already set. Ignoring the new assignment.");
                return;
            }

            _window = window;

            _window.StateChanged += OnWindowStateChanged;
            _window.Activated += OnWindowActivated;

            _logger.LogInformation("Main window set: {WindowType}", window.GetType().Name);
        }

        /// <inheritdoc/>
        public void InitializeWindow()
        {
            if (_window == null || _isInitialized) 
                return;

            _logger.LogInformation("Loading main window dimensions...");

            _window.Left = !double.IsNaN(_settingsService.MainWindowLeft)
                ? _settingsService.MainWindowLeft
                : (SystemParameters.WorkArea.Width - _window.Width) / 4;

            _window.Top = !double.IsNaN(_settingsService.MainWindowTop)
                ? _settingsService.MainWindowTop
                : (SystemParameters.WorkArea.Height - _window.Height) / 2;

            if (_settingsService.MainWindowFullScreen)
            {
                WindowState = WindowState.Maximized;
            }

            _logger.LogInformation("Main window dimensions loaded");

            _isInitialized = true;
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            if (_window is not null)
                WindowState = WindowState.Minimized;
        }

        /// <inheritdoc/>
        public void Maximize()
        {
            if (_window is not null)
                WindowState = WindowState.Maximized;
        }

        /// <inheritdoc/>
        public void Restore()
        {
            if (_window is not null)
                WindowState = WindowState.Normal;
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
        public void SaveWindowSettings()
        {
            if (_window == null) 
                return;

            _logger.LogInformation("Saving main window dimensions...");

            _settingsService.MainWindowLeft = _window.Left;
            _settingsService.MainWindowTop = _window.Top;
            _settingsService.MainWindowFullScreen = _window.WindowState == WindowState.Maximized;
            _settingsService.Save();

            _logger.LogInformation("Main window dimensions saved");
        }

        /// <inheritdoc/>
        public void Close()
        {
            _window?.Close();
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
                {
                    _window.WindowState = value;
                    WindowStateChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Handles the event triggered when the window's state changes.
        /// </summary>
        private void OnWindowStateChanged(object? sender, EventArgs e)
        {
            if (_window is null) return;

            var newState = _window.WindowState;

            WindowStateChanged?.Invoke(this, newState);
        }

        /// <summary>
        /// Handles the activation event of the window and raises the <see cref="WindowStateChanged"/> event.
        /// </summary>
        private void OnWindowActivated(object? sender, EventArgs e)
        {
            if (_window is null) return;

            WindowStateChanged?.Invoke(this, _window.WindowState);
        }
    }
}
