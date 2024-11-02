using System;
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
        public MainForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\Hotel1.db;Version=3;"); 
            LoadData();
        }
        private void BookingForm_Load(object sender, EventArgs e)
        {
            LoadClients();
            LoadRooms();
        }
        private void LoadClients()
        {
            string query = "SELECT Client_ID, Client_Name FROM Clients";
            SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Client_Name";
            comboBox1.ValueMember = "Client_ID";
        }

        private void LoadRooms()
        {
            string query = "SELECT Room_ID, Room_Number FROM Rooms";
            SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Room_Number";
            comboBox2.ValueMember = "Room_ID";
        }


        private void LoadData() 
        { 
            conn.Open(); 
            string query = "SELECT * FROM Room_Booking"; 
            adapter = new SQLiteDataAdapter(query, conn); 
            dt = new DataTable(); 
            adapter.Fill(dt); 
            dataGridView1.DataSource = dt; 
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
            int clientID = Convert.ToInt32(comboBox1.SelectedValue);
            int roomID = Convert.ToInt32(comboBox2.SelectedValue);
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;

            string query = "INSERT INTO Room_Booking (Client_ID_FK, Room_ID_FK, Start_Date, End_Date) VALUES (@ClientID, @RoomID, @StartDate, @EndDate)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@RoomID", roomID);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Комната успешно забронирована!");

        }
    }
}
