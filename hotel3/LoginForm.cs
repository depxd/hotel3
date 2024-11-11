using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel3
{
    public partial class LoginForm : Form
    {
        private string correctUsername = "admin";
        private string correctPassword = "password123";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click_Click(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == correctUsername && textBoxPassword.Text == correctPassword)
            {
                this.Hide(); // Скрываем форму входа
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += (s, args) => this.Close(); // Закрываем форму входа при закрытии MainForm
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Ошибка входа. Пожалуйста, проверьте логин и пароль.");
            }
        }

    }
}
