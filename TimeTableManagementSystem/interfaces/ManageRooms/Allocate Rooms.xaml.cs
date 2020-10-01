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
using System.Windows.Shapes;
using TimeTableManagementSystem.DB_Config;

namespace TimeTableManagementSystem.interfaces.ManageRooms
{
    /// <summary>
    /// Interaction logic for Allocate_Rooms.xaml
    /// </summary>
    public partial class Allocate_Rooms : Window
    {
        private string selectedRoom;
        private string rooms;
        private string tag;
        private string lec;
        private string subgroup;
        private string group;
        private string subject;
        private SQLiteConnection connection = db_config.connect();

        public Allocate_Rooms()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadComboBox();
        }

        private void BtnSelectRooms_Click(object sender, RoutedEventArgs e)
        {
    
            selectedRoom = SearchRoomCombo.Text;
            listViewRooms.Items.Add(selectedRoom);
        }

        private void BtnAllocateRoom_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < listViewRooms.Items.Count; i++)
            {
                rooms = rooms + listViewRooms.Items[i] + "\n";
            }

            connection.Open();
            tag = SearchTagCombo.Text;
            lec = SearchLecCombo.Text;
            subgroup = SearchSubGroupCombo.Text;
            group = SearchGroupCombo.Text;
            subject = SearchSubjCombo.Text;

            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandType = CommandType.Text;

                if (!String.IsNullOrEmpty(SearchTagCombo.Text) && !String.IsNullOrEmpty(SearchRoomCombo.Text))
                {
                    command.CommandText = "Insert into Room_Tag " +
                    "(RoomID, TagID) " +
                    "Values " +
                    "(@RoomID, @TagID)";

                    command.Parameters.AddWithValue("@RoomID", rooms);
                    command.Parameters.AddWithValue("@TagID", tag);
                }


                if (!String.IsNullOrEmpty(SearchLecCombo.Text) && !String.IsNullOrEmpty(SearchRoomCombo.Text))
                {
                    command.CommandText = "Insert into Room_Lec " +
                    "(RoomID, LecID) " +
                    "Values " +
                    "(@RoomID, @LecID)";

                    command.Parameters.AddWithValue("@RoomID", rooms);
                    command.Parameters.AddWithValue("@LecID", lec);
                }

                if (!String.IsNullOrEmpty(SearchGroupCombo.Text) && !String.IsNullOrEmpty(SearchRoomCombo.Text))
                {
                    command.CommandText = "Insert into Room_Group " +
                    "(RoomID, GroupID, SubGroupID) " +
                    "Values " +
                    "(@RoomID, @GroupID, 0)";

                    command.Parameters.AddWithValue("@RoomID", rooms);
                    command.Parameters.AddWithValue("@GroupID", group);
                }

                if (!String.IsNullOrEmpty(SearchSubGroupCombo.Text) && !String.IsNullOrEmpty(SearchRoomCombo.Text))
                {
                    command.CommandText = "Insert into Room_Group " +
                    "(RoomID, SubGroupID, GroupID) " +
                    "Values " +
                    "(@RoomID, @SubGroupID, 0)";

                    command.Parameters.AddWithValue("@RoomID", rooms);
                    command.Parameters.AddWithValue("@SubGroupID", subgroup);
                }

                if (!String.IsNullOrEmpty(SearchSubjCombo.Text) && !String.IsNullOrEmpty(SearchRoomCombo.Text) && !String.IsNullOrEmpty(SearchTagCombo.Text))
                {
                    command.CommandText = "Insert into Room_Sub_Tag " +
                    "(RoomID, SubID, TagID) " +
                    "Values " +
                    "(@RoomID, @SubID, @TagID)";

                    command.Parameters.AddWithValue("@RoomID", rooms);
                    command.Parameters.AddWithValue("@SubID", subject);
                    command.Parameters.AddWithValue("@TagID", tag);
                }

                if (!String.IsNullOrEmpty(SearchSessionCombo.Text))
                {

                }


                int rows = command.ExecuteNonQuery();



                if (rows > 0)
                {
                    MessageBox.Show("Condition has been added.");
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

        private void clearFields()
        {
            SearchRoomCombo.Text = "";
            SearchLecCombo.Text = "";
            SearchGroupCombo.Text = "";
            SearchSubGroupCombo.Text = "";
            SearchSubjCombo.Text = "";
            SearchTagCombo.Text = "";
            listViewRooms.Items.Clear();
        }

        private void loadComboBox()        
        {

            //populate tag combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Tag";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchTagCombo.Items.Add(reader1.GetString("tagName"));
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

            //populate lecturer combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Lecturer";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchLecCombo.Items.Add(reader1.GetString("Name"));
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

            //populate sub-group combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from SubGroup";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchSubGroupCombo.Items.Add(reader1.GetString("subgroupId"));
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

            //populate group combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Groups";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchGroupCombo.Items.Add(reader1.GetString("groupId"));
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

            //populate subject combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Subject";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchSubjCombo.Items.Add(reader1.GetString("Subject_Name"));
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

            //populate session combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Sessions";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchSessionCombo.Items.Add(reader1.GetString("Session_ID"));
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

            //populate rooms combo box
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from Location";

                SQLiteDataReader reader1 = command.ExecuteReader();

                while (reader1.Read())
                {
                    SearchRoomCombo.Items.Add(reader1.GetString("RoomNumber"));
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
