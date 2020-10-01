using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.StudentTimeTable
{
    /// <summary>
    /// Interaction logic for StudentTimeTable.xaml
    /// </summary>
    public partial class StudentTimeTable : Window
    {
        private SQLiteConnection connection = db_config.connect();
        public StudentTimeTable()
        {
            InitializeComponent();
            fetchDataToComboBox();
        }



        public void fetchDataToComboBox()
        {
            //Degree Program Name
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Program";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_degree_name.ItemsSource = ds.Tables[0].DefaultView;
                cmb_degree_name.DisplayMemberPath = ds.Tables[0].Columns["programName"].ToString();
                cmb_degree_name.SelectedValuePath = ds.Tables[0].Columns["programCode"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            //Group Numbers

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select DISTINCT g.groupNo from Groups g";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_group.ItemsSource = ds.Tables[0].DefaultView;
                cmb_group.DisplayMemberPath = ds.Tables[0].Columns["groupNo"].ToString();
                cmb_group.SelectedValuePath = ds.Tables[0].Columns["groupNo"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }



            //SubGrup Numbers

            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select DISTINCT s.subgroupNo from SubGroup s";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmb_sub_group.ItemsSource = ds.Tables[0].DefaultView;
                cmb_sub_group.DisplayMemberPath = ds.Tables[0].Columns["subgroupNo"].ToString();
                cmb_sub_group.SelectedValuePath = ds.Tables[0].Columns["subgroupNo"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

        }


        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
