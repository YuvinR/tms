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

namespace TimeTableManagementSystem.interfaces.ParallelSessions
{
    /// <summary>
    /// Interaction logic for ParralelSessions.xaml
    /// </summary>
    public partial class ParralelSessions : Window
    {
        List<int> selectedlist = new List<int>();

        private SQLiteConnection connection = db_config.connect();

        public ParralelSessions()
        {
            InitializeComponent();
            loadDataGrid();
        }

        private void SessionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void AddParallelClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = SessionDataGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["Session_ID"].ToString());
            selectedlist.Add(id);

        }


        private void RemoveParallelClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = SessionDataGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["Session_ID"].ToString());
            selectedlist.Remove(id);
        }

        private void Button_ClickMakePara(object sender, RoutedEventArgs e)
        {
            if (selectedlist.Count == 0)
            {
                MessageBox.Show("Please Select Sessions to Make Parallel");

            }

            else if (selectedlist.Count == 1)
            {
                MessageBox.Show("Please Select atleast 2 Sessions to Make Parallel");
            }
            else
            {
                Guid catNo = Guid.NewGuid();

                foreach (int item in selectedlist)
                {

                    connection.Open();

                    try
                    {
                        SQLiteCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;


                        command.CommandText = "Update Sessions Set ParallelCat = @ParallelCat Where Session_ID = @sID ";
                        command.Parameters.AddWithValue("@sID", item);
                        command.Parameters.AddWithValue("@ParallelCat", catNo.ToString());
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {


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
                        //loadDataGrid();
                    }
                }
                MessageBox.Show("Parallel Sessions Created!");
            }


        }


    }
}
