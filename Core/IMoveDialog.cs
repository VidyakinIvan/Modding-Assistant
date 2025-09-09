using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.Core
{
    interface IMoveDialog
    {
        public bool MoveDialogResult { get; set; }
        public bool ShowMoveDialog();
    }
}
