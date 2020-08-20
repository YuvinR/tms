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

namespace TimeTableManagementSystem.interfaces.StudentOperations
{
    /// <summary>
    /// Interaction logic for AddAcademicYandS.xaml
    /// </summary>
    public partial class AddAcademicYandS : Window
    {
        public AddAcademicYandS()
        {
            InitializeComponent();
        }

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AcademicYearTxt.Text) && String.IsNullOrEmpty(AcademicSemTxt.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Inserting Data!");
            }
           
        }
    }
}
