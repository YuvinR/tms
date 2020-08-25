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

namespace TimeTableManagementSystem.interfaces.Sessions
{
    /// <summary>
    /// Interaction logic for Add_Sessions.xaml
    /// </summary>
    public partial class Add_Sessions : Window
    {
        public Add_Sessions()
        {
            InitializeComponent();
        }

        private void BtnViewSessions_Click(object sender, RoutedEventArgs e)
        {
            View_Sessions view = new View_Sessions();
            this.Close();
            view.Show();
        }

        private void BtnCreateSessions_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
