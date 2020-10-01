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
    /// Interaction logic for SubGroupTb.xaml
    /// </summary>
    public partial class SubGroupTb : UserControl
    {
        private SQLiteConnection connection = db_config.connect();

        public SubGroupTb()
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

                command.CommandText = "Select * from SubGroup";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                subgrpsGrid.ItemsSource = dataTable.DefaultView;
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
       



        }
    }
}
