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
    /// Interaction logic for SessionTb.xaml
    /// </summary>
    public partial class SessionTb : UserControl
    {
        private SQLiteConnection connection = db_config.connect();

        public SessionTb()
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

                command.CommandText = "Select * from Sessions Where isDeAllocated = @isDeAllocated";
                command.Parameters.AddWithValue("@isDeAllocated", 0);

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                SessionDataGrid.ItemsSource = dataTable.DefaultView;
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
            DataRowView row = SessionDataGrid.SelectedItem as DataRowView;
            var id = (row.Row["Session_ID"].ToString());

            connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;


                command.CommandText = "Update Sessions Set isDeAllocated = @isDeAllocated Where Session_ID = @sID ";
                command.Parameters.AddWithValue("@sID", id);
                command.Parameters.AddWithValue("@isDeAllocated", 1);
                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Session has been Unavailable!");

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

        private void SessionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
