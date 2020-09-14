using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.LectureHallTimeTable
{
    /// <summary>
    /// Interaction logic for LectureHallTimeTable.xaml
    /// </summary>
    public partial class LectureHallTimeTable : Window
    {
        private SQLiteConnection connection = db_config.connect();
        public LectureHallTimeTable()
        {
            InitializeComponent();
            fetchDataToComboBox();
        }

        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {

        }

        public void fetchDataToComboBox()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT * FROM Location";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_location.ItemsSource = ds.Tables[0].DefaultView;
                cmb_location.DisplayMemberPath = ds.Tables[0].Columns["BuildingName"].ToString();
                cmb_location.SelectedValuePath = ds.Tables[0].Columns["BuildingName"].ToString();


                cmb_hall_number.ItemsSource = ds.Tables[0].DefaultView;
                cmb_hall_number.DisplayMemberPath = ds.Tables[0].Columns["RoomNumber"].ToString();
                cmb_hall_number.SelectedValuePath = ds.Tables[0].Columns["RoomNumber"].ToString();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
