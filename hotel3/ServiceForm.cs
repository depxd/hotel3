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
    public partial class ServiceForm : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;

        public ServiceForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\gmax0\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

        }
        private void LoadData()
        {
            conn.Open();
            string query = "SELECT * FROM Additional_Services";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            // Устанавливаем заголовки столбцов на русском языке
            dataGridView1.Columns["Service_ID_PK"].HeaderText = "ID услуги";
            dataGridView1.Columns["Service"].HeaderText = "Услуга";
            dataGridView1.Columns["Cost"].HeaderText = "Стоимость";
            dataGridView1.Columns["Availability"].HeaderText = "Доступность";
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["Service"].Value.ToString();
                textBox2.Text = selectedRow.Cells["Cost"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Availability"].Value.ToString();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            { string query = "INSERT INTO Additional_Services (Service, Cost, Availability) VALUES (@Service, @Cost, @Availability)";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@Service", textBox1.Text);
                cmd.Parameters.AddWithValue("@Cost", textBox2.Text);
                cmd.Parameters.AddWithValue("@Availability", textBox3.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData(); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            { 
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string serviceId = selectedRow.Cells["Service_ID_PK"].Value.ToString();
                string query = "UPDATE Additional_Services SET Service=@Service, Cost=@Cost, Availability=@Availability WHERE Service_ID_PK=@ServiceID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@Service", textBox1.Text);
                cmd.Parameters.AddWithValue("@Cost", textBox2.Text);
                cmd.Parameters.AddWithValue("@Availability", textBox3.Text);
                cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                conn.Open(); 
                cmd.ExecuteNonQuery(); 
                conn.Close(); 
                LoadData(); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дополнительную услугу для редактирования.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            { 
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string serviceId = selectedRow.Cells["Service_ID_PK"].Value.ToString();
                string query = "DELETE FROM Additional_Services WHERE Service_ID_PK=@ServiceID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дополнительную услугу для удаления.");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void ServiceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
