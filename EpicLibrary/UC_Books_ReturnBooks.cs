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
    public partial class UC_Books_ReturnBooks : UserControl
    {
        SqlConnection connection;
        public UC_Books_ReturnBooks()
        {
            InitializeComponent();

            label6.Visible = false;
            textBox4.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int MemberID = 0;
            int IssueID  = 0;
            decimal totalPrice = 0;
            try
            {
                MemberID = Convert.ToInt32(textBox3.Text);
                if (radioButton1.Checked) IssueID = Convert.ToInt32(textBox4.Text);

            }catch(FormatException exception)
            {
                MessageBox.Show("Invalid Input Try again :)", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         

            // checks if MemberID is in MemberTable
            if (MemberExist(MemberID)) 
            {
                int BookID;
              
                // User Purchased a Book
                if (radioButton1.Checked)
                {
                    // Return Just one book with memberID and IssueID
                  totalPrice =  getTotalPrice(MemberID, IssueID);
                  MessageBox.Show($"Total Price {totalPrice}");  
                }

                if (radioButton2.Checked)
                {
                    // Return all books with memberID
                    totalPrice = getTotalPrice(MemberID);
                    MessageBox.Show($"Total Price {totalPrice}");
                }

            }
            else
            {
                MessageBox.Show("User didnt buy a book or doesn't exist", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            clearInputs();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Return all books with memberID
            label6.Visible = false;
            textBox4.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Return Just one book with memberID and IssueID
            label6.Visible = true;
            textBox4.Visible = true;
        }

        bool MemberExist(int MemberID)
        {
            // Modified for MembersBooks Table
            string query =
               "SELECT COUNT(MemberID) FROM MembersBooks WHERE MemberID = @memberID;";

            int exist = 0;
            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@memberID", MemberID);

                exist = (int)command.ExecuteScalar();
                
            }

            if (exist > 0) return true;
            return false;
        }

        int getBookID(int MemberID,int IssueID,ref DateTime dateOfPurchase)
        {
            
            string query =
             "SELECT * FROM MembersBooks " +
             "WHERE MemberID = @memberID AND IssueID = @issueID;";

            string BookID = "nullVal";
            string DateOfPurchase = "nullVal";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@memberID", MemberID);
                command.Parameters.AddWithValue("@issueID", IssueID);

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        BookID = oReader["BookID"].ToString();
                        DateOfPurchase = oReader["DateOfPurchase"].ToString();

                        dateOfPurchase = Convert.ToDateTime(DateOfPurchase);
                        return Convert.ToInt32(BookID);
                    }
                }
            }

            MessageBox.Show($"BookID Bought :{BookID}" +
                            $"Date  : {DateOfPurchase} ");
            return 0;

        }

        decimal getTotalPrice(int MemberID,int IssueID)
        {
            DateTime dateOfPurchase = DateTime.MinValue;

            int BookID = getBookID(MemberID, IssueID,ref dateOfPurchase);

            decimal price;

            price = getBookPrice(BookID, dateOfPurchase);

            // dont forget to increase book Quantity if its Rented
            IncreaseBookByOne(Convert.ToInt32(BookID));

            // also remove IssueID from table
            removeIssueID(IssueID);
            return price;

        }
        decimal getTotalPrice(int MemberID)
        {
            string query =
              "SELECT * FROM MembersBooks " +
              "WHERE MemberID = @memberID";

            decimal totalPrice = 0;

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@memberID", MemberID);
        

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        string BookID = oReader["BookID"].ToString();
                        string DateOfPurchase = oReader["DateOfPurchase"].ToString();
                        string IssueID = oReader["IssueID"].ToString();

                        totalPrice += getBookPrice(Convert.ToInt32(BookID), Convert.ToDateTime(DateOfPurchase));

                        // dont forget to increase book Quantity if its Rented
                        IncreaseBookByOne(Convert.ToInt32(BookID)); // done :3

                        // Remove IssueID
                        removeIssueID(Convert.ToInt32(IssueID));
                    }
                    return totalPrice;
                }
            }
            return -1;
        }
        decimal getBookPrice(int BookID,DateTime dateOfPurchase)
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
                    while (oReader.Read())
                    {
                        string price = oReader["Price"].ToString();
                        string type = oReader["Type"].ToString();

                        // Maybe here add the  info to a list to view  invoice

                        //MessageBox.Show($"{String.Compare(type,"buy")}"); returns 1
                        //MessageBox.Show($"{String.Compare(type, "rent")}"); returns -1

                        if (String.Compare(type,"buy") == 1) return Convert.ToDecimal(price);

                        if (String.Compare(type, "rent") == -1) return RentedBook.getPrice(Convert.ToDecimal(price), dateOfPurchase);
                      
                    }
                }

                return -1;
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
                catch (NullReferenceException exception)
                {
                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return 0;
        }

        void IncreaseBookByOne(int BookID)
        {
            // Reduces the Quantity of the book by 1
            int currentQuantity = getBookQuantity(BookID);
            int UpdatedQuantity = currentQuantity + 1;

            // MessageBox.Show(UpdatedQuantity.ToString());

            string query =
              "UPDATE Books " +
              "SET Quantity = @updatedQuantity " +
              "WHERE BookID = @bookID AND Type = @type;";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);
                command.Parameters.AddWithValue("@updatedQuantity", UpdatedQuantity);
                command.Parameters.AddWithValue("@type","rent");

                try
                {
                    command.ExecuteScalar();
                }
                catch (SqlException exception)
                {

                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void removeIssueID(int IssueID)
        {
            string query =
                 "DELETE FROM MembersBooks " +
                  "WHERE IssueID = @issueID;";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@issueID", IssueID);

                try
                {
                    command.ExecuteScalar();
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void clearInputs()
        {
            textBox3.Text = "";
            textBox4.Text = "";
        }

    }
}
