using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
        }


        //Clear the username field automatically when clicked
        private void textUsername_Click(object sender, EventArgs e)
        {
            textUsername.Clear();
        }

        //Clear the password field and changed the characters into *
        private void textPassword_Click(object sender, EventArgs e)
        {
            textPassword.Clear();
            textPassword.PasswordChar = '*';
        }

        //check the username and password
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (textUsername.Text == "admin" && textPassword.Text == "admin")
            {
                textBox1.Visible = true;
                textBox1.Text = "Welcome, Please Wait!";
                textBox1.ForeColor = Color.White;
                await Task.Delay(550);
                this.Hide();
                formMain mainForm = new formMain();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                textBox1.Visible = true;
                textBox1.Text = "Wrong Password, Please Try Again";
                textBox1.ForeColor = Color.Red;
            }
        }

        //Automatically validate the username and password when enter is pressed in password field
        private void textPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(this, new EventArgs());
            }
        }
    }
}
