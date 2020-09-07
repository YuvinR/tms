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
using TimeTableManagementSystem.interfaces.Sessions;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Session.xaml
    /// </summary>
    public partial class Session : UserControl
    {
        public Session()
        {
            InitializeComponent();
        }

        private void BtnAddSession(object sender, RoutedEventArgs e)
        {
            Add_Sessions add_Sessions = new Add_Sessions();
            add_Sessions.Show();
        }

        private void BtnViewSession(object sender, RoutedEventArgs e)
        {
            View_Sessions view_Sessions = new View_Sessions();
            view_Sessions.Show();
        }
    }
}
