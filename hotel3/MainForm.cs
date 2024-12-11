using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace hotel3
{
    public partial class MainForm : Form
    {
        private SQLiteConnection conn;
        private SQLiteDataAdapter adapter;
        private DataTable bookingDt;
        private DataTable clientsDt;
        private DataTable roomsDt;
        private DataTable historyDataTable;
        public MainForm()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=C:\\Users\\79307\\Desktop\\hotel3\\Hotel1.db;Version=3;");
            LoadClients();
            LoadRooms();
            LoadServices();
            LoadBookings();
            LoadHistory();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }
        private void LoadServices()
        {
            string serviceQuery = "SELECT Service_ID_PK, Service || ' - ' || Cost AS DisplayService FROM Additional_Services";
            SQLiteDataAdapter servicesAdapter = new SQLiteDataAdapter(serviceQuery, conn);
            DataTable servicesDt = new DataTable();
            servicesAdapter.Fill(servicesDt);

            checkedListBox1.DataSource = servicesDt;
            checkedListBox1.DisplayMember = "DisplayService";
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

            // Настройка заголовков столбцов dataGridView2
            dataGridView2.Columns["Last_Name"].HeaderText = "Фамилия";
            dataGridView2.Columns["First_Name"].HeaderText = "Имя";
            dataGridView2.Columns["Patronymic"].HeaderText = "Отчество";
            dataGridView2.Columns["Passport_Series"].HeaderText = "Серия паспорта";
            dataGridView2.Columns["Passport_Number"].HeaderText = "Номер паспорта";
        }

        private void LoadRoomDetails(int roomId)
        {
            string query = "SELECT Room_ID_PK, Room_Type, Room_Cost FROM Rooms WHERE Room_ID_PK = @RoomID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@RoomID", roomId);

            SQLiteDataAdapter roomAdapter = new SQLiteDataAdapter(cmd);
            DataTable roomDt = new DataTable();
            roomAdapter.Fill(roomDt);

            dataGridView3.DataSource = roomDt;

            // Настройка столбцов dataGridView3
            dataGridView3.Columns["Room_ID_PK"].HeaderText = "Номер комнаты";
            dataGridView3.Columns["Room_Type"].HeaderText = "Класс номера";
            dataGridView3.Columns["Room_Cost"].HeaderText = "Стоимость номера в сутки";

            HighlightOccupiedDates(roomId);
        }
        private void HighlightOccupiedDates(int roomId)
        {
            string query = "SELECT Check_In_Date, Check_Out_Date FROM Room_Booking WHERE Room_ID_FK = @RoomID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@RoomID", roomId);

            SQLiteDataAdapter bookingAdapter = new SQLiteDataAdapter(cmd);
            DataTable bookingDates = new DataTable();
            bookingAdapter.Fill(bookingDates);

            // Очистка предыдущих выделенных дат
            monthCalendar1.RemoveAllBoldedDates();
            monthCalendar1.RemoveAllAnnuallyBoldedDates();
            monthCalendar1.RemoveAllMonthlyBoldedDates();

            foreach (DataRow row in bookingDates.Rows)
            {
                DateTime startDate = Convert.ToDateTime(row["Check_In_Date"]);
                DateTime endDate = Convert.ToDateTime(row["Check_Out_Date"]);

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    monthCalendar1.AddBoldedDate(date);
                }
            }

            monthCalendar1.UpdateBoldedDates();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int bookingId = Convert.ToInt32(selectedRow.Cells["Booking_ID_PK"].Value);
                int clientId = Convert.ToInt32(selectedRow.Cells["Client_ID_FK"].Value);
                string clientName = selectedRow.Cells["Client_Name"].Value.ToString();
                int roomId = Convert.ToInt32(selectedRow.Cells["Room_ID_FK"].Value);

                // Очищаем предыдущие данные
                listBox1.Items.Clear();

                LoadSelectedServices(bookingId);  // Загружаем услуги
                LoadClientDetails(clientId);      // Загружаем данные клиента
                LoadRoomDetails(roomId);          // Загружаем данные о комнате
                LoadGuestDetails(bookingId);      // Загружаем данные о жильцах

                // Очистка checkedListBox1 перед установкой новых значений
                foreach (int i in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }

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
            string roomQuery = "SELECT Room_ID_PK, Room_ID_PK || ' ' || Room_Type || ' ' || Room_Cost AS RoomType FROM Rooms WHERE Status = 'Доступен для бронирования'";
            SQLiteDataAdapter roomsAdapter = new SQLiteDataAdapter(roomQuery, conn);
            roomsDt = new DataTable();
            roomsAdapter.Fill(roomsDt);

            comboBox1.DataSource = roomsDt;
            comboBox1.DisplayMember = "RoomType";
            comboBox1.ValueMember = "Room_ID_PK";

            // Назначаем обработчик события SelectedIndexChanged для ComboBox
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                int roomId = Convert.ToInt32(comboBox1.SelectedValue);
                HighlightOccupiedDates(roomId);
            }
        }


        private void LoadBookings()
        {
            if (bookingDt != null)
            {
                bookingDt.Clear();
            }

            conn.Open();
            string query = @"
        SELECT 
            Room_Booking.Booking_ID_PK, 
            Room_Booking.Client_ID_FK, 
            Clients.Last_Name || ' ' || SUBSTR(Clients.First_Name, 1, 1) || '.' || SUBSTR(Clients.Patronymic, 1, 1) || '.' AS Client_Name,
            Room_Booking.Room_ID_FK,
            Rooms.Room_Type,
            Room_Booking.Check_In_Date,
            Room_Booking.Check_Out_Date
        FROM 
            Room_Booking
        JOIN 
            Clients ON Room_Booking.Client_ID_FK = Clients.Client_ID_PK
        JOIN 
            Rooms ON Room_Booking.Room_ID_FK = Rooms.Room_ID_PK";
            adapter = new SQLiteDataAdapter(query, conn);
            bookingDt = new DataTable();
            adapter.Fill(bookingDt);
            dataGridView1.DataSource = bookingDt;
            conn.Close();

            // Настройка столбцов dataGridView1
            dataGridView1.Columns["Booking_ID_PK"].HeaderText = "ID Бронирования";
            dataGridView1.Columns["Client_ID_FK"].HeaderText = "ID Клиента";
            dataGridView1.Columns["Client_Name"].HeaderText = "ФИО Клиента";
            dataGridView1.Columns["Room_ID_FK"].HeaderText = "Номер Комнаты";
            dataGridView1.Columns["Room_Type"].HeaderText = "Тип Комнаты";
            dataGridView1.Columns["Check_In_Date"].HeaderText = "Дата Заезда";
            dataGridView1.Columns["Check_Out_Date"].HeaderText = "Дата Выезда";

            dataGridView1.Columns["Client_ID_FK"].Visible = false;
            dataGridView1.Columns["Booking_ID_PK"].Visible = false;
            HighlightExpiringBookings();
        }


        private void HighlightExpiringBookings()
        {
            DateTime now = DateTime.Now;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Check_Out_Date"].Value != null)
                {
                    DateTime checkOutDate = Convert.ToDateTime(row.Cells["Check_Out_Date"].Value);
                    int daysLeft = (checkOutDate - now).Days;

                    if (daysLeft <= 1)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (daysLeft <= 3)
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else if (daysLeft <= 5)
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White; // Возвращаем к стандартному цвету
                    }
                }
            }
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
                int bookingId = Convert.ToInt32(selectedRow.Cells["Booking_ID_PK"].Value);
                int clientId = Convert.ToInt32(selectedRow.Cells["Client_ID_FK"].Value);
                int roomId = Convert.ToInt32(selectedRow.Cells["Room_ID_FK"].Value);
                DateTime startDate = Convert.ToDateTime(selectedRow.Cells["Check_In_Date"].Value);
                DateTime endDate = Convert.ToDateTime(selectedRow.Cells["Check_Out_Date"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string deleteQuery = "DELETE FROM Room_Booking WHERE Booking_ID_PK=@BookingID";
                    SQLiteCommand cmd = new SQLiteCommand(deleteQuery, conn);
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);

                    conn.Open();

                    // Добавляем запись в таблицу истории бронирований
                    AddToBookingHistory(bookingId, clientId, roomId, startDate, endDate);

                    // Удаляем запись из основной таблицы бронирований
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    // Обновляем список бронирований
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
        private void AddToBookingHistory(int bookingId, int clientId, int roomId, DateTime startDate, DateTime endDate)
        {
            string query = @"
        INSERT INTO Booking_History (Booking_ID_PK, Client_ID_FK, Room_ID_FK, Check_In_Date, Check_Out_Date)
        VALUES (@BookingID, @ClientID, @RoomID, @CheckInDate, @CheckOutDate)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingID", bookingId);
            cmd.Parameters.AddWithValue("@ClientID", clientId);
            cmd.Parameters.AddWithValue("@RoomID", roomId);
            cmd.Parameters.AddWithValue("@CheckInDate", startDate);
            cmd.Parameters.AddWithValue("@CheckOutDate", endDate);

            cmd.ExecuteNonQuery();
        }
        private void LoadHistory()
        {
            string query = "SELECT * FROM Booking_History";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
            historyDataTable = new DataTable();
            adapter.Fill(historyDataTable);
        }

        private void historyButton_Click(object sender, EventArgs e)
        {
            HistoryForm historyForm = new HistoryForm(); 
            historyForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int bookingId = Convert.ToInt32(selectedRow.Cells["Booking_ID_PK"].Value);

                GuestForm guestsForm = new GuestForm(conn, bookingId);
                guestsForm.ShowDialog();

                // Обновить список жильцов для выбранного бронирования
                LoadGuestDetails(bookingId);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронирование для управления жильцами.");
            }

        }
        private void LoadGuestDetails(int bookingId)
        {
            string query = "SELECT Guests.Last_Name || ' ' || Guests.First_Name || ' ' || Guests.Patronymic AS FullName " +
                           "FROM Booking_Guests " +
                           "JOIN Guests ON Booking_Guests.Guest_ID_FK = Guests.Guest_ID_PK " +
                           "WHERE Booking_Guests.Booking_ID_FK=@BookingID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingID", bookingId);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable guestsDt = new DataTable();
            adapter.Fill(guestsDt);

            listBox2.DataSource = guestsDt;
            listBox2.DisplayMember = "FullName";
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                RefreshApplication();
                return true; // Указываем, что обработали клавишу
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void RefreshApplication()
        {
            LoadClients();
            LoadRooms();
            LoadServices();
            LoadBookings();
            LoadHistory();
            // Обновление выделенных дат в monthCalendar
            if (comboBox1.SelectedValue != null)
            {
                int roomId = Convert.ToInt32(comboBox1.SelectedValue);
                HighlightOccupiedDates(roomId);
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
                Room_Booking.Booking_ID_PK, 
                Room_Booking.Client_ID_FK, 
                Clients.Last_Name || ' ' || SUBSTR(Clients.First_Name, 1, 1) || '.' || SUBSTR(Clients.Patronymic, 1, 1) || '.' AS Client_Name,
                Room_Booking.Room_ID_FK,
                Rooms.Room_Type,
                Room_Booking.Check_In_Date,
                Room_Booking.Check_Out_Date
            FROM 
                Room_Booking
            JOIN 
                Clients ON Room_Booking.Client_ID_FK = Clients.Client_ID_PK
            JOIN 
                Rooms ON Room_Booking.Room_ID_FK = Rooms.Room_ID_PK
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
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Close();
            loginForm.Show();
         
        }
    }
}
