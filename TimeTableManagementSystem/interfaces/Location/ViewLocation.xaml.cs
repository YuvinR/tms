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

namespace TimeTableManagementSystem.interfaces.Location
{
    /// <summary>
    /// Interaction logic for ViewLocation.xaml
    /// </summary>
    public partial class ViewLocation : Window
    {
        private SQLiteConnection connection = db_config.connect();

        private string bID;
        private string bName;
        private string rType;
        private string roomNo;
        private int rCapacity;

        public ViewLocation()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

                command.CommandText = "Select * from Location";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                dataAdapter.Fill(dataTable);

                LocDataGrid.ItemsSource = dataTable.DefaultView;
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

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = LocDataGrid.SelectedItem as DataRowView;
            bID = row.Row["ID"].ToString();

            connection.Open();
            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "Delete from Location where ID = @ID";
                command.Parameters.AddWithValue("@ID", bID);

                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Record has been Deleted Successfully!");
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
            DataRowView row = LocDataGrid.SelectedItem as DataRowView;

            UpdIDTxt.Text = row.Row["ID"].ToString();
            UpdBuildingTxt.Text = row.Row["BuildingName"].ToString();
            UpdRoomTypeCombo.Text = row.Row["RoomType"].ToString();
            UpdRoomNoTxt.Text = row.Row["RoomNumber"].ToString();
            UpdCapacityTxt.Text = row.Row["RoomCapacity"].ToString();

        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bID = UpdIDTxt.Text;
            bName= UpdBuildingTxt.Text;
            rType = UpdRoomTypeCombo.Text;
            roomNo = UpdRoomNoTxt.Text;
            rCapacity = int.Parse(UpdCapacityTxt.Text);
     
            connection.Open();

            try
            {

                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Update Location " +
                        "set BuildingName = @BuildingName, RoomType = @RoomType, RoomNumber = @RoomNumber, RoomCapacity = @RoomCapacity " +
                        "Where ID = @ID";

                cmd.Parameters.AddWithValue("@BuildingName", bName);
                cmd.Parameters.AddWithValue("@RoomType", rType);
                cmd.Parameters.AddWithValue("@RoomNumber", roomNo);
                cmd.Parameters.AddWithValue("@RoomCapacity", rCapacity);
                cmd.Parameters.AddWithValue("@ID", bID);

                int rows = cmd.ExecuteNonQuery();


                if (rows > 0)
                {
                    MessageBox.Show("Record has been Updated Successfully");
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
            clearFields();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTxt.Text;
            connection.Open();
            DataTable dataTable = new DataTable();

            SQLiteCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = "Select * from Location where BuildingName LIKE '%" + keyword + "' OR RoomNumber LIKE '%" + keyword + "%' OR RoomType Like '%" + keyword + "%'";

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

            dataAdapter.Fill(dataTable);

            LocDataGrid.ItemsSource = dataTable.DefaultView;

            connection.Close();
        }
        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTxt.Text))
            {
                loadDataGrid();
            }
        }
        private void clearFields()
        {
            UpdIDTxt.Text = "";
            UpdBuildingTxt.Text = "";
            UpdRoomTypeCombo.Text = "";
            UpdRoomNoTxt.Text = "";
            UpdCapacityTxt.Text = "";
        }
    }
}
