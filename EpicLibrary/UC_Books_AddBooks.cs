using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// For DataBase Business
using System.Configuration;
using System.Data.SqlClient;

namespace EpicLibrary
{

    public partial class UC_Books_AddBooks : UserControl
    {
        SqlConnection connection;
        string connectionString;
        public UC_Books_AddBooks()
        {
            InitializeComponent();

            // credentials for database    
            connectionString = ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int bookID = Convert.ToInt32(textBox1.Text);
            string bookName = textBox4.Text;
            double price = Convert.ToDouble(textBox2.Text);
            int quantity = Convert.ToInt32(numericUpDown1.Value);
            string authorName = textBox3.Text;
            decimal rating = numericUpDown2.Value;

            Book book = null;
            string type = "nullVal";

            // Rented Book
            if (radioButton1.Checked)
            {
                book = new RentedBook(bookID, bookName,authorName, quantity,(double)rating, price);
                type = "buy";
            }


            // Bought Book
            if (radioButton2.Checked)
            {
                book = new BoughtBook(bookID, bookName,authorName, quantity, (double)rating, price);
                type = "rent";
            }


            // Book Info now should be added to a Database
            AddBookToDB(book.ID, book.name, book.author, type, book.quantity, (decimal)book.rating, (decimal)price);

            clearInputs();
            MessageBox.Show("Books Added :)");


        }

       
      public void AddBookToDB(int ID, string name, string author, string type, int quantity, decimal rating, decimal price)
        {
            string query =
              "INSERT INTO Books " +
              "VALUES(@bookID,@bookName,@author,@type,@quantity,@rating,@price)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", ID);
                command.Parameters.AddWithValue("@bookName", name);
                command.Parameters.AddWithValue("@author", author);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@rating", rating);
                command.Parameters.AddWithValue("@price", price);


                command.ExecuteScalar();

            }
        }
      public void clearInputs()
        {
            textBox1.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 1;
            textBox3.Text = "";
            numericUpDown2.Value = 1;
        }    
    }
}
