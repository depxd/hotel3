using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace hotel3
{
    public partial class MainForm : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;
        private DataTable bookingDt;
        private DataTable clientsDt;
        private DataTable roomsDt;
        public MainForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\Hotel1.db;Version=3;");
            LoadClients();
            LoadRooms();
            LoadBookings();
        }
        private void LoadClients()
        {
            string clientQuery = "SELECT Client_ID_PK, Last_Name || ' ' || First_Name AS FullName FROM Clients";
            SQLiteDataAdapter clientsAdapter = new SQLiteDataAdapter(clientQuery, conn);
            clientsDt = new DataTable();
            clientsAdapter.Fill(clientsDt);

            comboBox2.DataSource = clientsDt;
            comboBox2.DisplayMember = "FullName";
            comboBox2.ValueMember = "Client_ID_PK";
        }
        private void LoadRooms()
        {
            string roomQuery = "SELECT Room_ID_PK, Room_Type || ' ' || Room_Cost AS RoomType FROM Rooms";
            SQLiteDataAdapter roomsAdapter = new SQLiteDataAdapter(roomQuery, conn);
            roomsDt = new DataTable();
            roomsAdapter.Fill(roomsDt);

            comboBox1.DataSource = roomsDt;
            comboBox1.DisplayMember = "RoomType";
            comboBox1.ValueMember = "Room_ID_PK";
        }
        private void LoadBookings()
        {
            conn.Open();
            string query = "SELECT * FROM Room_Booking";
            adapter = new SQLiteDataAdapter(query, conn);
            bookingDt = new DataTable();
            adapter.Fill(bookingDt);
            dataGridView1.DataSource = bookingDt;
            conn.Close();
        }
        private void buttonClient_Click(object sender, EventArgs e)
        {
            ClientForm clientsForm = new ClientForm();
            clientsForm.Show();
        }
        private void buttonStuff_Click(object sender, EventArgs e)
        {
            StuffForm employeesForm = new StuffForm();
            employeesForm.Show();
        }
        private void Room_Click(object sender, EventArgs e)
        {
            RoomFrom roomsForm = new RoomFrom();
            roomsForm.Show();
        }

        private void Service_Click(object sender, EventArgs e)
        {
            ServiceForm servicesForm = new ServiceForm();
            servicesForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Room_Booking (Client_ID_FK, Room_ID_FK, Check_In_Date, Check_Out_Date) VALUES (@ClientID, @RoomID, @CheckInDate, @CheckOutDate)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientID", comboBox2.SelectedValue);
            cmd.Parameters.AddWithValue("@RoomID", comboBox1.SelectedValue);
            cmd.Parameters.AddWithValue("@CheckInDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@CheckOutDate", dateTimePicker2.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadBookings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string bookingId = selectedRow.Cells["Booking_ID_PK"].Value.ToString();
                string query = "UPDATE Room_Booking SET Client_ID_FK=@ClientID, Room_ID_FK=@RoomID, Check_In_Date=@CheckInDate, Check_Out_Date=@CheckOutDate WHERE Booking_ID_PK=@BookingID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientID", comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@RoomID", comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@CheckInDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@CheckOutDate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadBookings();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронь для редактирования.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string bookingId = selectedRow.Cells["Booking_ID_PK"].Value.ToString();

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Room_Booking WHERE Booking_ID_PK=@BookingID";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронь для удаления.");
            }
        }
    }
}
