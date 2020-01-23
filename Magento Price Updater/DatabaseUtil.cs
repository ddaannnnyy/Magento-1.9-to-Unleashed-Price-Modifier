using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Magento_Price_Updater
{
    public class DatabaseUtil
    {
        private static string d = Directory.GetCurrentDirectory();
        private static string dbFile = "C:\\Users\\Danny\\source\repos\\Magento Price Updater\\Magento Price Updater\\databases\\database.db"; //should remove this hardcode
        private static string connectionString = $"Data Source={dbFile}";
        private static SQLiteConnection db = new SQLiteConnection(connectionString); //opens new sqlite connection using the connection string above

        /// <summary>
        /// gets an active connection to the database using the connection string in the DatabaseUnil.cs class
        /// </summary>
        /// <returns>if the method succeeds returns SQLiteConnection db, if the method fails with an ObjectDisposedException returns a new database connection. </returns>
        public static SQLiteConnection getConnection()
        {
            try
            {
                var fetchedDatabaseConnection = db.ConnectionString; //just a variable to make c# stop and wait for the database to provide a connection
            }
            catch (ObjectDisposedException ex)
            {               
                var db = getNewConnection(); //if there is a problem just open a new connection
                FileUtil.writeExeptionToFile(ex.ToString());
            }
            catch (Exception ex)
            {
                FileUtil.writeExeptionToFile(ex.Message);
            }

            return db;
        }
              
        /// <summary>
        /// gets a new connection to the daabase using the connection string in the DatabaseUtil.cs class
        /// </summary>
        /// <returns>if the method succeeds it will return SQLiteConnection db, if the method fails it will return null</returns>
        public static SQLiteConnection getNewConnection()
        {
            try
            {
                var db = new SQLiteConnection(connectionString);
                return db;
            }
            catch (Exception ex)
            {
                FileUtil.writeExeptionToFile(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// sets up a new database using the connection string and file name in DatabaseUtil.cs class
        /// </summary>
        /// <param name="seed">seed.sql or any text file with SQLite startup queries</param>
        public static void setupDatabase(string seed = null)
        {
            try
            { 
                

                if (seed != null) //if seed.sql is provided then it is executed when the database is created, if not then the database is created empty
                {
                    //TODO remove hardcode location
                    var dbFile = "C:\\Users\\Danny\\source\\repos\\Magento Price Updater\\Magento Price Updater\\databases\\database.db";

                    var connectionString = $"Data Source={dbFile}";
                    var seedLoc = "C:\\Users\\Danny\\source\\repos\\Magento Price Updater\\Magento Price Updater\\databases\\seed.sql";
                    var create = File.ReadAllText(seedLoc); //reads lines from the seed.sql file

                    var db = new SQLiteConnection(connectionString);

                    db.Execute(create);
                }
                else
                {
                    //TODO remove hardcode location
                    var dbFile = "database.db";
                    var connectionString = $"Data Source={dbFile}";
                    var create = "CREATE TABLE IF NOT EXISTS magentoImport; CREATE TABLE IF NOT EXISTS unleashedImport;";

                    var db = new SQLiteConnection(connectionString);

                    db.Execute(create);
                }
            }
            catch (Exception ex)
            {
                FileUtil.writeExeptionToFile(ex.Message);
            }
        }

        /// <summary>
        /// select query to write data to the database, returns a string[]
        /// </summary>
        /// <param name="table">table name that you are receieving data from</param>
        /// <param name="data">OPTIONAL: data that you want to retrieve. Default = *</param>
        /// <param name="where">OPTIONAL: WHERE conditioner </param>
        /// <param name="and">OPTIONAL: AND conditioner </param>
        /// <param name="or">OPTIONAL: OR conditioner </param>
        /// <returns>List<string> records on success returns null on fail</returns>
        public static List<string> selectDataToReader(string table, string data = "*", string where = "TRUE = TRUE", string and = "TRUE = TRUE", string or = "TRUE = TRUE")
        {
            var records = new List<string>();

            try
            {
                using (var db = getConnection().OpenAndReturn())
                {
                    string selectQuery = string.Format("SELECT {0} FROM {1} WHERE {2} AND {3} OR {4}", table, data, where, and, or);
                    var reader = db.ExecuteReader(selectQuery);

                    while (reader.Read())
                    {
                        try
                        {
                            records.Add(reader.ToString());
                        }
                        catch (Exception ex)
                        {
                            FileUtil.writeExeptionToFile(ex.Message);
                        }
                    }
                } return records;
            }
            catch (Exception ex)
            {
                FileUtil.writeExeptionToFile(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// insert query to write data to the database, used when providing data for every column in the database
        /// </summary>
        /// <param name="tableName">table name for insert query</param>
        /// <param name="data">data to be added to the database</param>
        public static void insertData(string tableName, string data)
        {
            using (var db = getConnection().OpenAndReturn()) //open connection
            using (var trans = db.BeginTransaction()) //start a transaction
            {
                try
                {
                    var query = String.Format("INSERT INTO {0} VALUES ({1});", tableName, data);
                    var result = db.Execute(query, trans); //forces c# to wait for the data to be completely written before moving on

                    trans.Commit(); //commit transaction
                }
                catch (Exception ex)
                {
                    FileUtil.writeExeptionToFile(ex.Message);
                    trans.Rollback(); //rollback transaction on fail to stop data corrupting
                }
            }
        }

        /// <summary>
        /// insert query to write data to the database with specific column specified
        /// </summary>
        /// <param name="tableName">table name for insert query</param>
        /// <param name="colName">column name for insert query</param>
        /// <param name="data">data to be added to the database</param>
        public static void insertData(string tableName, string colName, string data)
        {
            using (var db = getConnection().OpenAndReturn()) //open connection
            using (var trans = db.BeginTransaction()) //start a transaction
            {
                try
                {
                    var query = String.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, colName, data);
                    var result = db.Execute(query, trans); //forces c# to wait for the data to be completely written before moving on

                    trans.Commit(); //commit transaction
                }
                catch (Exception ex)
                {
                    trans.Rollback(); //rollback transaction on fail to stop data corrupting
                    FileUtil.writeExeptionToFile(ex.Message);
                }
            }
        }

        /// <summary>
        /// update query to write data to the database with data specified
        /// </summary>
        /// <param name="table">table name for update</param>
        /// <param name="set">columns and data to be updated (e.g. "DATA1 = VALUE1, DATA2 = VALUE2, DATA3 = VALUE3"</param>
        /// <param name="where">OPTIONAL: WHERE conditions. Default is TRUE = TRUE</param>
        /// <param name="and">OPTIONAL: AND conditions. Default is TRUE = TRUE</param>
        /// <param name="or">OPTIONAL: OR conditions. DEFAULT is TRUE = TRUE</param>
        public static void updateData(string table, string set, string where = "TRUE = TRUE", string and = "TRUE = TRUE", string or = "TRUE = TRUE")
        {
            using (var db = getConnection().OpenAndReturn()) //open connection
            using (var trans = db.BeginTransaction()) //start a transaction
            {
                try
                {
                    string updateQuery = string.Format("UPDATE {0} SET {1} WHERE {2} AND {3} OR {4};", table, set, where, and, or);
                    var result = db.Execute(updateQuery); //forces c# to wait for the data to be completely written before moving on
                    trans.Commit(); //commit transaction
                }
                catch (Exception ex)
                {
                    trans.Rollback(); //rollback transaction on fail to stop data corrupting
                    FileUtil.writeExeptionToFile(ex.Message);
                } 
            }
        }

    }
}
