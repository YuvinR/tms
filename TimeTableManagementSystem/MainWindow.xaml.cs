using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using TimeTableManagementSystem.Pages;
using System.Data.SQLite;
using TimeTableManagementSystem.interfaces.ManageTimeSlots;
using TimeTableManagementSystem.interfaces.ManageWorkingDaysAndHours;

namespace TimeTableManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartupConfigurations stc = new StartupConfigurations();
            stc.initiate();

            //ManageTimeSlots manageTimeSlots = new ManageTimeSlots();
            //manageTimeSlots.Show();
            //this.Hide();

            //ManageWorkignDaysAndHours manageworkigndaysandhours = new ManageWorkignDaysAndHours();
            //manageworkigndaysandhours.Show();
            //this.Hide();


        }


        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DashBoard());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Working_Days_and_Hours());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Lecturer());
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Student());
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Subject());
                    break;
                case 5:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Location());
                    break;
                case 6:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Session());
                    break;
                case 7:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new De_Allocation());
                    break;
                case 8:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Timetable());
                    break;

                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }
    }
}
