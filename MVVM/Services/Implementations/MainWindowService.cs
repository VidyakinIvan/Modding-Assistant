using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    internal class MainWindowService(ILogger<MainWindowService> logger) : IMainWindowService
    {
        private Window? _window;
        private readonly ILogger<MainWindowService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public void SetMainWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(window);
            _window = window;
        }
        public void Minimize()
        {
            if (_window is not null)
                _window.WindowState = WindowState.Minimized;
        }
        public void Maximize()
        {
            if (_window is not null)
                _window.WindowState = WindowState.Maximized;
        }
        public void Restore()
        {
            if (_window is not null)
                _window.WindowState = WindowState.Normal;
        }
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
        public void Close()
        {
            if (_window is not null)
                _window.Close();
        }
        public void Hide()
        {
            if (_window is not null)
                _window.Hide();
        }
        public double Left
        {
            get => _window?.Left ?? 0;
            set
            {
                if (_window is not null)
                    _window.Left = value;
            }
        }

        public double Top
        {
            get => _window?.Top ?? 0;
            set
            {
                if (_window is not null)
                    _window.Top = value;
            }
        }
        public double Width
        {
            get => _window?.Width ?? 0;
            set
            {
                if (_window is not null)
                    _window.Width = value >= _window.MinWidth ? value : _window.MinWidth;
            }
        }
        public double Height
        {
            get => _window?.Height ?? 0;
            set
            {
                if (_window is not null)
                    _window.Height = value >= _window.MinHeight ? value : _window.MinHeight;
            }
        }
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
