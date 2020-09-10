using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.ManageWorkingDaysAndHours
{
    /// <summary>
    /// Interaction logic for ManageWorkignDaysAndHours.xaml
    /// </summary>
    /// 


    public partial class ManageWorkignDaysAndHours : Window
    {
        private SQLiteConnection connection = db_config.connect();

        String weekType;
        int NoOfWorkingDays;
        int day_count = 0;
        int errorCount = 0;
        int checkBoxCounter = 0;

        Boolean editButtonPerformed = false;
        Boolean cancelButtonPerformed = false;

        int weekID;

        
        List<Chip> chipList;
        List<CheckBox> checkBoxList;

        String workingTimePerDay;
        String startTime;
        String endTime;
        String[] CHECKING_DAYS = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        int WEEK_ID = 0;
        String WEEK_TYPE = "";
        int NO_OF_WORKING_DAYS = 0;
        DateTime PER_DAY_TIME;
        DateTime STARATING_TIME;
        DateTime ENDING_TIME;
        List<string> workingDays;


        public ManageWorkignDaysAndHours()
        {
            InitializeComponent();
            initialLoad();
            fetchDataFromDB();
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
            ComboBoxItem cmbInputNoOfWorkingDays = (ComboBoxItem)cmbWorkingDayNumber.SelectedItem;
            workingDaysFilter();

            if(cmbInputNoOfWorkingDays != null)
            {
                this.NoOfWorkingDays = int.Parse(cmbInputNoOfWorkingDays.Content.ToString());
            }
            TimePicker StartingTime = (TimePicker)timeStartingTime;
            TimePicker EndingTime = (TimePicker)timeEndingTime;


            DateTime startingTime = new DateTime();
            DateTime endingTime = new DateTime();

            if(StartingTime.SelectedTime != null)
            {
                startingTime = (DateTime)StartingTime.SelectedTime;
            }

            if (EndingTime.SelectedTime != null)
            {
                endingTime = (DateTime)EndingTime.SelectedTime;
            }



            this.workingTimePerDay = (endingTime - startingTime).ToString();
            this.startTime = startingTime.TimeOfDay.ToString();
            this.endTime = endingTime.TimeOfDay.ToString();

            Random random = new Random();
            this.weekID = random.Next(2344);

            String sdsd = (endingTime - startingTime).ToString();
            var xds = sdsd.Length;

            if (startingTime != endingTime && this.NoOfWorkingDays > 0 && xds == 8)
            {

                connection.Open();

                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM working_week;";
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }




                foreach (var day in this.workingDays)
                {
                    connection.Open();

                    try
                    {
                        SQLiteCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT INTO working_week (week_id,week_type,no_of_days,per_day_time,starting_time,ending_time,day_name) " +
                            "VALUES(@WEEK_ID, @WEEk_TYPE,@NO_OF_DAYS,  @PER_DAY_TIME, @STARTING_TIME, @ENDING_TIME, @DAY_NAME);";
                        Console.WriteLine(command.CommandText);

                        command.Parameters.AddWithValue("@WEEK_ID", this.weekID);
                        command.Parameters.AddWithValue("@WEEk_TYPE", this.weekType);
                        command.Parameters.AddWithValue("@NO_OF_DAYS", this.NoOfWorkingDays);
                        command.Parameters.AddWithValue("@PER_DAY_TIME", this.workingTimePerDay);
                        command.Parameters.AddWithValue("@STARTING_TIME", this.startTime);
                        command.Parameters.AddWithValue("@ENDING_TIME", this.endTime);
                        command.Parameters.AddWithValue("@DAY_NAME", day);

                        this.errorCount = command.ExecuteNonQuery();

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

                if (this.errorCount > 0)
                {
                    clearFields();
                    initialLoad();
                    fetchDataFromDB();
                    MessageBox.Show("Data has been Inserted");
                }
                else
                {
                    MessageBox.Show("Error");
                }

            }
            else
            {
                MessageBox.Show("Please Check your details before submit");
            }
        }


        public void fetchDataFromDB()
        {

            this.chipList = new List<Chip>();
            this.chipList.Add(chip_monday);
            this.chipList.Add(chip_tuesday);
            this.chipList.Add(chip_wednesday);
            this.chipList.Add(chip_thursday);
            this.chipList.Add(chip_friday);
            this.chipList.Add(chip_saturday);
            this.chipList.Add(chip_sunday);

            for (int x = 0; x < this.chipList.Count; x++)
            {
                this.chipList[x].Opacity = 0.1;
                this.chipList[x].IsEnabled = true;
            }
            List<string> workingDaysNew = new List<string>();

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select * from working_week;";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);
                int i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    String WEEK_ID = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    String WEEK_TYPE = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    String NO_OF_WORKING_DAYS = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                    String PER_DAY_TIME = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                    String STARTING_TIME = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                    String ENDING_TIME = ds.Tables[0].Rows[i].ItemArray[5].ToString();
                    String DAYS = ds.Tables[0].Rows[i].ItemArray[6].ToString();

                    //timeStartingTime.SelectedTime = Convert.ToDateTime(STARTING_TIME);

                    workingDaysNew.Add(DAYS);
                    chip_working_time_per_day.Content = PER_DAY_TIME;
                    chip_no_of_working_days.Content = NO_OF_WORKING_DAYS;

                }


                int iew = 0;
                for (iew = 0; iew < this.CHECKING_DAYS.Length; iew++)
                {
                    System.Diagnostics.Debug.WriteLine("Days " + this.CHECKING_DAYS[iew]);
                }


                foreach (var temp in workingDaysNew)
                {
                    
                    for (int ie = 0; ie < this.CHECKING_DAYS.Length; ie++)
                    {
                        if(temp.Equals(CHECKING_DAYS[ie].ToString()))
                        {

                            System.Diagnostics.Debug.WriteLine("ie " + ie);
                            this.chipList[ie].Opacity = 1.0;
                        }
                    }
                }

                if(workingDaysNew.Count != 0)
                {
                    enableViewPanel();
                }
                else
                {
                    disableViewPanel();
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void workingDaysFilter()
        {
            if (cmbWeekday.IsChecked == true && cmbWeekend.IsChecked == true)
            {
                this.weekType = "full";
            }
            else if (cmbWeekday.IsChecked == true)
            {
                this.weekType = "weekday";
            }
            else if (cmbWeekend.IsChecked == true)
            {
                this.weekType = "weekend";
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {



            this.workingDays = new List<string>();
            ComboBoxItem cmbInputNoOfWorkingDays = (ComboBoxItem)cmbWorkingDayNumber.SelectedItem;
            workingDaysFilter();

            if (cmbInputNoOfWorkingDays != null)
            {
                this.NoOfWorkingDays = int.Parse(cmbInputNoOfWorkingDays.Content.ToString());
            }


            TimePicker StartingTime = (TimePicker)timeStartingTime;
            TimePicker EndingTime = (TimePicker)timeEndingTime;


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


            this.workingTimePerDay = (endingTime - startingTime).ToString();
            this.startTime = startingTime.TimeOfDay.ToString();
            this.endTime = endingTime.TimeOfDay.ToString();

            Random random = new Random();
            this.weekID = random.Next(2344);

            String sdsd = (endingTime - startingTime).ToString();
            var xds = sdsd.Length;

            if (startingTime != endingTime && this.NoOfWorkingDays > 0 && xds == 8)
            {

                connection.Open();

                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM working_week;";
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }


                foreach (var day in this.workingDays)
                {
                    connection.Open();

                    try
                    {
                        SQLiteCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT INTO working_week (week_id,week_type,no_of_days,per_day_time,starting_time,ending_time,day_name) " +
                            "VALUES(@WEEK_ID, @WEEk_TYPE,@NO_OF_DAYS,  @PER_DAY_TIME, @STARTING_TIME, @ENDING_TIME, @DAY_NAME);";
                        Console.WriteLine(command.CommandText);

                        command.Parameters.AddWithValue("@WEEK_ID", this.weekID);
                        command.Parameters.AddWithValue("@WEEk_TYPE", this.weekType);
                        command.Parameters.AddWithValue("@NO_OF_DAYS", this.NoOfWorkingDays);
                        command.Parameters.AddWithValue("@PER_DAY_TIME", this.workingTimePerDay);
                        command.Parameters.AddWithValue("@STARTING_TIME", this.startTime);
                        command.Parameters.AddWithValue("@ENDING_TIME", this.endTime);
                        command.Parameters.AddWithValue("@DAY_NAME", day);

                        this.errorCount = command.ExecuteNonQuery();

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

                if (this.errorCount > 0)
                {
                    clearFields();
                    initialLoad();
                    fetchDataFromDB();
                    MessageBox.Show("Data has been Updated");
                }
                else
                {
                    MessageBox.Show("Error");
                }

                btnUpdate.IsEnabled = false;
                btnAdd.IsEnabled = true;

            }
            else
            {
                MessageBox.Show("Please Check your details before submit");
            }

        }

        private void testMethod(object sender, DependencyPropertyChangedEventArgs e)
        {

            MessageBox.Show("sds");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM working_week;";
                this.errorCount = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            if (this.errorCount > 0)
            {
                chip_no_of_working_days.Content = "";
                chip_working_time_per_day.Content = "";
                fetchDataFromDB();
                MessageBox.Show("Data has been Removed");
            }
            else
            {
                MessageBox.Show("Error");
            }

        }

        public void disableViewPanel()
        {
            btnDelete.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnAdd.IsEnabled = true;

            lbl_days.Opacity = 0.1;
            lbl_time_per_day.Opacity = 0.1;
            lbl_no_working.Opacity = 0.1;

            chip_no_of_working_days.Opacity = 0.1;
            chip_no_of_working_days.IsEnabled = false;


            chip_working_time_per_day.Opacity = 0.1;
            chip_working_time_per_day.IsEnabled = false;

        }

        public void enableViewPanel()
        {
            btnDelete.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnAdd.IsEnabled = false;

            chip_no_of_working_days.Opacity = 1.0;
            chip_no_of_working_days.IsEnabled = false;

            lbl_days.Opacity = 1.0;
            lbl_time_per_day.Opacity = 1.0;
            lbl_no_working.Opacity = 1.0;

            chip_working_time_per_day.Opacity = 1.0;
            chip_working_time_per_day.IsEnabled = false;
        }

        public void clearFields()
        {
            this.cancelButtonPerformed = true;
            cmbWeekday.IsChecked = false;
            cmbWeekend.IsChecked = false;
            cmbWorkingDayNumber.SelectedIndex = -1;

            cmbMonday.IsChecked = false;
            cmbTuesday.IsChecked = false;
            cmbWednesday.IsChecked = false;
            cmbThursday.IsChecked = false;
            cmbFriday.IsChecked = false;
            cmbSaturday.IsChecked = false;
            cmbSunday.IsChecked = false;

            timeStartingTime.SelectedTime = null;
            timeEndingTime.SelectedTime = null;
            this.cancelButtonPerformed = false;
        }

        private void checkedTotalCounter(object sender, RoutedEventArgs e)
        {
            if(this.editButtonPerformed != true)
            {
                this.checkBoxCounter = checkBoxCounter + 1;
                System.Diagnostics.Debug.WriteLine("clicked " + this.checkBoxCounter);

                if (checkBoxCounter > day_count)
                {
                    MessageBox.Show("Your Maximum day count is " + day_count + ". Please select correct no of days");
                    cmbMonday.IsChecked = false;
                    cmbTuesday.IsChecked = false;
                    cmbWednesday.IsChecked = false;
                    cmbThursday.IsChecked = false;
                    cmbFriday.IsChecked = false;
                    cmbSaturday.IsChecked = false;
                    cmbSunday.IsChecked = false;
                }
            }
        }

        private void reduceCheckedCounter(object sender, RoutedEventArgs e)
        {
            if (this.editButtonPerformed != true)
            {
                this.checkBoxCounter = checkBoxCounter - 1;
                System.Diagnostics.Debug.WriteLine("clicked " + this.checkBoxCounter);
            }
        }

        private void dontKnow(object sender, SelectionChangedEventArgs e)
        {
            if (this.editButtonPerformed != true)
            {
                ComboBoxItem cmbInputNoOfWorkingDays1 = (ComboBoxItem)cmbWorkingDayNumber.SelectedItem;
                if (cmbInputNoOfWorkingDays1 != null)
                {
                    this.day_count = int.Parse(cmbInputNoOfWorkingDays1.Content.ToString());
                }


                if (checkBoxCounter > day_count)
                {
                    MessageBox.Show("Your Maximum day count is " + day_count + ". Please select correct no of days");
                    cmbMonday.IsChecked = false;
                    cmbTuesday.IsChecked = false;
                    cmbWednesday.IsChecked = false;
                    cmbThursday.IsChecked = false;
                    cmbFriday.IsChecked = false;
                    cmbSaturday.IsChecked = false;
                    cmbSunday.IsChecked = false;
                }

                System.Diagnostics.Debug.WriteLine("count " + this.day_count);
            }
        }

        public void fillFormDetails()
        {

            this.checkBoxList = new List<CheckBox>();
            this.checkBoxList.Add(cmbMonday);
            this.checkBoxList.Add(cmbTuesday);
            this.checkBoxList.Add(cmbWednesday);
            this.checkBoxList.Add(cmbThursday);
            this.checkBoxList.Add(cmbFriday);
            this.checkBoxList.Add(cmbSaturday);
            this.checkBoxList.Add(cmbSunday);

            List<string> workingDaysNew = new List<string>();

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select * from working_week;";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                String WEEK_ID = "";
                String WEEK_TYPE = "";
                String NO_OF_WORKING_DAYS = "";
                String PER_DAY_TIME = "";
                String STARTING_TIME = "";
                String ENDING_TIME = "";
                String DAYS = "";

                dataAdapter.Fill(ds);
                int i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    WEEK_ID = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    WEEK_TYPE = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    NO_OF_WORKING_DAYS = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                    PER_DAY_TIME = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                    STARTING_TIME = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                    ENDING_TIME = ds.Tables[0].Rows[i].ItemArray[5].ToString();
                    DAYS = ds.Tables[0].Rows[i].ItemArray[6].ToString();

                    workingDaysNew.Add(DAYS);


                }

                this.day_count = int.Parse(NO_OF_WORKING_DAYS);
                this.checkBoxCounter = int.Parse(NO_OF_WORKING_DAYS);
                cmbWorkingDayNumber.SelectedItem = int.Parse(NO_OF_WORKING_DAYS);
                timeStartingTime.SelectedTime = Convert.ToDateTime(STARTING_TIME);
                timeEndingTime.SelectedTime = Convert.ToDateTime(ENDING_TIME);

                if(WEEK_TYPE == "full")
                {
                    cmbWeekday.IsChecked = true;
                    cmbWeekend.IsChecked = true;
                }
                else if(WEEK_TYPE == "weekday")
                {
                    cmbWeekday.IsChecked = true;
                }
                else
                {
                    cmbWeekend.IsChecked = true;
                }

                for(int ix = 0; ix<7; ix++)
                {
                    if( ix == (this.day_count - 1))
                    {
                        cmbWorkingDayNumber.SelectedIndex = ix;
                    }
                }


                foreach (var temp in workingDaysNew)
                {

                    for (int ie = 0; ie < this.CHECKING_DAYS.Length; ie++)
                    {
                        if (temp.Equals(CHECKING_DAYS[ie].ToString()))
                        {

                            System.Diagnostics.Debug.WriteLine("ie " + ie);
                            this.checkBoxList[ie].IsChecked = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            this.editButtonPerformed = false;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            this.editButtonPerformed = true;
            btnUpdate.IsEnabled = true;
            btnAdd.IsEnabled = false;
            fillFormDetails();
        }

        private void timeValidator(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {

            DateTime startingTime = new DateTime();
            DateTime endingTime = new DateTime();

            if (timeStartingTime.SelectedTime != null)
            {
                startingTime = (DateTime)timeStartingTime.SelectedTime;
            }


            if (timeEndingTime.SelectedTime != null)
            {
                endingTime = (DateTime)timeEndingTime.SelectedTime;
            }

            if(cancelButtonPerformed == false)
            {
                if (startingTime == endingTime)
                {
                    MessageBox.Show("Selected Times are same, Can not calculate the working hours");
                    //timeStartingTime.SelectedTime = null;
                    //timeEndingTime.SelectedTime = null;
                }

                String sdsd = (endingTime - startingTime).ToString();
                var xds = sdsd.Length;

                if (xds == 9)
                {
                    MessageBox.Show("Wrong Time Selection, Please Check");
                    //timeStartingTime.SelectedTime = null;
                    //timeEndingTime.SelectedTime = null;
                }
            }

            
 
        }
    }
}
