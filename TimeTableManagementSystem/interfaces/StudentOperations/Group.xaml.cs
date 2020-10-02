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
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : Window
    {
        private SQLiteConnection connection = db_config.connect();

        public Group()
        {
            InitializeComponent();
            fillupComboAcYnS();
            fillupComboProgram();
            loadDataGrid();
        }

        private void fillupComboAcYnS()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from AcademicYnS";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds, "AcademicYnS");

                //foreach(DataRow dr in dataTable.Rows)
                //{

                //    comboacys.Items.Add(dr["Year"].ToString());
                //}
                comboacys.ItemsSource = ds.Tables[0].DefaultView;
                comboacys.DisplayMemberPath = ds.Tables[0].Columns["code"].ToString();
                comboacys.SelectedValuePath = ds.Tables[0].Columns["id"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }


        }


        private void fillupComboProgram()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Program";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds, "Program");

                //foreach(DataRow dr in dataTable.Rows)
                //{

                //    comboacys.Items.Add(dr["Year"].ToString());
                //}
                comboProgram.ItemsSource = ds.Tables[0].DefaultView;
                comboProgram.DisplayMemberPath = ds.Tables[0].Columns["programName"].ToString();
                comboProgram.SelectedValuePath = ds.Tables[0].Columns["id"].ToString();


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
            DataRowView row = groupsGrid.SelectedItem as DataRowView;
            int id = int.Parse(row.Row["ID"].ToString());


            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Groups where id = @id";
                command.Parameters.AddWithValue("@id", id);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {

                    command.CommandText = "Delete from SubGroup where groupId = @groupId";
                    command.Parameters.AddWithValue("@groupId", id.ToString());

                    int srow = command.ExecuteNonQuery();
                    if (rows > 0)
                    {

                        MessageBox.Show("Group Data has been Deleted!");
                    }
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

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(comboacys.Text) || String.IsNullOrEmpty(comboProgram.Text) || String.IsNullOrEmpty(groupNo.Text) || String.IsNullOrEmpty(groupid.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
            else
            {
                int no = int.Parse(groupNo.Text.ToString());
                int yns = int.Parse(comboacys.SelectedValue.ToString());
                int prgm = int.Parse(comboProgram.SelectedValue.ToString());
                string code = groupid.Text;

                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    if (saveButton.Content.ToString() == "Save")
                    {
                        command.CommandText = "Insert into Groups " +
                                               "(yearSem, groupNo,groupId,prgmID,isDeAllocated) " +
                                               "Values " +
                                               "(@yearSem, @groupNo,@groupId,@prgmID,@isDeAllocated)";
                        command.Parameters.AddWithValue("@yearSem", yns);
                        command.Parameters.AddWithValue("@groupNo", no);
                        command.Parameters.AddWithValue("@groupId", code);
                        command.Parameters.AddWithValue("@prgmID", prgm);
                        command.Parameters.AddWithValue("@isDeAllocated", 0);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("New Group Has been Added!");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurd");
                        }
                    }
                    else
                    {
                        var idx = int.Parse(idTxt.Text.ToString());
                        command.CommandText = "Update Groups Set yearSem = @yearSem, groupNo = @groupNo, groupId = @groupId,prgmID = @prgmID,isDeAllocated = @isDeAllocated Where id = @idx ";
                        command.Parameters.AddWithValue("@yearSem", yns);
                        command.Parameters.AddWithValue("@groupNo", no);
                        command.Parameters.AddWithValue("@groupId", code);
                        command.Parameters.AddWithValue("@idx", idx);
                        command.Parameters.AddWithValue("@prgmID", prgm);
                        command.Parameters.AddWithValue("@isDeAllocated", 0);
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
                    clear();
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

                command.CommandText = "Select * from Groups";

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
            var id = int.Parse(row.Row["ID"].ToString());
            idTxt.Visibility = Visibility.Visible;
            idTxt.IsEnabled = false;
            idTxt.Text = row.Row["ID"].ToString();
            comboacys.SelectedValue = int.Parse(row.Row["yearSem"].ToString());
            comboProgram.SelectedValue = int.Parse(row.Row["prgmID"].ToString());
            groupNo.Text = row.Row["groupNo"].ToString();
            saveButton.Content = "Edit";

            
        }

        private void Button_ClickGenerate(object sender, RoutedEventArgs e)
        {
            string no = groupNo.Text;
            string yns = comboacys.Text.ToString();
            string prgm = comboProgram.Text.ToString();
            
            groupid.Text = yns + "." + prgm + "." + no;
            groupid.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Visible;



        }

        private void Button_ClickClear(object sender, RoutedEventArgs e)
        {
            clear();
        }

        public void clear()
        {
            comboacys.Text = "";
            comboProgram.Text = "";
            groupNo.Text = "";
            groupid.Visibility = Visibility.Hidden;
            saveButton.Visibility = Visibility.Hidden;
            idTxt.Visibility = Visibility.Hidden;
            saveButton.Content = "Save";
        }


    }
}
