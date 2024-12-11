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
    public partial class HistoryForm : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;

        public HistoryForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadData();
        }
        private void LoadData()
        {
            conn.Open();
            string query = @"
                SELECT 
                    Booking_History.Booking_ID_PK, 
                    Clients.Last_Name || ' ' || SUBSTR(Clients.First_Name, 1, 1) || '.' || SUBSTR(Clients.Patronymic, 1, 1) || '.' AS Client_Name,
                    Rooms.Room_ID_PK,
                    Rooms.Room_Type,
                    Booking_History.Check_In_Date,
                    Booking_History.Check_Out_Date
                FROM 
                    Booking_History
                JOIN 
                    Clients ON Booking_History.Client_ID_FK = Clients.Client_ID_PK
                JOIN 
                    Rooms ON Booking_History.Room_ID_FK = Rooms.Room_ID_PK";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridViewHistory.DataSource = dt;
            conn.Close();

            // Настройка столбцов dataGridView1
            dataGridViewHistory.Columns["Booking_ID_PK"].HeaderText = "ID Бронирования";
            dataGridViewHistory.Columns["Client_Name"].HeaderText = "ФИО Клиента";
            dataGridViewHistory.Columns["Room_ID_PK"].HeaderText = "Номер Комнаты";
            dataGridViewHistory.Columns["Room_Type"].HeaderText = "Тип Комнаты";
            dataGridViewHistory.Columns["Check_In_Date"].HeaderText = "Дата Заезда";
            dataGridViewHistory.Columns["Check_Out_Date"].HeaderText = "Дата Выезда";
            dataGridViewHistory.Columns["Booking_ID_PK"].Visible = false;
        }


        private void HistoryForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewHistory.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewHistory.SelectedRows[0];
                int bookingId = Convert.ToInt32(selectedRow.Cells["Booking_ID_PK"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Booking_History WHERE Booking_ID_PK=@BookingID";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    // Обновление данных в таблице после удаления
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim();
            string[] parts = input.Split(' ');

            string firstName = parts.Length > 1 ? parts[1] : "";
            string lastName = parts.Length > 0 ? parts[0] : "";
            string middleName = parts.Length > 2 ? parts[2] : "";

            using (var connection = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;"))
            {
                connection.Open(); 
                string query = @"
            SELECT 
                Booking_History.Booking_ID_PK, 
                Clients.Last_Name || ' ' || SUBSTR(Clients.First_Name, 1, 1) || '.' || SUBSTR(Clients.Patronymic, 1, 1) || '.' AS Client_Name,
                Rooms.Room_ID_PK,
                Rooms.Room_Type,
                Booking_History.Check_In_Date,
                Booking_History.Check_Out_Date
            FROM 
                Booking_History
            JOIN 
                Clients ON Booking_History.Client_ID_FK = Clients.Client_ID_PK
            JOIN 
                Rooms ON Booking_History.Room_ID_FK = Rooms.Room_ID_PK
            WHERE 
                Clients.First_Name LIKE @firstName AND 
                Clients.Last_Name LIKE @lastName AND 
                Clients.Patronymic LIKE @middleName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", "%" + firstName + "%");
                    command.Parameters.AddWithValue("@lastName", "%" + lastName + "%");
                    command.Parameters.AddWithValue("@middleName", "%" + middleName + "%");

                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridViewHistory.DataSource = dataTable;
                    }
                }
            }
        }
    }
}
