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

namespace TimeTableManagementSystem.interfaces.ManageWorkingDaysAndHours
{
    /// <summary>
    /// Interaction logic for ManageWorkignDaysAndHours.xaml
    /// </summary>
    public partial class ManageWorkignDaysAndHours : Window
    {
        private SQLiteConnection connection = db_config.connect();

        String weekType;
        int NoOfWorkingDays;
        int WorkingHours = 0;
        int WorkingMinutes = 0;
        List<string> workingDays;
        String workingTimePerDay;
        String startTime;
        String endTime;

        public ManageWorkignDaysAndHours()
        {
            InitializeComponent();
            initialLoad();
        }

        private void toggleWeekday(object sender, RoutedEventArgs e)
        {
            EnableWeekdays();
        }

        private void toggleWeekEnd(object sender, RoutedEventArgs e)
        {
            EnableWeekEnds();
        }


        private void disableWeekend(object sender, RoutedEventArgs e)
        {
            DisableWeekend();
        }

        private void disableWeekday(object sender, RoutedEventArgs e)
        {
            DisableWeekday();
        }


        public void initialLoad()
        {
            cmbMonday.IsEnabled = false;
            cmbTuesday.IsEnabled = false;
            cmbWednesday.IsEnabled = false;
            cmbThursday.IsEnabled = false;
            cmbFriday.IsEnabled = false;
            cmbSaturday.IsEnabled = false;
            cmbSunday.IsEnabled = false;
        }

        public void EnableWeekdays()
        {
            cmbMonday.IsEnabled = true;
            cmbTuesday.IsEnabled = true;
            cmbWednesday.IsEnabled = true;
            cmbThursday.IsEnabled = true;
            cmbFriday.IsEnabled = true;
        }

        public void EnableWeekEnds()
        {
            cmbSunday.IsEnabled = true;
            cmbSaturday.IsEnabled = true;
        }

        public void DisableWeekend()
        {
            cmbSaturday.IsEnabled = false;
            cmbSunday.IsEnabled = false;
        }

        public void DisableWeekday()
        {
            cmbMonday.IsEnabled = false;
            cmbTuesday.IsEnabled = false;
            cmbWednesday.IsEnabled = false;
            cmbThursday.IsEnabled = false;
            cmbFriday.IsEnabled = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.workingDays = new List<string>();
            ComboBoxItem cmbInpuTStartTime = (ComboBoxItem)cmbStartTime.SelectedItem;
            ComboBoxItem cmbInputEndTime = (ComboBoxItem)cmbEndTime.SelectedItem;
            ComboBoxItem cmbInputWorkingHours = (ComboBoxItem)cmbHours.SelectedItem;
            ComboBoxItem cmbInputWorkingMinutes = (ComboBoxItem)cmbMin.SelectedItem;
            ComboBoxItem cmbInputNoOfWorkingDays = (ComboBoxItem)cmbWorkingDayNumber.SelectedItem;

            workingDaysFilter();

            this.WorkingHours = int.Parse(cmbInputWorkingHours.Content.ToString());
            this.WorkingMinutes = int.Parse(cmbInputWorkingMinutes.Content.ToString());
            this.NoOfWorkingDays = int.Parse(cmbInputNoOfWorkingDays.Content.ToString());
            this.startTime = cmbInpuTStartTime.Content.ToString();
            this.endTime = cmbInputEndTime.Content.ToString();

            Random random = new Random();
            int id = random.Next(1258);





            foreach (var day in this.workingDays)
            {
                connection.Open();

                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO working_days_hours (week_id, type, no_working_days, working_hours, working_min, start_time, end_time, days) " +
                                                                    "VALUES(@WEEKID,@TYPE,@NO_WORKING_DAYS,@WORKING_HOURS,@WORKING_MIN,@START_TIME,@END_TIME,@DAYS);";
                    Console.WriteLine(command.CommandText);

                    command.Parameters.AddWithValue("@WEEKID", id);
                    command.Parameters.AddWithValue("@TYPE", this.weekType.ToString());
                    command.Parameters.AddWithValue("@NO_WORKING_DAYS", this.NoOfWorkingDays);
                    command.Parameters.AddWithValue("@WORKING_HOURS", this.WorkingHours);
                    command.Parameters.AddWithValue("@WORKING_MIN", this.WorkingMinutes);
                    command.Parameters.AddWithValue("@START_TIME", this.startTime);
                    command.Parameters.AddWithValue("@END_TIME", this.endTime);
                    command.Parameters.AddWithValue("@DAYS", day);


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
            }

        }




        public void workingDaysFilter()
        {
            if (cmbWeekday.IsChecked == true)
            {
                this.weekType = "weekday";
            }
            else if (cmbWeekend.IsChecked == true)
            {
                this.weekType = "weekend";
            }
            else if (cmbWeekday.IsChecked == true && cmbWeekend.IsChecked == true)
            {
                this.weekType = "full";
            }
            else if (cmbWeekday.IsChecked == false && cmbWeekend.IsChecked == false)
            {
                this.weekType = "";
                MessageBox.Show("Please Select the Type");
            }

            if (cmbMonday.IsChecked == true)
            {
                this.workingDays.Add("Monday");
            }


            if (cmbTuesday.IsChecked == true)
            {
                this.workingDays.Add("Tuesday");
            }

            if (cmbWednesday.IsChecked == true)
            {
                this.workingDays.Add("Wednesday");
            }

            if (cmbThursday.IsChecked == true)
            {
                this.workingDays.Add("Thursday");
            }

            if (cmbFriday.IsChecked == true)
            {
                this.workingDays.Add("Friday");
            }

            if (cmbSaturday.IsChecked == true)
            {
                this.workingDays.Add("Saturday");
            }

            if (cmbSunday.IsChecked == true)
            {
                this.workingDays.Add("Sunday");
            }
        }
    }
}
