using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpicLibrary
{
    public partial class UC_Members : UserControl
    {
        public UC_Members()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Add Members tab
            uC_Members_AddMembers1.BringToFront();

            uC_Members_ViewMembers1.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // View Members tab
            uC_Members_ViewMembers1.BringToFront();

            uC_Members_ViewMembers1.Visible = true;
        }
    }
}
