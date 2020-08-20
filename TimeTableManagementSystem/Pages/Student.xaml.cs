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
using TimeTableManagementSystem.interfaces.StudentOperations;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        public Student()
        {
            InitializeComponent();
        }

        private void BtnYnSAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAcademicYandS addAcademicYandS = new AddAcademicYandS();
            addAcademicYandS.Show();
        }

        private void BtnProgAdd_Click(object sender, RoutedEventArgs e)
        {
            Program program = new Program();
            program.Show();
        }

        private void BtnGroup_Click(object sender, RoutedEventArgs e)
        {
            Group group = new Group();
            group.Show();
        }

        private void BtnSubGroup_Click(object sender, RoutedEventArgs e)
        {
            Sub_Group sub_Group = new Sub_Group();
            sub_Group.Show();
        }
    }
}
