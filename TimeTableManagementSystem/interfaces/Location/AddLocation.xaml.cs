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

namespace TimeTableManagementSystem.interfaces.Location
{
    /// <summary>
    /// Interaction logic for AddLocation.xaml
    /// </summary>
    public partial class AddLocation : Window
    {
        private SQLiteConnection connection = db_config.connect();

        private string bName;
        private string rType;
        private string roomNo;
        private int rCapacity;



        public AddLocation()
        {
            InitializeComponent();
        }

        private void BtnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(buildnameTxt.Text) && String.IsNullOrEmpty(roomtypComboBox.Text) && String.IsNullOrEmpty(roomnoTxt.Text) && String.IsNullOrEmpty(capacityTxt.Text))
            
            {
                MessageBox.Show("Please fill in all the fields before proceeding!");
            }
            else
            {

                bName = buildnameTxt.Text;
                rType = roomtypComboBox.Text;
                roomNo = roomnoTxt.Text;

                rCapacity = int.Parse(capacityTxt.Text);

                connection.Open();

                try
                {
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "insert into Location" +
                            "(BuildingName, RoomType, RoomNumber, RoomCapacity)" +
                            " Values" +
                            "(@BuildingName, @RoomType, @RoomNumber, @RoomCapacity)";

                    cmd.Parameters.AddWithValue("@BuildingName", bName);
                    cmd.Parameters.AddWithValue("@RoomType", rType);
                    cmd.Parameters.AddWithValue("@RoomNumber", roomNo);
                    cmd.Parameters.AddWithValue("@RoomCapacity", rCapacity);


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
            buildnameTxt.Text = "";
            roomtypComboBox.Text = "";
            roomnoTxt.Text = "";
            capacityTxt.Text = "";
        }
    }
}
