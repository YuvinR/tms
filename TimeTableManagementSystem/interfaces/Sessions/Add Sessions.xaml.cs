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
            bool returnExsits = false;
            //bool returnExsits = checkDBValues();

            /*if (String.IsNullOrEmpty(textListInput.Text) || String.IsNullOrEmpty(SbjComboBox.Text) || String.IsNullOrEmpty(GrpIdComboBox.Text) || String.IsNullOrEmpty(TagComboBox.Text)
                || String.IsNullOrEmpty(StdCountTxt.Text) || String.IsNullOrEmpty(DurationTxt.Text))
            {*/
            for(int i = 0; i < listViewInput.Items.Count; i++)
            {
                lecturers = lecturers + listViewInput.Items[i] + "\n";
            }

            if ((listViewInput.Items.Count == 0 ) || String.IsNullOrEmpty(SbjComboBox.Text) || String.IsNullOrEmpty(GrpIdComboBox.Text) || String.IsNullOrEmpty(TagComboBox.Text)
                || String.IsNullOrEmpty(StdCountTxt.Text) || String.IsNullOrEmpty(DurationTxt.Text))
            {
                    MessageBox.Show("Please fill the Text Boxes Before Creating Sessions!");
            }else if(returnExsits == true)
            {
                MessageBox.Show("Same Session Details are Already Added to DB!");
            }
            else
            {
                connection.Open();
                //lecturers = textListInput.Text;
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
                    command.Parameters.AddWithValue("@Subject_Name", subject);

                    subCode = (string)command.ExecuteScalar();

                    command.CommandText = "Insert into Sessions " +
                                        "(Lecturers, Subject, Subject_Code, Tag, GroupID, Student_Count,Duration,isDeAllocated) " +
                                        "Values " +
                                        "(@Lecturers, @Subject, @Subject_Code, @Tag, @GroupID, @Student_Count, @Duration,0)";

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

                    //With the List view. Use this Method
                    addingToOtherTable();
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

        //With the List view. Implementation of the method
        private void addingToOtherTable()
        {
            string Sellec = "";
            bool done = false; 
            
            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;

                command.CommandText = "Select MAX(Session_ID) from Sessions";

                int sesID = (int)(long)command.ExecuteScalar();

                for (int i = 0; i < listViewInput.Items.Count; i++)
                {
                    Sellec = (string)listViewInput.Items[i];
                    command.CommandText = "Insert into Subject_Lecturers " +
                                        "(sessionID, Lecturers ) " +
                                        "Values " +
                                        "(@sessionID, @Lecturers )";

                    command.Parameters.AddWithValue("@sessionID", sesID);
                    command.Parameters.AddWithValue("@Lecturers", Sellec) ;

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        done = true;
                    }
                    else
                    {
                        done = false;
                    }
                }

                if (done == true)
                {
                    MessageBox.Show("lecturers has been added");
                }
                else
                {
                    MessageBox.Show("Error Occurd in adding lecturers to DB");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
               
            finally
            {
               
            }
        }

        private bool checkDBValues()
        {
            bool exsits = false;
            //lecturers = textListInput.Text;
            subject = SbjComboBox.Text;
            groupID = GrpIdComboBox.Text;
            tag = TagComboBox.Text;

            connection.Open();

            try
            {

                
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;

                command.CommandText = "Select Lecturers, Subject, Tag, GroupID from Sessions";

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet);

                int i = 0;
                for (i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
                {
                    String dbLec = dataSet.Tables[0].Rows[i].ItemArray[0].ToString();
                    String dbSub = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                    String dbTag = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    String dbGrp = dataSet.Tables[0].Rows[i].ItemArray[3].ToString();

                    if(lecturers.Equals(dbLec) && subject.Equals(dbSub) && groupID.Equals(dbGrp) && tag.Equals(dbTag))
                    {
                        exsits = true;
                        break;
                    }
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

            return exsits;
        }

        private void BtnSelectSessions_Click(object sender, RoutedEventArgs e)
        {
            selectedLec = LctComboBox.Text;
            /*if (String.IsNullOrEmpty(textListInput.Text))
            {
                textListInput.Text =selectedLec;
            }
            else
            {
                textListInput.Text = textListInput.Text + "\n" + selectedLec;
            }*/

            listViewInput.Items.Add(selectedLec);
        }

        private void loadComboBox()
        {
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Lecturer Where isDeAllocated = 0";

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
           
        }

        private void clearFields()
        {
            LctComboBox.Text = "";
            //textListInput.Text = "";
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

        private void BtnClearLecturers_Click(object sender, RoutedEventArgs e)
        {
            //textListInput.Text = "";
        }

        private void TagComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (TagComboBox.Text.ToLower() == "lecture" || TagComboBox.Text.ToLower() == "tutorial")
            {
                GrpIdComboBox.Items.Clear();
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    //Get Groups and Add it to combobox
                    command.CommandText = "Select * from Groups Where isDeAllocated = 0";

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
            else if (TagComboBox.Text.ToLower() == "practical")
            {
                GrpIdComboBox.Items.Clear();
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    //Get Groups and Add it to combobox
                    command.CommandText = "Select * from SubGroup Where isDeAllocated = 0";

                    SQLiteDataReader reader4 = command.ExecuteReader();

                    while (reader4.Read())
                    {
                        GrpIdComboBox.Items.Add(reader4.GetString("subgroupId"));
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
        }
    }
}
