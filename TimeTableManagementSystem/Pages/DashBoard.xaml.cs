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

            /* SQLiteConnection connection = db_config.connect();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "Select Count(Subject_Code), Offered_Year from Subject group by Offered_Year";
            var dataSet = new DataSet();
            SQLiteDataAdapter sqliteAdapter = new SQLiteDataAdapter(command);
            sqliteAdapter.Fill(dataSet);
            SeriesCollection series = new SeriesCollection();
            int i = 0;
            for (i=0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
            {
                int subcount = (int)dataSet.Tables[0].Rows[i].ItemArray[0];
                String years = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                series.Add(new PieSeries() { Title = years, Values = new ChartValues<int> { subcount }, DataLabels = true, LabelPoint = labelPoint });
          

            } */

            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
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
