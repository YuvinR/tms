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

namespace TimeTableManagementSystem.interfaces.Lecturer
{
    /// <summary>
    /// Interaction logic for Add_Lecturer.xaml
    /// </summary>
    public partial class Add_Lecturer : Window
    {
        private SQLiteConnection connection = db_config.connect();

        private string empID;
        private string empName;
        private string faculty;
        private string department;
        private string center;
        private string building;
        private string level;
        private double rank;


        public Add_Lecturer()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadComboBoxes();
        }

        private void BtnViewLecturer_Click(object sender, RoutedEventArgs e)
        {
            View_Lecturer view_Lecturers = new View_Lecturer();
            this.Close();
            view_Lecturers.Show();
        }

        private void loadComboBoxes()
        {
            connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Building";

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BuildingCombo.Items.Add(reader.GetString("name"));
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

        private void clearFields()
        {
            EmpIdTxt.Text = "";
            EmpNameTxt.Text = "";
            FacultyCombo.Text = "";
            EmpDepTxt.Text = "";
            CenterCombo.Text = "";
            BuildingCombo.Text = "";
            LevelCombo.Text = "";
            RankTxt.Text = "";
        }

        private void BtnAddLecturer_Click(object sender, RoutedEventArgs e)
        {
            

            if (string.IsNullOrEmpty(EmpIdTxt.Text) || string.IsNullOrEmpty(EmpNameTxt.Text) || string.IsNullOrEmpty(FacultyCombo.Text) || string.IsNullOrEmpty(EmpDepTxt.Text)
                || string.IsNullOrEmpty(CenterCombo.Text) || string.IsNullOrEmpty(BuildingCombo.Text) || string.IsNullOrEmpty(LevelCombo.Text) || string.IsNullOrEmpty(RankTxt.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
            else
            {
                connection.Open();

                empID = EmpIdTxt.Text;
                empName = EmpNameTxt.Text;
                faculty = FacultyCombo.Text;
                department = EmpDepTxt.Text;
                center = CenterCombo.Text;
                building = BuildingCombo.Text;
                level = LevelCombo.Text;
                rank = double.Parse(RankTxt.Text);

                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "Insert into Lecturer " +
                                        "(EmployeeID, Name, Faculty, Department, Center, Building, Level, Rank) " +
                                        "Values " +
                                        "(@EmployeeID, @Name, @Faculty, @Department, @Center, @Building, @Level, @Rank)";

                    command.Parameters.AddWithValue("@EmployeeID", empID);
                    command.Parameters.AddWithValue("@Name", empName);
                    command.Parameters.AddWithValue("@Faculty", faculty);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Center", center);
                    command.Parameters.AddWithValue("@Building", building);
                    command.Parameters.AddWithValue("@Level", level);
                    command.Parameters.AddWithValue("@Rank", rank);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("New Employee Has been Added!");
                    }
                    else
                    {
                        MessageBox.Show("Error Occurd");
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
                clearFields();
            }
        }

        private void LevelCombo_DropDownClosed(object sender, EventArgs e)
        {
            if (LevelCombo.Text.Equals("Professor") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"1.{EmpIdTxt.Text}";
            }
            else if (LevelCombo.Text.Equals("Assistant Professor") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"2.{EmpIdTxt.Text}";
            }
            else if (LevelCombo.Text.Equals("Senior Lecturer(HG)") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"3.{EmpIdTxt.Text}";
            }
            else if (LevelCombo.Text.Equals("Senior Lecturer") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"4.{EmpIdTxt.Text}";
            }
            else if (LevelCombo.Text.Equals("Lecturer") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"5.{EmpIdTxt.Text}";
            }
            else if (LevelCombo.Text.Equals("Assistant Lecturer") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"6.{EmpIdTxt.Text}";
            }
            else if (LevelCombo.Text.Equals("Instructors") && EmpIdTxt.Text != null)
            {
                RankTxt.Text = $"7.{EmpIdTxt.Text}";
            }
        }
    }
}
