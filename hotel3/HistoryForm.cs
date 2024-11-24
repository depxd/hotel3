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
            string query = "SELECT * FROM Booking_History";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridViewHistory.DataSource = dt;
            conn.Close();
            // Настройка столбцов dataGridView1
            dataGridViewHistory.Columns["Booking_ID_PK"].HeaderText = "ID Бронирования";
            dataGridViewHistory.Columns["Client_ID_FK"].HeaderText = "ID Клиента";
            dataGridViewHistory.Columns["Room_ID_FK"].HeaderText = "ID Комнаты";
            dataGridViewHistory.Columns["Check_In_Date"].HeaderText = "Дата Заезда";
            dataGridViewHistory.Columns["Check_Out_Date"].HeaderText = "Дата Выезда";
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

    }
}
