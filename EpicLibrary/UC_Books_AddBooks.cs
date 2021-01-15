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
        string connectionString = "nullVal";
        bool isAddingQuantity = false;
        public UC_Books_AddBooks()
        {
            InitializeComponent();

            // credentials for database    
            //connectionString = ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!isAddingQuantity)
            {
                enableInputs(true);
                try
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
                        book = new RentedBook(bookID, bookName, authorName, quantity, (double)rating, price);
                        type = "buy";
                    }


                    // Bought Book
                    if (radioButton2.Checked)
                    {
                        book = new BoughtBook(bookID, bookName, authorName, quantity, (double)rating, price);
                        type = "rent";
                    }


                    // Book Info now should be added to a Database
                    int duplicateKeyError = AddBookToDB(book.ID, book.name, book.author, type, book.quantity, (decimal)book.rating, (decimal)price);

                    if (duplicateKeyError != 1)
                    {
                        if (BookExist(duplicateKeyError))
                        {
                            if (MessageBox.Show("Book Already Exists, Do you wanna add more quantities to the same book?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                setBookInfo(duplicateKeyError); // this function Also assigns the textboxes value
                                isAddingQuantity = true;
                            }
                            else
                            {
                                clearInputs();
                            }

                        }
                    }else
                        clearInputs();

                }
                catch (FormatException exception)
                {
                    
                    MessageBox.Show("Invalid Input Try again :)", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clearInputs();
                }

               
            }
            else
            {
                
              int bookID = Convert.ToInt32(textBox1.Text);
              int quantity = Convert.ToInt32(numericUpDown1.Value);

              if(MessageBox.Show($"Do you wanna add {quantity} more books to the BookID : {bookID}?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    addToBookQuantity(bookID, quantity);
                }
                else
                {
                    isAddingQuantity = false;
                    clearInputs();
                    enableInputs(true);
                }
              

            isAddingQuantity = false;
               
            }


        }

       
        public int AddBookToDB(int ID, string name, string author, string type, int quantity, decimal rating, decimal price)
        {
            string query =
              "INSERT INTO Books " +
              "VALUES(@bookID,@bookName,@author,@type,@quantity,@rating,@price)";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
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

                try
                {
                    command.ExecuteScalar();
                    return 1;
                }
                catch(SqlException exception)
                {
                   
                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    try
                    {
                        int duplicateKey = ID; // if !null value
                        return duplicateKey;
                    }catch(Exception exception_1)
                    {
                        MessageBox.Show(exception_1.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return 1;

            }
        }
        bool BookExist(int BookID)
        {
            string query =
                "SELECT COUNT(BookID) FROM Books WHERE BookID = @bookID;";

            int exist = 0;
            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);

                exist = (int)command.ExecuteScalar();

            }

            if (exist == 1) return true;
            return false;
        }
        public void setBookInfo(int BookID)
        {
            string query =
                "SELECT * FROM Books " +
                "WHERE BookID = @bookID;";


            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    // Auto fill Inputs luxury
                    while (oReader.Read())
                    {
                        textBox1.Text = BookID.ToString();
                        textBox4.Text = oReader["Name"].ToString();
                        textBox3.Text = oReader["Author"].ToString();
                        numericUpDown2.Value = Convert.ToDecimal(oReader["Rating"].ToString());
                        textBox2.Text = oReader["Price"].ToString();

                        //  small bug need doesn't switch correctly
                        if (String.Compare(oReader["Type"].ToString(), "buy") == 1)
                        {
                            radioButton1.Checked = true;
                        }
                        else
                            radioButton2.Checked = true;

                        isAddingQuantity = true;

                        enableInputs(false);
                      

                    }
                    
                }
            }
        }

        void enableInputs(bool enable)
        {
            textBox1.Enabled = enable;
            textBox4.Enabled = enable;
            textBox3.Enabled = enable;
            numericUpDown2.Enabled = enable;
            textBox2.Enabled = enable;
            radioButton1.Enabled = enable;
            radioButton2.Enabled = enable;
        }

        void addToBookQuantity(int BookID,int quantity)
        {
            // Adds to Book's Quantity in DB
            int currentQuantity = getBookQuantity(BookID);
            int UpdatedQuantity = currentQuantity + quantity;

            // MessageBox.Show(UpdatedQuantity.ToString());

            string query =
              "UPDATE Books " +
              "SET Quantity = @updatedQuantity " +
              "WHERE BookID = @bookID;";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);
                command.Parameters.AddWithValue("@updatedQuantity", UpdatedQuantity);

                try
                {
                    command.ExecuteScalar();
                    MessageBox.Show($"There is now {UpdatedQuantity} for BookID: {BookID}");
                    clearInputs();
                    enableInputs(true);
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        int getBookQuantity(int BookID)
        {
            string query =
              "Select  Quantity FROM Books " +
              "WHERE BookID = @bookID;";


            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);
                try
                {
                    int Quantity = (int)command.ExecuteScalar();
                    return Quantity;
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return 0;
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
