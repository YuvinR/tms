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

        private int error_count = 0;
        

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

            String sdsd = (this.timeDuration).ToString();
            var xds = sdsd.Length;

            if (startingTime != endingTime  && xds == 8) 
            { 


                connection.Open();

                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO time_slots (start_time,end_time,time_duration) VALUES(@StartTime,@EndTime,@TimeDuration);";
                    Console.WriteLine(command.CommandText);

                    //command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@StartTime", this.startTime);
                    command.Parameters.AddWithValue("@EndTime", this.endTime);
                    command.Parameters.AddWithValue("@TimeDuration", this.timeDuration);

                    this.error_count = command.ExecuteNonQuery();

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
                clearFields();

                if (this.error_count > 0)
                {
                    MessageBox.Show("Data has been Inserted");

                }
                else
                {
                    MessageBox.Show("Error");
                }

                this.error_count = 0;

            }
            else
            {
                MessageBox.Show("Starting Time Or Ending time not in correct format, Please Check!");
            }

        }

        private void UpdateTimeSlots(object sender, RoutedEventArgs e)
        {

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

            String sdsd = (this.timeDuration).ToString();
            var xds = sdsd.Length;

            if (startingTime != endingTime && xds == 8)
            {

                connection.Open();

                try
                {

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;

                    command.CommandText = "UPDATE time_slots SET start_time = @StartTime, end_time = @EndTime, time_duration = @TimeDuration WHERE id = @RecordID;";

                    command.Parameters.AddWithValue("@RecordID", this.recordID);
                    command.Parameters.AddWithValue("@StartTime", this.startTime);
                    command.Parameters.AddWithValue("@EndTime", this.endTime);
                    command.Parameters.AddWithValue("@TimeDuration", this.timeDuration);


                    this.error_count = command.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    connection.Close();
                }

                populateGrid();
                clearFields();

                btnUpdate.IsEnabled = false;
                btnAdd.IsEnabled = true;

                if (this.error_count > 0)
                {
                    MessageBox.Show("Record has been Updated");
                }
                else
                {
                    MessageBox.Show("Error Occured");
                }

            }
            else
            {
                MessageBox.Show("Starting Time Or Ending time not in correct format, Please Check!");
            }

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

            clearFields();

            string startTime;
            string endTime;

            DataRowView selectedRow = manageTimeSlotGrid.SelectedItem as DataRowView;

            startTime = selectedRow.Row["start_time"].ToString();
            endTime = selectedRow.Row["end_time"].ToString();
            this.recordID = selectedRow.Row["id"].ToString();

            TimePickerStartingTime.SelectedTime = Convert.ToDateTime(startTime);
            TimePickerEndingTime.SelectedTime = Convert.ToDateTime(endTime);

            //filterTimeWhenEdit(startTime, endTime);

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

                this.error_count = command.ExecuteNonQuery();


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

            if (this.error_count > 0)
            {
                MessageBox.Show("Data has been Deleted!");
            }
            else
            {
                MessageBox.Show("Error Occured");
            }

            this.error_count = 0;

        }



        public void clearFields()
        {
            TimePickerEndingTime.SelectedTime = null;
            TimePickerStartingTime.SelectedTime = null;
        }



    }
}
