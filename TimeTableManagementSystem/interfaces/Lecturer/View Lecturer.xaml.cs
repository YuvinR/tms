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
    /// Interaction logic for View_Lecturer.xaml
    /// </summary>
    public partial class View_Lecturer : Window
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

        public View_Lecturer()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataGrid();
            loadComboBoxes();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = LecDataGrid.SelectedItem as DataRowView;
            empID = row.Row["EmployeeID"].ToString();

            connection.Open();
            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Lecturer where EmployeeID = @EmployeeID";
                command.Parameters.AddWithValue("@EmployeeID", empID);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Lecturer Data has been Deleted!");
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

        private void loadDataGrid()
        {
            connection.Open();

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Lecturer";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                LecDataGrid.ItemsSource = dataTable.DefaultView;
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
            UpdEmpIdTxt.Text = "";
            UpdEmpNameTxt.Text = "";
            UpdFacultyCombo.Text = "";
            UpdEmpDepTxt.Text = "";
            UpdCenterCombo.Text = "";
            UpdBuildingCombo.Text = "";
            UpdLevelCombo.Text = "";
            UpdRankTxt.Text = "";
        }

        private void loadComboBoxes()
        {
            connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select DISTINCT BuildingName from Location";

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UpdBuildingCombo.Items.Add(reader.GetString("BuildingName"));
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = LecDataGrid.SelectedItem as DataRowView;

            UpdEmpIdTxt.Text = row.Row["EmployeeID"].ToString();
            UpdEmpNameTxt.Text = row.Row["Name"].ToString();
            UpdFacultyCombo.Text = row.Row["Faculty"].ToString();
            UpdEmpDepTxt.Text = row.Row["Department"].ToString();
            UpdCenterCombo.Text = row.Row["Center"].ToString();
            UpdBuildingCombo.Text = row.Row["Building"].ToString();
            UpdLevelCombo.Text = row.Row["Level"].ToString();
            UpdRankTxt.Text = row.Row["Rank"].ToString();
        }

        private void UpdLevelCombo_DropDownClosed(object sender, EventArgs e)
        {
            if (UpdLevelCombo.Text.Equals("Professor") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"1.{UpdEmpIdTxt.Text}";
            }
            else if (UpdLevelCombo.Text.Equals("Assistant Professor") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"2.{UpdEmpIdTxt.Text}";
            }
            else if (UpdLevelCombo.Text.Equals("Senior Lecturer(HG)") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"3.{UpdEmpIdTxt.Text}";
            }
            else if (UpdLevelCombo.Text.Equals("Senior Lecturer") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"4.{UpdEmpIdTxt.Text}";
            }
            else if (UpdLevelCombo.Text.Equals("Lecturer") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"5.{UpdEmpIdTxt.Text}";
            }
            else if (UpdLevelCombo.Text.Equals("Assistant Lecturer") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"6.{UpdEmpIdTxt.Text}";
            }
            else if (UpdLevelCombo.Text.Equals("Instructors") && UpdEmpIdTxt.Text != null)
            {
                UpdRankTxt.Text = $"7.{UpdEmpIdTxt.Text}";
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTxt.Text;
            connection.Open();
            DataTable dataTable = new DataTable();

            SQLiteCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = "Select * from Lecturer where EmployeeID LIKE '%" + keyword + "%' OR Name LIKE '%" + keyword + "%' OR Faculty Like '%" + keyword + "%'";

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

            dataAdapter.Fill(dataTable);

            LecDataGrid.ItemsSource = dataTable.DefaultView;

            connection.Close();
        }

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTxt.Text))
            {
                loadDataGrid();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           
            if (String.IsNullOrEmpty(UpdEmpIdTxt.Text) || String.IsNullOrEmpty(UpdEmpNameTxt.Text) || String.IsNullOrEmpty(UpdFacultyCombo.Text) || String.IsNullOrEmpty(UpdEmpDepTxt.Text)
                || String.IsNullOrEmpty(UpdCenterCombo.Text) || String.IsNullOrEmpty(UpdBuildingCombo.Text) || String.IsNullOrEmpty(UpdLevelCombo.Text) || String.IsNullOrEmpty(UpdRankTxt.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
            else
            {
                connection.Open();
                empID = UpdEmpIdTxt.Text;
                empName = UpdEmpNameTxt.Text;
                faculty = UpdFacultyCombo.Text;
                department = UpdEmpDepTxt.Text;
                center = UpdCenterCombo.Text;
                building = UpdBuildingCombo.Text;
                level = UpdLevelCombo.Text;
                rank = double.Parse(UpdRankTxt.Text);

                try
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "Update Lecturer " +
                                          "Set Name = @Name, Faculty = @Faculty, Department = @Department, Center = @Center, Building = @Building, Level = @Level, Rank = @Rank " +
                                          "Where EmployeeID = @EmployeeID ";

                    command.Parameters.AddWithValue("@Name", empName);
                    command.Parameters.AddWithValue("@Faculty", faculty);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Center", center);
                    command.Parameters.AddWithValue("@Building", building);
                    command.Parameters.AddWithValue("@Level", level);
                    command.Parameters.AddWithValue("@Rank", rank);
                    command.Parameters.AddWithValue("@EmployeeID", empID);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show($"Employee {empID} Has been Updated!");
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
                    clearFields();
                }
            }
            loadDataGrid();
            
        }
    }
}
