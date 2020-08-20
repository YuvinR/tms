using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Tag.xaml
    /// </summary>
    public partial class Tag : UserControl
    {
        public Tag()
        {
            InitializeComponent();
        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            Sub_Group sub_Group = new Sub_Group();
            sub_Group.Show();
        }
    }


}
