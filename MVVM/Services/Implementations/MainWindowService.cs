using Modding_Assistant.MVVM.Services.Interfaces;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    internal class MainWindowService(Window window) : IMainWindowService
    {
        private readonly Window _window = window;
        public void Minimize() => _window.WindowState = WindowState.Minimized;
        public void Maximize() => _window.WindowState = WindowState.Maximized;
        public void Restore() => _window.WindowState = WindowState.Normal;
        public void DragMove() => _window.DragMove();
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
        public double Width => _window.Width;
        public double Height => _window.Height;
        public WindowState WindowState
        {
            get => _window.WindowState;
            set => _window.WindowState = value;
        }
    }
}
