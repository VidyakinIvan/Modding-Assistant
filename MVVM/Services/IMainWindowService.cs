using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.Services
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
        double Width { get; }
        double Height { get; }
        WindowState WindowState { get; set; }
    }
}
