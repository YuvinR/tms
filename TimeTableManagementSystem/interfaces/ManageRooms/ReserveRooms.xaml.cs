using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.ManageRooms
{
    /// <summary>
    /// Interaction logic for ReserveRooms.xaml
    /// </summary>
    public partial class ReserveRooms : Window
    {
        private string rName;
        private string rDate;
        private string sTime;
        private string eTime;
        private SQLiteConnection connection = db_config.connect();

        public ReserveRooms()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadComboBox();
        }

        private void BtnReserveRoom_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(SearchRoomCombo.Text) || String.IsNullOrEmpty(resDate.Text) || String.IsNullOrEmpty(startTime.Text) || String.IsNullOrEmpty(endTime.Text))

            {
                MessageBox.Show("Please fill in all the fields before proceeding!");
            }
            else
            {

                rName = SearchRoomCombo.Text;
                rDate= resDate.Text;
                sTime = startTime.Text;
                eTime = endTime.Text;

                connection.Open();

                try
                {
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "insert into Room_Reserve" +
                            "(RoomID, ResDate, STime, ETime)" +
                            " Values" +
                            "(@RoomID, @ResDate, @STime, @ETime)";

                    cmd.Parameters.AddWithValue("@RoomID", rName);
                    cmd.Parameters.AddWithValue("@ResDate", rDate);
                    cmd.Parameters.AddWithValue("@STime", sTime);
                    cmd.Parameters.AddWithValue("@ETime", eTime);


                    int rows = cmd.ExecuteNonQuery();


                    if (rows > 0)
                    {
                        MessageBox.Show("Data has been Inserted Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Error Occured");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                clearFields();
            }
        }

        private void clearFields()
        {
            SearchRoomCombo.Text = "";
            resDate.Text = "";
            startTime.Text = "";
            endTime.Text = "";
        }

        private void loadComboBox()
        {
            //populate rooms combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Location";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchRoomCombo.Items.Add(reader1.GetString("RoomNumber"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


    }
}
