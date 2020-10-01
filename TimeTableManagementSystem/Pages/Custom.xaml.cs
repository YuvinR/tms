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
using TimeTableManagementSystem.interfaces.ConsecutiveSessions;
using TimeTableManagementSystem.interfaces.NonOverlap;
using TimeTableManagementSystem.interfaces.ParallelSessions;
using TimeTableManagementSystem.interfaces.StudentOperations;
using TimeTableManagementSystem.interfaces.Unavailable;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Custom.xaml
    /// </summary>
    public partial class Custom : UserControl
    {
        public Custom()
        {
            InitializeComponent();
        }

        private void BtnYnSAdd_Click(object sender, RoutedEventArgs e)
        {
            UnAvailable unava = new UnAvailable();
            unava.Show();
        }

        private void Btn_ConsecutiveClick(object sender, RoutedEventArgs e)
        {
            Consecutive consec = new Consecutive();
            consec.Show();
        }

        private void Btn_ParallelClick(object sender, RoutedEventArgs e)
        {
            ParralelSessions consec = new ParralelSessions();
            consec.Show();
        }

        private void Btn_NonOvClick(object sender, RoutedEventArgs e)
        {
            NonOverlap consec = new NonOverlap();
            consec.Show();
        }
    }
}
