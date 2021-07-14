using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DashBoard
{
    public partial class ChartView : Form
    {
        public ChartView()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User(textBox1.Text, textBox2.Text);
            textBox1.Text = "Admin";
            textBox2.Text = "Admin";
            string error = "Please Enter Correct Credentials";
            if(textBox1.Text!=user.user|| textBox2.Text!=user.Password)
            {
                var result = MessageBox.Show(error);
                                
            }
            else
            {
                Form1 form1 = new Form1();
                form1.Show();
            }
        }
    }
}
