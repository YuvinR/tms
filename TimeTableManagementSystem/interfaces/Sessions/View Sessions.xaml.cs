﻿using System;
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

namespace TimeTableManagementSystem.interfaces.Sessions
{
    /// <summary>
    /// Interaction logic for View_Sessions.xaml
    /// </summary>
    public partial class View_Sessions : Window
    {

        private SQLiteConnection connection = db_config.connect();

        public View_Sessions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadComboBox();
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

                command.CommandText = "Select * from Sessions";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void loadComboBox()
        {
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Lecturer";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchLecCombo.Items.Add(reader1.GetString("Name"));
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

            //Get Subjects and add it to Combo Box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Subject";

                SQLiteDataReader reader2 = command.ExecuteReader();

                while (reader2.Read())
                {
                    SearchSubCombo.Items.Add(reader2.GetString("Subject_Name"));
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

            //Get Tags and add it to Combo Box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Tag";

                SQLiteDataReader reader3 = command.ExecuteReader();

                while (reader3.Read())
                {
                    SearchTagCombo.Items.Add(reader3.GetString("tagName"));
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

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                //Get Groups and Add it to combobox
                command.CommandText = "Select * from Groups";

                SQLiteDataReader reader4 = command.ExecuteReader();

                while (reader4.Read())
                {
                    SearchGrpCombo.Items.Add(reader4.GetString("groupId"));
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

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                //Get Groups and Add it to combobox
                command.CommandText = "Select * from SubGroup";

                SQLiteDataReader reader5 = command.ExecuteReader();

                while (reader5.Read())
                {
                    SearchGrpCombo.Items.Add(reader5.GetString("subgroupId"));
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = SessionDataGrid.SelectedItem as DataRowView;
            int sesID = int.Parse(row.Row["Session_ID"].ToString());

            connection.Open();
            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Sessions where Session_ID = @Session_ID";
                command.Parameters.AddWithValue("@Session_ID", sesID);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Session Data has been Deleted!");
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

        private void BtnSearchFields_Click(object sender, RoutedEventArgs e)
        {
            string lec = SearchLecCombo.Text;
            string tag = SearchTagCombo.Text;
            string grp = SearchGrpCombo.Text;
            string sub = SearchSubCombo.Text;

            if (!String.IsNullOrEmpty(lec) && !String.IsNullOrEmpty(sub) && !String.IsNullOrEmpty(grp) && !String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%' AND Subject = @Subject AND GroupID = @GroupID AND Tag = @Tag";

                command.Parameters.AddWithValue("@Subject", sub);
                command.Parameters.AddWithValue("@GroupID", grp);
                command.Parameters.AddWithValue("@Tag", tag);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if(!String.IsNullOrEmpty(lec) && !String.IsNullOrEmpty(sub) && !String.IsNullOrEmpty(grp))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%' AND Subject = @Subject AND GroupID = @GroupID";

                command.Parameters.AddWithValue("@Subject", sub);
                command.Parameters.AddWithValue("@GroupID", grp);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if(!String.IsNullOrEmpty(lec) && !String.IsNullOrEmpty(sub) && !String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%' AND Subject = @Subject AND Tag = @Tag";

                command.Parameters.AddWithValue("@Subject", sub);
                command.Parameters.AddWithValue("@Tag", tag);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if(!String.IsNullOrEmpty(sub) && !String.IsNullOrEmpty(grp) && !String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Subject =@Subject AND GroupID = @GroupID AND Tag = @Tag";

                command.Parameters.AddWithValue("@Subject", sub);
                command.Parameters.AddWithValue("@GroupID", grp);
                command.Parameters.AddWithValue("@Tag", tag);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if(!String.IsNullOrEmpty(lec) && !String.IsNullOrEmpty(sub))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%' AND Subject = @Subject";

                command.Parameters.AddWithValue("@Subject", sub);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if(!String.IsNullOrEmpty(lec) && !String.IsNullOrEmpty(grp))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%' AND GroupID = @GroupID";

                command.Parameters.AddWithValue("@GroupID", grp);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if (!String.IsNullOrEmpty(lec) && !String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%' AND Tag = @Tag";

                command.Parameters.AddWithValue("@Tag", tag);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
            else if (!String.IsNullOrEmpty(sub) && !String.IsNullOrEmpty(grp))
            {

                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Subject = @Subject AND GroupID = @GroupID";

                command.Parameters.AddWithValue("@Subject", sub);
                command.Parameters.AddWithValue("@GroupID", grp);

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }

            else if (!String.IsNullOrEmpty(sub) && !String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Subject = @Subject AND Tag = @Tag";

                command.Parameters.AddWithValue("@Subject", sub);
                command.Parameters.AddWithValue("@Tag", tag);

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }

            else if (!String.IsNullOrEmpty(grp) && !String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where GroupID = @GroupID AND Tag = @Tag";

                command.Parameters.AddWithValue("@GroupID", grp);
                command.Parameters.AddWithValue("@Tag", tag);

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }

            else if (!String.IsNullOrEmpty(lec))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Lecturers LIKE '%" + lec + "%'";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }

            else if (!String.IsNullOrEmpty(sub))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Subject = @Subject";

                command.Parameters.AddWithValue("@Subject", sub);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }

            else if (!String.IsNullOrEmpty(grp))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where GroupID = @GroupID";

                command.Parameters.AddWithValue("@GroupID", grp);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }

            else if (!String.IsNullOrEmpty(tag))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Sessions where Tag = @Tag";

                command.Parameters.AddWithValue("@Tag", tag);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;

                connection.Close();
            }
        }

        private void BtnClearFields_Click(object sender, RoutedEventArgs e)
        {
            SearchLecCombo.Text = "";
            SearchTagCombo.Text = "";
            SearchGrpCombo.Text = "";
            SearchSubCombo.Text = "";

            loadDataGrid();
        }
    }
}
