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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTableManagementSystem.DB_Config;
using TimeTableManagementSystem.interfaces.LectureHallTimeTable;
using TimeTableManagementSystem.interfaces.LecturerTimeTable;
using TimeTableManagementSystem.interfaces.StudentTimeTable;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Timetable.xaml
    /// </summary>
    public partial class Timetable : UserControl
    {

        private SQLiteConnection connection = db_config.connect();

        public Timetable()
        {
            InitializeComponent();
            //loadDataGrid();
        }


        private void BtnLecTime_Click(object sender, RoutedEventArgs e)
        {
            LecturerTimeTable lecturer_Time = new LecturerTimeTable();
            lecturer_Time.Show();
        }


        private void BtnStdTime_Click(object sender, RoutedEventArgs e)
        {
            StudentTimeTable student_Time = new StudentTimeTable();
            student_Time.Show();
        }

        private void BtnLecHallTime_Click(object sender, RoutedEventArgs e)
        {
            LectureHallTimeTable lectureHall_Time = new LectureHallTimeTable();
            lectureHall_Time.Show();
        }

       
    }
}
