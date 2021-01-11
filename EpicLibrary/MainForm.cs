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
    
    public partial class MainForm : Form
    {
       

        // Giving dragging power to panels ( pain )
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
          
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Admin Radio Button
            uC_Admin1.BringToFront();           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Books Radio Button
            uC_Books1.BringToFront();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Members Radio Button
            uC_Members1.BringToFront();
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Home Radio Button
            uC_Home1.BringToFront();
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
   
    }
}
