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
using TimeTableManagementSystem.interfaces.Location;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : UserControl
    {
        public Location()
        {
            InitializeComponent();
        }

        private void BtnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            
            AddLocation Add_Location = new AddLocation();
            
            Add_Location.Show();
        }

        private void BtnViewLocation_Click(object sender, RoutedEventArgs e)
        {

            ViewLocation View_Location = new ViewLocation();

            View_Location.Show();
        }
    }
}
