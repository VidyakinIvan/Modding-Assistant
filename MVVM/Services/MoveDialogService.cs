using Modding_Assistant.MVVM.View;
using Modding_Assistant.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services
{
    internal class MoveDialogService : IMoveModDialogService
    {
        public int? ShowNumberDialog()
        {
            var moveDialog = new MoveModView();
            var viewModel = new MoveModViewModel();
            moveDialog.DataContext = viewModel;
            if (moveDialog.ShowDialog() == true)
                return moveDialog.ModNumber;
            else
                return null;
        }
    }
}
