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
    public partial class UC_Members_ViewMembers : UserControl
    {
        public UC_Members_ViewMembers()
        {
            InitializeComponent();
            try
            {
                this.membersTableAdapter.Refresh(this.libraryDatabaseDataSet.Members);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        public void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.membersTableAdapter.Refresh(this.libraryDatabaseDataSet.Members);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void UC_Members_ViewMembers_Load(object sender, EventArgs e)
        {
            try
            {
                this.membersTableAdapter.Refresh(this.libraryDatabaseDataSet.Members);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
