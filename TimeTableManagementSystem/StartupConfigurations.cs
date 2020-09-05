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

                    cmd.CommandText = "CREATE TABLE Groups(id Integer Primary Key Autoincrement,yearSem Integer,groupNo Integer,groupId varchar(100),prgmID Integer);";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("SubGroup"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE SubGroup(id Integer Primary Key Autoincrement,groupId varchar(100),subgroupNo Integer,subgroupId varchar(100));";
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

                    cmd.CommandText = "CREATE TABLE Lecturer (EmployeeID TEXT Primay Key, Name  TEXT, Faculty TEXT, Department TEXT, Center TEXT, Building  TEXT, Level TEXT, Rank REAL);";
                                      

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
                                        "Subject_Code   TEXT, " +
                                        "Tag  TEXT, " +
                                        "GroupID TEXT, " +
                                        "Student_Count INTEGER, " +
                                        "Duration  INTEGER, " +
                                        "PRIMARY KEY(Session_ID));";*/

                    cmd.CommandText = "CREATE TABLE Sessions(Session_ID INTEGER Primary Key Autoincrement, Lecturers TEXT, Subject TEXT, Subject_Code TEXT, Tag TEXT, GroupID TEXT, Student_Count INTEGER,Duration  INTEGER);";


                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch(Exception ex)
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
