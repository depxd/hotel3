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
            conn = new SQLiteConnection("Data Source=C:\\Users\\gmax0\\Desktop\\Hotel1.db;Version=3;");
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
    }
}
