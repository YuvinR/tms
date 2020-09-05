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
    /// Interaction logic for TagOp.xaml
    /// </summary>
    public partial class TagOp : Window
    {
        private SQLiteConnection connection = db_config.connect();

        private int id;
        private string tagname;
        private string tagcode;

        public TagOp()
        {
            InitializeComponent();
            loadDataGrid();
        }

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(nameTxt.Text) && String.IsNullOrEmpty(codeTxt.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
            else
            {
                tagname = nameTxt.Text;
                tagcode = codeTxt.Text;

                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    if (saveButton.Content.ToString() == "Save")
                    {
                        command.CommandText = "Insert into Tag " +
                                               "(tagCode, tagName) " +
                                               "Values " +
                                               "(@tagcode, @tagname)";
                        command.Parameters.AddWithValue("@tagcode", tagcode);
                        command.Parameters.AddWithValue("@tagname", tagname);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("New Tag Has been Added!");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurd");
                        }
                    }
                    else
                    {
                        var idx = int.Parse(idTxt.Text.ToString());
                        command.CommandText = "Update Tag Set tagCode = @tagcode, tagName = @tagname Where id = @idx ";
                        command.Parameters.AddWithValue("@tagcode", tagcode);
                        command.Parameters.AddWithValue("@tagname", tagname);
                        command.Parameters.AddWithValue("@idx", idx);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Tag Has been Updated!");
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

                command.CommandText = "Select * from Tag";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                tagsGrid.ItemsSource = dataTable.DefaultView;
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
            DataRowView row = tagsGrid.SelectedItem as DataRowView;
            id = int.Parse(row.Row["ID"].ToString());

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Tag where id = @id";
                command.Parameters.AddWithValue("@id", id);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Tag has been Deleted!");
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
            DataRowView row = tagsGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["ID"].ToString());
            idTxt.Visibility = Visibility.Visible;
            idTxt.IsEnabled = false;
            idTxt.Text = row.Row["ID"].ToString();
            codeTxt.Text = row.Row["tagCode"].ToString();
            nameTxt.Text = row.Row["tagName"].ToString();
            saveButton.Content = "Edit";

        }

        private void Button_ClickClear(object sender, RoutedEventArgs e)
        {
            clear();
        }

        public void clear()
        {
            codeTxt.Text = "";
            nameTxt.Text = "";
            idTxt.Text = "";
            idTxt.Visibility = Visibility.Hidden;
            saveButton.Content = "Save";
        }


    }
}
