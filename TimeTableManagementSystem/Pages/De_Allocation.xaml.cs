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
using TimeTableManagementSystem.interfaces.ManageRooms;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for De_Allocation.xaml
    /// </summary>
    public partial class De_Allocation : UserControl
    {
        public De_Allocation()
        {
            InitializeComponent();
        }

        private void BtnAddRoom(object sender, RoutedEventArgs e)
        {
            Allocate_Rooms allocate_Rooms = new Allocate_Rooms();
            allocate_Rooms.Show();
        }

        private void BtnReserveRoom(object sender, RoutedEventArgs e)
        {
            ReserveRooms reserveRooms = new ReserveRooms();
            reserveRooms.Show();
        }
    }
}
