using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel3
{
    public partial class StuffForm : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;

        public StuffForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\Hotel1.db;Version=3;");
            LoadData();
        }
        private void LoadData()
        {
            conn.Open();
            string query = "SELECT * FROM Staff";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Staff (Last_Name, First_Name, Patronymic, Specialization, Phone_Number, Address) VALUES (@LastName, @FirstName, @Patronymic, @Specialization, @PhoneNumber, @Address)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
            cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text);
            cmd.Parameters.AddWithValue("@Specialization", textBox4.Text);
            cmd.Parameters.AddWithValue("@PhoneNumber", textBox5.Text);
            cmd.Parameters.AddWithValue("@Address", textBox6.Text); 
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close(); 
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            { 
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int staffId = Convert.ToInt32(row.Cells["Staff_ID_PK"].Value);
                string query = "UPDATE Staff SET Last_Name=@LastName, First_Name=@FirstName, Patronymic=@Patronymic, Specialization=@Specialization, Phone_Number=@PhoneNumber, Address=@Address WHERE Staff_ID_PK=@StaffID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text);
                cmd.Parameters.AddWithValue("@Specialization", textBox4.Text);
                cmd.Parameters.AddWithValue("@PhoneNumber", textBox5.Text);
                cmd.Parameters.AddWithValue("@Address", textBox6.Text);
                cmd.Parameters.AddWithValue("@StaffID", staffId); conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для редактирования.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int staffId = Convert.ToInt32(row.Cells["Staff_ID_PK"].Value);
                string query = "DELETE FROM Staff WHERE Staff_ID_PK=@StaffID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@StaffID", staffId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления.");
            }
        }
    }
}
