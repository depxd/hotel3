using hotel3;
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
    public partial class SysAdminForm : Form
    {
        public SysAdminForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StuffForm employeesForm = new StuffForm();
            employeesForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RoomFrom roomsForm = new RoomFrom();
            roomsForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ServiceForm servicesForm = new ServiceForm();
            servicesForm.Show();
        }

        private void SysAdminForm_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Показать сообщение с подтверждением
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из учетной записи?", "Выход из учетной записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Если пользователь подтверждает выход
            if (result == DialogResult.Yes)
            {
                // Создать экземпляр LoginForm
                LoginForm loginForm = new LoginForm();

                // Показать LoginForm и закрыть SysAdminForm
                loginForm.Show();
                this.Close(); // Закрыть SysAdminForm
            }
        }
    }
}


