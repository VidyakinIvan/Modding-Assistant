using System.Windows;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface IMainWindowService
    {
        void Minimize();
        void Maximize();
        void Restore();
        void DragMove();
        void Close();
        void Hide();
        double Left { get; set; }
        double Top { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        WindowState WindowState { get; set; }
    }
}
