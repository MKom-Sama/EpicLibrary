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
    public partial class UC_Books_ReturnBooks : UserControl
    {
        public UC_Books_ReturnBooks()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // checks if MemberID is in MemberTable

            // gets the corresponding BookIDs & DateOfPurchase from RefrenceTable (IssueID/MemberID/BookID/DateOfPurchase)
            
            // Calculates the totalPrice & return totalPrice

            /*Waits on Confirmation...*/

            // Removes Coloumns with IssueID
            // Add Returned Books to the BookTable. (Just Adjust Quantity)
        }
    }
}
