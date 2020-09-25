using System;
using System.Collections.Generic;
using System.Text;
using TimeTableManagementSystem.DB_Config;
using System.Data.SQLite;
using System.Data;

namespace TimeTableManagementSystem
{
    public class StartupConfigurations
    {
        private SQLiteConnection connection = db_config.connect();
       
        public void initiate()
        {
            
           
            try
            {
                if (!checkIfExist("AcademicYnS"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE AcademicYnS (id Integer Primary Key Autoincrement,year varchar(100),semester varchar(100),code varchar(100) );";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Program"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Program(id Integer Primary Key Autoincrement,programCode varchar(100),programName varchar(100) );";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Groups"))
                {

                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Groups(id Integer Primary Key Autoincrement,yearSem Integer,groupNo Integer,groupId varchar(100),prgmID Integer, isDeAllocated INTEGER CHECK(isDeAllocated = 0 OR isDeAllocated = 1));";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("SubGroup"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE SubGroup(id Integer Primary Key Autoincrement,groupId varchar(100),subgroupNo Integer,subgroupId varchar(100), isDeAllocated INTEGER CHECK(isDeAllocated = 0 OR isDeAllocated = 1));";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Tag"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Tag(id Integer Primary Key Autoincrement,tagCode varchar(100),tagName varchar(100) );";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Lecturer"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    /*cmd.CommandText = "CREATE TABLE Lecturer ( " +
                                        "EmployeeID    TEXT, " +
                                        "Name  TEXT, " +
                                        "Faculty   TEXT, " +
                                        "Department    TEXT, " +
                                        "Center    TEXT, " +
                                        "Building  TEXT, " +
                                        "Level TEXT, " +
                                        "Rank  REAL, " +
                                        "PRIMARY KEY(EmployeeID));";*/

                    cmd.CommandText = "CREATE TABLE Lecturer (EmployeeID TEXT Primay Key, Name  TEXT, Faculty TEXT, Department TEXT, Center TEXT, Building  TEXT, Level TEXT, Rank REAL, isDeAllocated INTEGER CHECK(isDeAllocated = 0 OR isDeAllocated = 1));";
                                      

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                if (!checkIfExist("Subject"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    /*cmd.CommandText = "CREATE TABLE Subject ( " +
                                        "Subject_Code  TEXT, " +
                                        "Subject_Name TEXT, " +
                                        "Offered_Year  TEXT, " +
                                        "Offered_Semester   TEXT, " +
                                        "Lecture_Hours  INTEGER, " +
                                        "Tutorial_Hours INTEGER, " +
                                        "Lab_Hours INTEGER, " +
                                        "Evaluation_Hours  INTEGER, " +
                                        "PRIMARY KEY(Subject_Code));";*/

                    cmd.CommandText = "CREATE TABLE Subject (Subject_Code TEXT Primary Key, Subject_Name TEXT, Offered_Year TEXT, Offered_Semester TEXT, Lecture_Hours INTEGER, Tutorial_Hours INTEGER, Lab_Hours INTEGER, Evaluation_Hours  INTEGER);";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Sessions"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    /*cmd.CommandText = "CREATE TABLE 'Sessions' ( " +
                                        "Session_ID  INTEGER Autoincrement, " +
                                        "Lecturers TEXT, " +
                                        "Subject  TEXT, " +
                                        "Subject_Code   TEXT, "+
                                        "Tag  TEXT, " +
                                        "GroupID TEXT, " +
                                        "Student_Count INTEGER, " +
                                        "Duration  INTEGER, " +
                                        "PRIMARY KEY(Session_ID));";*/

                    cmd.CommandText = "CREATE TABLE Sessions(Session_ID INTEGER Primary Key Autoincrement, Lecturers TEXT, Subject TEXT, Subject_Code TEXT, Tag TEXT, GroupID TEXT, Student_Count INTEGER,Duration  INTEGER, isDeAllocated INTEGER CHECK(isDeAllocated = 0 OR isDeAllocated = 1), NonOverLapCat varchar(100),ConsecutiveCat varchar(100),ParallelCat varchar(100));";


                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                //Wickramanayake T.R.D db scripts


                if (!checkIfExist("working_week"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE working_week (week_id INTEGER NOT NULL, " +
                                        "week_type TEXT NOT NULL, " +
                                        "no_of_days INTEGER NOT NULL, "+
                                        "per_day_time TEXT NOT NULL,	" +
                                        "starting_time TEXT NOT NULL, " +
                                        "ending_time TEXT NOT NULL,	" +
                                        "day_name TEXT NOT NULL); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                //Wickramanayake T.R.D db scripts
                if (!checkIfExist("Location"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Location (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "BuildingName TEXT NOT NULL, " +
                                        "RoomType TEXT NOT NULL,	" +
                                        "RoomNumber TEXT NOT NULL, " +
                                        "RoomCapacity INTEGER NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                //Wickramanayake T.R.D db scripts
                if (!checkIfExist("time_slots"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE time_slots ( id INTEGER, start_time TEXT NOT NULL, end_time TEXT NOT NULL, time_duration TEXT, PRIMARY KEY(id AUTOINCREMENT)); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                //Wickramanayake T.R.D db scripts

                //Room Allocation Tables
                if (!checkIfExist("Room_Tag"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Tag (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "TagID TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Room_Sub_Tag"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Sub_Tag (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "SubID TEXT NOT NULL, " +
                                        "TagID TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Room_Lec"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Lec (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "LecID TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Room_Group"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Group (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "GroupID TEXT NOT NULL, " +
                                        "SubGroupID TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Room_Session"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Session (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "SessionID TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Room_Con_Session"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Con_Session (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "ConSessionID TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Room_Reserve"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "CREATE TABLE Room_Reserve (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "RoomID TEXT NOT NULL, " +
                                        "ResDate TEXT NOT NULL, " +
                                        "STime TEXT NOT NULL, " +
                                        "ETime TEXT NOT NULL ); ";

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                if (!checkIfExist("Paralel_Session"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Paralel_Session(id Integer Primary Key Autoincrement,sessionId Integer,sessionCategory Integer);";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                if (!checkIfExist("Non_Overlapped_Session"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Non_Overlapped_Session(id Integer Primary Key Autoincrement,sessionId Integer,sessionCategory Integer);";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }


            }
            catch (Exception ex)
            {

            }
        }


        public bool checkIfExist(string tableName)
        {
            connection.Open();
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master  WHERE name='" + tableName + "'";
            var result = cmd.ExecuteScalar();
            connection.Close();
            return result != null && result.ToString() == tableName ? true : false;
          
        }

    }
}
