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
        private Dictionary<string, string> userRoles = new Dictionary<string, string>
        {
            { "admin", "password123" },
            { "sysadmin", "sysadminpass" },
            { "staff", "staffpass" }
        };
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if (userRoles.ContainsKey(username) && userRoles[username] == password)
            {
                this.Hide(); // Hide login form
                Form roleForm;

                if (username == "admin")
                {
                    roleForm = new MainForm();
                }
                else if (username == "sysadmin")
                {
                    roleForm = new SysAdminForm(); // Form for system admin
                }
                else
                {
                    roleForm = new StuffFormWork(); // Form for staff
                }

                roleForm.FormClosed += (s, args) => this.Close(); // Close login form when the role form is closed
                roleForm.Show();
            }
            else
            {
                MessageBox.Show("Ошибка входа. Пожалуйста, проверьте логин и пароль.");
            }
        }

    }
}


