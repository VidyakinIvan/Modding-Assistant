using Modding_Assistant.MVVM.ViewModel;
using System.Windows;

namespace Modding_Assistant.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for MoveModView.xaml
    /// </summary>
    public partial class MoveModView : Window
    {
        public MoveModView()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (e.NewValue is MoveModViewModel vm)
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
                if (DataContext is MoveModViewModel vm)
                    return vm.ModNumber;
                else
                    return null;
            }
            set
            {
                if (DataContext is MoveModViewModel vm)
                {
                    vm.ModNumber = value;
                }
            }
        }
    }
}
