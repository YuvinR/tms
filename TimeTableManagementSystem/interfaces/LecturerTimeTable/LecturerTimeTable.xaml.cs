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

namespace TimeTableManagementSystem.interfaces.LecturerTimeTable
{
    /// <summary>
    /// Interaction logic for LecturerTimeTable.xaml
    /// </summary>
    public partial class LecturerTimeTable : Window
    {
        private SQLiteConnection connection = db_config.connect();
        public LecturerTimeTable()
        {
            InitializeComponent();
            fetchDataToComboBox();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_generate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(cmbLecturerName.SelectedValue.ToString());
        }

        public void fetchDataToComboBox()
        {
            try
            {
                DataTable dataTable = new DataTable();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "Select * from Lecturer";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);

                cmbLecturerName.ItemsSource = ds.Tables[0].DefaultView;
                cmbLecturerName.DisplayMemberPath = ds.Tables[0].Columns["Name"].ToString();
                cmbLecturerName.SelectedValuePath = ds.Tables[0].Columns["EmployeeID"].ToString();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

    }
}
