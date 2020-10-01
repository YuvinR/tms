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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.Unavailable.Components
{
    /// <summary>
    /// Interaction logic for LecturerTb.xaml
    /// </summary>
    public partial class LecturerTb : UserControl
    {
        private SQLiteConnection connection = db_config.connect();

        public LecturerTb()
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
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LecDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
