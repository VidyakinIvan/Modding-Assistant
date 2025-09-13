using Microsoft.Extensions.DependencyInjection;
using Modding_Assistant.MVVM.Services.Interfaces;
using Modding_Assistant.MVVM.View.Windows;
using Modding_Assistant.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class MoveDialogService : IMoveModDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public MoveDialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public int? ShowNumberDialog()
        {
            var moveDialog = _serviceProvider.GetRequiredService<MoveModView>();
            var viewModel = _serviceProvider.GetRequiredService<MoveModViewModel>();
            moveDialog.Owner = _serviceProvider.GetRequiredService<MainWindow>();
            moveDialog.DataContext = viewModel;
            if (moveDialog.ShowDialog() == true)
                return moveDialog.ModNumber;
            else
                return null;
        }
    }
}
