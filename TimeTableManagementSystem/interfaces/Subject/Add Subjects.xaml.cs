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

namespace TimeTableManagementSystem.interfaces.Subject
{
    /// <summary>
    /// Interaction logic for Add_Subjects.xaml
    /// </summary>
    public partial class Add_Subjects : Window
    {
        private SQLiteConnection connection = db_config.connect();

        private string sbjCode;
        private string sbjName;
        private string sbjYear;
        private string sbjSem;
        private int numOfLecHr;
        private int numOfTuteHr;
        private int numOfLabHr;
        private int numOfEvlHr;

        public Add_Subjects()
        {
            InitializeComponent();
        }

        private void BtnviewSbj_Click(object sender, RoutedEventArgs e)
        {
            View_Subjects view_Subjects = new View_Subjects();
            this.Close();
            view_Subjects.Show();
        }

        private void BtnAddSbj_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (string.IsNullOrEmpty(SbjCodeTxt.Text) || string.IsNullOrEmpty(SbjNameTxt.Text) || string.IsNullOrEmpty(OffYearTxt.Text) || string.IsNullOrEmpty(OffSemTxt.Text)
                || string.IsNullOrEmpty(NumLecHrTxt.Text) || string.IsNullOrEmpty(NumTuteHrTxt.Text) || string.IsNullOrEmpty(NumLabHrTxt.Text) || string.IsNullOrEmpty(NumEvaHrTxt.Text))
            {
                MessageBox.Show("Please fill the Required Fields Before Inserting Data!");
            }
            else {

                connection.Open();

                sbjCode = SbjCodeTxt.Text;
                sbjName = SbjNameTxt.Text;
                sbjYear = OffYearTxt.Text;
                sbjSem = OffSemTxt.Text;

                numOfLecHr = int.Parse(NumLecHrTxt.Text);
                numOfTuteHr = int.Parse(NumTuteHrTxt.Text);
                numOfLabHr = int.Parse(NumLabHrTxt.Text);
                numOfEvlHr = int.Parse(NumEvaHrTxt.Text);

                try
                {
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "insert into Subject" +
                            "(Subject_Code, Subject_Name, Offered_Year, Offered_Semester, Lecture_Hours, Tutorial_Hours, Lab_Hours, Evaluation_Hours)" +
                            " Values" +
                            "(@Subject_Code, @Subject_Name, @Offered_Year, @Offered_Semester, @Lecture_Hours, @Tutorial_Hours, @Lab_Hours, @Evaluation_Hours)";

                    cmd.Parameters.AddWithValue("@Subject_Code", sbjCode);
                    cmd.Parameters.AddWithValue("@Subject_Name", sbjName);
                    cmd.Parameters.AddWithValue("@Offered_Year", sbjYear);
                    cmd.Parameters.AddWithValue("@Offered_Semester", sbjSem);
                    cmd.Parameters.AddWithValue("@Lecture_Hours", numOfLecHr);
                    cmd.Parameters.AddWithValue("@Tutorial_Hours", numOfTuteHr);
                    cmd.Parameters.AddWithValue("@Lab_Hours", numOfLabHr);
                    cmd.Parameters.AddWithValue("@Evaluation_Hours", numOfEvlHr);

                    int rows = cmd.ExecuteNonQuery();


                    if (rows > 0)
                    {
                        MessageBox.Show("Data has been Inserted");
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
            }

            clearFields();
        }

        private void clearFields()
        {
            SbjCodeTxt.Text = "";
            SbjNameTxt.Text = "";
            OffYearTxt.Text = "";
            OffSemTxt.Text = "";

            NumLecHrTxt.Text = "";
            NumTuteHrTxt.Text = "";
            NumLabHrTxt.Text = "";
            NumEvaHrTxt.Text = "";
        }
    }
}
