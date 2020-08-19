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
using TimeTableManagementSystem.interfaces.Subject;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class Subject : UserControl
    {
        public Subject()
        {
            InitializeComponent();
        }

        private void BtnViewSub_Click(object sender, RoutedEventArgs e)
        {
            View_Subjects view = new View_Subjects();
            view.Show();
        }

        private void BtnAddSub_Click(object sender, RoutedEventArgs e)
        {
            Add_Subjects add = new Add_Subjects();
            add.Show();
        }

       
    }
}
