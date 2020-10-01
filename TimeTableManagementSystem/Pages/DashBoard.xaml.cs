using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using LiveCharts;
using LiveCharts.Wpf;
using TimeTableManagementSystem.DB_Config;
using TimeTableManagementSystem.interfaces.Location;
using TimeTableManagementSystem.interfaces.Subject;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : UserControl
    {


        public DashBoard()
        {
            InitializeComponent();
            this.PieChartExample();
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public void PieChartExample()
        {
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;

            SQLiteConnection connection = db_config.connect();

            //Subject
            SQLiteCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "Select  Offered_Year, Count(Subject_Code) as Sub_Count From Subject Group by Offered_Year";

            SQLiteDataAdapter sqliteAdapter = new SQLiteDataAdapter(command);

            DataSet dataSet = new DataSet();

            sqliteAdapter.Fill(dataSet);
            //SQLiteDataReader reader = command.ExecuteReader();



            SeriesCollection series = new SeriesCollection();
            int i = 0;
            for (i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
            {
                String years = "Year " + dataSet.Tables[0].Rows[i].ItemArray[0].ToString();
                int subcount = (int)(long)(dataSet.Tables[0].Rows[i].ItemArray[1]);

                series.Add(new PieSeries() { Title = years, Values = new ChartValues<int> { subcount }, DataLabels = true, LabelPoint = PointLabel });
                SubPieChart.Series = series;
            }

            //Lecturer
            SQLiteCommand command1 = connection.CreateCommand();
            command1.CommandType = CommandType.Text;
            command1.CommandText = "Select  Level, Count(EmployeeID) as Lec_Count From Lecturer Group by Level";

            SQLiteDataAdapter sqliteAdapter1 = new SQLiteDataAdapter(command1);

            DataSet dataSet1 = new DataSet();

            sqliteAdapter1.Fill(dataSet1);
            SeriesCollection series1 = new SeriesCollection();
            
            for (i = 0; i <= dataSet1.Tables[0].Rows.Count - 1; i++)
            {
                String levels= dataSet1.Tables[0].Rows[i].ItemArray[0].ToString();
                int leccount = (int)(long)(dataSet1.Tables[0].Rows[i].ItemArray[1]);

                series1.Add(new PieSeries() { Title = levels, Values = new ChartValues<int> { leccount }, DataLabels = true, LabelPoint = PointLabel });
                LecPieChart.Series = series1;
            }

            //Student Group
          SQLiteCommand command2 = connection.CreateCommand();
            command2.CommandType = CommandType.Text;
            command2.CommandText = "Select  y.year, Count(s.id) as Group_Count From AcademicYnS y, SubGroup s Group by y.year";

            SQLiteDataAdapter sqliteAdapter2 = new SQLiteDataAdapter(command2);

            DataSet dataSet2 = new DataSet();

            sqliteAdapter2.Fill(dataSet2);
            SeriesCollection series2 = new SeriesCollection();

            for (i = 0; i <= dataSet2.Tables[0].Rows.Count - 1; i++)
            {
                String gyears = "Year "+ dataSet2.Tables[0].Rows[i].ItemArray[0].ToString();
                int groupcount = (int)(long)(dataSet2.Tables[0].Rows[i].ItemArray[1]);

                series2.Add(new PieSeries() { Title = gyears, Values = new ChartValues<int> { groupcount }, DataLabels = true, LabelPoint = PointLabel });
                GroupPieChart.Series = series2;
            } 

        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

    }
}
