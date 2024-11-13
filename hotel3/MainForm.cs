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
        private DataTable bookingDt;
        private DataTable clientsDt;
        private DataTable roomsDt;
        public MainForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadClients();
            LoadRooms();
            LoadServices();
            LoadBookings();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }
        private void LoadServices()
        {
            string serviceQuery = "SELECT Service_ID_PK, Service FROM Additional_Services";
            SQLiteDataAdapter servicesAdapter = new SQLiteDataAdapter(serviceQuery, conn);
            DataTable servicesDt = new DataTable();
            servicesAdapter.Fill(servicesDt);

            checkedListBox1.DataSource = servicesDt;
            checkedListBox1.DisplayMember = "Service";
            checkedListBox1.ValueMember = "Service_ID_PK";
        }
        private void LoadClientDetails(int clientId)
        {
            string query = "SELECT Last_Name, First_Name, Patronymic, Passport_Series, Passport_Number FROM Clients WHERE Client_ID_PK = @ClientID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientID", clientId);

            SQLiteDataAdapter clientAdapter = new SQLiteDataAdapter(cmd);
            DataTable clientDt = new DataTable();
            clientAdapter.Fill(clientDt);

            dataGridView2.DataSource = clientDt;
        }
        private void LoadRoomDetails(int roomId)
        {
            string query = "SELECT Room_Type, Room_Cost FROM Rooms WHERE Room_ID_PK = @RoomID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@RoomID", roomId);

            SQLiteDataAdapter roomAdapter = new SQLiteDataAdapter(cmd);
            DataTable roomDt = new DataTable();
            roomAdapter.Fill(roomDt);

            dataGridView3.DataSource = roomDt;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int bookingId = Convert.ToInt32(selectedRow.Cells["Booking_ID_PK"].Value);
                int clientId = Convert.ToInt32(selectedRow.Cells["Client_ID_FK"].Value);
                int roomId = Convert.ToInt32(selectedRow.Cells["Room_ID_FK"].Value);

                // Очищаем предыдущие данные
               listBox1.Items.Clear();

                LoadSelectedServices(bookingId);  // Загружаем услуги
                LoadClientDetails(clientId);        // Загружаем данные клиента
                LoadRoomDetails(roomId);            // Загружаем детали номера

                // Очистка checkedListBox1 перед установкой новых значений
                // Очистка состояния выбранных элементов в checkedListBox1
                foreach (int i in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }
                // Здесь также нужно обновить checkedListBox1
                string query = "SELECT Service_ID_FK FROM Booking_Service WHERE Booking_ID_FK=@BookingID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);

                SQLiteDataAdapter servicesAdapter = new SQLiteDataAdapter(cmd);
                DataTable servicesDt = new DataTable();
                servicesAdapter.Fill(servicesDt);

                foreach (DataRow row in servicesDt.Rows)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)checkedListBox1.Items[i];
                        if (Convert.ToInt32(item["Service_ID_PK"]) == Convert.ToInt32(row["Service_ID_FK"]))
                        {
                            checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                        }
                    }
                }
            }
            else
            {
                listBox1.Items.Clear();
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;
            }
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
            string roomQuery = "SELECT Room_ID_PK, Room_Type || ' ' || Room_Cost AS RoomType FROM Rooms WHERE Status = 'Доступен для бронирования'";
            SQLiteDataAdapter roomsAdapter = new SQLiteDataAdapter(roomQuery, conn);
            roomsDt = new DataTable();
            roomsAdapter.Fill(roomsDt);

            comboBox1.DataSource = roomsDt;
            comboBox1.DisplayMember = "RoomType";
            comboBox1.ValueMember = "Room_ID_PK";
        }

        private void LoadBookings()
        {
            if (bookingDt != null)
            {
                bookingDt.Clear();
            }

            conn.Open();
            string query = "SELECT * FROM Room_Booking";
            adapter = new SQLiteDataAdapter(query, conn);
            bookingDt = new DataTable();
            adapter.Fill(bookingDt);
            dataGridView1.DataSource = bookingDt;
            conn.Close();
        }
        private void LoadSelectedServices(int bookingId)
        {
            string query = "SELECT Additional_Services.Service " +
                           "FROM Booking_Service " +
                           "JOIN Additional_Services ON Booking_Service.Service_ID_FK = Additional_Services.Service_ID_PK " +
                           "WHERE Booking_Service.Booking_ID_FK = @BookingID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingID", bookingId);
            SQLiteDataAdapter servicesAdapter = new SQLiteDataAdapter(cmd);
            DataTable servicesDt = new DataTable();
            servicesAdapter.Fill(servicesDt);

            // Очистка checkedListBox2 перед добавлением новых значений
            listBox1.Items.Clear();

            foreach (DataRow row in servicesDt.Rows)
            {
                listBox1.Items.Add(row["Service"].ToString());
            }
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
            conn.Open();

            // Проверка на пересечение с уже существующими бронированиями
            string checkQuery = "SELECT COUNT(*) FROM Room_Booking WHERE Room_ID_FK = @RoomID AND (" +
                                "(@CheckInDate BETWEEN Check_In_Date AND Check_Out_Date) OR " +
                                "(@CheckOutDate BETWEEN Check_In_Date AND Check_Out_Date) OR " +
                                "(@CheckInDate <= Check_In_Date AND @CheckOutDate >= Check_Out_Date))";
            SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@RoomID", comboBox1.SelectedValue);
            checkCmd.Parameters.AddWithValue("@CheckInDate", dateTimePicker1.Value);
            checkCmd.Parameters.AddWithValue("@CheckOutDate", dateTimePicker2.Value);

            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count > 0)
            {
                MessageBox.Show("Комната занята в выбранный период. Пожалуйста, выберите другую комнату или измените даты бронирования.");
                conn.Close();
                return;
            }
            string query = "INSERT INTO Room_Booking (Client_ID_FK, Room_ID_FK, Check_In_Date, Check_Out_Date) VALUES (@ClientID, @RoomID, @CheckInDate, @CheckOutDate)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClientID", comboBox2.SelectedValue);
            cmd.Parameters.AddWithValue("@RoomID", comboBox1.SelectedValue);
            cmd.Parameters.AddWithValue("@CheckInDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@CheckOutDate", dateTimePicker2.Value);
            cmd.ExecuteNonQuery();

            long bookingId = conn.LastInsertRowId;

            foreach (var item in checkedListBox1.CheckedItems)
            {
                DataRowView row = (DataRowView)item;
                int serviceId = Convert.ToInt32(row["Service_ID_PK"]);

                string serviceQuery = "INSERT INTO Booking_Service (Booking_ID_FK, Service_ID_FK) VALUES (@BookingID, @ServiceID)";
                SQLiteCommand serviceCmd = new SQLiteCommand(serviceQuery, conn);
                serviceCmd.Parameters.AddWithValue("@BookingID", bookingId);
                serviceCmd.Parameters.AddWithValue("@ServiceID", serviceId);
                serviceCmd.ExecuteNonQuery();
            }

            conn.Close();
            LoadBookings();
            listBox1.Items.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string bookingId = selectedRow.Cells["Booking_ID_PK"].Value.ToString();

                // Обновление информации о бронировании
                string query = "UPDATE Room_Booking SET Client_ID_FK=@ClientID, Room_ID_FK=@RoomID, Check_In_Date=@CheckInDate, Check_Out_Date=@CheckOutDate WHERE Booking_ID_PK=@BookingID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientID", comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@RoomID", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@CheckInDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@CheckOutDate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                conn.Open();
                cmd.ExecuteNonQuery();

                // Удаление старых записей о дополнительных услугах для данного бронирования
                string deleteServicesQuery = "DELETE FROM Booking_Service WHERE Booking_ID_FK=@BookingID";
                SQLiteCommand deleteServicesCmd = new SQLiteCommand(deleteServicesQuery, conn);
                deleteServicesCmd.Parameters.AddWithValue("@BookingID", bookingId);
                deleteServicesCmd.ExecuteNonQuery();

                // Добавление выбранных дополнительных услуг
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    DataRowView row = (DataRowView)item;
                    int serviceId = Convert.ToInt32(row["Service_ID_PK"]);

                    string serviceQuery = "INSERT INTO Booking_Service (Booking_ID_FK, Service_ID_FK) VALUES (@BookingID, @ServiceID)";
                    SQLiteCommand serviceCmd = new SQLiteCommand(serviceQuery, conn);
                    serviceCmd.Parameters.AddWithValue("@BookingID", bookingId);
                    serviceCmd.Parameters.AddWithValue("@ServiceID", serviceId);
                    serviceCmd.ExecuteNonQuery();
                }
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

                    // Очистка listBox1 после удаления бронирования
                    listBox1.Items.Clear();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронь для удаления.");
            }

        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
        }
    }
}
