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
    /// 

    public class SessionDetails
    {
        private string sessionID;
        private string lecturers;
        private string subject;
        private string subjectCode;
        private string tag;
        private string groupID;
        private int studentCount;
        private int duration;
        private int isDeAllocated;
        private string roomID;


        public string SessionID { get; set; }
        public string Lecturers { get; set; }
        public string Subject { get; set; }
        public string SubjectCode { get; set; }
        public string Tag { get; set; }
        public string GroupID { get; set; }
        public string RoomID { get; set; }
        

        public int StudentCount { get; set; }
        public int Duration { get; set; }
        public int IsDeAlocated { get; set; }

    }


    public partial class LectureHallTimeTable : Window
    {
        private SQLiteConnection connection = db_config.connect();


        String[] CHECKING_DAYS = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        string[] timeSlots;
        string[] mondayTime;
        string[] tuesdayTime;
        string[] wednesdayTime;
        string[] thursdayTimme;
        string[] fridayTime;
        string[] saturdayTime;
        string[] sundayTime;
        string[] dayName;
        int noOfSessions = 0;
        int noOfDays = 0;
        string roomNumber;

        int loopbreak = 0;

        string[,] finalSlotArray;

        Boolean IsFullWeek = false;

        int rowCount;
        int columnCount;
        int totalSessionDuration = 0;
        int weekDaysCount = 0;
        int weekEndCount = 0;
        int availableTimeSlotCount = 0;

        List<SessionDetails> sessionDetails = new List<SessionDetails>();
        List<SessionDetails> sessionDetailsSecondTurnFiltering = new List<SessionDetails>();
        List<String> columnNames = new List<string>();

        //Testting
        List<string> ABCD = new List<string>();
        List<string> NEW_LIST = new List<string>();



        public LectureHallTimeTable()
        {
            loadFull();
            DatHeaddersaddingToTheTable();

            InitializeComponent();
            fetchDataToComboBox();
            btn_print_table.IsEnabled = false;
        }

        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {
            this.roomNumber = cmb_hall_number.SelectedValue.ToString();
            generateTable(this.roomNumber);
        }





        public void generateTable(string roomNumber)
        {
            //Session Details
            try
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where RoomID = '" + roomNumber + "'";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();


                dataAdapter.Fill(ds);

                this.noOfSessions = ds.Tables[0].Rows.Count;

                int i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    SessionDetails session = new SessionDetails();

                    session.SessionID = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    session.Lecturers = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    session.Subject = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                    session.SubjectCode = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                    session.Tag = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                    session.GroupID = ds.Tables[0].Rows[i].ItemArray[5].ToString();
                    session.StudentCount = int.Parse(ds.Tables[0].Rows[i].ItemArray[6].ToString());
                    session.Duration = int.Parse(ds.Tables[0].Rows[i].ItemArray[7].ToString());
                    session.IsDeAlocated = int.Parse(ds.Tables[0].Rows[i].ItemArray[8].ToString());


                    if (session.Duration == 2)
                    {
                        this.totalSessionDuration = this.totalSessionDuration + 2;
                    }

                    if (session.Duration == 1)
                    {
                        this.totalSessionDuration = this.totalSessionDuration + 1;
                    }

                    sessionDetails.Add(session);

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


        Random r = new Random();
        public int RandomFilter(int min, int max, int exclude)
        {

            //int rNumber = r.Next(min, max);
            int rNumber = 0;
            int temp = 0;
            do
            {
                rNumber = r.Next(min, max);
                temp = rNumber;
            } while (rNumber == 4);

            return rNumber;

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


                //cmb_hall_number.ItemsSource = ds.Tables[0].DefaultView;
                //cmb_hall_number.DisplayMemberPath = ds.Tables[0].Columns["RoomNumber"].ToString();
                //cmb_hall_number.SelectedValuePath = ds.Tables[0].Columns["RoomNumber"].ToString();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadRommNumbers(object sender, EventArgs e)
        {

            if(cmb_location.SelectedValue != null)
            {
                string locationName = cmb_location.SelectedValue.ToString();


                try
                {
                    DataTable dataTable = new DataTable();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;

                    command.CommandText = "SELECT * FROM Location WHERE BuildingName = '" + locationName + "';";

                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                    DataSet ds = new DataSet();

                    dataAdapter.Fill(ds);

                    //cmb_location.ItemsSource = ds.Tables[0].DefaultView;
                    //cmb_location.DisplayMemberPath = ds.Tables[0].Columns["BuildingName"].ToString();
                    //cmb_location.SelectedValuePath = ds.Tables[0].Columns["BuildingName"].ToString();


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

        public void loadFull()
        {
            //WeekDetails

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select * from working_week;";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                String DAY = "";

                dataAdapter.Fill(ds);

                this.noOfDays = ds.Tables[0].Rows.Count;
                this.dayName = new string[ds.Tables[0].Rows.Count];

                this.columnCount = ds.Tables[0].Rows.Count;

                if (this.columnCount == 0)
                {
                    MessageBox.Show("Before Proceed To Time Table Creation Please Initiate a Working Week!");
                    //this.Close();
                }
                else
                {
                    int i = 0;
                    for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {

                        DAY = ds.Tables[0].Rows[i].ItemArray[6].ToString();

                        System.Diagnostics.Debug.WriteLine("DAY " + DAY);

                        if (DAY == "Saturday" || DAY == "Sunday")
                        {
                            this.IsFullWeek = true;
                            this.weekEndCount = this.weekEndCount + 1;
                        }
                        else
                        {
                            this.IsFullWeek = false;
                            this.weekDaysCount = this.weekDaysCount + 1;
                        }

                        dayName[i] = DAY;
                        this.columnNames.Add(DAY);

                    }

                    System.Diagnostics.Debug.WriteLine("weekend count" + this.weekEndCount);
                    System.Diagnostics.Debug.WriteLine("weekday count" + this.weekDaysCount);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }



            //TimeSlots Details
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT * FROM time_slots;";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                String TIME = "";

                dataAdapter.Fill(ds);

                this.timeSlots = new string[ds.Tables[0].Rows.Count];
                this.mondayTime = new string[ds.Tables[0].Rows.Count];
                this.tuesdayTime = new string[ds.Tables[0].Rows.Count];
                this.wednesdayTime = new string[ds.Tables[0].Rows.Count];
                this.thursdayTimme = new string[ds.Tables[0].Rows.Count];
                this.fridayTime = new string[ds.Tables[0].Rows.Count];
                this.saturdayTime = new string[ds.Tables[0].Rows.Count];
                this.sundayTime = new string[ds.Tables[0].Rows.Count];

                this.rowCount = ds.Tables[0].Rows.Count;

                if (this.rowCount == 0)
                {
                    MessageBox.Show("Before Proceed To Time Table Creation Please Initiate Time Slots!");
                    //this.Close();
                }

                int i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {

                    TIME = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    timeSlots[i] = TIME;
                    mondayTime[i] = TIME;
                    tuesdayTime[i] = TIME;
                    wednesdayTime[i] = TIME;
                    thursdayTimme[i] = TIME;
                    fridayTime[i] = TIME;
                    saturdayTime[i] = TIME;
                    sundayTime[i] = TIME;

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

        public void DatHeaddersaddingToTheTable()
        {

            if (this.IsFullWeek)
            {
                System.Diagnostics.Debug.WriteLine("Full Week " + this.IsFullWeek);
                this.NEW_LIST.Add("Monday");
                this.NEW_LIST.Add("Tuesday");
                this.NEW_LIST.Add("Wednesday");
                this.NEW_LIST.Add("Thursday");
                this.NEW_LIST.Add("Friday");
                this.NEW_LIST.Add("Saturday");
                this.NEW_LIST.Add("Sunday");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Full Week " + this.IsFullWeek);
                this.NEW_LIST.Add("Monday");
                this.NEW_LIST.Add("Tuesday");
                this.NEW_LIST.Add("Wednesday");
                this.NEW_LIST.Add("Thursday");
                this.NEW_LIST.Add("Friday");
            }

        }

        private void btn_print_table_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
