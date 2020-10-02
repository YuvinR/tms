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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IO;

using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.Net.Security;
using Microsoft.Win32;
using System.Runtime.CompilerServices;

namespace TimeTableManagementSystem.interfaces.StudentTimeTable
{
    /// <summary>
    /// Interaction logic for StudentTimeTable.xaml
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
        private string nonOverLapSessions;
        private string concecativeSessions;
        private string parallelSessions;
        private string roomID;
        private string mainGroupID;


        public string SessionID { get; set; }
        public string Lecturers { get; set; }
        public string Subject { get; set; }
        public string SubjectCode { get; set; }
        public string Tag { get; set; }
        public string GroupID { get; set; }
        public int StudentCount { get; set; }
        public int Duration { get; set; }
        public int IsDeAlocated { get; set; }
        public string NonOverLapSessions { get; set; }
        public string ConcecativeSessions { get; set; }
        public string ParallelSessions { get; set; }
        public string RoomID { get; set; }
        public string MaingGroupID { get; set; }

    }

    public partial class StudentTimeTable : Window
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
        string groupNumber;

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

        public StudentTimeTable()
        {
            InitializeComponent();
            fetchDataToComboBox();
            btn_print_table.IsEnabled = false;
        }

        public void cleanVariables()
        {
            this.noOfSessions = 0;
            this.noOfDays = 0;
            this.groupNumber = "";

            this.loopbreak = 0;

            this.IsFullWeek = false;

            this.rowCount = 0;
            this.columnCount = 0;
            this.totalSessionDuration = 0;
            this.weekDaysCount = 0;
            this.weekEndCount = 0;
            this.availableTimeSlotCount = 0;

            this.ABCD.Clear();
            this.sessionDetails.Clear();
            this.NEW_LIST.Clear();
            this.columnNames.Clear();
            this.sessionDetailsSecondTurnFiltering.Clear();

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

        public void fetchDataToComboBox()
        {
            //Degree Program Name
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Program";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_degree_name.ItemsSource = ds.Tables[0].DefaultView;
                cmb_degree_name.DisplayMemberPath = ds.Tables[0].Columns["programName"].ToString();
                cmb_degree_name.SelectedValuePath = ds.Tables[0].Columns["programName"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            //Group Numbers

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select DISTINCT g.groupNo from Groups g";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_group.ItemsSource = ds.Tables[0].DefaultView;
                cmb_group.DisplayMemberPath = ds.Tables[0].Columns["groupNo"].ToString();
                cmb_group.SelectedValuePath = ds.Tables[0].Columns["groupNo"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }



            //SubGrup Numbers

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select DISTINCT s.subgroupNo from SubGroup s";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_sub_group.ItemsSource = ds.Tables[0].DefaultView;
                cmb_sub_group.DisplayMemberPath = ds.Tables[0].Columns["subgroupNo"].ToString();
                cmb_sub_group.SelectedValuePath = ds.Tables[0].Columns["subgroupNo"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

        }


        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {

            cleanVariables();
            loadFull();
            DatHeaddersaddingToTheTable();

            if (string.IsNullOrEmpty(cmb_year.Text) || string.IsNullOrEmpty(cmb_semester.Text) || cmb_group.SelectedValue == null || cmb_degree_name.SelectedValue == null)
            {
                MessageBox.Show("Please Complete fields before Generating Table!");
            }
            else
            {
                string grupNumberCharcter = "";
                if (cmb_group.SelectedValue.ToString().Length == 1)
                {
                    grupNumberCharcter = "0" + cmb_group.SelectedValue.ToString();
                }

                string year = cmb_year.Text.ToString();
                string semester = cmb_semester.Text.ToString();

                this.groupNumber = ("Y" + year + ".S" + semester + "." + cmb_degree_name.SelectedValue.ToString() + "." + grupNumberCharcter).ToString();

                System.Diagnostics.Debug.WriteLine("Number " + this.groupNumber);
                generateTable(this.groupNumber);
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


        public void generateTable(string groupDetails)
        {

            this.sessionDetails.Clear();
            if (this.IsFullWeek)
            {
                this.availableTimeSlotCount = (this.weekDaysCount * 2) + (this.weekEndCount * 11);
            }
            else
            {
                this.availableTimeSlotCount = (this.weekDaysCount * 8) + (this.weekEndCount * 11);
            }




            //Session Details
            try
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT * FROM Sessions WHERE MainGroupID = '" + groupDetails + "' AND isDeAllocated = 0";

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

                    session.NonOverLapSessions = ds.Tables[0].Rows[i].ItemArray[9].ToString();
                    session.ConcecativeSessions = ds.Tables[0].Rows[i].ItemArray[10].ToString();
                    session.ParallelSessions = ds.Tables[0].Rows[i].ItemArray[11].ToString();
                    session.RoomID = ds.Tables[0].Rows[i].ItemArray[12].ToString();
                    session.MaingGroupID = ds.Tables[0].Rows[i].ItemArray[13].ToString();



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

            System.Diagnostics.Debug.WriteLine("total sessioncount " + this.totalSessionDuration);
            System.Diagnostics.Debug.WriteLine("total slot count " + this.availableTimeSlotCount);
            if (this.totalSessionDuration < this.availableTimeSlotCount)
            //if (true)
            {
                Random random = new Random();

                if (this.IsFullWeek)
                {
                    this.finalSlotArray = new string[12, 7];
                    for (int row = 0; row < 12; row++)
                    {
                        for (int col = 0; col < 7; col++)
                        {
                            if (row == 5)
                            {
                                finalSlotArray[row, col] = "XXXXXXXX";
                            }
                            else
                            {
                                finalSlotArray[row, col] = "x";
                            }

                        }
                    }
                }
                else
                {
                    this.finalSlotArray = new string[9, 5];
                    for (int row = 0; row < 9; row++)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            if (row == 4)
                            {
                                finalSlotArray[row, col] = "XXXXXXXX";
                            }
                            else
                            {
                                finalSlotArray[row, col] = "x";
                            }

                        }
                    }
                }




                Random mondayRandom = new Random();
                Random tuesdayRandom = new Random();
                Random wednesdayRandom = new Random();
                Random thursdayRandom = new Random();
                Random fridayRandom = new Random();

                while (this.sessionDetails.Count > 0)
                {
                    this.loopbreak++;

                    this.noOfDays = this.columnCount;
                    foreach (var sesionObj in this.sessionDetails.ToList())
                    {

                        int randomNumber = random.Next(0, this.noOfDays);

                        SessionDetails duration1Session1 = new SessionDetails();
                        SessionDetails duration1Session2 = new SessionDetails();
                        SessionDetails defaultDurationSession = new SessionDetails();

                        if (sesionObj.Duration == 2)
                        {
                            this.totalSessionDuration = this.totalSessionDuration + 2;
                            duration1Session1 = sesionObj;
                            duration1Session2 = sesionObj;
                        }

                        if (sesionObj.Duration == 1)
                        {
                            this.totalSessionDuration = this.totalSessionDuration + 1;
                            defaultDurationSession = sesionObj;
                        }


                        if (this.IsFullWeek)
                        {
                            //Weekend Switch Case

                            switch (this.dayName[randomNumber])
                            {


                                case "Monday":

                                    int mdRandom1 = RandomFilter(0, (this.mondayTime.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {

                                        if (mdRandom1 == 10 || mdRandom1 == 11)
                                        {

                                            if ((this.finalSlotArray[mdRandom1, 0] == "x") && (this.finalSlotArray[mdRandom1 + 1, 0] == "x") && ((mdRandom1 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom1, 0] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers +"\n"+duration1Session1.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                mdRandom1++;
                                                this.finalSlotArray[mdRandom1, 0] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (mdRandom1 == 10 || mdRandom1 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom1, 0] == "x") && ((mdRandom1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom1, 0] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }

                                    break;



                                case "Tuesday":

                                    int mdRandom2 = RandomFilter(0, (this.tuesdayTime.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom2 == 10 || mdRandom2 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom2, 1] == "x") && (this.finalSlotArray[mdRandom2 + 1, 1] == "x") && ((mdRandom2 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom2, 1] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                mdRandom2++;
                                                this.finalSlotArray[mdRandom2, 1] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (mdRandom2 == 10 || mdRandom2 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom2, 1] == "x") && ((mdRandom2) != 12))
                                            {
                                                this.finalSlotArray[mdRandom2, 1] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }

                                    break;



                                case "Wednesday":

                                    int mdRandom3 = RandomFilter(0, (this.wednesdayTime.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom3 == 10 || mdRandom3 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom3, 2] == "x") && (this.finalSlotArray[mdRandom3 + 1, 2] == "x") && ((mdRandom3 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom3, 2] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                mdRandom3++;
                                                this.finalSlotArray[mdRandom3, 2] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (mdRandom3 == 10 || mdRandom3 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom3, 2] == "x") && ((mdRandom3) != 12))
                                            {
                                                this.finalSlotArray[mdRandom3, 2] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }

                                    break;



                                case "Thursday":

                                    int mdRandom4 = RandomFilter(0, (this.thursdayTimme.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom4 == 10 || mdRandom4 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom4, 3] == "x") && (this.finalSlotArray[mdRandom4 + 1, 3] == "x") && ((mdRandom4 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom4, 3] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                mdRandom4++;
                                                this.finalSlotArray[mdRandom4, 3] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (mdRandom4 == 10 || mdRandom4 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom4, 3] == "x") && ((mdRandom4) != 4) && ((mdRandom4) != 9))
                                            {
                                                this.finalSlotArray[mdRandom4, 3] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }

                                    break;



                                case "Friday":

                                    int mdRandom5 = RandomFilter(0, (this.fridayTime.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom5 == 10 || mdRandom5 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom5, 4] == "x") && (this.finalSlotArray[mdRandom5 + 1, 4] == "x") && ((mdRandom5 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom5, 4] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                mdRandom5++;
                                                this.finalSlotArray[mdRandom5, 4] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (mdRandom5 == 10 || mdRandom5 == 11)
                                        {
                                            if ((this.finalSlotArray[mdRandom5, 4] == "x") && ((mdRandom5) != 12))
                                            {
                                                this.finalSlotArray[mdRandom5, 4] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                this.sessionDetails.Remove(defaultDurationSession);
                                            }
                                        }
                                    }

                                    break;


                                case "Saturday":

                                    int mdRandom6 = RandomFilter(0, (this.saturdayTime.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom6 == 1 || mdRandom6 == 7)
                                        {
                                            if ((this.finalSlotArray[(mdRandom6 - 1), 5] != "x") && (this.finalSlotArray[mdRandom6, 5] == "x") && (this.finalSlotArray[mdRandom6 + 1, 5] == "x") && ((mdRandom6 + 1) != 5) && ((mdRandom6 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom6, 5] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.saturdayTime = this.saturdayTime.Where((source, index) => index != mdRandom6).ToArray();
                                                mdRandom6++;
                                                this.finalSlotArray[mdRandom6, 5] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.saturdayTime = this.saturdayTime.Where((source, index) => index != mdRandom6).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom6, 5] == "x") && (this.finalSlotArray[mdRandom6 + 1, 5] == "x") && ((mdRandom6 + 1) != 5) && ((mdRandom6 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom6, 5] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.saturdayTime = this.saturdayTime.Where((source, index) => index != mdRandom6).ToArray();
                                                mdRandom6++;
                                                this.finalSlotArray[mdRandom6, 5] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.saturdayTime = this.saturdayTime.Where((source, index) => index != mdRandom6).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom6, 5] == "x") && ((mdRandom6) != 5) && ((mdRandom6) != 12))
                                        {
                                            this.finalSlotArray[mdRandom6, 5] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.saturdayTime = this.saturdayTime.Where((source, index) => index != mdRandom6).ToArray();
                                            this.sessionDetails.Remove(defaultDurationSession);
                                        }
                                    }


                                    break;


                                case "Sunday":

                                    int mdRandom7 = RandomFilter(0, (this.sundayTime.Length - 1), 5);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom7 == 1 || mdRandom7 == 7)
                                        {
                                            if ((this.finalSlotArray[(mdRandom7 - 1), 6] != "x") && (this.finalSlotArray[mdRandom7, 6] == "x") && (this.finalSlotArray[mdRandom7 + 1, 5] == "x") && ((mdRandom7 + 1) != 5) && ((mdRandom7 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom7, 6] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.sundayTime = this.sundayTime.Where((source, index) => index != mdRandom7).ToArray();
                                                mdRandom7++;
                                                this.finalSlotArray[mdRandom7, 6] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.sundayTime = this.sundayTime.Where((source, index) => index != mdRandom7).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom7, 6] == "x") && (this.finalSlotArray[mdRandom7 + 1, 6] == "x") && ((mdRandom7 + 1) != 5) && ((mdRandom7 + 1) != 12))
                                            {
                                                this.finalSlotArray[mdRandom7, 6] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.sundayTime = this.sundayTime.Where((source, index) => index != mdRandom7).ToArray();
                                                mdRandom7++;
                                                this.finalSlotArray[mdRandom7, 6] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.sundayTime = this.sundayTime.Where((source, index) => index != mdRandom7).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom7, 6] == "x") && ((mdRandom7) != 5) && ((mdRandom7) != 12))
                                        {
                                            this.finalSlotArray[mdRandom7, 6] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.sundayTime = this.sundayTime.Where((source, index) => index != mdRandom7).ToArray();
                                            this.sessionDetails.Remove(defaultDurationSession);
                                        }
                                    }

                                    break;

                                default:
                                    break;
                            }

                        }
                        else
                        {
                            //Weekday Switch Case
                            switch (this.dayName[randomNumber])
                            {


                                case "Monday":

                                    int mdRandom1 = RandomFilter(0, (this.mondayTime.Length - 1), 4);

                                    if (sesionObj.Duration == 2)
                                    {

                                        if (mdRandom1 == 1 || mdRandom1 == 6)
                                        {
                                            if ((this.finalSlotArray[(mdRandom1 - 1), 0] != "x") && (this.finalSlotArray[mdRandom1, 0] == "x") && (this.finalSlotArray[mdRandom1 + 1, 0] == "x") && ((mdRandom1 + 1) != 4) && ((mdRandom1 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom1, 0] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                mdRandom1++;
                                                this.finalSlotArray[mdRandom1, 0] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom1, 0] == "x") && (this.finalSlotArray[mdRandom1 + 1, 0] == "x") && ((mdRandom1 + 1) != 4) && ((mdRandom1 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom1, 0] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                mdRandom1++;
                                                this.finalSlotArray[mdRandom1, 0] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom1, 0] == "x") && ((mdRandom1) != 4) && ((mdRandom1) != 9))
                                        {
                                            this.finalSlotArray[mdRandom1, 0] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                            this.sessionDetails.Remove(sesionObj);
                                        }
                                    }

                                    break;



                                case "Tuesday":

                                    int mdRandom2 = RandomFilter(0, (this.tuesdayTime.Length - 1), 4);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom2 == 1 || mdRandom2 == 6)
                                        {
                                            if ((this.finalSlotArray[(mdRandom2 - 1), 1] != "x") && (this.finalSlotArray[mdRandom2, 1] == "x") && (this.finalSlotArray[mdRandom2 + 1, 1] == "x") && ((mdRandom2 + 1) != 4) && ((mdRandom2 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom2, 1] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                mdRandom2++;
                                                this.finalSlotArray[mdRandom2, 1] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom2, 1] == "x") && (this.finalSlotArray[mdRandom2 + 1, 1] == "x") && ((mdRandom2 + 1) != 4) && ((mdRandom2 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom2, 1] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                mdRandom2++;
                                                this.finalSlotArray[mdRandom2, 1] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom2, 1] == "x") && ((mdRandom2) != 4) && ((mdRandom2) != 9))
                                        {
                                            this.finalSlotArray[mdRandom2, 1] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                            this.sessionDetails.Remove(sesionObj);
                                        }
                                    }

                                    break;



                                case "Wednesday":

                                    int mdRandom3 = RandomFilter(0, (this.wednesdayTime.Length - 1), 4);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom3 == 1 || mdRandom3 == 6)
                                        {
                                            if ((this.finalSlotArray[(mdRandom3 - 1), 2] != "x") && (this.finalSlotArray[mdRandom3, 2] == "x") && (this.finalSlotArray[mdRandom3 + 1, 2] == "x") && ((mdRandom3 + 1) != 4) && ((mdRandom3 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom3, 2] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                mdRandom3++;
                                                this.finalSlotArray[mdRandom3, 2] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom3, 2] == "x") && (this.finalSlotArray[mdRandom3 + 1, 2] == "x") && ((mdRandom3 + 1) != 4) && ((mdRandom3 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom3, 2] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                mdRandom3++;
                                                this.finalSlotArray[mdRandom3, 2] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom3, 2] == "x") && ((mdRandom3) != 4) && ((mdRandom3) != 9))
                                        {
                                            this.finalSlotArray[mdRandom3, 2] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                            this.sessionDetails.Remove(sesionObj);
                                        }
                                    }

                                    break;



                                case "Thursday":

                                    int mdRandom4 = RandomFilter(0, (this.thursdayTimme.Length - 1), 4);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom4 == 1 || mdRandom4 == 6)
                                        {
                                            if ((this.finalSlotArray[(mdRandom4 - 1), 3] != "x") && (this.finalSlotArray[mdRandom4, 3] == "x") && (this.finalSlotArray[mdRandom4 + 1, 3] == "x") && ((mdRandom4 + 1) != 4) && ((mdRandom4 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom4, 3] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                mdRandom4++;
                                                this.finalSlotArray[mdRandom4, 3] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom4, 3] == "x") && (this.finalSlotArray[mdRandom4 + 1, 3] == "x") && ((mdRandom4 + 1) != 4) && ((mdRandom4 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom4, 3] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                mdRandom4++;
                                                this.finalSlotArray[mdRandom4, 3] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom4, 3] == "x") && ((mdRandom4) != 4) && ((mdRandom4) != 9))
                                        {
                                            this.finalSlotArray[mdRandom4, 3] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                            this.sessionDetails.Remove(sesionObj);
                                        }
                                    }

                                    break;



                                case "Friday":

                                    int mdRandom5 = RandomFilter(0, (this.fridayTime.Length - 1), 4);

                                    if (sesionObj.Duration == 2)
                                    {
                                        if (mdRandom5 == 1 || mdRandom5 == 6)
                                        {
                                            if ((this.finalSlotArray[(mdRandom5 - 1), 4] != "x") && (this.finalSlotArray[mdRandom5, 4] == "x") && (this.finalSlotArray[mdRandom5 + 1, 4] == "x") && ((mdRandom5 + 1) != 4) && ((mdRandom5 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom5, 4] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                mdRandom5++;
                                                this.finalSlotArray[mdRandom5, 4] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                        else
                                        {
                                            if ((this.finalSlotArray[mdRandom5, 4] == "x") && (this.finalSlotArray[mdRandom5 + 1, 4] == "x") && ((mdRandom5 + 1) != 4) && ((mdRandom5 + 1) != 9))
                                            {
                                                this.finalSlotArray[mdRandom5, 4] = duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID + "\n" + duration1Session1.Lecturers + "\n" + duration1Session1.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                mdRandom5++;
                                                this.finalSlotArray[mdRandom5, 4] = duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID + "\n" + duration1Session2.Lecturers + "\n" + duration1Session2.RoomID;
                                                this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                                this.sessionDetails.Remove(sesionObj);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((this.finalSlotArray[mdRandom5, 4] == "x") && ((mdRandom5) != 4) && ((mdRandom5) != 9))
                                        {
                                            this.finalSlotArray[mdRandom5, 4] = defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID + "\n" + defaultDurationSession.Lecturers + "\n" + defaultDurationSession.RoomID;
                                            this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                            this.sessionDetails.Remove(defaultDurationSession);
                                        }
                                    }

                                    break;

                                default:
                                    break;
                            }
                        }



                        //Weekend Switch Case

                    }

                    if (loopbreak > 200)
                    {
                        MessageBox.Show("System Error Occured Try Again");
                        break;

                    }



                }


                var dg = new DataGrid();
                this.MainGrid.Children.Add(dg);

                var column1 = new DataGridTextColumn();
                column1.Header = "Time";
                column1.Binding = new Binding("Time");
                dg.Columns.Add(column1);


                foreach (var day in this.NEW_LIST)
                {
                    var column = new DataGridTextColumn();
                    column.Header = day;
                    column.Binding = new Binding(day);
                    dg.Columns.Add(column);
                }

                string[,] abcd = new string[1, this.columnCount];

                if (this.IsFullWeek)
                {
                    for (int row = 0; row < 12; row++)
                    {
                        dg.Items.Add(new DataItem { Time = this.timeSlots[row], Monday = finalSlotArray[row, 0], Tuesday = finalSlotArray[row, 1], Wednesday = finalSlotArray[row, 2], Thursday = finalSlotArray[row, 3], Friday = finalSlotArray[row, 4], Saturday = finalSlotArray[row, 5], Sunday = finalSlotArray[row, 6] });
                    }
                }
                else
                {
                    for (int row = 0; row < 9; row++)
                    {
                        dg.Items.Add(new DataItem { Time = this.timeSlots[row], Monday = finalSlotArray[row, 0], Tuesday = finalSlotArray[row, 1], Wednesday = finalSlotArray[row, 2], Thursday = finalSlotArray[row, 3], Friday = finalSlotArray[row, 4] });
                    }
                }

                btn_print_table.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Selected days not enough for genarating this time table, Please chek days");
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

        private void btn_print_table_Click(object sender, RoutedEventArgs e)
        {
            string pdfPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var MainDirectory = new DirectoryInfo(pdfPath + "\\");


            if (MainDirectory.Exists)
            {
                MainDirectory.CreateSubdirectory("Genareted Time Tables");
                Directory.CreateDirectory(pdfPath + "\\" + "Genareted Time Tables" + "\\" + "Students Time Table");
                Directory.CreateDirectory(pdfPath + "\\" + "Genareted Time Tables" + "\\" + "Lecturers Time Table");
                Directory.CreateDirectory(pdfPath + "\\" + "Genareted Time Tables" + "\\" + "Lecture Hall Time Table");
            }


            string LecturerPDFPath = pdfPath + "\\" + "Genareted Time Tables" + "\\" + "Lecturers Time Table";
            string LectureHallPDFPath = pdfPath + "\\" + "Genareted Time Tables" + "\\" + "Lecture Hall Time Table";
            string StudentPDFPath = pdfPath + "\\" + "Genareted Time Tables" + "\\" + "Students Time Table";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF (*.pdf)|*.pdf";
            sfd.FileName = StudentPDFPath + "\\" + this.groupNumber + ".pdf";
            bool fileError = false;


            if (File.Exists(sfd.FileName))
            {
                try
                {
                    File.Delete(sfd.FileName);
                }
                catch (IOException ex)
                {
                    fileError = true;
                    MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                }
            }
            if (!fileError)
            {
                try
                {
                    PdfPTable pdfTable = new PdfPTable(this.NEW_LIST.Count + 1);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.HorizontalAlignment = 1;

                    pdfTable.AddCell("Time");
                    foreach (var dayName in this.NEW_LIST)
                    {
                        pdfTable.AddCell(dayName.ToString());
                    }


                    for (int row = 0; row < this.timeSlots.Length; row++)
                    {
                        pdfTable.AddCell(this.timeSlots[row].ToString());
                        for (int col = 0; col < this.NEW_LIST.Count; col++)
                        {
                            pdfTable.AddCell(this.finalSlotArray[row, col].ToString());
                        }
                    }



                    using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("Time Table PDF Exported Successfully ! \n Please visit " + StudentPDFPath, "PDF EXPORT STATUS");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :" + ex.Message);
                }
            }
        }
    }
}
