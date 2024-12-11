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
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
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
            // Устанавливаем заголовки столбцов на русском языке
            dataGridView1.Columns["Staff_ID_PK"].HeaderText = "ID сотрудника";
            dataGridView1.Columns["Last_Name"].HeaderText = "Фамилия";
            dataGridView1.Columns["First_Name"].HeaderText = "Имя";
            dataGridView1.Columns["Patronymic"].HeaderText = "Отчество";
            dataGridView1.Columns["Specialization"].HeaderText = "Специализация";
            dataGridView1.Columns["Phone_Number"].HeaderText = "Номер телефона";
            dataGridView1.Columns["Address"].HeaderText = "Адрес";
            dataGridView1.Columns["Staff_ID_PK"].Visible = false;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["Last_Name"].Value.ToString();
                textBox2.Text = selectedRow.Cells["First_Name"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Patronymic"].Value.ToString();
                textBox4.Text = selectedRow.Cells["Specialization"].Value.ToString();
                textBox5.Text = selectedRow.Cells["Phone_Number"].Value.ToString();
                textBox6.Text = selectedRow.Cells["Address"].Value.ToString();
            }
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

        private void StuffForm_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string input = textBox7.Text.Trim();
            string[] parts = input.Split(' ');

            string firstName = parts.Length > 1 ? parts[1] : "";
            string lastName = parts.Length > 0 ? parts[0] : "";
            string middleName = parts.Length > 2 ? parts[2] : "";

            using (var connection = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT * FROM Staff WHERE First_Name LIKE @firstName AND Last_Name LIKE @lastName AND Patronymic LIKE @middleName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", "%" + firstName + "%");
                    command.Parameters.AddWithValue("@lastName", "%" + lastName + "%");
                    command.Parameters.AddWithValue("@middleName", "%" + middleName + "%");

                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
