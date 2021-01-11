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
    public partial class UC_Books : UserControl
    {
     
        public UC_Books()
        {
            InitializeComponent();

        }

   
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Add Books tab
            uC_Books_AddBooks1.BringToFront();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Books Storage tab
            uC_Books_Storage1.BringToFront();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Return Books tab
            uC_Books_ReturnBooks1.BringToFront();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Issue Books tab
            uC_Books_IssueBooks1.BringToFront();
        }
    }
}
