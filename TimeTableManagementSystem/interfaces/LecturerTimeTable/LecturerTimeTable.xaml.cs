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

namespace TimeTableManagementSystem.interfaces.LecturerTimeTable
{
    /// <summary>
    /// Interaction logic for LecturerTimeTable.xaml
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


        public string SessionID { get; set; }
        public string Lecturers { get; set; }
        public string Subject { get; set; }
        public string SubjectCode { get; set; }
        public string Tag { get; set; }
        public string GroupID { get; set; }

        public int StudentCount { get; set; }
        public int Duration { get; set; }
        public int IsDeAlocated { get; set; }

    }
    public partial class LecturerTimeTable : Window
    {
        private SQLiteConnection connection = db_config.connect();


        String[] CHECKING_DAYS = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        string[] timeSlots;
        string[] mondayTime;
        string[] tuesdayTime;
        string[] wednesdayTime;
        string[] thursdayTimme;
        string[] fridayTime;
        string[] dayName;
        int noOfSessions = 0;
        int noOfDays = 0;

        int loopbreak = 0;

        string[,] finalSlotArray;

        int rowCount;
        int columnCount;

        List<SessionDetails> sessionDetails = new List<SessionDetails>();
        List<SessionDetails> sessionDetailsSecondTurnFiltering = new List<SessionDetails>();
        List<String> columnNames = new List<string>();

        //Testting
        List<string> ABCD = new List<string>();
        List<string> NEW_LIST = new List<string>();

        public LecturerTimeTable()
        {
            loadFull();

            this.NEW_LIST.Add("Monday");
            this.NEW_LIST.Add("Tuesday");
            this.NEW_LIST.Add("Wednesday");
            this.NEW_LIST.Add("Thursday");
            this.NEW_LIST.Add("Friday");
            InitializeComponent();
            fetchDataToComboBox();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(cmbLecturerName.SelectedValue.ToString());
            generateTable(cmbLecturerName.SelectedValue.ToString());
        }

        public void fetchDataToComboBox()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Lecturer";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmbLecturerName.ItemsSource = ds.Tables[0].DefaultView;
                cmbLecturerName.DisplayMemberPath = ds.Tables[0].Columns["Name"].ToString();
                cmbLecturerName.SelectedValuePath = ds.Tables[0].Columns["Name"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }


        public void generateTable(string lecturerName)
        {
            loadFull();
            this.sessionDetails.Clear();

            DataItem dataItem = new DataItem();

            for(int xd = 0; xd < this.dayName.Length; xd++)
            {
                dataItem.MyDates[xd] = this.dayName[xd];
                System.Diagnostics.Debug.WriteLine("Date Item ##################" + dataItem.MyDates[xd]);
                ABCD.Add(this.dayName[xd]);
            }


            //Session Details
            try
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lecturerName + "%'";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();


                dataAdapter.Fill(ds);

                this.noOfSessions = ds.Tables[0].Rows.Count;
                System.Diagnostics.Debug.WriteLine("Number Of Sessions " + ds.Tables[0].Rows.Count);


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



            Random random = new Random();

            this.finalSlotArray = new string[9, 5];

            for (int row = 0; row<9; row++)
            {
                for (int col = 0;  col<5; col++)
                {
                    if(row == 4)
                    {
                        finalSlotArray[row, col] = "XXXXXXXX";
                    }
                    else
                    {
                        finalSlotArray[row, col] = "x";
                    }
                    
                }
            }

            System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------------");


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

                    System.Diagnostics.Debug.WriteLine("Random Number " + randomNumber);
                    System.Diagnostics.Debug.WriteLine("Day Name For Random " + this.dayName[randomNumber]);

                    SessionDetails duration1Session1 = new SessionDetails();
                    SessionDetails duration1Session2 = new SessionDetails();
                    SessionDetails defaultDurationSession = new SessionDetails();

                    if (sesionObj.Duration == 2)
                    {
                        duration1Session1 = sesionObj;
                        duration1Session2 = sesionObj;

                        System.Diagnostics.Debug.WriteLine("DUration 2 " + duration1Session1.Duration + " " + duration1Session2.Duration);
                    }

                    if (sesionObj.Duration == 1)
                    {
                        defaultDurationSession = sesionObj;
                        System.Diagnostics.Debug.WriteLine("DUration 1 " + defaultDurationSession.Duration);
                    }



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
                                        this.finalSlotArray[mdRandom1, 0] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                        mdRandom1++;
                                        this.finalSlotArray[mdRandom1, 0] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                                else
                                {
                                    if ((this.finalSlotArray[mdRandom1, 0] == "x") && (this.finalSlotArray[mdRandom1 + 1, 0] == "x") && ((mdRandom1 + 1) != 4) && ((mdRandom1 + 1) != 9))
                                    {
                                        this.finalSlotArray[mdRandom1, 0] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                        mdRandom1++;
                                        this.finalSlotArray[mdRandom1, 0] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.mondayTime = this.mondayTime.Where((source, index) => index != mdRandom1).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }

                            }
                            else
                            {
                                if ((this.finalSlotArray[mdRandom1, 0] == "x") && ((mdRandom1) != 4) && ((mdRandom1) != 9))
                                {
                                    this.finalSlotArray[mdRandom1, 0] = defaultDurationSession.SessionID + " : " + defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID;
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
                                        this.finalSlotArray[mdRandom2, 1] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                        mdRandom2++;
                                        this.finalSlotArray[mdRandom2, 1] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                                else
                                {
                                    if ((this.finalSlotArray[mdRandom2, 1] == "x") && (this.finalSlotArray[mdRandom2 + 1, 1] == "x") && ((mdRandom2 + 1) != 4) && ((mdRandom2 + 1) != 9))
                                    {
                                        this.finalSlotArray[mdRandom2, 1] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                        mdRandom2++;
                                        this.finalSlotArray[mdRandom2, 1] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.tuesdayTime = this.tuesdayTime.Where((source, index) => index != mdRandom2).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }

                            }
                            else
                            {
                                if ((this.finalSlotArray[mdRandom2, 1] == "x") && ((mdRandom2) != 4) && ((mdRandom2) != 9))
                                {
                                    this.finalSlotArray[mdRandom2, 1] = defaultDurationSession.SessionID + " : " + defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID;
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
                                        this.finalSlotArray[mdRandom3, 2] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                        mdRandom3++;
                                        this.finalSlotArray[mdRandom3, 2] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                                else
                                {
                                    if ((this.finalSlotArray[mdRandom3, 2] == "x") && (this.finalSlotArray[mdRandom3 + 1, 2] == "x") && ((mdRandom3 + 1) != 4) && ((mdRandom3 + 1) != 9))
                                    {
                                        this.finalSlotArray[mdRandom3, 2] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                        mdRandom3++;
                                        this.finalSlotArray[mdRandom3, 2] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.wednesdayTime = this.wednesdayTime.Where((source, index) => index != mdRandom3).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                            }
                            else
                            {
                                if ((this.finalSlotArray[mdRandom3, 2] == "x") && ((mdRandom3) != 4) && ((mdRandom3) != 9))
                                {
                                    this.finalSlotArray[mdRandom3, 2] = defaultDurationSession.SessionID + " : " + defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID;
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
                                        this.finalSlotArray[mdRandom4, 3] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                        mdRandom4++;
                                        this.finalSlotArray[mdRandom4, 3] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                                else
                                {
                                    if ((this.finalSlotArray[mdRandom4, 3] == "x") && (this.finalSlotArray[mdRandom4 + 1, 3] == "x") && ((mdRandom4 + 1) != 4) && ((mdRandom4 + 1) != 9))
                                    {
                                        this.finalSlotArray[mdRandom4, 3] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                        mdRandom4++;
                                        this.finalSlotArray[mdRandom4, 3] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.thursdayTimme = this.thursdayTimme.Where((source, index) => index != mdRandom4).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                            }
                            else
                            {
                                if ((this.finalSlotArray[mdRandom4, 3] == "x") && ((mdRandom4) != 4) && ((mdRandom4) != 9))
                                {
                                    this.finalSlotArray[mdRandom4, 3] = defaultDurationSession.SessionID + " : " + defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID;
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
                                        this.finalSlotArray[mdRandom5, 4] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                        mdRandom5++;
                                        this.finalSlotArray[mdRandom5, 4] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                                else
                                {
                                    if ((this.finalSlotArray[mdRandom5, 4] == "x") && (this.finalSlotArray[mdRandom5 + 1, 4] == "x") && ((mdRandom5 + 1) != 4) && ((mdRandom5 + 1) != 9))
                                    {
                                        this.finalSlotArray[mdRandom5, 4] = duration1Session1.SessionID + " : " + duration1Session1.SubjectCode + "-" + duration1Session1.Subject + "(" + duration1Session1.Tag + ")" + "\n" + duration1Session1.GroupID;
                                        this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                        mdRandom5++;
                                        this.finalSlotArray[mdRandom5, 4] = duration1Session2.SessionID + " : " + duration1Session2.SubjectCode + "-" + duration1Session2.Subject + "(" + duration1Session2.Tag + ")" + "\n" + duration1Session2.GroupID;
                                        this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                        this.sessionDetails.Remove(sesionObj);
                                    }
                                }
                            }
                            else
                            {
                                if ((this.finalSlotArray[mdRandom5, 4] == "x") && ((mdRandom5) != 4) && ((mdRandom5) != 9))
                                {
                                    this.finalSlotArray[mdRandom5, 4] = defaultDurationSession.SessionID + " : " + defaultDurationSession.SubjectCode + "-" + defaultDurationSession.Subject + "(" + defaultDurationSession.Tag + ")" + "\n" + defaultDurationSession.GroupID;
                                    this.fridayTime = this.fridayTime.Where((source, index) => index != mdRandom5).ToArray();
                                    this.sessionDetails.Remove(defaultDurationSession);
                                }
                            }

                            break;

                        default:
                            System.Diagnostics.Debug.WriteLine("Random Number " + randomNumber);
                            break;
                    }

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


            for (int row = 0; row < 9; row++)
            {
                dg.Items.Add(new DataItem { Time = this.timeSlots[row], Monday = finalSlotArray[row, 0], Tuesday = finalSlotArray[row, 1], Wednesday = finalSlotArray[row, 2], Thursday = finalSlotArray[row, 3], Friday = finalSlotArray[row, 4] });
            }

            //PopulateGridViewWithTimeTable();

        }

        
        public void PopulateGridViewWithTimeTable()
        {

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

                System.Diagnostics.Debug.WriteLine("Day Count " + ds.Tables[0].Rows.Count);
                this.noOfDays = ds.Tables[0].Rows.Count;
                this.dayName = new string[ds.Tables[0].Rows.Count];

                this.columnCount = ds.Tables[0].Rows.Count;

                int i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {

                    DAY = ds.Tables[0].Rows[i].ItemArray[6].ToString();
                    dayName[i] = DAY;
                    this.columnNames.Add(DAY);

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

                System.Diagnostics.Debug.WriteLine("TiemRow Count " + ds.Tables[0].Rows.Count);
                this.timeSlots = new string[ds.Tables[0].Rows.Count];
                this.mondayTime = new string[ds.Tables[0].Rows.Count];
                this.tuesdayTime = new string[ds.Tables[0].Rows.Count];
                this.wednesdayTime = new string[ds.Tables[0].Rows.Count];
                this.thursdayTimme = new string[ds.Tables[0].Rows.Count];
                this.fridayTime = new string[ds.Tables[0].Rows.Count];

                this.rowCount = ds.Tables[0].Rows.Count;

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

    }
}
