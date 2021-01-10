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
    public partial class UC_Members_AddMembers : UserControl
    {
        public UC_Members_AddMembers()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(textBox3.Text);
            string name = textBox1.Text;
            string phoneNumber = textBox4.Text;


            Member member;

            // Student Member
            if (radioButton1.Checked)
                member = new Student(ID, name, phoneNumber);

            // Staff Member 
            if (radioButton2.Checked)
                member = new Staff(ID, name, phoneNumber);

            // Add Member to DB
         }
    }
}
