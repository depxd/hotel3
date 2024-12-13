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
    public partial class StuffFormWork : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;
        public StuffFormWork()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadData();
        }
        private void LoadData()
        {
            conn.Open();
            string query = @"
            SELECT Staff.Last_Name || ' ' || Staff.First_Name || ' ' || Staff.Patronymic AS FullName,
                   Rooms.Room_ID_PK,
                   Rooms.Room_Type
            FROM Rooms
            JOIN Staff ON Rooms.Staff_ID_FK = Staff.Staff_ID_PK";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();

            // Устанавливаем заголовки столбцов на русском языке
            dataGridView1.Columns["FullName"].HeaderText = "ФИО сотрудника";
            dataGridView1.Columns["Room_ID_PK"].HeaderText = "Номер комнаты";
            dataGridView1.Columns["Room_Type"].HeaderText = "Тип комнаты";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchValue = textBox1.Text.Trim().ToLower();
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("FullName LIKE '%{0}%'", searchValue);
            dataGridView1.DataSource = dv.ToTable();
        }

        private void button2_Click(object sender, EventArgs e)
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
