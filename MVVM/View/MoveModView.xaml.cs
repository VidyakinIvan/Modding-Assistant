using Modding_Assistant.Core;
using Modding_Assistant.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modding_Assistant.MVVM.View
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
                if (e.NewValue is MoveModViewModel vmNew)
                {
                    vmNew.RequestOk += () => { DialogResult = true; Hide(); };
                    vmNew.RequestClose += () => Hide();
                }
            };
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
