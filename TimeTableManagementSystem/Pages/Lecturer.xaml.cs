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
using TimeTableManagementSystem.interfaces.Lecturer;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Lecturer.xaml
    /// </summary>
    public partial class Lecturer : UserControl
    {
        public Lecturer()
        {
            InitializeComponent();
        }

        private void BtnViewLec_Click(object sender, RoutedEventArgs e)
        {
            View_Lecturer view_Lecturer = new View_Lecturer();
            view_Lecturer.Show();
        }

        private void BtnAddLec_Click(object sender, RoutedEventArgs e)
        {
            Add_Lecturer add_Lecturer = new Add_Lecturer();
            add_Lecturer.Show();
        }
    }
}
