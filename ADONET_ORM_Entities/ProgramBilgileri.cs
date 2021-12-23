using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADONET_ORM_Entities
{
    public static class ProgramBilgileri
    {
        public static int TurYokSelectedValue { get; set; } = -1;
        public static void CombonunSelectedIndexiniResetle(ComboBox c)
        {
            c.SelectedIndex = -1;
        }
    }
}
