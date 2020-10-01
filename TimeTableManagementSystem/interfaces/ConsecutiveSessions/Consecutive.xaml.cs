using System;
using System.Collections;
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

namespace TimeTableManagementSystem.interfaces.ConsecutiveSessions
{
    /// <summary>
    /// Interaction logic for Consecutive.xaml
    /// </summary>
    public partial class Consecutive : Window
    {
        List<int> selectedlist = new List<int>();

        private SQLiteConnection connection = db_config.connect();

        public Consecutive()
        {
            InitializeComponent();
            fillupComboTags();
        }

        private void fillupComboTags()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Subject";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds, "Subject");

                comboSubjects.ItemsSource = ds.Tables[0].DefaultView;
                comboSubjects.DisplayMemberPath = ds.Tables[0].Columns["Subject_Code"].ToString();
                comboSubjects.SelectedValuePath = ds.Tables[0].Columns["Subject_Name"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }


        }

        private void Button_ClickGenerate(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(comboSubjects.Text))
            {
                MessageBox.Show("Subject is Required!");
            }
            else
            {
                string subject = comboSubjects.Text.ToString();
                connection.Open();
                try
                {
                    DataTable dataTable = new DataTable();

                    SQLiteCommand command = connection.CreateCommand();

                    command.CommandType = CommandType.Text;

                    command.CommandText = "Select * from Sessions Where isDeAllocated = @isDeAllocated AND Subject_Code = @subCode";
                    command.Parameters.AddWithValue("@isDeAllocated", 0);
                    command.Parameters.AddWithValue("@subCode", subject);

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
        }

        private void SessionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void AddConsecutiveClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = SessionDataGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["Session_ID"].ToString());
            selectedlist.Add(id);

        }


        private void RemoveCosecutiveClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = SessionDataGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["Session_ID"].ToString());
            selectedlist.Remove(id);
        }

        private void Button_ClickMakeConsec(object sender, RoutedEventArgs e)
        {
            if(selectedlist.Count == 0)
            {
                MessageBox.Show("Please Select Sessions to Make Consecutive");
                
            }

            else if (selectedlist.Count == 1)
            {
                MessageBox.Show("Please Select atleast 2 Sessions to Make Consecutive");
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


                        command.CommandText = "Update Sessions Set ConsecutiveCat = @ConsecutiveCat Where Session_ID = @sID ";
                        command.Parameters.AddWithValue("@sID", item);
                        command.Parameters.AddWithValue("@ConsecutiveCat", catNo.ToString());
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
                MessageBox.Show("Consecutive Sessions Created!");
            }

            
        }



    }
}
