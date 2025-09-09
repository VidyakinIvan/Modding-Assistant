using Modding_Assistant.Core;
using System;
using System.Collections.Generic;
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
    public partial class MoveModView : Window, IMoveDialog
    {
        private bool moveDialogResult = false;
        public MoveModView()
        {
            InitializeComponent();
        }

        public bool MoveDialogResult { 
            get => moveDialogResult; 
            set => moveDialogResult = value; 
        }

        public bool ShowMoveDialog()
        {
            ShowDialog();
            return MoveDialogResult;
        }
    }
}
