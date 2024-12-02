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
    }
}
