using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace hotel3
{
    public partial class RoomFrom : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable dt;
        private DataTable staffDt;

        public RoomFrom()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadData();
            LoadStaffData();
           

        }
        private void LoadData()
        {
            conn.Open();
            string query = @"
                SELECT Rooms.Room_ID_PK, Rooms.Room_Type, Rooms.Status, Rooms.Room_Cost, Staff.Staff_ID_PK || ' ' || Staff.Last_Name || ' ' || Staff.First_Name AS StaffID
                FROM Rooms
                JOIN Staff ON Rooms.Staff_ID_FK = Staff.Staff_ID_PK";
            adapter = new SQLiteDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void LoadStaffData()
        {
            string staffQuery = "SELECT Staff_ID_PK, Last_Name || ' ' || First_Name AS FullName FROM Staff";
            SQLiteDataAdapter staffAdapter = new SQLiteDataAdapter(staffQuery, conn);
            staffDt = new DataTable();
            staffAdapter.Fill(staffDt);

            comboBox1.DataSource = staffDt;
            comboBox1.DisplayMember = "FullName";
            comboBox1.ValueMember = "Staff_ID_PK";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Rooms (Room_Type, Status, Room_Cost, Staff_ID_FK) VALUES (@RoomType, @Status, @RoomCost, @StaffID)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@RoomType", textBox1.Text);
            cmd.Parameters.AddWithValue("@Status", textBox2.Text);
            cmd.Parameters.AddWithValue("@RoomCost", textBox3.Text);
            cmd.Parameters.AddWithValue("@StaffID", comboBox1.SelectedValue);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string roomId = selectedRow.Cells["Room_ID_PK"].Value.ToString();
                string query = "UPDATE Rooms SET Room_Type=@RoomType, Status=@Status, Room_Cost=@RoomCost, Staff_ID_FK=@StaffID WHERE Room_ID_PK=@RoomID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@RoomType", textBox1.Text);
                cmd.Parameters.AddWithValue("@Status", textBox2.Text);
                cmd.Parameters.AddWithValue("@RoomCost", textBox3.Text);
                cmd.Parameters.AddWithValue("@StaffID", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@RoomID", roomId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите номер для редактирования.");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string roomId = selectedRow.Cells["Room_ID_PK"].Value.ToString();
                string query = "DELETE FROM Rooms WHERE Room_ID_PK=@RoomID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@RoomID", roomId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите номер для удаления.");
            }
        }
    }
}
