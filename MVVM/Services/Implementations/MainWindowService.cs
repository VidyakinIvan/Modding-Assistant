using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    internal class MainWindowService(Window window) : IMainWindowService
    {
        private readonly Window _window = window ?? throw new ArgumentNullException(nameof(window));
        public void Minimize() => _window.WindowState = WindowState.Minimized;
        public void Maximize() => _window.WindowState = WindowState.Maximized;
        public void Restore() => _window.WindowState = WindowState.Normal;
        public void DragMove()
        {
            try
            {
                if (_window.WindowState != WindowState.Maximized &&
                    _window.ResizeMode != ResizeMode.NoResize)
                {
                    _window.DragMove();
                }
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"DragMove failed: {ex.Message}");
            }
        }
        public void Close() => _window.Close();
        public void Hide() => _window.Hide();
        public double Left
        {
            get => _window.Left;
            set => _window.Left = value;
        }

        public double Top
        {
            get => _window.Top;
            set => _window.Top = value;
        }
        public double Width
        {
            get => _window.Width;
            set
            {
                _window.Width = value >= _window.MinWidth ? value : _window.MinWidth;
            }
        }
        public double Height
        {
            get => _window.Height;
            set
            {
                _window.Height = value >= _window.MinHeight ? value : _window.MinHeight;
            }
        }
        public WindowState WindowState
        {
            get => _window.WindowState;
            set => _window.WindowState = value;
        }
    }
}
