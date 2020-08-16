using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Net.Http.Headers;

namespace TimeTableManagementSystem.DB_Config
{
    public class db_config
    {
       

        public static SQLiteConnection connect()
        {
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dbFilePath = "";

            bool a = !string.IsNullOrEmpty(dbPath);
            bool b = !Directory.Exists(dbPath);

            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
                
                
            }
            dbFilePath = dbPath + "\\yourDb.db";
            if (!System.IO.File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);

            }


            var conString = new SQLiteConnectionStringBuilder();
            conString.DataSource = dbFilePath;

            SQLiteConnection connection = new SQLiteConnection(conString.ConnectionString);

            //var rablecmd = connection.CreateCommand();
            //rablecmd.CommandText = "CREATE TABLER";


            return connection;
        }
        //public string  createDbFile()
        //{
        //    if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
        //    {
        //        Directory.CreateDirectory(dbPath);
        //        dbFilePath = dbPath + "\\yourDb.db";
        //        return dbFilePath;
        //    }
              
        //    if (!System.IO.File.Exists(dbFilePath))
        //    {
        //        SQLiteConnection.CreateFile(dbFilePath);
        //        return null;
        //    }
        //    return null;
        //}

    }
}
