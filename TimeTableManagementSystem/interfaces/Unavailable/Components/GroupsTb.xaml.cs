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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.Unavailable.Components
{
    /// <summary>
    /// Interaction logic for GroupsTb.xaml
    /// </summary>
    public partial class GroupsTb : UserControl
    {
        private SQLiteConnection connection = db_config.connect();

        public GroupsTb()
        {
            InitializeComponent();
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

                command.CommandText = "Select * from Groups  Where isDeAllocated = @isDeAllocated";
                command.Parameters.AddWithValue("@isDeAllocated", 0);

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                groupsGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = groupsGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["id"].ToString());

            connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;


                command.CommandText = "Update Groups Set isDeAllocated = @isDeAllocated Where id = @GID ";
                command.Parameters.AddWithValue("@GID", id);
                command.Parameters.AddWithValue("@isDeAllocated", 1);
                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Group has been Unavailable!");

                }
                else
                {
                    MessageBox.Show("Error Occurd");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occurd" + ex);
            }
            finally
            {
                connection.Close();
                loadDataGrid();
            }


        }
    }
}
