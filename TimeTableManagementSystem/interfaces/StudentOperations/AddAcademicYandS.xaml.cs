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
    /// Interaction logic for AddAcademicYandS.xaml
    /// </summary>
    public partial class AddAcademicYandS : Window
    {
        private SQLiteConnection connection = db_config.connect();

        private int id;
        private string year;
        private string semester;

        public AddAcademicYandS()
        {
            InitializeComponent();
            loadDataGrid();
        }

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            
                if (String.IsNullOrEmpty(AcademicYearTxt.Text) || string.IsNullOrEmpty(AcademicSemTxt.Text))
                {
                    MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
                }
                else
                {
                    year = AcademicYearTxt.Text;
                    semester = AcademicSemTxt.Text;
                    string code = "Y"+year + "." + "S" + semester;
                    try
                    {
                        connection.Open();
                        SQLiteCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                    if (saveButton.Content.ToString() == "Save")
                    {
                        command.CommandText = "Insert into AcademicYnS " +
                                               "(year, semester,code) " +
                                               "Values " +
                                               "(@year, @semester,@code)";
                        command.Parameters.AddWithValue("@year", year);
                        command.Parameters.AddWithValue("@semester", semester);
                        command.Parameters.AddWithValue("@code", code);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("New Academic Year And Semester Has been Added!");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurd");
                        }
                    }
                    else
                    {
                        var idx = int.Parse(idTxt.Text.ToString());
                        command.CommandText = "Update AcademicYnS Set year = @year, semester = @semester, code = @code Where id = @idx ";
                        command.Parameters.AddWithValue("@year", year);
                        command.Parameters.AddWithValue("@semester", semester);
                        command.Parameters.AddWithValue("@code", "Y" + year + "." + "S" + semester);
                        command.Parameters.AddWithValue("@idx", idx);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Academic Year And Semester Has been Updated!");
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

                command.CommandText = "Select * from AcademicYnS";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                AcYnSDataGrid.ItemsSource = dataTable.DefaultView;
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
            DataRowView row = AcYnSDataGrid.SelectedItem as DataRowView;
            id = int.Parse(row.Row["ID"].ToString());

            
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from AcademicYnS where id = @id";
                command.Parameters.AddWithValue("@id", id);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("academic Year And Sem Data has been Deleted!");
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
            DataRowView row = AcYnSDataGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["ID"].ToString());
            idTxt.Visibility = Visibility.Visible;
            idTxt.IsEnabled = false;
            idTxt.Text = row.Row["ID"].ToString();
            AcademicYearTxt.Text = row.Row["year"].ToString();
            AcademicSemTxt.Text = row.Row["semester"].ToString();
            saveButton.Content = "Edit";

        }

        private void Button_ClickClear(object sender, RoutedEventArgs e)
        {
            clear();
        }

        public void clear()
        {
             AcademicYearTxt.Text ="";
             AcademicSemTxt.Text = "";
            idTxt.Text = "";
            idTxt.Visibility = Visibility.Hidden;
            saveButton.Content = "Save";
        }


    }
}
