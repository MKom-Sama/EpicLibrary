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

    public partial class UC_Books_AddBooks : UserControl
    {
        public UC_Books_AddBooks()
        {
            InitializeComponent();
        }
        /* miniDB */
    
        private void button1_Click(object sender, EventArgs e)
        {
            int bookID =Convert.ToInt32(textBox1.Text);
            string bookName = textBox4.Text;
            double price = Convert.ToDouble(textBox2.Text);
            int quantity = Convert.ToInt32(numericUpDown1.Value);

            Book book = null;
            // Rented Book
            if (radioButton1.Checked) 
                book = new RentedBook(bookID,bookName,/*author name*/"",quantity,/*rating*/0,price);

            // Bought Book
            if (radioButton2.Checked)
                book = new BoughtBook(bookID, bookName,/*author name*/"", quantity,/*rating*/0, price);

            // Book Info now should be added to a Database



            MessageBox.Show("Books Added :)");

          
    
        }
    }
}
