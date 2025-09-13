using Modding_Assistant.MVVM.ViewModel;
using System.Windows;

namespace Modding_Assistant.MVVM.View.Dialogs
{
    /// <summary>
    /// Interaction logic for MoveModView.xaml
    /// </summary>
    public partial class MoveModsDialog : Window
    {
        public MoveModsDialog()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (e.NewValue is MoveModsViewModel vm)
                {
                    vm.RequestOk += OnRequestOk;
                    vm.RequestClose += OnRequestClose;
                }
            };
        }

        private void OnRequestOk()
        {
            DialogResult = true;
            Close();
        }

        private void OnRequestClose()
        {
            Close();
        }

        public int? ModNumber
        {
            get
            {
                if (DataContext is MoveModsViewModel vm)
                    return vm.ModNumber;
                else
                    return null;
            }
            set
            {
                if (DataContext is MoveModsViewModel vm)
                {
                    vm.ModNumber = value;
                }
            }
        }
    }
}
