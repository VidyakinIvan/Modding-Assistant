using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface INotificationService
    {
        void ShowError(string message, string caption);
        void ShowWarning(string message, string caption);
        void ShowInformation(string message, string caption);
        bool ShowConfirmation(string message, string caption);
    }
}
