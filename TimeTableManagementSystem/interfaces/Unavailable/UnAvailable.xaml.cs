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
        }

        private void ListViewMenu_SelectionChanged()
        {
            int index = 0;

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new LecturerTb());
                    break;
                //case 1:
                //    GridPrincipal.Children.Clear();
                //    GridPrincipal.Children.Add(new Sessions());
                //    break;
                //case 2:
                //    GridPrincipal.Children.Clear();
                //    GridPrincipal.Children.Add(new GruopTb());
                //    break;
                //case 3:
                //    GridPrincipal.Children.Clear();
                //    GridPrincipal.Children.Add(new Student());
                //    break;
                //case 4:
                //    GridPrincipal.Children.Clear();
                //    GridPrincipal.Children.Add(new Subject());
                //    break;
                default:
                    break;
            }
        }

    }
}
