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
            
            connection.Open();
            try
            {
               

                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE test (id integer primary key, text varchar(100));";
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
