﻿using System;
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
using System.Data;
using System.Data.SQLite;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.Sessions
{
    /// <summary>
    /// Interaction logic for Add_Sessions.xaml
    /// </summary>
    public partial class Add_Sessions : Window
    {
        private string selectedLec;
        private string lecturers;
        private string subject;
        private string subCode;
        private string tag;
        private string groupID;
        private int stdCount;
        private int duration;

        private SQLiteConnection connection = db_config.connect();

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
            if (String.IsNullOrEmpty(SbjComboBox.Text) || String.IsNullOrEmpty(GrpIdComboBox.Text) || String.IsNullOrEmpty(TagComboBox.Text)
                && String.IsNullOrEmpty(StdCountTxt.Text) || String.IsNullOrEmpty(DurationTxt.Text))
            {
                MessageBox.Show("Please fill the Text Boxes Before Creating Sessions!");
            }
            else
            {
                connection.Open();
                lecturers = textListInput.Text;
                subject = SbjComboBox.Text;
                groupID = GrpIdComboBox.Text;
                tag = TagComboBox.Text;
                duration = int.Parse(DurationTxt.Text);
                stdCount = int.Parse(StdCountTxt.Text);

                try
                {
                    SQLiteCommand command = connection.CreateCommand();

                    command.CommandType = CommandType.Text;

                    command.CommandText = "Select Subject_Code from Subject Where Subject_Name = @Subject_Name";
                    command.Parameters.AddWithValue("@Subject", subject);

                    subCode = (string) command.ExecuteScalar();

                    command.CommandText = "Insert into Sessions " +
                                        "(Lecturers, Subject, Subject_Code, Tag, GroupID, Student_Count,Duration) " +
                                        "Values " +
                                        "(@Lecturers, @Subject, @Subject_Code, @Tag, @GroupID, @Student_Count, @Duration)";

                    command.Parameters.AddWithValue("@Lecturers",lecturers);
                    command.Parameters.AddWithValue("@Subject", subject);
                    command.Parameters.AddWithValue("@Subject_Code",subCode);
                    command.Parameters.AddWithValue("@Tag", tag);
                    command.Parameters.AddWithValue("@GroupID", groupID);
                    command.Parameters.AddWithValue("@Student_Count", stdCount);
                    command.Parameters.AddWithValue("@Duration",duration);
                 

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("New Session Has been Created.");
                    }
                    else
                    {
                        MessageBox.Show("Error Occurd");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                clearFields();
            }
        }

        private void BtnSelectSessions_Click(object sender, RoutedEventArgs e)
        {
            selectedLec = LctComboBox.Text;

            textListInput.Text = selectedLec + "\n" ;
        }

        private void loadComboBox()
        {
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Lecturer";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    LctComboBox.Items.Add(reader1.GetString("Name"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            //Get Subjects and add it to Combo Box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Subject";

                SQLiteDataReader reader2 = command.ExecuteReader();

                while (reader2.Read())
                {
                    SbjComboBox.Items.Add(reader2.GetString("Subject_Name"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                //Get Tags and add it to Combo Box
                command.CommandText = "Select * from Tag";

                SQLiteDataReader reader3 = command.ExecuteReader();

                while (reader3.Read())
                {
                    TagComboBox.Items.Add(reader3.GetString("tagName"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            try {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                //Get Groups and Add it to combobox
                command.CommandText = "Select * from Groups";

                SQLiteDataReader reader4 = command.ExecuteReader();

                while (reader4.Read())
                {
                    GrpIdComboBox.Items.Add(reader4.GetString("groupId"));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void clearFields()
        {
            LctComboBox.Text = "";
            //listViewInput.Items.Clear();
            textListInput.Text = "";
            SbjComboBox.Text = "";
            GrpIdComboBox.Text = "";
            TagComboBox.Text = "";
            StdCountTxt.Text = "";
            DurationTxt.Text = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadComboBox();
        }
    }
}
