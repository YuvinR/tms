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

namespace TimeTableManagementSystem.interfaces.StudentOperations
{
    /// <summary>
    /// Interaction logic for Program.xaml
    /// </summary>
    public partial class Program : Window
    {

        private int id;
        private string programCode;
        private string programName;
        private SQLiteConnection connection = db_config.connect();


        public Program()
        {
            
            InitializeComponent();
            loadDataGrid();
        }


        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(pcCodeTxt.Text) && String.IsNullOrEmpty(pcnameTxt.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
            else
            {
                programCode = pcCodeTxt.Text;
                programName = pcnameTxt.Text;

                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    if (saveButton.Content.ToString() == "Save")
                    {
                        command.CommandText = "Insert into Program " +
                                               "(programCode, programName) " +
                                               "Values " +
                                               "(@programCode, @programName)";
                        command.Parameters.AddWithValue("@programCode", programCode);
                        command.Parameters.AddWithValue("@programName", programName);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("New Program Has been Added!");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurd");
                        }
                    }
                    else
                    {
                        var idx = int.Parse(idTxt.Text.ToString());
                        command.CommandText = "Update Program Set programCode = @programCode, programName = @programName Where id = @idx ";
                        command.Parameters.AddWithValue("@programCode", programCode);
                        command.Parameters.AddWithValue("@programName", programName);
                        command.Parameters.AddWithValue("@idx", idx);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Program Has been Updated!");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurd");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    loadDataGrid();
                }

            }
        }





        private void loadDataGrid()
        {
            connection.Open();
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Program";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                programsgrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = programsgrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["ID"].ToString());
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Program where id = @id";
                command.Parameters.AddWithValue("@id", id);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Programs Data has been Deleted!");
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



        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = programsgrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["ID"].ToString());
            idTxt.Visibility = Visibility.Visible;
            idTxt.IsEnabled = false;
            idTxt.Text = row.Row["ID"].ToString();
            pcCodeTxt.Text = row.Row["programCode"].ToString();
            pcnameTxt.Text = row.Row["programName"].ToString();
            saveButton.Content = "Edit";

        }


        private void Button_ClickClear(object sender, RoutedEventArgs e)
        {
            clear();
        }

        public void clear()
        {
            pcCodeTxt.Text = "";
            pcnameTxt.Text = "";
            idTxt.Text = "";
            idTxt.Visibility = Visibility.Hidden;
            saveButton.Content = "Save";
        }



    }
}
