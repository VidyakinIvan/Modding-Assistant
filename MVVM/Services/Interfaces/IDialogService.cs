using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface IDialogService
    {
        Task<string?> ShowSaveFileDialogAsync(string defaultFileName, string filter);
        Task ShowMessageAsync(string title, string message);
        Task ShowErrorAsync(string title, string message);
    }
}
