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
    public partial class UC_Books_IssueBooks : UserControl
    {
        public UC_Books_IssueBooks()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // issue Book
            int BookID = Convert.ToInt32(textBox3.Text);
            int MemberID = Convert.ToInt32(textBox4.Text);

            // Check if BookID is in BooksTable

            // Check if MemberID is in MembersTable

            // Check if MemberID is in RefrenceTable (IssueID/MemberID/BookID/DateOfPurchase)

                // if true create  new coloumn with new data & return old IssueID 
                
                // if false create new coloumn with new data & return new IssueID 

        }
    }
}
