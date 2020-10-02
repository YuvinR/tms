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
    /// Interaction logic for Sub_Group.xaml
    /// </summary>
    public partial class Sub_Group : Window
    {

        private SQLiteConnection connection = db_config.connect();

        public Sub_Group()
        {
            InitializeComponent();
            fillupComboGroup();
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

                command.CommandText = "Select * from SubGroup ";

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


        private void fillupComboGroup()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Groups";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds, "Groups");

                //foreach(DataRow dr in dataTable.Rows)
                //{

                //    comboacys.Items.Add(dr["Year"].ToString());
                //}
                comboGroup.ItemsSource = ds.Tables[0].DefaultView;
                comboGroup.DisplayMemberPath = ds.Tables[0].Columns["groupId"].ToString();
                comboGroup.SelectedValuePath = ds.Tables[0].Columns["id"].ToString();


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
            string no = subgroupnoTxt.Text;
            string grp = comboGroup.Text.ToString();

            subgroupID.Text = grp + "." + no;
            subgroupID.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Visible;
        }


        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(comboGroup.Text) || String.IsNullOrEmpty(subgroupnoTxt.Text) || String.IsNullOrEmpty(subgroupID.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
            else
            {
                int no = int.Parse(subgroupnoTxt.Text.ToString());
                int yns = int.Parse(comboGroup.SelectedValue.ToString());
                string code = subgroupID.Text.ToString();

                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    if (saveButton.Content.ToString() == "Save")
                    {
                        command.CommandText = "Insert into SubGroup" +
                                               "(groupId, subgroupNo,subgroupId,isDeAllocated) " +
                                               "Values " +
                                               "(@groupId, @subgroupNo,@subgroupId,@isDeAllocated)";
                        command.Parameters.AddWithValue("@groupId", yns);
                        command.Parameters.AddWithValue("@subgroupNo", no);
                        command.Parameters.AddWithValue("@subgroupId", code);
                        command.Parameters.AddWithValue("@isDeAllocated", 0);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("New Sub-Group Has been Added!");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurd");
                        }
                    }
                    else
                    {
                        var idx = int.Parse(idTxt.Text.ToString());
                        command.CommandText = "Update SubGroup Set groupId = @groupId, subgroupNo = @subgroupNo, subgroupId = @subgroupId,isDeAllocated = @isDeAllocated Where id = @idx ";
                        command.Parameters.AddWithValue("@groupId", yns);
                        command.Parameters.AddWithValue("@subgroupNo", no);
                        command.Parameters.AddWithValue("@subgroupId", code);
                        command.Parameters.AddWithValue("@idx", idx);
                        command.Parameters.AddWithValue("@isDeAllocated", 0);
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("SubGroupData Has been Updated!");
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

        private void Button_ClickClear(object sender, RoutedEventArgs e)
        {
            clear();
        }

        public void clear()
        {
            comboGroup.Text = "";
            subgroupnoTxt.Text = "";
            subgroupID.Visibility = Visibility.Hidden;
            saveButton.Visibility = Visibility.Hidden;
            idTxt.Visibility = Visibility.Hidden;
            saveButton.Content = "Save";
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = subgrpsGrid.SelectedItem as DataRowView;
            int id = int.Parse(row.Row["ID"].ToString());


            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from SubGroup where id = @id";
                command.Parameters.AddWithValue("@id", id);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Sub_Group Data has been Deleted!");
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
            DataRowView row = subgrpsGrid.SelectedItem as DataRowView;
            var id = int.Parse(row.Row["ID"].ToString());
            idTxt.Visibility = Visibility.Visible;
            idTxt.IsEnabled = false;
            idTxt.Text = row.Row["ID"].ToString();
            comboGroup.SelectedValue = int.Parse(row.Row["groupId"].ToString());
            subgroupnoTxt.Text = row.Row["subgroupNo"].ToString();
            saveButton.Content = "Edit";


        }



    }





}
