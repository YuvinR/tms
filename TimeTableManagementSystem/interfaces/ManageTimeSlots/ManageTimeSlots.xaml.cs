using MaterialDesignThemes.Wpf;
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
        private String recordID;

        

        public ManageTimeSlots()
        {
            InitializeComponent();
            initalLoad();
        }




        private void addTimeSlots(object sender, RoutedEventArgs e)
        {

            Random random = new Random();
            int id = random.Next(1258);

            TimePicker StartingTime = (TimePicker)TimePickerStartingTime;
            TimePicker EndingTime = (TimePicker)TimePickerEndingTime;

            DateTime startingTime = new DateTime();
            DateTime endingTime = new DateTime();


            if (StartingTime.SelectedTime != null)
            {
                startingTime = (DateTime)StartingTime.SelectedTime;
            }

            if (EndingTime.SelectedTime != null)
            {
                endingTime = (DateTime)EndingTime.SelectedTime;
            }

            this.startTime = startingTime.TimeOfDay.ToString();
            this.endTime = endingTime.TimeOfDay.ToString();
            this.timeDuration = (endingTime - startingTime).ToString();


            // cmbEndTimeInput = (ComboBoxItem)cmbEndTime.SelectedItem;
            //ComboBoxItem cmbStartTimeInput = (ComboBoxItem)cmbStartTime.SelectedItem;

            //this.startTime = cmbStartTimeInput.Content.ToString();
            //this.endTime = cmbEndTimeInput.Content.ToString();

            //TimeSpan time = DateTime.Parse(this.endTime).Subtract(DateTime.Parse(this.startTime));

            //this.timeDuration = time.ToString(@"hh\:mm");
            


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

                command.CommandText = "UPDATE time_slots SET start_time = @StartTime, end_time = @EndTime, time_duration = @TimeDuration WHERE id = @RecordID;";

                command.Parameters.AddWithValue("@RecordID", this.recordID);
                command.Parameters.AddWithValue("@StartTime", this.startTime );
                command.Parameters.AddWithValue("@EndTime", this.endTime);
                command.Parameters.AddWithValue("@TimeDuration", this.timeDuration);


                int rows = command.ExecuteNonQuery();


                if (rows > 0)
                {
                    MessageBox.Show("Record has been Updated");
                }
                else
                {
                    MessageBox.Show("Error Occured");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                connection.Close();
            }
            cmbStartTime.SelectedIndex = -1;
            cmbEndTime.SelectedIndex = -1;
            populateGrid();
            btnUpdate.IsEnabled = false;
            btnAdd.IsEnabled = true;
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



        private void initalLoad()
        {
            populateGrid();
            btnUpdate.IsEnabled = false;
        }

        private void editFields(object sender, RoutedEventArgs e)
        {
            string startTime;
            string endTime;

            DataRowView selectedRow = manageTimeSlotGrid.SelectedItem as DataRowView;

            startTime = selectedRow.Row["start_time"].ToString();
            endTime = selectedRow.Row["end_time"].ToString();
            this.recordID = selectedRow.Row["id"].ToString();

            filterTimeWhenEdit(startTime, endTime);

            btnUpdate.IsEnabled = true;
            btnAdd.IsEnabled = false;
        }

        private void deleteRecord(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = manageTimeSlotGrid.SelectedItem as DataRowView;
            string recordID = selectedRow.Row["id"].ToString();

            connection.Open();
            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM time_slots WHERE id = @RecordID";
                command.Parameters.AddWithValue("@RecordID", recordID);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Data has been Deleted!");
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

            populateGrid();

        }





        //Filtering method to time
        public void filterTimeWhenEdit(string StartTime, string EndTime)
        {

            string startTimeObj = StartTime;
            string endTimeObj = EndTime;

            string[] cmbTimes = { "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00",
                                  "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00",
                                  "18:30", "19:00", "19:30", "20:00", "20:30" };

            for (int i = 0; i < cmbTimes.Length; i++)
            {
                if (startTimeObj.ToString() == cmbTimes[i].ToString())
                {
                    cmbStartTime.SelectedIndex = i;
                }

                if (endTimeObj.ToString() == cmbTimes[i].ToString())
                {
                    cmbEndTime.SelectedIndex = i;
                }

            }
        }

    }
}
