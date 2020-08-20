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
                    cmd.CommandText = "CREATE TABLE AcademicYnS (id integer primary key,year varchar(100),semester varchar(100) );";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Program"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Program(id integer primary key,programCode varchar(100),programName varchar(100) );";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Group"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Group(id integer primary key,acyearnsemId integer,groupNo integer,groupId varchar(100) );";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("SubGroup"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE SubGroup(id integer primary key,groupId varchar(100),subgroupNo integer,subgroupId varchar(100));";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                if (!checkIfExist("Tag"))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE Tag(id integer primary key,tagCode varchar(100),tagName varchar(100) );";
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
