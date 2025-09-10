using Modding_Assistant.MVVM.View;
using Modding_Assistant.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modding_Assistant.MVVM.Services
{
    public class MoveDialogService(Window owner) : IMoveModDialogService
    {
        private readonly Window owner = owner;

        public int? ShowNumberDialog()
        {
            var moveDialog = new MoveModView();
            var viewModel = new MoveModViewModel();
            moveDialog.DataContext = viewModel;
            moveDialog.Owner = owner;
            if (moveDialog.ShowDialog() == true)
                return moveDialog.ModNumber;
            else
                return null;
        }
    }
}
