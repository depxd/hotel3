using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace hotel3
{
    public partial class GuestForm : Form
    {
        private SQLiteConnection conn;
        private long bookingId;
        private int selectedGuestId;

        public GuestForm(SQLiteConnection connection, long bookingId)
        {
            InitializeComponent();
            this.conn = connection;
            this.bookingId = bookingId;
            LoadGuests();
            LoadAttachedGuests();
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
        }
        private void LoadGuests()
        {
            string query = "SELECT Guest_ID_PK, Last_Name || ' ' || First_Name || ' ' || Patronymic AS FullName FROM Guests";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable guestsDt = new DataTable();
            adapter.Fill(guestsDt);

            listBox1.DataSource = guestsDt;
            listBox1.DisplayMember = "FullName";
            listBox1.ValueMember = "Guest_ID_PK";
        }
        private void LoadAttachedGuests()
        {
            string query = "SELECT Guests.Guest_ID_PK, Guests.Last_Name || ' ' || Guests.First_Name || ' ' || Guests.Patronymic AS FullName " +
                           "FROM Booking_Guests " +
                           "JOIN Guests ON Booking_Guests.Guest_ID_FK = Guests.Guest_ID_PK " +
                           "WHERE Booking_Guests.Booking_ID_FK = @BookingID";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingID", bookingId);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable attachedGuestsDt = new DataTable();
            adapter.Fill(attachedGuestsDt);

            listBox2.DataSource = attachedGuestsDt;
            listBox2.DisplayMember = "FullName";
            listBox2.ValueMember = "Guest_ID_PK";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Guests (Last_Name, First_Name, Patronymic) VALUES (@LastName, @FirstName, @Patronymic)";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
            cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadGuests();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                DataRowView selectedGuest = (DataRowView)listBox1.SelectedItem;
                int guestId = Convert.ToInt32(selectedGuest["Guest_ID_PK"]);

                string query = "DELETE FROM Guests WHERE Guest_ID_PK=@GuestID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@GuestID", guestId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadGuests();
                LoadAttachedGuests();  // Обновить список прикрепленных жильцов
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите жильца для удаления.");
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                DataRowView selectedGuest = (DataRowView)listBox1.SelectedItem;
                int guestId = Convert.ToInt32(selectedGuest["Guest_ID_PK"]);

                string query = "INSERT INTO Booking_Guests (Booking_ID_FK, Guest_ID_FK) VALUES (@BookingID, @GuestID)";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                cmd.Parameters.AddWithValue("@GuestID", guestId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadAttachedGuests();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите жильца для прикрепления.");
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                DataRowView selectedGuest = (DataRowView)listBox2.SelectedItem;
                int guestId = Convert.ToInt32(selectedGuest["Guest_ID_PK"]);

                string query = "DELETE FROM Booking_Guests WHERE Booking_ID_FK=@BookingID AND Guest_ID_FK=@GuestID";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                cmd.Parameters.AddWithValue("@GuestID", guestId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                LoadAttachedGuests();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите жильца для отвязки.");
            }

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
   
        }

        private void button5_Click(object sender, EventArgs e)
        {
         
        }
    }

}

