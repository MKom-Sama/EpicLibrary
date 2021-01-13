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
    public partial class UC_Books_IssueBooks : UserControl
    {
        SqlConnection connection;
        public UC_Books_IssueBooks()
        {
            InitializeComponent();
            //connectionString = ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool valid = false;
            int BookID = 0;
            int MemberID = 0;
            try
            {
                // issue Book
                BookID = Convert.ToInt32(textBox3.Text);
                MemberID = Convert.ToInt32(textBox4.Text);
                valid = true;

            }catch(FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }



            while (valid)
            {
                // Check if BookID is in BooksTable

                if (!BookExist(BookID))
                {
                    MessageBox.Show("Book Doesn't Exist!");break;
                }

                // Check if MembersID is in MembersTable

                if (!MemberExist(MemberID))
                {
                    MessageBox.Show("Member Doesn't Exist!");break;
                }

                // Add BookID/MemberID/DateOFPurchase and return IssueID

                int issueID = AddDataToDB(BookID,MemberID);

                label4.Text = issueID.ToString();

                MessageBox.Show($"New IssueID  : {issueID}\n" +
                                $"For MemberID : {MemberID}\n" +
                                $"BookID : {BookID}");

                //Reduce Book Quantity in Books Table

                ReduceBook(BookID);


                clearInputs();
                break;
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

                exist =  (int)command.ExecuteScalar();

            }

            if (exist == 1) return true;
            return false;
        }

        bool MemberExist(int MemberID)
        {
            string query =
               "SELECT COUNT(MemberID) FROM Members WHERE MemberID = @memberID;";

            int exist = 0;
            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@memberID", MemberID);

                exist = (int)command.ExecuteScalar();

            }

            if (exist == 1) return true;
            return false;
        }

        int AddDataToDB(int BookID,int MemberID)
        {
            string query =
               "INSERT INTO MembersBooks " +
               "VALUES(@bookID,@memberID,@dateOfPurchase)";


            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);
                command.Parameters.AddWithValue("@memberID", MemberID);             
                command.Parameters.AddWithValue("@dateOfPurchase", DateTime.Now);


                try
                {
                    command.ExecuteScalar();
                    return IssueID(BookID, MemberID);
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            return 0;
        }

        int IssueID(int BookID,int MemberID)
        {
            string query =
              "Select  IssueID FROM MembersBooks " +
              "WHERE BookID = @bookID AND MemberID = @memberID;";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@bookID", BookID);
                command.Parameters.AddWithValue("@memberID", MemberID);
                try
                {
                    int issueID = (int)command.ExecuteScalar();
                    return issueID;
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            return 0;
        }

        void clearInputs()
        {
            textBox3.Text = "";
            textBox4.Text = "";
        }

        void ReduceBook(int BookID)
        {
            // Reduces the Quantity of the book by 1
           int currentQuantity =  getBookQuantity(BookID);
           int UpdatedQuantity = currentQuantity - 1;

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
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message);
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
                    MessageBox.Show(exception.Message);
                }
            }
            return 0;
        }
    }
}
