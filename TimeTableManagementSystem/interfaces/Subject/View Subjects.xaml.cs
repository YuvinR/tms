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
    /// Interaction logic for View_Subjects.xaml
    /// </summary>
    public partial class View_Subjects : Window
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


        public View_Subjects()
        {
            InitializeComponent();
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataGrid();
        }


        private void loadDataGrid()
        {
            connection.Open();
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Subject";

                SQLiteDataAdapter sqliteAdapter = new SQLiteDataAdapter(command);

                sqliteAdapter.Fill(dataTable);

                ViewSbjGrid.ItemsSource = dataTable.DefaultView;
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = ViewSbjGrid.SelectedItem as DataRowView;
            sbjCode = row.Row["Subject_Code"].ToString();

            connection.Open();
            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Subject where Subject_Code = @Subject_Code";
                command.Parameters.AddWithValue("@Subject_Code", sbjCode);

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

            loadDataGrid();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = ViewSbjGrid.SelectedItem as DataRowView;

            UptSubCodeTxt.Text = row.Row["Subject_Code"].ToString();
            UptSubNameTxt.Text = row.Row["Subject_Name"].ToString();
            UptOffYearTxt.Text = row.Row["Offered_Year"].ToString();
            UptOffSemTxt.Text = row.Row["Offered_Semester"].ToString();
            UptNumLecHrTxt.Text = row.Row["Lecture_Hours"].ToString();
            UptNumTuteHrTxt.Text = row.Row["Tutorial_Hours"].ToString();
            UptNumLabTxt.Text = row.Row["Lab_Hours"].ToString();
            UptNumEvaTxt.Text = row.Row["Evaluation_Hours"].ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(UptSubCodeTxt.Text) || string.IsNullOrEmpty(UptSubNameTxt.Text) || string.IsNullOrEmpty(UptOffYearTxt.Text) || string.IsNullOrEmpty(UptOffSemTxt.Text)
                || string.IsNullOrEmpty(UptNumLecHrTxt.Text) || string.IsNullOrEmpty(UptNumTuteHrTxt.Text) || string.IsNullOrEmpty(UptNumLabTxt.Text) || string.IsNullOrEmpty(UptNumEvaTxt.Text))
            {
                MessageBox.Show("Please fill the Required Fields Before Inserting Data!");
            }
            else
            {
                sbjCode = UptSubCodeTxt.Text;
                sbjName = UptSubNameTxt.Text;
                sbjYear = UptOffYearTxt.Text;
                sbjSem = UptOffSemTxt.Text;

                numOfLecHr = int.Parse(UptNumLecHrTxt.Text);
                numOfTuteHr = int.Parse(UptNumTuteHrTxt.Text);
                numOfLabHr = int.Parse(UptNumLabTxt.Text);
                numOfEvlHr = int.Parse(UptNumEvaTxt.Text);

                connection.Open();

                try
                {

                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "Update Subject " +
                            "set Subject_Name = @Subject_Name, Offered_Year = @Offered_Year, Offered_Semester = @Offered_Semester, Lecture_Hours = @Lecture_Hours, Tutorial_Hours = @Tutorial_Hours, Lab_Hours = @Lab_Hours, Evaluation_Hours = @Evaluation_Hours " +
                            "Where Subject_Code = @Subject_Code";

                    cmd.Parameters.AddWithValue("@Subject_Name", sbjName);
                    cmd.Parameters.AddWithValue("@Offered_Year", sbjYear);
                    cmd.Parameters.AddWithValue("@Offered_Semester", sbjSem);
                    cmd.Parameters.AddWithValue("@Lecture_Hours", numOfLecHr);
                    cmd.Parameters.AddWithValue("@Tutorial_Hours", numOfTuteHr);
                    cmd.Parameters.AddWithValue("@Lab_Hours", numOfLabHr);
                    cmd.Parameters.AddWithValue("@Evaluation_Hours", numOfEvlHr);
                    cmd.Parameters.AddWithValue("@Subject_Code", sbjCode);

                    int rows = cmd.ExecuteNonQuery();


                    if (rows > 0)
                    {
                        MessageBox.Show("Record has been Updated");
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

            loadDataGrid();
            clearFields();
        }

        private void clearFields()
        {
            UptSubCodeTxt.Text = "";
            UptSubNameTxt.Text = "";
            UptOffYearTxt.Text = "";
            UptOffSemTxt.Text = "";
            UptNumLecHrTxt.Text = "";
            UptNumTuteHrTxt.Text = "";
            UptNumLabTxt.Text = "";
            UptNumEvaTxt.Text = "";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTxt.Text;
            connection.Open();
            DataTable dataTable = new DataTable();

            SQLiteCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = "Select * from Subject where Subject_Name LIKE '%" + keyword + "' OR Subject_Code LIKE '%" + keyword + "%' OR Offered_Year Like '%" + keyword + "%'";

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

            dataAdapter.Fill(dataTable);

            ViewSbjGrid.ItemsSource = dataTable.DefaultView;

            connection.Close();
        }

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTxt.Text))
            {
                loadDataGrid();
            }
        }
    }
}
