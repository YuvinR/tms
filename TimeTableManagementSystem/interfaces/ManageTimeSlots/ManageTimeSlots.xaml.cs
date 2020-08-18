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

namespace TimeTableManagementSystem.interfaces.ManageTimeSlots
{
    /// <summary>
    /// Interaction logic for ManageTimeSlots.xaml
    /// </summary>
    public partial class ManageTimeSlots : Window
    {
        public ManageTimeSlots()
        {
            InitializeComponent();
        }

        private void getStartingTime(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem) startTime.SelectedItem;
            MessageBox.Show(comboBoxItem.Content.ToString());
        }

        private void getEndingTime(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem) endTime.SelectedItem;
            MessageBox.Show(comboBoxItem.Content.ToString());
        }

        private void addTimeSlots(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateTimeSlots(object sender, RoutedEventArgs e)
        {

        }
    }
}
