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
using TimeTableManagementSystem.interfaces.Unavailable.Components;
using TimeTableManagementSystem.Pages;

namespace TimeTableManagementSystem.interfaces.Unavailable
{
    /// <summary>
    /// Interaction logic for UnAvailable.xaml
    /// </summary>
    public partial class UnAvailable : Window
    {
        public UnAvailable()
        {
            InitializeComponent();
            Lecs.IsChecked = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            var txt =  li.Content.ToString();
       

            switch (txt)
            {
                case "Lecturers":
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new LecturerTb());
                    break;
                case "Sessions":
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new SessionTb());
                    break;
                case "Groups":
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new GroupsTb());
                    break;
                case "Sub-Groups":
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new SubGroupTb());
                    break;
                default:
                    break;
            }
        }

    }
}
