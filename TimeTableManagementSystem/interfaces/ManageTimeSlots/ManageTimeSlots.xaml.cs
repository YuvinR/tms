using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
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

namespace TimeTableManagementSystem.interfaces.ManageTimeSlots
{
    /// <summary>
    /// Interaction logic for ManageTimeSlots.xaml
    /// </summary>
    public partial class ManageTimeSlots : Window
    {

        private SQLiteConnection connection = db_config.connect();
        private String startTime;
        private String endTime;
        private String timeDuration;

        

        public ManageTimeSlots()
        {
            InitializeComponent();
            populateGrid();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void getStartingTime(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem) cmbStartTime.SelectedItem;
            //MessageBox.Show(comboBoxItem.Content.ToString());
        }

        private void getEndingTime(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem) cmbEndTime.SelectedItem;
            String x = comboBoxItem.Content.ToString().Replace(":", "");
            //MessageBox.Show(x);
        }

        private void addTimeSlots(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int id = random.Next(1258);

            ComboBoxItem cmbEndTimeInput = (ComboBoxItem)cmbEndTime.SelectedItem;
            ComboBoxItem cmbStartTimeInput = (ComboBoxItem)cmbStartTime.SelectedItem;

            this.startTime = cmbStartTimeInput.Content.ToString();
            this.endTime = cmbEndTimeInput.Content.ToString();

            TimeSpan time = DateTime.Parse(this.endTime).Subtract(DateTime.Parse(this.startTime));

            this.timeDuration = time.ToString(@"hh\:mm");


            connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO time_slots (id,start_time,end_time,time_duration) VALUES(@id,@StartTime,@EndTime,@TimeDuration);";
                Console.WriteLine(command.CommandText);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@StartTime", this.startTime);
                command.Parameters.AddWithValue("@EndTime", this.endTime);
                command.Parameters.AddWithValue("@TimeDuration", this.timeDuration);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Data has been Inserted");
                }
                else
                {
                    MessageBox.Show("Error");
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

            populateGrid();


        }

        private void UpdateTimeSlots(object sender, RoutedEventArgs e)
        {

        }

        private void populateGrid()
        {
            connection.Open();
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM time_slots;";

                SQLiteDataAdapter sqliteAdapter = new SQLiteDataAdapter(command);

                sqliteAdapter.Fill(dataTable);

                manageTimeSlotGrid.ItemsSource = dataTable.DefaultView;

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
