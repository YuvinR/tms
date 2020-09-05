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
        }

        private void loadDataGrid()
        {

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
        }
    }
}
