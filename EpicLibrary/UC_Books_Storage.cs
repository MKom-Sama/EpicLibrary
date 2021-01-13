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
    public partial class UC_Books_Storage : UserControl
    {
        public UC_Books_Storage()
        {
            InitializeComponent();
            try
            {
                this.booksTableAdapter.Refresh(this.libraryDatabaseDataSet.Books);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.booksTableAdapter.Refresh(this.libraryDatabaseDataSet.Books);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
