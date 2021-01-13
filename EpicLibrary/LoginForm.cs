using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Runtime.InteropServices;

namespace EpicLibrary
{
    
    public partial class LoginForm : Form
    {

        // Giving dragging power to panels ( pain )
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        static MainForm mainForm = new MainForm();
       
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // validate input
            // authenticate input
            string inputUsername = textBox1.Text;
            string inputPassword = textBox2.Text;

            if (AuthenticateInput(inputUsername,inputPassword))
            {
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show($"Wrong Input or Password BRUH");
                textBox1.Text = "";
                textBox2.Text = "";
            }
                 
        }

        bool AuthenticateInput(string user,string pass)
        {
            // should authenticate from a database
            string correctUser = "";
            string correctPass = "";
            if (user.Equals(correctUser) && pass.Equals(correctPass)) return true;
            return false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
