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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTableManagementSystem.interfaces.ManageTimeSlots;
using TimeTableManagementSystem.interfaces.ManageWorkingDaysAndHours;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Working_Days_and_Hours.xaml
    /// </summary>
    public partial class Working_Days_and_Hours : UserControl
    {
        public Working_Days_and_Hours()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ManageWorkignDaysAndHours manageWorkignDaysAndHours = new ManageWorkignDaysAndHours();
            manageWorkignDaysAndHours.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ManageTimeSlots manageTimeSlots = new ManageTimeSlots();
            manageTimeSlots.Show();
        }
    }
}
