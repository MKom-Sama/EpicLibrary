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
    public partial class UC_Members_AddMembers : UserControl
    {
        SqlConnection connection;
        string connectionString;

        public UC_Members_AddMembers()
        {
            InitializeComponent();

            // credentials for database    
            //connectionString = ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString;
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32(textBox3.Text);
                string name = textBox1.Text;
                string phoneNumber = textBox4.Text;


                Member member = null;
                string type = "nullValue";
                // Student Member
                if (radioButton1.Checked)
                {
                    member = new Student(ID, name, phoneNumber);
                    type = "student";
                }


                // Staff Member 
                if (radioButton2.Checked)
                {
                    member = new Staff(ID, name, phoneNumber);
                    type = "staff";
                }

                AddMemberToDB(member.ID, member.name, member.phoneNumber, type);

            }catch(FormatException exception)
            {               
                MessageBox.Show("Invalid Input Try again :)", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            ClearInputs();

            
        }

        public void AddMemberToDB(int id,string name,string phoneNumber,string type)
        {
            string query =
                "INSERT INTO Members " +
                "VALUES(@memberID,@memberName,@phoneNumber,@type)";

            using (connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EpicLibrary.Properties.Settings.LibraryDatabaseConnectionString"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@memberID", id);
                command.Parameters.AddWithValue("@memberName", name);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@type", type);


                try
                {
                    command.ExecuteScalar();

                }catch(SqlException exception)
                {
                    int duplicateKey = id; // if !null value
                   
                    MessageBox.Show(exception.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                
            
                //MessageBox.Show("Number of People in DB : " + rows);
            }
        }

        public void ClearInputs()
        {
            textBox3.Text = "";
            textBox1.Text = "";
            textBox4.Text = "";
        }
    }
}
