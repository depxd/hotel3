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
    public partial class ClientForm : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;

        public ClientForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\gmax0\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadData();
        }
        private void LoadData()
        { 
            conn.Open();
            string query = "SELECT * FROM Clients";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // Настройка столбцов dataGridView1
            dataGridView1.Columns["Client_ID_PK"].HeaderText = "ID Клиента";
            dataGridView1.Columns["Last_Name"].HeaderText = "Фамилия";
            dataGridView1.Columns["First_Name"].HeaderText = "Имя";
            dataGridView1.Columns["Patronymic"].HeaderText = "Отчество";
            dataGridView1.Columns["Birth_Date"].HeaderText = "Дата Рождения";
            dataGridView1.Columns["Passport_Series"].HeaderText = "Серия Паспорта";
            dataGridView1.Columns["Passport_Number"].HeaderText = "Номер Паспорта";
            dataGridView1.Columns["Address"].HeaderText = "Адрес";
            dataGridView1.Columns["Citizenship"].HeaderText = "Гражданство";
            dataGridView1.Columns["Email"].HeaderText = "Email";
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["Last_Name"].Value.ToString();
                textBox2.Text = selectedRow.Cells["First_Name"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Patronymic"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["Birth_Date"].Value);
                textBox4.Text = selectedRow.Cells["Passport_Series"].Value.ToString();
                textBox5.Text = selectedRow.Cells["Passport_Number"].Value.ToString();
                textBox6.Text = selectedRow.Cells["Address"].Value.ToString();
                textBox7.Text = selectedRow.Cells["Citizenship"].Value.ToString();
                textBox8.Text = selectedRow.Cells["Email"].Value.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Clients (Last_Name, First_Name, Patronymic, Birth_Date, Passport_Series, Passport_Number, Address, Citizenship, Email) VALUES (@LastName, @FirstName, @Patronymic, @BirthDate, @PassportSeries, @PassportNumber, @Address, @Citizenship, @Email)"; 
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
            cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text);
            cmd.Parameters.AddWithValue("@BirthDate", dateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@PassportSeries", textBox4.Text);
            cmd.Parameters.AddWithValue("@PassportNumber", textBox5.Text);
            cmd.Parameters.AddWithValue("@Address", textBox6.Text);
            cmd.Parameters.AddWithValue("@Citizenship", textBox7.Text);
            cmd.Parameters.AddWithValue("@Email", textBox8.Text);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            { 
                string clientId = dataGridView1.SelectedRows[0].Cells["Client_ID_PK"].Value.ToString(); 
                string query = "DELETE FROM Clients WHERE Client_ID_PK=@ClientId"; 
                SQLiteCommand cmd = new SQLiteCommand(query, conn); 
                cmd.Parameters.AddWithValue("@ClientId", clientId); 
                conn.Open(); cmd.ExecuteNonQuery(); 
                conn.Close(); 
                LoadData(); 
            } 
            else 
            { 
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            { 
                string clientId = dataGridView1.SelectedRows[0].Cells["Client_ID_PK"].Value.ToString(); 
                string query = "UPDATE Clients SET Last_Name=@LastName, First_Name=@FirstName, Patronymic=@Patronymic, Birth_Date=@BirthDate, Passport_Series=@PassportSeries, Passport_Number=@PassportNumber, Address=@Address, Citizenship=@Citizenship, Email=@Email WHERE Client_ID_PK=@ClientId";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientId", clientId);
                cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text);
                cmd.Parameters.AddWithValue("@BirthDate", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@PassportSeries", textBox4.Text);
                cmd.Parameters.AddWithValue("@PassportNumber", textBox5.Text);
                cmd.Parameters.AddWithValue("@Address", textBox6.Text);
                cmd.Parameters.AddWithValue("@Citizenship", textBox7.Text);
                cmd.Parameters.AddWithValue("@Email", textBox8.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            } 
            else
            { 
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
